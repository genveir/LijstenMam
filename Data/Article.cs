using LijstenMam.ElasticSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Data
{
    public class Article : FileElement
    {
        public Article(long paragraphNumber, string text) : base(paragraphNumber, text) 
        {
            while (this.Text.Contains("\t\t")) { this.Text = this.Text.Replace("\t\t", "\t"); }
            this.Text = this.Text.Trim();
        }

        public IEnumerable<string> GetAuthors()
        {
            if (string.IsNullOrWhiteSpace(Text)) return null;

            var withoutTitle = Text.Split('\t').First();

            var differentAuthors = withoutTitle.Split(" en ").Select(name => name.Trim());

            return differentAuthors;
        }

        public string GetTitle()
        {
            if (string.IsNullOrWhiteSpace(Text)) return null;

            var split = Text.Split('\t', 2);
            if (split.Length != 2) return null;

            var withoutAuthors = split[1];

            return withoutAuthors.Trim();
        }

        public override void AddTo(FileElement element)
        {
            element.Add(this);
        }

        public override void Convert(DocumentConverter converter, FileElementDTO parentInfo)
        {
            converter.ConvertElement(this, parentInfo);
        }

        public override void Add(Genre genre)
        {
            if (this.Parent == null) throw new NotImplementedException("article has no parent");

            this.Parent.Add(genre);
        }

        public override void Add(Book book)
        {
            if (this.Parent == null) throw new NotImplementedException("article has no parent");

            this.Parent.Add(book);
        }

        public override void Add(Article article)
        {
            if (this.Parent == null) throw new NotImplementedException("article has no parent");

            this.Parent.Add(article);
        }
    }
}
