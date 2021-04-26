using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Data
{
    public interface IFileService
    {
        Task LoadFile(Stream fileStream, string name);

        Task Reset();

        File File { get; }
    }
}
