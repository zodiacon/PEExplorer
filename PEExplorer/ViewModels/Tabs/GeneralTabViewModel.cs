using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Microsoft.Diagnostics.Runtime.Utilities;
using PEExplorer.Core;

namespace PEExplorer.ViewModels.Tabs {
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    class GeneralTabViewModel : TabViewModelBase {
        [ImportingConstructor]
        public GeneralTabViewModel(MainViewModel vm) : base(vm) {
        }

        public override bool CanClose => false;

        public override string Icon => "/icons/General.ico";

        public override string Text => "General";

        public string FullPathName => MainViewModel.PathName;

        IEnumerable<PEHeaderProperty> _headerProperties;

        public IEnumerable<PEHeaderProperty> HeaderProperties {
            get {
                if(_headerProperties == null) {
                    var header = MainViewModel.PEHeader;
                    _headerProperties = new List<PEHeaderProperty> {
                        new PEHeaderProperty { Name = "Base of Code", Value = ToDecHex(header.BaseOfCode) },
                        new PEHeaderProperty { Name = "Address of Entry Point", Value = ToDecHex(header.AddressOfEntryPoint) },
                        new PEHeaderProperty { Name = "Is Managed?", Value = header.IsManaged.ToString() },
                        new PEHeaderProperty { Name = "Machine", Value = header.Machine.ToString() },
                        new PEHeaderProperty { Name = "Magic", Value = ToDecHex(header.Magic), Info = MagicToPEFormat(header.Magic) },
                        new PEHeaderProperty { Name = "Major Image Version", Value = header.MajorImageVersion.ToString() },
                        new PEHeaderProperty { Name = "Minor Image Version", Value = header.MinorImageVersion.ToString() },
                        new PEHeaderProperty { Name = "Major Linker Version", Value = header.MajorLinkerVersion.ToString() },
                        new PEHeaderProperty { Name = "Minor Linker Version", Value = header.MinorLinkerVersion.ToString() },
                        new PEHeaderProperty { Name = "Loader Flags", Value = ToDecHex(header.LoaderFlags) },
                        new PEHeaderProperty { Name = "Subsystem", Value = header.Subsystem.ToString(), Info = ((SubsystemType)header.Subsystem).ToString() },
                        new PEHeaderProperty { Name = "DLL Characteristics", Value = ToDecHex(header.DllCharacteristics), Info = ((DllCharacteristics)header.DllCharacteristics).ToString() },
                        new PEHeaderProperty { Name = "File Alignment", Value = ToDecHex(header.FileAlignment) },
                        new PEHeaderProperty { Name = "Size of Code", Value = ToDecHex(header.SizeOfCode) },
                        new PEHeaderProperty { Name = "Size of Image", Value = ToDecHex(header.SizeOfImage) },
                        new PEHeaderProperty { Name = "Major OS Version", Value = header.MajorOperatingSystemVersion.ToString() },
                        new PEHeaderProperty { Name = "Minor OS Version", Value = header.MinorOperatingSystemVersion.ToString() },
                        new PEHeaderProperty { Name = "Major Subsystem Version", Value = header.MajorSubsystemVersion.ToString() },
                        new PEHeaderProperty { Name = "Minor Subsystem Version", Value = header.MinorSubsystemVersion.ToString() },
                        new PEHeaderProperty { Name = "Size of Headers", Value = ToDecHex(header.SizeOfHeaders) },
                        new PEHeaderProperty { Name = "Size of Heap Commit", Value = ToDecHex(header.SizeOfHeapCommit) },
                        new PEHeaderProperty { Name = "Size of Heap Reserve", Value = ToDecHex(header.SizeOfHeapReserve) },
                        new PEHeaderProperty { Name = "Size of Uninitialized Data", Value = ToDecHex(header.SizeOfUninitializedData) },
                        new PEHeaderProperty { Name = "Size of Initialized Data", Value = ToDecHex(header.SizeOfInitializedData) },
                        new PEHeaderProperty { Name = "Size of Optional Header", Value = ToDecHex(header.SizeOfOptionalHeader) },
                        new PEHeaderProperty { Name = "Date Time Stamp", Value = ToDecHex((ulong)header.TimeDateStampSec), Info = header.TimeDateStamp.ToString() },
                        new PEHeaderProperty { Name = "Section Alignment", Value = ToDecHex(header.SectionAlignment) },
                        new PEHeaderProperty { Name = "Pointer to Symbol Table", Value = ToDecHex(header.PointerToSymbolTable) },
                        new PEHeaderProperty { Name = "Number of Sections", Value = header.NumberOfSections.ToString() },
                        new PEHeaderProperty { Name = "Number of Symbols", Value = header.NumberOfSymbols.ToString() },
                        new PEHeaderProperty { Name = "Number of RVA and Sizes", Value = header.NumberOfRvaAndSizes.ToString() },
                        new PEHeaderProperty { Name = "Signature", Value = ToDecHex(header.Signature) },
                        new PEHeaderProperty { Name = "Checksum", Value = ToDecHex(header.CheckSum) },
                        header.ImportAddressTableDirectory.Size == 0 ? null : new PEHeaderProperty { Name = "Import Address Table Directory", Value = FromDirectory(header.ImportAddressTableDirectory) },
                        header.ImportDirectory.Size == 0 ? null : new PEHeaderProperty { Name = "Import Directory", Value = FromDirectory(header.ImportDirectory) },
                        header.ResourceDirectory.Size == 0 ? null : new PEHeaderProperty { Name = "Resource Directory", Value = FromDirectory(header.ResourceDirectory) },
                        header.BaseRelocationDirectory.Size == 0 ? null : new PEHeaderProperty { Name = "Base Relocation Directory", Value = FromDirectory(header.BaseRelocationDirectory) },
                        header.BoundImportDirectory.Size == 0 ? null : new PEHeaderProperty { Name = "Bound Import Directory", Value = FromDirectory(header.BoundImportDirectory) },
                        header.ExceptionDirectory.Size == 0 ? null : new PEHeaderProperty { Name = "Exception Directory", Value = FromDirectory(header.ExceptionDirectory) },
                        header.ExportDirectory.Size == 0 ? null : new PEHeaderProperty { Name = "Export Directory", Value = FromDirectory(header.ExportDirectory) },
                        header.LoadConfigurationDirectory.Size == 0 ? null : new PEHeaderProperty { Name = "Load Configuration Directory", Value = FromDirectory(header.LoadConfigurationDirectory) },
                        new PEHeaderProperty { Name = "Global Pointer", Value = ToDecHex((uint)header.GlobalPointerDirectory.VirtualAddress) },
                        header.ComDescriptorDirectory.Size == 0 ? null : new PEHeaderProperty { Name = "CLR Descriptor Directory", Value = FromDirectory(header.ComDescriptorDirectory) },
                        header.DebugDirectory.Size == 0 ? null : new PEHeaderProperty { Name = "Debug Directory", Value = FromDirectory(header.DebugDirectory) },
                        header.DelayImportDirectory.Size == 0 ? null : new PEHeaderProperty { Name = "Delay Import Directory", Value = FromDirectory(header.DelayImportDirectory) },
                        header.ArchitectureDirectory.Size == 0 ? null : new PEHeaderProperty { Name = "Architecture Directory", Value = FromDirectory(header.ArchitectureDirectory) },
                        header.ThreadStorageDirectory.Size == 0 ? null : new PEHeaderProperty { Name = "Thread Storage Directory", Value = FromDirectory(header.ThreadStorageDirectory) },
                        header.CertificatesDirectory.Size == 0 ? null : new PEHeaderProperty { Name = "Certificate Directory", Value = FromDirectory(header.CertificatesDirectory) },
                    }.Where(p => p != null).OrderBy(p => p.Name);
                }
                return _headerProperties;
            }
        }

        private string FromDirectory(IMAGE_DATA_DIRECTORY dir) {
            return $"Offset: {ToDecHex((uint)dir.VirtualAddress)}, Size:{ToDecHex((uint)dir.Size)}";
        }

        private string MagicToPEFormat(ushort magic) {
            switch(magic) {
                case 0x10b: return "PE32";
                case 0x20b: return "PE32+";
                case 0x107: return "ROM";
            }
            return "Unknown";
        }

        private string ToDecHex(ulong n) {
            return $"{n} (0x{n:X})";
        }

        private string _searchText;

        public string SearchText {
            get { return _searchText; }
            set { if(SetProperty(ref _searchText, value)) {
                    var view = CollectionViewSource.GetDefaultView(HeaderProperties);
                    if(string.IsNullOrWhiteSpace(value))
                        view.Filter = null;
                    else {
                        var name = value.ToLower();
                        view.Filter = o => ((PEHeaderProperty)o).Name.ToLower().Contains(name);
                    }
                }
            }
        }

    }
}
