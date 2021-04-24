using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Data
{
    public class Book : FileElement
    {
        public Book(long paragraphNumber, string text) : base(paragraphNumber, text) { }

        public override void AddTo(FileElement element)
        {
            element.Add(this);
        }

        public override void Add(Category category)
        {
            if (this.Parent == null) throw new NotImplementedException("book has no parent");

            this.Parent.Add(category);
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
