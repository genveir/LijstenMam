using Elasticsearch.Net;
using LijstenMam.Data;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.ElasticSearch
{
    public class ESClient
    {
        protected ElasticClient client;

        public ESClient()
        {
            var uris = new List<Uri>() { new Uri("http://localhost:9200") };

            var connectionPool = new StaticConnectionPool(uris);

            var settings = new ConnectionSettings(connectionPool)
                .PingTimeout(TimeSpan.FromMilliseconds(100))
                .DefaultIndex(FileElementIndex.INDEX_NAME);

            client = new ElasticClient(settings);
        }

        public async Task Fill(Document document)
        {
            await FileElementIndex.Reset(client);

            var books = DocumentConverter.Convert(document);

            await FileElementIndex.Add(client, books);
        }

        public async Task<bool> Check()
        {
            var pingResponse = await client
                .PingAsync(p => p.Human());

            if (pingResponse == null || !pingResponse.IsValid) return false;
            return true;
        }

        public async Task<IEnumerable<long>> Search(string term, SearchOptions options)
        {
            term = term.ToLower().Trim();

            var query = BuildQuery(term, options);

            var searchResponse = await client.SearchAsync<FileElementDTO>(s => s
                .StoredFields(fs => fs
                    .Field(f => f.ParagraphNumber))
                .Query(query));

            if (!searchResponse.IsValid)
                throw new InvalidElasticSearchResponseException("Search-call to ElasticSearch did not return a valid response",
                    searchResponse.OriginalException);

            return searchResponse
                .Hits
                .Select(h => h.Id)
                .Select(id => long.Parse(id));
        }

        private Func<QueryContainerDescriptor<FileElementDTO>, QueryContainer> BuildQuery(string term, SearchOptions options)
        {
            var tokens = term.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var queries = new List<QueryContainer>();

            if (tokens.Length == 0) queries.Add(new MatchNoneQuery());
            
            foreach(var token in tokens)
            {
                queries.Add(BuildTermMatchQuery(token, options));
            }

            var combinedQuery = queries.Aggregate((a, b) => a && b);

            Func<QueryContainerDescriptor<FileElementDTO>, QueryContainer> result = q => combinedQuery;

            return result;
        }

        private QueryContainer BuildTermMatchQuery(string token, SearchOptions options)
        {
            var fieldQueries = new List<QueryContainer>();
            if (options.MatchGenre) fieldQueries.Add(CreateMatchQuery(token, Infer.Field<FileElementDTO>(f => f.Genres)));
            if (options.MatchBookTitle) fieldQueries.Add(CreateMatchQuery(token, Infer.Field<FileElementDTO>(f => f.BookTitle)));
            if (options.MatchBookCompiler) fieldQueries.Add(CreateMatchQuery(token, Infer.Field<FileElementDTO>(f => f.Compilers)));
            if (options.MatchArticleTitle) fieldQueries.Add(CreateMatchQuery(token, Infer.Field<FileElementDTO>(f => f.ArticleTitle)));
            if (options.MatchArticleAuthor) fieldQueries.Add(CreateMatchQuery(token, Infer.Field<FileElementDTO>(f => f.Authors)));

            return fieldQueries.Aggregate((a, b) => a || b);
        }

        private QueryContainer CreateMatchQuery(string token, Field field)
        {
            QueryContainer match = new MatchQuery()
            {
                Field = field,
                Query = token,
                Fuzziness = Fuzziness.Auto,
                Boost = 0.1
            } ||
            new MatchQuery()
            {
                Field = field,
                Query = token,
                Boost = 1
            };

            return match;
        }
    }
}
