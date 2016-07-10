using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExplorer.Core {
    public class ExportedSymbol {
        public string Name { get; set; }
        public int Ordinal { get; set; }
        public uint Address { get; set; }
        public string ForwardName { get; set; }
    }
}
