using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Search
{
    public class FileElementIndex
    {
        public const string INDEX_NAME = "bestandselementen";

        public static async Task Reset(ElasticClient client)
        {
            await Delete(client);
            await Create(client);
        }

        public static async Task Add(ElasticClient client, IEnumerable<FileElementDTO> books)
        {
            var overflow = (books.Count() % 10000 == 0) ? 0 : 1;
            var numRuns = books.Count() / 10000 + overflow;

            for (int n = 0; n < numRuns; n++)
            {
                var page = books.Skip(n * 10000).Take(10000);

                var bulkResponse = await client.BulkAsync(b => b.IndexMany(page));

                if (!bulkResponse.IsValid)
                    throw new InvalidElasticSearchResponseException("AddOrUpdateMany-call to ElasticSearch did not return a valid response",
                        bulkResponse.OriginalException);
            }
        }

        private static async Task Delete(ElasticClient client)
        {
            var deleteResponse = await client.Indices.DeleteAsync(FileElementIndex.INDEX_NAME);

            if (!deleteResponse.IsValid)
            {
                if (deleteResponse.ServerError?.Status != 404)
                {
                    throw new InvalidElasticSearchResponseException("Clear-call to ElasticSearch did not return a valid response",
                        deleteResponse.OriginalException);
                }
            }
        }

        private static async Task Create(ElasticClient client)
        {
            var createResponse = await client.Indices.CreateAsync(INDEX_NAME, c => c
                .Settings(s => s
                    .Analysis(a => a
                        .Normalizers(n => n
                            .Custom("lowercase", cn => cn
                                .Filters("lowercase", "asciifolding")
                ))))
                .Map<FileElementDTO>(m => m.Properties(ps => ps
                    .Number(s => s.Name(n => n.ParagraphNumber))
                    .Text(s => s.Name(n => n.Genres))
                    .Text(s => s.Name(n => n.Compilers))
                    .Text(s => s.Name(n => n.BookTitle))
                    .Text(s => s.Name(n => n.BookSearchData))
                    .Text(s => s.Name(n => n.Authors))
                    .Text(s => s.Name(n => n.ArticleTitle))
                    .Text(s => s.Name(n => n.ArticleSearchData))
                ))
            );

            if (!createResponse.IsValid)
                throw new InvalidElasticSearchResponseException("Clear-call to ElasticSearch did not return a valid response",
                    createResponse.OriginalException);
        }
    }
}
