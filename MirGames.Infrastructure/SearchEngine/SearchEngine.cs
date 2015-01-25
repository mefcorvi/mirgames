// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="SearchEngine.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.SearchEngine
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    using Lucene.Net.Analysis.Ru;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;
    using Lucene.Net.Store;

    using Version = Lucene.Net.Util.Version;

    /// <summary>
    /// Base implementation of search engine.
    /// </summary>
    internal sealed class SearchEngine : ISearchEngine
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchEngine"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public SearchEngine(ISettings settings)
        {
            this.settings = settings;
        }

        /// <inheritdoc />
        public void Index(int id, string documentType, string text, params SearchIndexTerm[] terms)
        {
            using (var analyzer = GetAnalyzer())
            using (var directory = this.GetIndexDirectory())
            using (var writer = new IndexWriter(directory, analyzer, false, IndexWriter.MaxFieldLength.LIMITED))
            {
                var doc = new Document();
                doc.Add(new Field("id", id.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("text", text, Field.Store.NO, Field.Index.ANALYZED));
                doc.Add(new Field("type", documentType, Field.Store.YES, Field.Index.NOT_ANALYZED));

                if (terms != null)
                {
                    foreach (var term in terms)
                    {
                        var indexState = Field.Index.NOT_ANALYZED;

                        if (term.IsIndexed && term.IsNormalized)
                        {
                            indexState = Field.Index.ANALYZED;
                        }

                        if (term.IsIndexed && !term.IsNormalized)
                        {
                            indexState = Field.Index.ANALYZED_NO_NORMS;
                        }

                        if (!term.IsIndexed)
                        {
                            indexState = Field.Index.NOT_ANALYZED;
                        }

                        doc.Add(new Field(term.Key, term.Value, Field.Store.NO, indexState));
                    }
                }

                writer.AddDocument(doc);
                writer.Commit();
            }
        }

        /// <inheritdoc />
        public void Remove(int id, string documentType)
        {
            using (var writer = new IndexWriter(this.GetIndexDirectory(), GetAnalyzer(), false, IndexWriter.MaxFieldLength.LIMITED))
            {
                var idClause = new BooleanClause(new TermQuery(new Term("id", id.ToString(CultureInfo.InvariantCulture))), Occur.MUST);
                var typeClause = new BooleanClause(new TermQuery(new Term("type", documentType)), Occur.MUST);
                var query = new BooleanQuery { idClause, typeClause };

                writer.DeleteDocuments(query);
                writer.Commit();
            }
        }

        /// <inheritdoc />
        public void ClearIndex()
        {
            using (var writer = new IndexWriter(this.GetIndexDirectory(), GetAnalyzer(), true, IndexWriter.MaxFieldLength.LIMITED))
            {
                writer.DeleteAll();
                writer.Commit();
            }
        }

        /// <inheritdoc />
        public IEnumerable<SearchResult> Search(string documentType, string searchString, params SearchIndexTerm[] terms)
        {
            using (var analyzer = GetAnalyzer())
            using (var directory = this.GetIndexDirectory())
            {
                var query = GetQuery(documentType, searchString, terms, analyzer);
                var results = new List<SearchResult>();

                using (var searcher = new IndexSearcher(directory, true))
                {
                    TopDocs hits = searcher.Search(query, 1000);

                    foreach (var scoreDoc in hits.ScoreDocs)
                    {
                        Document doc = searcher.Doc(scoreDoc.Doc);
                        results.Add(new SearchResult
                            {
                                DocumentType = doc.Get("type"),
                                Score = scoreDoc.Score,
                                Id = int.Parse(doc.Get("id"))
                            });
                    }
                }

                return results;
            }
        }

        /// <inheritdoc />
        public int GetCount(string documentType, string searchString, params SearchIndexTerm[] terms)
        {
            using (var analyzer = GetAnalyzer())
            using (var directory = this.GetIndexDirectory())
            {
                var query = GetQuery(documentType, searchString, terms, analyzer);
                using (var searcher = new IndexSearcher(directory, true))
                {
                    TopDocs hits = searcher.Search(query, 1000);
                    return hits.TotalHits;
                }
            }
        }

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <param name="documentType">Type of the document.</param>
        /// <param name="searchString">The search string.</param>
        /// <param name="terms">The terms.</param>
        /// <param name="analyzer">The analyzer.</param>
        /// <returns>The query.</returns>
        private static BooleanQuery GetQuery(
            string documentType,
            string searchString,
            IEnumerable<SearchIndexTerm> terms,
            RussianAnalyzer analyzer)
        {
            var parser = new QueryParser(Version.LUCENE_30, "text", analyzer);
            Query textQuery = parser.Parse(searchString);

            var query = new BooleanQuery
            {
                new BooleanClause(textQuery, Occur.MUST),
                new BooleanClause(new TermQuery(new Term("type", documentType)), Occur.MUST)
            };

            if (terms != null)
            {
                foreach (var term in terms)
                {
                    query.Add(new BooleanClause(new TermQuery(new Term(term.Key, term.Value)), Occur.MUST));
                }
            }

            return query;
        }

        /// <summary>
        /// Gets the analyzer.
        /// </summary>
        /// <returns>The analyzer.</returns>
        private static RussianAnalyzer GetAnalyzer()
        {
            return new RussianAnalyzer(Version.LUCENE_30);
        }

        /// <summary>
        /// Gets the index directory.
        /// </summary>
        /// <returns>The index directory.</returns>
        private SimpleFSDirectory GetIndexDirectory()
        {
            var path = this.settings.GetValue<string>("SearchEngine.IndexDirectory");
            return new SimpleFSDirectory(new DirectoryInfo(path));
        }
    }
}