using LijstenMam.Shared;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Data
{
    public class FileService
    {
       private File File { get; set; }

        public async Task LoadFile(Stream fileStream)
        {
            var bytes = await UploadFile(fileStream);
            var document = await ConvertToDocX(bytes);

            File = await Parse(document);
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

        private async Task<File> Parse(XWPFDocument docX)
        {
            return await Task.FromResult(new File());
        }
    }
}
