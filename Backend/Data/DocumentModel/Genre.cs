using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Backend.Data.DocumentModel
{
    public class Genre : FileElement
    {
        public Genre(long paragraphNumber, string text) : base( paragraphNumber, text) 
        {
            
        }

        internal override void AddTo(FileElement element)
        {
            element.Add(this);
        }

        internal override void Add(Genre genre)
        {
            if (this.Parent == null) throw new NotImplementedException("genre has no parent");

            this.Parent.Add(genre);
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
            var para = doc.CreateParagraph();
            var run = para.CreateRun();
            run.FontFamily ="Arial";
            run.FontSize = 12;
            run.IsBold = true;
            run.SetText(this.RawText);

            await WriteChildren(doc);
        }

        internal override void SetElementData()
        {
            this.ElementData = this.Parent.ElementData.Copy();
            this.ElementData.Genres = this.ElementData.Genres.Append(this.RawText);
        }
    }
}
