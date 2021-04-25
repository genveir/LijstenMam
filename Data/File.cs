using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Data
{
    public class File
    {
        public string Name { get; set; }

        public Document FileRoot { get; set; }
        private Dictionary<long, FileElement> ElementsByParagraphId { get; set; }

        public File()
        {
            FileRoot = new Document(Name);
        }

        public void MapElements(FileElement element)
        {
            if (ElementsByParagraphId == null) ElementsByParagraphId = new Dictionary<long, FileElement>();
            ElementsByParagraphId[element.ParagraphNumber] = element;

            foreach (var child in element.Children) MapElements(child);
        }

        public IEnumerable<FileElement> GetElementsByParagraphId(IEnumerable<long> paragraphIds)
        {
            if (ElementsByParagraphId == null) MapElements(FileRoot);

            return paragraphIds.Select(pid => ElementsByParagraphId[pid]).ToList();
        }
    }
}
