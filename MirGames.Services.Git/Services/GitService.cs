namespace MirGames.Services.Git.Services
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;

    using MirGames.Infrastructure;
    using MirGames.Services.Git.Public.Services;

    using Repository = LibGit2Sharp.Repository;

    internal sealed class GitService : IGitService
    {
        /// <summary>
        /// The repository path provider.
        /// </summary>
        private readonly IRepositoryPathProvider repositoryPathProvider;

        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// The repository security.
        /// </summary>
        private readonly IRepositorySecurity repositorySecurity;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitService" /> class.
        /// </summary>
        /// <param name="repositoryPathProvider">The repository path provider.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="repositorySecurity">The repository security.</param>
        public GitService(IRepositoryPathProvider repositoryPathProvider, ISettings settings, IRepositorySecurity repositorySecurity)
        {
            Contract.Requires(repositoryPathProvider != null);
            Contract.Requires(settings != null);
            Contract.Requires(repositorySecurity != null);

            this.repositoryPathProvider = repositoryPathProvider;
            this.settings = settings;
            this.repositorySecurity = repositorySecurity;
        }

        /// <inheritdoc />
        public void ReceivePack(string repositoryName, Stream input, Stream output)
        {
            if (!this.repositorySecurity.CanWrite(repositoryName))
            {
                throw new UnauthorizedAccessException(
                    string.Format("Current user have no rights to write repository {0}", repositoryName));
            }

            string repositoryPath = this.repositoryPathProvider.GetPath(repositoryName);
            EnsureIsRepository(repositoryPath);
            
            this.RunGitCmd("receive-pack", false, repositoryPath, input, output);
        }

        /// <inheritdoc />
        public void UploadPack(string repositoryName, Stream input, Stream output)
        {
            if (!this.repositorySecurity.CanRead(repositoryName))
            {
                throw new UnauthorizedAccessException(
                    string.Format("Current user have no rights to read repository {0}", repositoryName));
            }

            string repositoryPath = this.repositoryPathProvider.GetPath(repositoryName);
            EnsureIsRepository(repositoryPath);

            this.RunGitCmd("upload-pack", false, repositoryPath, input, output);
        }

        /// <inheritdoc />
        public void GetInfoRefs(string repositoryName, string service, Stream input, Stream output)
        {
            if (service.EqualsIgnoreCase("git-upload-pack"))
            {
                if (!this.repositorySecurity.CanRead(repositoryName))
                {
                    throw new UnauthorizedAccessException(
                        string.Format("Current user have no rights to read repository {0}", repositoryName));
                }
            }
            else if (!this.repositorySecurity.CanWrite(repositoryName))
            {
                throw new UnauthorizedAccessException(
                    string.Format("Current user have no rights to write repository {0}", repositoryName));
            }

            string repositoryPath = this.repositoryPathProvider.GetPath(repositoryName);
            EnsureIsRepository(repositoryPath);

            var advertiseRefsContent = FormatMessage(string.Format("# service={0}\n", service)) + FlushMessage();

            using (var textWriter = new StreamWriter(output))
            {
                textWriter.Write(advertiseRefsContent);
            }

            this.RunGitCmd(service.Substring(4), true, repositoryPath, input, output);
        }

        private static void EnsureIsRepository(string repositoryPath)
        {
            if (!Repository.IsValid(repositoryPath))
            {
                throw new Exception("The specified directory is not a valid git repository.");
            }
        }

        /// <summary>
        /// Formats the message.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The result.</returns>
        private static string FormatMessage(string input)
        {
            return (input.Length + 4).ToString("X").PadLeft(4, '0') + input;
        }

        /// <summary>
        /// Flushes the message.
        /// </summary>
        /// <returns>The flush message.</returns>
        private static string FlushMessage()
        {
            return "0000";
        }

        /// <summary>
        /// Gets the GIT path.
        /// </summary>
        /// <returns>The path.</returns>
        private string GetGitPath()
        {
            return this.settings.GetValue<string>("Services.Git.Executable");
        }

        /// <summary>
        /// Runs the GIT command.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="advertiseRefs">if set to <c>true</c> refs will be advertised.</param>
        /// <param name="workingDir">The working directory.</param>
        /// <param name="inStream">The in stream.</param>
        /// <param name="outStream">The out stream.</param>
        /// <exception cref="System.Exception">GIT could not be started</exception>
        private void RunGitCmd(string serviceName, bool advertiseRefs, string workingDir, Stream inStream, Stream outStream)
        {
            Contract.Requires(!string.IsNullOrEmpty(serviceName));
            Contract.Requires(!string.IsNullOrEmpty(workingDir));
            Contract.Requires(inStream != null);
            Contract.Requires(outStream != null);

            var args = serviceName + " --stateless-rpc";
            if (advertiseRefs)
            {
                args += " --advertise-refs";
            }

            args += " \"" + workingDir + "\"";

            var info = new System.Diagnostics.ProcessStartInfo(this.GetGitPath(), args)
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = Path.GetDirectoryName(workingDir),
            };

            using (var process = System.Diagnostics.Process.Start(info))
            {
                if (process == null)
                {
                    throw new Exception("Git could not be started");
                }

                inStream.CopyTo(process.StandardInput.BaseStream);
                process.StandardInput.Write('\0');

                var buffer = new byte[16 * 1024];
                int read;
                while ((read = process.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outStream.Write(buffer, 0, read);
                    outStream.Flush();
                }

                process.WaitForExit();
            }
        }
    }
}