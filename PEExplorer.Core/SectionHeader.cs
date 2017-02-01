using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PEExplorer.Core {
	[Flags]
	public enum SectionFlags : uint {
		NoPad = 8,
		Code = 0x20,
		InitializedData = 0x40,
		UninitializedData = 0x80,
		Other = 0x100,
		Info = 0x200,
		Remove = 0x800,
		Comdat = 0x1000,
		GPRel = 0x80000,
		Align1Byte = 0x100000,
		Align2Bytes = 0x200000,
		ExtendedReloc = 0x1000000,
		Discardable = 0x2000000,
		NotCached = 0x4000000,
		NotPaged = 0x8000000,
		Shared =	0x10000000,
		Execute =	0x20000000,
		Read =		0x40000000,
		Write =		0x80000000,
	}

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SectionHeader {
        public string Name {
            get {
                fixed (byte* ptr = NameBytes) {
                    if(ptr[7] == 0)
                        return Marshal.PtrToStringAnsi((IntPtr)ptr);
                    else
                        return Marshal.PtrToStringAnsi((IntPtr)ptr, 8);
                }
            }
        }
        public fixed byte NameBytes[8];
        public uint VirtualSize;
        public uint VirtualAddress;
        public uint SizeOfRawData;
        public uint PointerToRawData;
        public uint PointerToRelocations;
        public uint PointerToLinenumbers;
        public ushort NumberOfRelocations;
        public ushort NumberOfLinenumbers;
        public SectionFlags Characteristics;
    };
}
