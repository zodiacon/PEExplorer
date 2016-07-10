using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Diagnostics.Runtime.Utilities;

namespace PEExplorer.Core {
    public static class PEHelper {
        public unsafe static ICollection<ExportedSymbol> GetExports(PEHeader header, MemoryMappedViewAccessor accessor) {
            var dir = header.ExportDirectory;
            var offset = header.RvaToFileOffset(dir.VirtualAddress);

            IMAGE_EXPORT_DIRECTORY exportDirectory;
            accessor.Read(offset, out exportDirectory);

            var count = exportDirectory.NumberOfNames;
            var list = new List<ExportedSymbol>((int)count);

            var namesOffset = header.RvaToFileOffset((int)exportDirectory.AddressOfNames);
            var ordinalOffset = header.RvaToFileOffset((int)exportDirectory.AddressOfOrdinals);
            var functionsOffset = header.RvaToFileOffset((int)exportDirectory.AddressOfFunctions);

            var ordinalBase = (int)exportDirectory.Base;

            var name = new sbyte[64];
            fixed (sbyte* p = name) {
                for(uint i = 0; i < count; i++) {
                    
                    //read name

                    var offset2 = accessor.ReadUInt32(namesOffset + i * 4);
                    var offset3 = header.RvaToFileOffset((int)offset2);
                    accessor.ReadArray(offset3, name, 0, name.Length);
                    var functionName = new string(p);

                    // read ordinal

                    int ordinal = accessor.ReadUInt16(ordinalOffset + i * 2) + ordinalBase;

                    // read function address

                    string forwarder = null;
                    var address = accessor.ReadUInt32(functionsOffset + i * 4);
                    var fileAddress = header.RvaToFileOffset((int)address);
                    if(fileAddress > offset) {
                        // forwarder
                        accessor.ReadArray(header.RvaToFileOffset((int)address), name, 0, name.Length);
                        forwarder = new string(p);
                    }

                    list.Add(new ExportedSymbol {
                        Name = functionName,
                        Ordinal = ordinal,
                        Address = address,
                        ForwardName = forwarder
                    });
                }
            }

            return list;
        }
    }
}
