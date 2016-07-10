using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExplorer.Core {
    public enum SubsystemType : ushort {
        Unknown,
        Native,
        WindowsGUI,
        WindowsCUI,
        PosixCUI = 7,
        WindowsCEGUI = 9,
        EfiApplication = 10,
        EfiBootServiceDriver = 11,
        EfiRuntimeDriver = 12,
        EfiROM = 13,
        XBOX = 14
    }

    [Flags]
    public enum DllCharacteristics : ushort {
        None = 0,
        HighEntropyVA = 0x20,
        DynamicBase = 0x40,
        ForceIntegrity = 0x80,
        NxCompat = 0x100,
        NoIsolation = 0x200,
        NoSEH = 0x400,
        NoBind = 0x800,
        AppContainer = 0x1000,
        WDMDriver = 0x2000,
        ControlFlowGuard = 0x4000,
        TerminalServerAware = 0x8000
    }
}
