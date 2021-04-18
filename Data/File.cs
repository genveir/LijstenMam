using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Data
{
    public class File
    {
        public string Name { get; set; }

        public string[] Contents { get; set; } = new string[0];

        public File Copy()
        {
            var newContents = new string[this.Contents.Length];
            if (newContents.Length > 0)
            {
                this.Contents.CopyTo(newContents, 0);
            }

            return new File()
            {
                Name = this.Name,
                Contents = newContents
            };
        }
    }
}
