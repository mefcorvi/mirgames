namespace MirGames.Services.Git.Services
{
    using System;
    using System.IO;
    using System.IO.Compression;

    using LibGit2Sharp;

    internal class ZipArchiver : ArchiverBase, IDisposable
    {
        /// <summary>
        /// The zipArchive.
        /// </summary>
        private readonly ZipArchive zipArchive;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipArchiver"/> class.
        /// </summary>
        /// <param name="output">The output.</param>
        public ZipArchiver(Stream output)
        {
            this.zipArchive = new ZipArchive(output, ZipArchiveMode.Create, true);
        }

        /// <inheritdoc />
        public override void BeforeArchiving(Tree tree, ObjectId oid, DateTimeOffset modificationTime)
        {
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.zipArchive.Dispose();
        }

        /// <inheritdoc />
        protected override void AddTreeEntry(string path, TreeEntry entry, DateTimeOffset modificationTime)
        {
            switch (entry.Mode)
            {
                case Mode.GitLink:
                case Mode.Directory:
                    break;
                case Mode.ExecutableFile:
                case Mode.NonExecutableFile:
                case Mode.NonExecutableGroupWritableFile:
                    var blob = (Blob)entry.Target;

                    var zipEntry = this.zipArchive.CreateEntry(path);
                    using (var stream = zipEntry.Open())
                    {
                        CopyStream(blob.GetContentStream(), stream);
                    }

                    break;
                case Mode.SymbolicLink:
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unsupported file mode: {0} (sha1: {1}).", entry.Mode, entry.Target.Id.Sha));
            }
        }

        /// <summary>
        /// Copies the stream.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        private static void CopyStream(Stream source, Stream target)
        {
            const int BufSize = 0x1000;
            var buf = new byte[BufSize];
            int bytesRead;

            while ((bytesRead = source.Read(buf, 0, BufSize)) > 0)
            {
                target.Write(buf, 0, bytesRead);
            }
        }
    }
}