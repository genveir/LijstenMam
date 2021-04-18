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

        private static object LockObj = new object();

        public FileRepository(string folder)
        {
            this.folder = folder;
        }

        public List<File> GetFiles()
        {
            lock (LockObj)
            {
                var files = Directory.GetFiles(folder, "*.txt");

                var parsed = new List<File>();
                foreach (var file in files)
                {
                    parsed.Add(ParseFile(file));
                }

                return parsed;
            }
        }

        public int Count => GetFiles().Count;

        public void Add(string fileName)
        {
            lock (LockObj)
            {
                var path = Path.Combine(folder, fileName + ".txt");

                using (new StreamWriter(new FileStream(path, FileMode.Create))) { }
            }
        }

        public File GetFile(string fileName)
        {
            lock (LockObj)
            {

                var path = Path.Combine(folder, fileName + ".txt");

                if (System.IO.File.Exists(path))
                {
                    return ParseFile(path);
                }
                else return null;
            }
        }

        private File ParseFile(string path)
        {
            lock (LockObj)
            {
                var name = Path.GetFileNameWithoutExtension(path);
                IEnumerable<string> contents;
                using (var sr = new StreamReader(new FileStream(path, FileMode.Open)))
                {
                    contents = sr.ReadToEnd().Split(Environment.NewLine);
                }

                return new File()
                {
                    Name = name,
                    Contents = contents
                };
            }
        }
    }
}
