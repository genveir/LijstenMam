using LijstenMam.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Data
{
    public class FileService
    {
        public async Task<List<File>> GetFiles()
        {
            var files = new List<File>()
            {
                new File() {Name = "Boeken" },
                new File() {Name = "Tijdschriften" }
            };

            return await Task.FromResult(files);
        }
    }
}
