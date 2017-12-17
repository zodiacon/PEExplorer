using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Diagnostics.Runtime.Utilities;
using System.Diagnostics;
using System.IO;

namespace PEExplorer.Core {
    public unsafe sealed class PEParser : IDisposable {
        readonly MemoryMappedViewAccessor _accessor;
        readonly MemoryMappedFile _memFile;
        void* _address;

        public string FileName { get; }

        public PEParser(void* address) {
            _address = address;
        }

        public PEParser(string filename) {
            _memFile = MemoryMappedFile.CreateFromFile(filename, FileMode.Open, null, 0, MemoryMappedFileAccess.Read);
            _accessor = _memFile.CreateViewAccessor(0, 0, MemoryMappedFileAccess.Read);
            FileName = filename;
        }

        T ReadData<T>(int offset) where T : struct {
            if (_address == null) {
                _accessor.Read<T>(offset, out var data);
                return data;
            }
            else {
                var data = new T();
                var p = (byte*)_address + offset;
                var ptr = new IntPtr(p);
                Marshal.StructureToPtr<T>(data, ptr, false);
                Buffer.MemoryCopy(p, ptr.ToPointer(), Marshal.SizeOf<T>(), Marshal.SizeOf<T>());
                return data;
            }
        }

        ImportedSymbol GetSymbolFromImport(int pointer) {
            bool pe64 = Header.IsPE64;
            var ordinal = -1;
            var nameRva = 0;
            if (pe64) {
                var lvalue = _accessor.ReadUInt64(pointer);
                if (lvalue == 0) return null;

                var isOrdinal = (lvalue & (1UL << 63)) > 0;
                if (isOrdinal)
                    ordinal = (ushort)(lvalue & 0xffff);
                else
                    nameRva = (int)(lvalue & ((1L << 31) - 1));
            }
            else {
                var ivalue = _accessor.ReadUInt32(pointer);
                if (ivalue == 0) return null;
                if ((ivalue & 0x80000000) > 0)
                    ordinal = (ushort)(ivalue & 0xffff);
                else
                    nameRva = (int)(ivalue & ((1L << 31) - 1));
            }

            if (nameRva > 0) {
                var offset2 = Header.RvaToFileOffset(nameRva);
                var hint = _accessor.ReadUInt16(offset2);
                var chars = new List<byte>();
                for (; ; ) {
                    var ch = _accessor.ReadByte(offset2 + 2 + chars.Count);
                    if (ch == 0) {
                        var symbol = new ImportedSymbol {
                            Name = Encoding.ASCII.GetString(chars.ToArray()),
                            Hint = hint,
                        };
                        if (symbol.Name.Contains("@@"))
                            symbol.UndecoratedName = GetUndecoratedName(symbol.Name);
                        return symbol;
                    }
                    chars.Add(ch);
                };
            }
            return null;
        }


        public ICollection<ImportedSymbol> GetImportAddressTable() {
            var dir = Header.ImportAddressTableDirectory;
            var offset = Header.RvaToFileOffset(dir.VirtualAddress);
            var pe64 = Header.IsPE64;
            var size = pe64 ? 8 : 4;
            var symbols = new List<ImportedSymbol>(16);

            var pointer = offset;
            for (; ; ) {
                var symbol = GetSymbolFromImport(pointer);
                if (symbol == null)
                    break;

                symbols.Add(symbol);
                pointer += size;
            }

            return symbols;
        }

        public unsafe ICollection<ImportedLibrary> GetImports() {
            var dir = Header.ImportDirectory;
            if (dir.Size == 0)
                return null;

            var offset = Header.RvaToFileOffset(dir.VirtualAddress);
            var pe64 = Header.IsPE64;
            var size = pe64 ? 8 : 4;
            var imports = new List<ImportedLibrary>(8);

            for (; ; ) {
                IMAGE_IMPORT_DIRECTORY importDirectory;
                _accessor.Read(offset, out importDirectory);
                if (importDirectory.ImportLookupTable == 0)
                    importDirectory.ImportLookupTable = importDirectory.ImportAddressTable;
                if (importDirectory.ImportLookupTable == 0)
                    break;

                ImportedLibrary library = null;
                var importLookupTable = Header.RvaToFileOffset(importDirectory.ImportLookupTable);
                var hintNameTable = Header.RvaToFileOffset(importDirectory.ImportAddressTable);
                var nameOffset = Header.RvaToFileOffset(importDirectory.NameRva);

                var pointer = importLookupTable;
                for (; ; ) {
                    var ordinal = -1;
                    var nameRva = 0;
                    if (pe64) {
                        var lvalue = _accessor.ReadUInt64(pointer);
                        if (lvalue == 0) break;

                        var isOrdinal = (lvalue & (1UL << 63)) > 0;
                        if (isOrdinal)
                            ordinal = (ushort)(lvalue & 0xffff);
                        else
                            nameRva = (int)(lvalue & ((1L << 31) - 1));
                    }
                    else {
                        var ivalue = _accessor.ReadUInt32(pointer);
                        if (ivalue == 0) break;
                        if ((ivalue & 0x80000000) > 0)
                            ordinal = (ushort)(ivalue & 0xffff);
                        else
                            nameRva = (int)(ivalue & ((1L << 31) - 1));
                    }

                    if (library == null) {
                        var bytes = new sbyte[128];
                        fixed (sbyte* p = bytes) {
                            _accessor.ReadArray(nameOffset, bytes, 0, bytes.Length);
                            library = new ImportedLibrary {
                                LibraryName = new string(p)
                            };
                        }
                    }

                    if (nameRva > 0) {
                        var offset2 = Header.RvaToFileOffset(nameRva);
                        var hint = _accessor.ReadUInt16(offset2);
                        var chars = new List<byte>();
                        for (; ; ) {
                            var ch = _accessor.ReadByte(offset2 + 2 + chars.Count);
                            if (ch == 0) {
                                var symbol = new ImportedSymbol {
                                    Name = Encoding.ASCII.GetString(chars.ToArray()),
                                    Hint = hint,
                                };
                                if (symbol.Name.Contains("@@"))
                                    symbol.UndecoratedName = GetUndecoratedName(symbol.Name);
                                library.Symbols.Add(symbol);
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

            return imports;
        }

        [DllImport("dbghelp.dll", CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
        internal static extern uint UnDecorateSymbolName(string name, StringBuilder undecoratedName, int length, uint flags);

        public static string GetUndecoratedName(string name, uint flags = 0) {
            var sb = new StringBuilder(128);
            if (UnDecorateSymbolName(name, sb, sb.Capacity, flags) == 0)
                return null;
            return sb.ToString();
        }

        public unsafe ICollection<ExportedSymbol> GetExports() {
            var dir = Header.ExportDirectory;
            var offset = Header.RvaToFileOffset(dir.VirtualAddress);

            IMAGE_EXPORT_DIRECTORY exportDirectory;
            _accessor.Read(offset, out exportDirectory);

            var count = exportDirectory.NumberOfNames;
            var exports = new List<ExportedSymbol>(count);

            var namesOffset = exportDirectory.AddressOfNames != 0 ? Header.RvaToFileOffset(exportDirectory.AddressOfNames) : 0;
            var ordinalOffset = exportDirectory.AddressOfOrdinals != 0 ? Header.RvaToFileOffset(exportDirectory.AddressOfOrdinals) : 0;
            var functionsOffset = Header.RvaToFileOffset(exportDirectory.AddressOfFunctions);

            var ordinalBase = exportDirectory.Base;

            var name = new sbyte[64];
            fixed (sbyte* p = name) {
                for (uint i = 0; i < count; i++) {

                    //read name

                    var offset2 = _accessor.ReadUInt32(namesOffset + i * 4);
                    var offset3 = Header.RvaToFileOffset((int)offset2);
                    _accessor.ReadArray(offset3, name, 0, name.Length);
                    var functionName = new string(p);

                    // read ordinal

                    var ordinal = _accessor.ReadUInt16(ordinalOffset + i * 2) + ordinalBase;

                    // read function address

                    string forwarder = null;
                    var address = _accessor.ReadUInt32(functionsOffset + i * 4);
                    var fileAddress = Header.RvaToFileOffset((int)address);
                    if (fileAddress > dir.VirtualAddress && fileAddress < dir.VirtualAddress + dir.Size) {
                        // forwarder
                        _accessor.ReadArray(Header.RvaToFileOffset((int)address), name, 0, name.Length);
                        forwarder = new string(p);
                    }

                    exports.Add(new ExportedSymbol {
                        Name = functionName,
                        Ordinal = ordinal,
                        Address = address,
                        ForwardName = forwarder,
                        UndecoratedName = forwarder == null ? GetUndecoratedName(functionName) : string.Empty
                    });
                }
            }

            return exports;
        }

        internal IMAGE_LOAD_CONFIG_DIRECTORY32 GetLoadConfigDirectory32() {
            var dir = Header.LoadConfigurationDirectory;
            var offset = Header.RvaToFileOffset(dir.VirtualAddress);

            IMAGE_LOAD_CONFIG_DIRECTORY32 configDirectory;
            _accessor.Read(offset, out configDirectory);

            return configDirectory;
        }

        internal IMAGE_LOAD_CONFIG_DIRECTORY64 GetLoadConfigDirectory64() {
            var dir = Header.LoadConfigurationDirectory;
            var offset = Header.RvaToFileOffset(dir.VirtualAddress);

            IMAGE_LOAD_CONFIG_DIRECTORY64 configDirectory;
            _accessor.Read(offset, out configDirectory);

            return configDirectory;
        }

        public LoadConfiguration GetLoadConfiguration() {
            LoadConfiguration loadConfig;
            if (Environment.Is64BitProcess) {
                var config64 = GetLoadConfigDirectory64();
                loadConfig = new LoadConfiguration(this, ref config64);
            }
            else {
                var config32 = GetLoadConfigDirectory32();
                loadConfig = new LoadConfiguration(this, ref config32);
            }


            return loadConfig;
        }

        public ICollection<SectionData> GetSectionHeaders() {
            var sections = new List<SectionData>();
            var sectionHeaderSize = Marshal.SizeOf<SectionHeader>();
            Debug.Assert(sectionHeaderSize == 40);

            var offset = Header.PEHeaderSize - Header.NumberOfSections * sectionHeaderSize;
            for (int i = 0; i < Header.NumberOfSections; i++) {
                _accessor.Read(offset, out SectionHeader header);
                sections.Add(new SectionData(header));
                offset += sectionHeaderSize;
            }
            return sections;
        }

        public void Dispose() {
            if (_isOwner) {
                _accessor.Dispose();
                _memFile.Dispose();
                _stm.Dispose();
            }
        }
    }
}
