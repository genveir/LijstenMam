using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Data
{
    public class File
    {
        public string Name { get; set; }

        public IEnumerable<string> Contents { get; set; } = new List<string>();

        public File Copy()
        {
            var newContents = new List<string>(Contents);

            return new File()
            {
                Name = this.Name,
                Contents = newContents
            };
        }
    }
}
