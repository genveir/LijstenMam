using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Backend.ElasticSearch
{
    [ElasticsearchType(IdProperty = "ParagraphNumber")]
    public class FileElementDTO
    {
        public FileElementDTO()
        {
            this.Genres = new List<string>();
            this.Compilers = new List<string>();
            this.Authors = new List<string>();
            this.BookSearchData = new List<string>();
            this.ArticleSearchData = new List<string>();
        }

        public long ParagraphNumber { get; set; }

        public IEnumerable<string> Genres { get; set; }

        public IEnumerable<string> Compilers { get; set; }

        public string BookTitle { get; set; }

        public IEnumerable<string> BookSearchData { get; set; }

        public IEnumerable<string> Authors { get; set; }

        public string ArticleTitle { get; set; }

        public IEnumerable<string> ArticleSearchData { get; set; }
    }
}
