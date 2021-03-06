using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Backend.Data
{
    public class ElementData
    {
        public ElementData()
        {
            this.Genres = new List<string>();
            this.Compilers = new List<string>();
            this.Authors = new List<string>();
            this.BookSearchTerms = new List<string>();
            this.ArticleSearchTerms = new List<string>();
        }

        public IEnumerable<string> Genres { get; set; }

        public IEnumerable<string> Compilers { get; set; }

        public string BookTitle { get; set; }

        public IEnumerable<string> BookSearchTerms { get; set; }

        public IEnumerable<string> Authors { get; set; }

        public string ArticleTitle { get; set; }

        public IEnumerable<string> ArticleSearchTerms { get; set; }

        public ElementData Copy()
        {
            return new ElementData()
            {
                Genres = new List<string>(this.Genres),
                Compilers = new List<string>(this.Compilers),
                BookTitle = this.BookTitle,
                BookSearchTerms = new List<string>(this.BookSearchTerms),
                Authors = new List<string>(this.Authors),
                ArticleTitle = this.ArticleTitle,
                ArticleSearchTerms = new List<string>(this.ArticleSearchTerms)
            };
        }
    }
}
