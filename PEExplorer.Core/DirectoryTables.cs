using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PEExplorer.Core {
	[StructLayout(LayoutKind.Sequential)]
	public struct IMAGE_EXPORT_DIRECTORY {
		public uint Characteristics;
		public uint TimeDateStamp;
		public ushort MajorVersion;
		public ushort MinorVersion;
		public int Name;
		public int Base;
		public int NumberOfFunctions;
		public int NumberOfNames;
		public int AddressOfFunctions;     // RVA from base of image
		public int AddressOfNames;         // RVA from base of image
		public int AddressOfOrdinals;  // RVA from base of image
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct IMAGE_RESOURCE_DIRECTORY {
		public int Characteristics;
		public int TimeDateStamp;
		public short MajorVersion;
		public short MinorVersion;
		public ushort NumberOfNamedEntries;
		public ushort NumberOfIdEntries;
		//  IMAGE_RESOURCE_DIRECTORY_ENTRY DirectoryEntries[];
	};

	[StructLayout(LayoutKind.Sequential)]
	public struct IMAGE_IMPORT_DIRECTORY {
		public int ImportLookupTable;
		public int TimeDateStamp;
		public int ForwarderChain;
		public int NameRva;
		public int ImportAddressTable;
	}


	[StructLayout(LayoutKind.Sequential)]
	public struct _IMAGE_DEBUG_DIRECTORY {
		public uint Characteristics;
		public uint TimeDateStamp;
		public ushort MajorVersion;
		public ushort MinorVersion;
		public ImageDebugType Type;
		public uint SizeOfData;
		public uint AddressOfRawData;
		public uint PointerToRawData;
	}


	[StructLayout(LayoutKind.Sequential)]
	public struct IMAGE_LOAD_CONFIG_DIRECTORY64 {
		public uint Size;
		public uint TimeDateStamp;
		public ushort MajorVersion;
		public ushort MinorVersion;
		public uint GlobalFlagsClear;
		public uint GlobalFlagsSet;
		public uint CriticalSectionDefaultTimeout;
		public ulong DeCommitFreeBlockThreshold;
		public ulong DeCommitTotalFreeThreshold;
		public ulong LockPrefixTable;
		public ulong MaximumAllocationSize;
		public ulong VirtualMemoryThreshold;
		public ulong ProcessAffinityMask;
		public uint ProcessHeapFlags;
		public ushort CSDVersion;
		public ushort Reserved1;
		public ulong EditList;
		public ulong SecurityCookie;
		public ulong SEHandlerTable;
		public ulong SEHandlerCount;
		public ulong GuardCFCheckFunctionPointer;       // VA
		public ulong GuardCFDispatchFunctionPointer;    // VA
		public ulong GuardCFFunctionTable;              // VA
		public ulong GuardCFFunctionCount;
		public ControlFlowGuardFlags GuardFlags;

		public IMAGE_LOAD_CONFIG_CODE_INTEGRITY CodeIntegrity;
		uint GuardAddressTakenIatEntryTable; // VA
		uint GuardAddressTakenIatEntryCount;
		uint GuardLongJumpTargetTable;       // VA
		uint GuardLongJumpTargetCount;
		uint DynamicValueRelocTable;         // VA
		uint HybridMetadataPointer;          // VA
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct IMAGE_LOAD_CONFIG_DIRECTORY32 {
		public uint Size;
		public uint TimeDateStamp;
		public ushort MajorVersion;
		public ushort MinorVersion;
		public uint GlobalFlagsClear;
		public uint GlobalFlagsSet;
		public uint CriticalSectionDefaultTimeout;
		public uint DeCommitFreeBlockThreshold;
		public uint DeCommitTotalFreeThreshold;
		public uint LockPrefixTable;
		public uint MaximumAllocationSize;
		public uint VirtualMemoryThreshold;
		public uint ProcessAffinityMask;
		public uint ProcessHeapFlags;
		public ushort CSDVersion;
		public ushort Reserved1;
		public uint EditList;
		public uint SecurityCookie;
		public uint SEHandlerTable;
		public uint SEHandlerCount;
		public uint GuardCFCheckFunctionPointer;       // VA
		public uint GuardCFDispatchFunctionPointer;    // VA
		public uint GuardCFFunctionTable;              // VA
		public uint GuardCFFunctionCount;
		public ControlFlowGuardFlags GuardFlags;

		public IMAGE_LOAD_CONFIG_CODE_INTEGRITY CodeIntegrity;
		ulong GuardAddressTakenIatEntryTable; // VA
		ulong GuardAddressTakenIatEntryCount;
		ulong GuardLongJumpTargetTable;       // VA
		ulong GuardLongJumpTargetCount;
		ulong DynamicValueRelocTable;         // VA
		ulong HybridMetadataPointer;          // VA
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct IMAGE_LOAD_CONFIG_CODE_INTEGRITY {
		public ushort Flags;
		public ushort Catalog;
		public uint CatalogOffset;
		public uint Reserved;
	}
}