using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LijstenMam.Backend.Data.DocumentModel
{
    public class DocumentWriter
    {
        public async Task WriteDocument(File file, string name)
        {
            var docToWrite = new XWPFDocument();

            await file.FileRoot.Write(docToWrite);

            using (var fs = new FileStream(name, FileMode.Create))
            {
                docToWrite.Write(fs);

                fs.Close();
            }
        }
    }
}
