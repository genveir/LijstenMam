using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LijstenMam.Backend.Data.DocumentModel
{
    public static class DocumentWriter
    {
        public static async Task WriteDocument(Document document, string name)
        {
            var docToWrite = new XWPFDocument();

            await document.Write(docToWrite);

            using (var fs = new FileStream(name, FileMode.Create))
            {
                docToWrite.Write(fs);

                fs.Close();
            }
        }
    }
}
