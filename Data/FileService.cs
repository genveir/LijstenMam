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

        public IEnumerable<File> Files { get; private set; } = new List<File>();

        public File CurrentFile { get; set; }

        public FileService(FileRepository fileRepository)
        {
            this.FileRepository = fileRepository;

            var files = fileRepository.GetFiles();

            CurrentFile = files.First();
        }

        public void Update()
        {
            Files = FileRepository.GetFiles();
        }

        public void AddFile()
        {
            var fileName = "file" + FileRepository.Count;

            FileRepository.Add(fileName);

            CurrentFile = FileRepository.GetFile(fileName);

            Update();
        }

        public void Save(File file)
        {

        }

        public void SetFile(File file)
        {
            CurrentFile = file;
        }
    }
}
