using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Backend.Data
{
    public class FileService : IFileService
    {
        public File File { get; set; }

        public async Task LoadFile(Stream fileStream, string name)
        {
            var bytes = await UploadFile(fileStream);
            var document = await ConvertToDocX(bytes);

            File = await Parse(document, name);
        }

        public async Task Reset()
        {
            await Task.Run(() => File = null);
        }

        private async Task<byte[]> UploadFile(Stream fileStream)
        {
            var length = (int)fileStream.Length;
            var contents = new byte[length];

            await fileStream.ReadAsync(contents, 0, length);

            return contents;
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

        private async Task<File> Parse(XWPFDocument docX, string name)
        {
            var doc = new Document(name);
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
