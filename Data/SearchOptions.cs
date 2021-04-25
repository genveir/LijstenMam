using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Data
{
    public class SearchOptions
    {
        public bool MatchGenre = true;

        public bool MatchBookTitle = true;

        public bool MatchBookCompiler = true;

        public bool MatchArticleAuthor = true;

        public bool MatchArticleTitle = true;
    }
}
