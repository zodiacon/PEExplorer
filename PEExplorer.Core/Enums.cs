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

	[Flags]
	public enum ImageCharacteristics : ushort {
		RelocsStripped = 1,
		ExecutableImage = 2,
		LineNumsStripped = 4,
		LocalSymbolsStripped = 8,
		AggressiveTrimWorkingSet = 0x10,
		LargeAddressAware = 0x20,
		LittleEndian = 0x80,
		Machine32Bit = 0x100,
		DebugInfoStripped = 0x200,
		RemovableRunFromSwap = 0x400,
		NetRunFromSwap = 0x800,
		SystemFile = 0x1000,
		DllFile = 0x2000,
		SingleCpuOnly = 0x4000,
		BigEndian = 0x8000
	}

	public enum ImageDebugType {
		Unknown,
		Coff,
		CodeView,
		Fpo,
		Misc,
		Exception,
		Fixup,
		Borland = 9
	}

	[Flags]
	public enum ControlFlowGuardFlags {
		Instrumented = 0x100,
		WriteInstrumented = 0x200,
		FunctionTablePresent = 0x400,
		SecurityCookieUnused = 0x800,
		ProtectDelayLoadIAT = 0x1000,
		DelayLoadIATOwnSection = 0x2000,
		ExportSuppressInfo = 0x4000,
		EnableExportSuppression = 0x8000,
		LongJumpTablePresent = 0x10000
	}
}
