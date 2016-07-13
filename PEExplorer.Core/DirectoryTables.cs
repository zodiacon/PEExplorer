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

}
