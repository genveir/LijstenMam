using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Data
{
    public class Genre : FileElement
    {
        public Genre(long paragraphNumber, string text) : base( paragraphNumber, text) { }

        public override void AddTo(FileElement element)
        {
            element.Add(this);
        }

        public override void Add(Genre category)
        {
            if (this.Parent == null) throw new NotImplementedException("category has no parent");

            this.Parent.Add(category);
        }

        public override void Add(Book book)
        {
            this._Children.Add(book);
            book.Parent = this;
        }

        public override void Add(Article article)
        {
            this._Children.Add(article);
            article.Parent = this;
        }
    }
}
