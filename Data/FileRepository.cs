using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Data
{
    public class FileRepository
    {
        private string folder;

        public FileRepository(string folder)
        {
            this.folder = folder;
        }

        public List<File> GetFiles()
        {
            var files = Directory.GetFiles(folder, "*.txt");

            var parsed = new List<File>();
            foreach(var file in files)
            {
                parsed.Add(ParseFileName(file));
            }

            return parsed;
        }

        public int Count => GetFiles().Count;

        public void Add(string fileName)
        {
            var path = Path.Combine(folder, fileName + ".txt");

            System.IO.File.Create(path);
        }

        public File GetFile(string fileName)
        {
            var path = Path.Combine(folder, fileName + ".txt");

            if (System.IO.File.Exists(path)) return ParseFileName(path);
            else return null;
        }

        private File ParseFileName(string fileName)
        {
            var name = Path.GetFileNameWithoutExtension(fileName);

            return new File()
            {
                Name = name
            };
        }
    }
}
