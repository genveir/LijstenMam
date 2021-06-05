using LijstenMam.Backend.Data.DocumentModel;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Backend.Data
{
    public abstract class FileElement
    {
        public FileElement(long paragraphNumber, string text)
        {
            while (text.Contains("\t\t")) text = text.Replace("\t\t", "\t");

            this.ParagraphNumber = paragraphNumber;
            this.RawText = text;

            this._Children = new List<FileElement>();
        }

        public long ParagraphNumber { get; set; }

        public string RawText { get; set; }

        public ElementData ElementData { get; set; }

        internal abstract void Add(Genre genre);

        internal abstract void Add(Book book);

        internal abstract void Add(Article article);

        internal abstract void AddTo(FileElement element);

        internal abstract void SetElementData();

        internal abstract Task Write(XWPFDocument doc);
        protected virtual async Task WriteChildren(XWPFDocument doc)
        {
            foreach (var child in Children) await child.Write(doc);
        }
        internal virtual string numberingId { get; set; }

        public FileElement Parent { get; set; }

        protected List<FileElement> _Children { get; }
        public IEnumerable<FileElement> Children => _Children.OrderBy(c => c.RawText);
    }
}
