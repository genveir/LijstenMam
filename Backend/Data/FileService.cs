using LijstenMam.Backend.Data.DocumentModel;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LijstenMam.Backend.Data
{
    public class FileService : IFileService
    {
        public File File { get; set; }

        public async Task LoadFile(Stream fileStream, string name)
        {
            var bytes = await UploadFile(fileStream);

            var document = await DocumentReader.FromFile(bytes, name);

            File = new() { FileRoot = document, Name = name };
        }

        public async Task LoadExample()
        {
            var embedded = this.GetType().Assembly.GetManifestResourceNames().Single();

            var stream = this.GetType().Assembly.GetManifestResourceStream(embedded);

            await LoadFile(stream, "Example from backend.docx");
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
    }
}
