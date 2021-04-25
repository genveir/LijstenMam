using LijstenMam.ElasticSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Data
{
    public class Book : FileElement
    {
        public Book(long paragraphNumber, string text) : base(paragraphNumber, text) 
        {
            if (this.Text != null)
            {
                while (this.Text.Contains("\t\t")) { this.Text = this.Text.Replace("\t\t", "\t"); }
                this.Text = this.Text.Trim();
            }
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
            if (this.Parent == null) throw new NotImplementedException("book has no parent");

            this.Parent.Add(genre);
        }

        public override void Add(Book book)
        {
            if (this.Parent == null) throw new NotImplementedException("book has no parent");

            this.Parent.Add(book);
        }

        public override void Add(Article article)
        {
            this._Children.Add(article);
            article.Parent = this;
        }
    }
}
