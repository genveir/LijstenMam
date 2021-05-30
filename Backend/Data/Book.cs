using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Backend.Data
{
    public class Book : FileElement
    {
        public Book(long paragraphNumber, string text) : base(paragraphNumber, text) 
        {
            
        }

        public IEnumerable<string> GetAuthors()
        {
            if (string.IsNullOrWhiteSpace(RawText)) return null;

            var withoutTitle = RawText.Split('\t').First();

            var differentAuthors = withoutTitle.Split(" en ").Select(name => name.Trim());

            return differentAuthors;
        }

        public string GetTitle()
        {
            if (string.IsNullOrWhiteSpace(RawText)) return null;

            var split = RawText.Split('\t', 2);
            if (split.Length != 2) return null;

            var withoutAuthors = split[1];

            return withoutAuthors.Trim();
        }


        internal override void AddTo(FileElement element)
        {
            element.Add(this);
        }

        internal override void Add(Genre genre)
        {
            if (this.Parent == null) throw new NotImplementedException("book has no parent");

            this.Parent.Add(genre);
        }

        internal override void Add(Book book)
        {
            if (this.Parent == null) throw new NotImplementedException("book has no parent");

            this.Parent.Add(book);
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
            run.FontFamily = "Arial";
            run.FontSize = 12;
            run.SetText(this.RawText);

            if (Children.Any())
            {
                var numbering = doc.CreateNumbering();
                numberingId = numbering.AddNum("1");
            }

            await WriteChildren(doc);
        }

        internal override void SetElementData()
        {
            this.ElementData = Parent.ElementData.Copy();
            this.ElementData.Compilers = this.GetAuthors();
            this.ElementData.BookTitle = this.GetTitle();
        }
    }
}
