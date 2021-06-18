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
        public FileElement(long paragraphNumber, string text, string search)
        {
            this.ParagraphNumber = paragraphNumber;
            this.RawText = text;
            this.RawSearch = search;

            this._Children = new List<FileElement>();
        }

        public IEnumerable<string> GetSearchTerms()
        {
            if (string.IsNullOrWhiteSpace(RawSearch)) return new List<string>();

            var split = RawSearch.Split(new char[] { '–', '-' });

            var searchTerms = split
                .Select(t => t.Trim());

            return searchTerms;
        }

        public long ParagraphNumber { get; set; }

        public string RawText { get; set; }

        public string RawSearch { get; set; }

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
