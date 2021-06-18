using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Backend.Data
{
    public class SearchOptions
    {
        public bool MatchGenre = true;

        public bool MatchBookTitle = true;

        public bool MatchBookCompiler = true;

        public bool MatchBookSearchData = true;

        public bool MatchArticleAuthor = true;

        public bool MatchArticleTitle = true;

        public bool MatchArticleSearchDAta = true;
    }
}
