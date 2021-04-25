using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Data
{
    public class Document : FileElement
    {
        public Document(string name) : base(-1, name) { }

        public override void AddTo(FileElement element)
        {
            throw new NotImplementedException("cannot add a document to another file element");
        }

        public override void Add(Genre category)
        {
            this._Children.Add(category);
            category.Parent = this;
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
