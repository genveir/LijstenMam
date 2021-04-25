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
                .DefaultIndex(FileElementIndex.INDEX_NAME);

            client = new ElasticClient(settings);
        }

        public void Fill(Document document)
        {
            FileElementIndex.Reset(client);

            var books = DocumentConverter.Convert(document);

            FileElementIndex.Add(client, books);
        }
    }
}
