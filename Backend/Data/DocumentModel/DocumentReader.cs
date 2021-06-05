using LijstenMam.Backend.Data.DocumentModel;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LijstenMam.Backend.Data.DocumentModel
{
    public class DocumentReader
    {
        public async Task<File> ReadFile(byte[] fileContents, string name)
        {
            var docx = await ConvertToDocX(fileContents);

            var file = await Parse(docx, name);

            return file;
        }

        private async Task<File> Parse(XWPFDocument docX, string name)
        {
            var doc = new DocumentModel.Document(name);
            FileElement currentElement = doc;

            await Task.Run(() =>
            {
                for (int n = 0; n < docX.Paragraphs.Count; n++)
                {
                    currentElement = ParseParagraph(n, docX.Paragraphs[n], currentElement);
                }
            });

            return new File() { Name = name, FileRoot = doc };
        }

        private async Task<XWPFDocument> ConvertToDocX(byte[] contents)
        {
            return await Task.Run(() =>
            {
                XWPFDocument document;
                using (var memStream = new MemoryStream(contents))
                {
                    document = new XWPFDocument(memStream);
                }

                return document;
            });
        }

        private FileElement ParseParagraph(int paragraphNumber, XWPFParagraph paragraph, FileElement currentElement)
        {
            var text = paragraph.Text;
            if (string.IsNullOrWhiteSpace(text)) return currentElement;

            var numIndent = paragraph.NumLevelText;
            var isBold = paragraph.IRuns.Any(ir =>
            {
                var run = ir as XWPFRun;
                return run?.IsBold ?? false;
            });

            FileElement toAdd;
            if (isBold) toAdd = new Genre(paragraphNumber, text.Trim());
            else if (numIndent != null) toAdd = new Article(paragraphNumber, text);
            else toAdd = new Book(paragraphNumber, text);

            toAdd.AddTo(currentElement);
            toAdd.SetElementData();

            return toAdd;
        }
    }
}
