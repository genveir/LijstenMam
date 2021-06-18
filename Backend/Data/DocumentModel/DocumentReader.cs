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
    public static class DocumentReader
    {
        public static async Task<Document> FromFile(byte[] fileContents, string name)
        {
            var docx = await ConvertToDocX(fileContents);

            var document = await Parse(docx, name);

            return document;
        }

        private static async Task<Document> Parse(XWPFDocument docX, string name)
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

            return doc;
        }

        private static async Task<XWPFDocument> ConvertToDocX(byte[] contents)
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

        private static FileElement ParseParagraph(int paragraphNumber, XWPFParagraph paragraph, FileElement currentElement)
        {
            var text = paragraph.Text;
            if (string.IsNullOrWhiteSpace(text)) return currentElement;

            var numIndent = paragraph.NumLevelText;
            var isBold = paragraph.IRuns.Any(ir =>
            {
                var run = ir as XWPFRun;
                return run?.IsBold ?? false;
            });
            var italicRuns = paragraph.IRuns.Where(ir =>
            {
                var run = ir as XWPFRun;
                return run?.IsItalic ?? false;
            });

            var otherRuns = paragraph.IRuns.Except(italicRuns);

            var baseText = FormatRuns(otherRuns);
            var searchText = FormatRuns(italicRuns);

            FileElement toAdd;
            if (isBold) toAdd = new Genre(paragraphNumber, baseText, searchText);
            else if (numIndent != null) toAdd = new Article(paragraphNumber, baseText, searchText);
            else toAdd = new Book(paragraphNumber, baseText, searchText);

            toAdd.AddTo(currentElement);
            toAdd.SetElementData();

            return toAdd;
        }

        private static string FormatRuns(IEnumerable<IRunElement> runs)
        {
            var runText = runs
                .Select(r => r.ToString())
                .Select(r => r.Trim());

            var text = string.Join(" ", runs);

            while (text.Contains("\t\t")) text = text.Replace("\t\t", "\t");

            return text;
        }
    }
}
