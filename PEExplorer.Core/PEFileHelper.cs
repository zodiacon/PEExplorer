using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Diagnostics.Runtime.Utilities;

namespace PEExplorer.Core {
    public class PEFileHelper {
        public readonly PEHeader Header;
        readonly MemoryMappedViewAccessor _accessor;

        List<ExportedSymbol> _exports;
        List<ImportedLibrary> _imports;

        public PEFileHelper(PEHeader header, MemoryMappedViewAccessor accessor) {
            Header = header;
            _accessor = accessor;
        }

        public unsafe ICollection<ImportedLibrary> GetImports() {
            if(_imports == null) {

                var dir = Header.ImportDirectory;
                var offset = Header.RvaToFileOffset(dir.VirtualAddress);
                var pe64 = Header.IsPE64;
                var size = pe64 ? 8 : 4;
                var imports = new List<ImportedLibrary>();

                for(;;) {
                    IMAGE_IMPORT_DIRECTORY importDirectory;
                    _accessor.Read(offset, out importDirectory);
                    if(importDirectory.ImportLookupTable == 0)
                        break;

                    ImportedLibrary library = null;
                    var importLookupTable = Header.RvaToFileOffset(importDirectory.ImportLookupTable);
                    var hintNameTable = Header.RvaToFileOffset(importDirectory.ImportAddressTable);
                    var nameOffset = Header.RvaToFileOffset(importDirectory.NameRva);

                    var pointer = importLookupTable;
                    for(;;) {
                        var ordinal = -1;
                        var nameRva = 0;
                        if(pe64) {
                            ulong lvalue = _accessor.ReadUInt64(pointer);
                            if(lvalue == 0) break;

                            var isOrdinal = (lvalue & (1UL << 63)) > 0;
                            if(isOrdinal)
                                ordinal = (ushort)(lvalue & 0xffff);
                            else
                                nameRva = (int)(lvalue & ((1L << 31) - 1));
                        }
                        else {
                            uint ivalue = _accessor.ReadUInt32(pointer);
                            if(ivalue == 0) break;
                            if((ivalue & 0x80000000) > 0)
                                ordinal = (ushort)(ivalue & 0xffff);
                            else
                                nameRva = (int)(ivalue & ((1L << 31) - 1));
                        }

                        if(library == null) {
                            var bytes = new sbyte[64];
                            fixed (sbyte* p = bytes) {
                                _accessor.ReadArray(nameOffset, bytes, 0, bytes.Length);
                                library = new ImportedLibrary {
                                    LibraryName = new string(p)
                                };
                            }
                        }

                        if(nameRva > 0) {
                            var offset2 = Header.RvaToFileOffset(nameRva);
                            var hint = _accessor.ReadUInt16(offset2);
                            var chars = new List<byte>();
                            for(;;) {
                                var ch = _accessor.ReadByte(offset2 + 2 + chars.Count);
                                if(ch == 0) {
                                    library.Symbols.Add(new ImportedSymbol {
                                        Name = Encoding.ASCII.GetString(chars.ToArray()),
                                        Hint = hint,
                                    });
                                    break;
                                }
                                chars.Add(ch);
                            };
                        }

                        pointer += size;
                    }
                    imports.Add(library);
                    library = null;

                    offset += 20;
                }

                _imports = imports;
            }
            return _imports;
        }

        public unsafe ICollection<ExportedSymbol> GetExports() {
            if(_exports == null) {
                var dir = Header.ExportDirectory;
                var offset = Header.RvaToFileOffset(dir.VirtualAddress);

                IMAGE_EXPORT_DIRECTORY exportDirectory;
                _accessor.Read(offset, out exportDirectory);

                var count = exportDirectory.NumberOfNames;
                var list = new List<ExportedSymbol>(count);

                var namesOffset = Header.RvaToFileOffset(exportDirectory.AddressOfNames);
                var ordinalOffset = Header.RvaToFileOffset(exportDirectory.AddressOfOrdinals);
                var functionsOffset = Header.RvaToFileOffset((int)exportDirectory.AddressOfFunctions);

                var ordinalBase = (int)exportDirectory.Base;

                var name = new sbyte[64];
                fixed (sbyte* p = name) {
                    for(uint i = 0; i < count; i++) {

                        //read name

                        var offset2 = _accessor.ReadUInt32(namesOffset + i * 4);
                        var offset3 = Header.RvaToFileOffset((int)offset2);
                        _accessor.ReadArray(offset3, name, 0, name.Length);
                        var functionName = new string(p);

                        // read ordinal

                        int ordinal = _accessor.ReadUInt16(ordinalOffset + i * 2) + ordinalBase;

                        // read function address

                        string forwarder = null;
                        var address = _accessor.ReadUInt32(functionsOffset + i * 4);
                        var fileAddress = Header.RvaToFileOffset((int)address);
                        if(fileAddress > dir.VirtualAddress && fileAddress < dir.VirtualAddress + dir.Size) {
                            // forwarder
                            _accessor.ReadArray(Header.RvaToFileOffset((int)address), name, 0, name.Length);
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
                _exports = list;
            }

            return _exports;
        }
    }
}
