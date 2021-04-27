using LijstenMam.ElasticSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Data
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

        public FileElement Parent { get; set; }

        protected List<FileElement> _Children { get; }
        public IEnumerable<FileElement> Children => _Children.OrderBy(c => c.RawText);
    }
}
