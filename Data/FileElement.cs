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
            this.ParagraphNumber = paragraphNumber;
            this.Text = text;

            this._Children = new List<FileElement>();
        }

        public abstract void Add(Category category);

        public abstract void Add(Book book);

        public abstract void Add(Article article);

        public abstract void AddTo(FileElement element);

        public long ParagraphNumber { get; set; }
        public string Text { get; set; }

        public FileElement Parent { get; set; }

        protected List<FileElement> _Children { get; }
        public IEnumerable<FileElement> Children => _Children;
    }
}
