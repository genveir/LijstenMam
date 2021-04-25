using LijstenMam.ElasticSearch;
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

        public override void Convert(DocumentConverter converter, FileElementDTO parentInfo)
        {
            converter.ConvertElement(this, parentInfo);
        }

        public override void Add(Genre genre)
        {
            if (this.Parent == null) throw new NotImplementedException("genre has no parent");

            this.Parent.Add(genre);
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
