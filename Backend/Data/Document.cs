using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Backend.Data
{
    public class Document : FileElement
    {
        public Document(string name) : base(-1, name) 
        {
            SetElementData();
        }

        internal override void AddTo(FileElement element)
        {
            throw new NotImplementedException("cannot add a document to another file element");
        }

        internal override void Add(Genre genre)
        {
            this._Children.Add(genre);
            genre.Parent = this;
        }

        internal override void Add(Book book)
        {
            this._Children.Add(book);
            book.Parent = this;
        }

        internal override void Add(Article article)
        {
            this._Children.Add(article);
            article.Parent = this;
        }

        internal override async Task Write(XWPFDocument doc)
        {
            await WriteChildren(doc);
        }

        internal override void SetElementData()
        {
            this.ElementData = new ElementData();
        }
    }
}
