using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Data
{
    public class File
    {
        public string Name { get; set; }

        public Document FileRoot { get; set; }

        public File()
        {
            FileRoot = new Document(Name);
        }
    }
}
