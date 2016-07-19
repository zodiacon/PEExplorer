using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PEExplorer.Core {
    [StructLayout(LayoutKind.Sequential)]
    public struct ExceptionTableEntry32 {
        public int BeginAddress;
        public int EndAddress;
        public int ExceptionHandler;
        public int HandlerData;
        public int PrologEndAddress;
    }

    public struct ExceptionTableEntry64 {
        public int BeginAddress;
        public int EndAddress;
        public int UnwindInformation;
    }
}
