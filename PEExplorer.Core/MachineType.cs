using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExplorer.Core {
    public enum MachineType : ushort {
        Unknown = 0,
        X86 = 332,
        Arm = 0x1c0,
        Arm_NT = 0x1c4, 
        IA64 = 512,
        Amd64 = 34404,
        Arm64 = 43620,

    }
}
