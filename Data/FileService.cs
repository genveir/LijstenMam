using LijstenMam.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Data
{
    public class FileService
    {
        private List<File> files;

        public FileService()
        {
            files = new List<File>()
            {
                new File() {Name = "Boeken" },
                new File() {Name = "Tijdschriften" }
            };
        }


        public async Task<List<File>> GetFiles()
        {
            return await Task.FromResult(files);
        }

        public void AddFile()
        {
            files.Add(new File() { Name = "file" + files.Count });
        }
    }
}
