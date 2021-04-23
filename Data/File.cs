using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LijstenMam.Data
{
    public class File
    {
        public string Name { get; set; }

        public IEnumerable<Line> Lines { get; set; }
    }

    public class Line
    {
        public long LineNumber { get; set; }
        public string Text { get; set; }
    }
}
