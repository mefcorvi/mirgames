// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="RepositoryExtensions.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Services.Git.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    using LibGit2Sharp;

    internal static class RepositoryExtensions
    {
        /// <summary>
        /// Gets the tree commits.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="tree">The tree.</param>
        /// <returns>The tree commits.</returns>
        public static IEnumerable<KeyValuePair<TreeEntry, Commit>> GetCommits(this Repository repository, IEnumerable<TreeEntry> tree)
        {
            var treeItems = new List<TreeEntry>(tree);
            var result = new List<KeyValuePair<TreeEntry, Commit>>(treeItems.Count);

            var visitedCommits = new HashSet<string>();
            var queue = new Queue<Commit>();

            var commit = repository.Head.Tip;
            queue.Enqueue(commit);
            visitedCommits.Add(commit.Sha);

            while (queue.Count > 0 && treeItems.Count > 0)
            {
                commit = queue.Dequeue();
                var hasFound = new bool[treeItems.Count];

                foreach (var parent in commit.Parents)
                {
                    for (var i = 0; i < treeItems.Count; i++)
                    {
                        var treeEntry = parent[treeItems[i].Path];

                        if (treeEntry == null)
                        {
                            continue;
                        }

                        var found = treeEntry.Target.Sha == treeItems[i].Target.Sha;
                        if (found && visitedCommits.Add(parent.Sha))
                        {
                            queue.Enqueue(parent);
                        }

                        hasFound[i] = hasFound[i] || found;
                    }
                }

                for (var i = treeItems.Count - 1; i >= 0; i--)
                {
                    if (!hasFound[i])
                    {
                        result.Add(new KeyValuePair<TreeEntry, Commit>(treeItems[i], commit));
                        treeItems.RemoveAt(i);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the tree commits.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="tree">The tree.</param>
        /// <returns>The tree commits.</returns>
        public static Commit GetCommit(this Repository repository, TreeEntry tree)
        {
            return repository.GetCommits(new[] { tree }).First().Value;
        }
    }
}
