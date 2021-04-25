using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.ElasticSearch
{
    public class FileElementDTO
    {
        public FileElementDTO()
        {
            this.Compilers = new List<string>();
            this.Authors = new List<string>();
        }

        public long ParagraphNumber { get; set; }

        public string Genre { get; set; }

        public IEnumerable<string> Compilers { get; set; }

        public string BookTitle { get; set; }

        public IEnumerable<string> Authors { get; set; }

        public string ArticleTitle { get; set; }

        public FileElementDTO Copy()
        {
            return new FileElementDTO()
            {
                ParagraphNumber = this.ParagraphNumber,
                Genre = this.Genre,
                Compilers = new List<string>(this.Compilers),
                BookTitle = this.BookTitle,
                Authors = new List<string>(this.Authors),
                ArticleTitle = this.ArticleTitle
            };
        }
    }
}
