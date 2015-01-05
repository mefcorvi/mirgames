// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GitService.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Services.Git.Services
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;
    using MirGames.Services.Git.Public.Events;
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
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// The principal provider.
        /// </summary>
        private readonly Func<ClaimsPrincipal> principalProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitService" /> class.
        /// </summary>
        /// <param name="repositoryPathProvider">The repository path provider.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="repositorySecurity">The repository security.</param>
        /// <param name="eventBus">The event bus.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        /// <param name="principalProvider">The principal provider.</param>
        public GitService(
            IRepositoryPathProvider repositoryPathProvider,
            ISettings settings,
            IRepositorySecurity repositorySecurity,
            IEventBus eventBus,
            IReadContextFactory readContextFactory,
            Func<ClaimsPrincipal> principalProvider)
        {
            Contract.Requires(repositoryPathProvider != null);
            Contract.Requires(settings != null);
            Contract.Requires(repositorySecurity != null);
            Contract.Requires(eventBus != null);
            Contract.Requires(principalProvider != null);

            this.repositoryPathProvider = repositoryPathProvider;
            this.settings = settings;
            this.repositorySecurity = repositorySecurity;
            this.eventBus = eventBus;
            this.readContextFactory = readContextFactory;
            this.principalProvider = principalProvider;
        }

        /// <inheritdoc />
        public void ReceivePack(string repositoryName, Stream input, Stream output)
        {
            int? userId = this.principalProvider.Invoke().GetUserId();

            if (!userId.HasValue || !this.repositorySecurity.CanWrite(repositoryName))
            {
                throw new UnauthorizedAccessException(
                    string.Format("Current user has no rights to write repository {0}", repositoryName));
            }

            string repositoryPath = this.repositoryPathProvider.GetPath(repositoryName);
            EnsureIsRepository(repositoryPath);
            
            this.RunGitCmd("receive-pack", false, repositoryPath, input, output);

            int repositoryId = this.GetRepositoryId(repositoryName);

            this.eventBus.Raise(new RepositoryUpdatedEvent { RepositoryId = repositoryId, AuthorId = userId.Value });
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
            int? userId = this.principalProvider.Invoke().GetUserId();

            if (service.EqualsIgnoreCase("git-upload-pack"))
            {
                if (!this.repositorySecurity.CanRead(repositoryName))
                {
                    throw new UnauthorizedAccessException(
                        string.Format("Current user have no rights to read repository {0}", repositoryName));
                }
            }
            else if (!userId.HasValue || !this.repositorySecurity.CanWrite(repositoryName))
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

        /// <summary>
        /// Gets the repository identifier.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <returns>The repository identifier.</returns>
        private int GetRepositoryId(string repositoryName)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                return readContext.Query<Entities.Repository>().First(r => r.Name == repositoryName).Id;
            }
        }
    }
}