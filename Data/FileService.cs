using LijstenMam.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Data
{
    public class FileService
    {
        private FileRepository FileRepository { get; set; }
        
        public File CurrentFile { get; set; }

        public FileService(FileRepository fileRepository)
        {
            this.FileRepository = fileRepository;

            var files = fileRepository.GetFiles();

            CurrentFile = files.First();
        }


        public async Task<List<File>> GetFiles()
        {
            return await Task.FromResult(FileRepository.GetFiles());
        }

        public void AddFile()
        {
            var fileName = "file" + FileRepository.Count;

            FileRepository.Add(fileName);

            CurrentFile = FileRepository.GetFile(fileName);
        }

        public void SetFile(string fileName)
        {
            var file = FileRepository.GetFile(fileName);

            CurrentFile = file;
        }
    }
}
