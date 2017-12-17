using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Zodiacon.PEParsing;

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

        IEnumerable<GenericProperty> _headerProperties;

        public IEnumerable<GenericProperty> HeaderProperties {
            get {
                if(_headerProperties == null) {
                    var header = MainViewModel.PEHeader;
                    var fileHeader = MainViewModel.PEParser.FileHeader;

                    _headerProperties = new List<GenericProperty> {
                        new GenericProperty { Name = "Base of Code", Value = ToDecHex(header.BaseOfCode) },
                        new GenericProperty { Name = "Address of Entry Point", Value = ToDecHex(header.AddressOfEntryPoint) },
                        new GenericProperty { Name = "Image Base", Value = ToDecHex(header.ImageBase) },
                        new GenericProperty { Name = "Is Managed?", Value = header.IsManaged.ToString() },
                        new GenericProperty { Name = "Machine", Value = ((ushort)fileHeader.Machine).ToString(), Info = fileHeader.Machine.ToString() },
                        new GenericProperty { Name = "Magic", Value = ToDecHex((ushort)header.Magic), Info = MagicToPEFormat((ushort)header.Magic) },
                        new GenericProperty { Name = "Major Image Version", Value = header.MajorImageVersion.ToString() },
                        new GenericProperty { Name = "Minor Image Version", Value = header.MinorImageVersion.ToString() },
                        new GenericProperty { Name = "Major Linker Version", Value = header.MajorLinkerVersion.ToString() },
                        new GenericProperty { Name = "Minor Linker Version", Value = header.MinorLinkerVersion.ToString() },
                        new GenericProperty { Name = "Loader Flags", Value = ToDecHex(header.LoaderFlags) },
                        new GenericProperty { Name = "Subsystem", Value = header.Subsystem.ToString(), Info = ((SubsystemType)header.Subsystem).ToString() },
                        new GenericProperty { Name = "Characteristics", Value = ToDecHex((ulong)fileHeader.Characteristics), Info = fileHeader.Characteristics.ToString() },
                        new GenericProperty { Name = "Dll Characteristics", Value = ToDecHex((ulong)header.DllCharacteristics), Info = header.DllCharacteristics.ToString() },
                        new GenericProperty { Name = "File Alignment", Value = ToDecHex(header.FileAlignment) },
                        new GenericProperty { Name = "Size of Code", Value = ToDecHex(header.SizeOfCode) },
                        new GenericProperty { Name = "Size of Image", Value = ToDecHex(header.SizeOfImage) },
                        new GenericProperty { Name = "Major OS Version", Value = header.MajorOperatingSystemVersion.ToString() },
                        new GenericProperty { Name = "Minor OS Version", Value = header.MinorOperatingSystemVersion.ToString() },
                        new GenericProperty { Name = "Major Subsystem Version", Value = header.MajorSubsystemVersion.ToString() },
                        new GenericProperty { Name = "Minor Subsystem Version", Value = header.MinorSubsystemVersion.ToString() },
                        new GenericProperty { Name = "Size of Headers", Value = ToDecHex(header.SizeOfHeaders) },
                        new GenericProperty { Name = "Size of Stack Commit", Value = ToDecHex(header.SizeOfStackCommit) },
                        new GenericProperty { Name = "Size of Stack Reserve", Value = ToDecHex(header.SizeOfStackReserve) },
                        new GenericProperty { Name = "Size of Heap Commit", Value = ToDecHex(header.SizeOfHeapCommit) },
                        new GenericProperty { Name = "Size of Heap Reserve", Value = ToDecHex(header.SizeOfHeapReserve) },
                        new GenericProperty { Name = "Size of Uninitialized Data", Value = ToDecHex(header.SizeOfUninitializedData) },
                        new GenericProperty { Name = "Size of Initialized Data", Value = ToDecHex(header.SizeOfInitializedData) },
                        new GenericProperty { Name = "Size of Optional Header", Value = ToDecHex(fileHeader.SizeOfOptionalHeader) },
                        new GenericProperty { Name = "Date Time Stamp", Value = ToDecHex((ulong)fileHeader.TimeDateStamp), Info = fileHeader.TimeDateStamp.ToString() },
                        new GenericProperty { Name = "Section Alignment", Value = ToDecHex(header.SectionAlignment) },
                        new GenericProperty { Name = "Pointer to Symbol Table", Value = ToDecHex(fileHeader.PointerToSymbolTable) },
                        new GenericProperty { Name = "Number of Sections", Value = fileHeader.NumberOfSections.ToString() },
                        new GenericProperty { Name = "Number of Symbols", Value = fileHeader.NumberOfSymbols.ToString() },
                        new GenericProperty { Name = "Number of RVA and Sizes", Value = header.NumberOfRvaAndSizes.ToString() },
                        new GenericProperty { Name = "Signature", Value = ToDecHex(MainViewModel.PEParser.Signature) },
                        new GenericProperty { Name = "Checksum", Value = ToDecHex(header.CheckSum) },
                        header.ImportAddressTableDirectory.Size == 0 ? null : new GenericProperty { Name = "Import Address Table Directory", Value = FromDirectory(header.ImportAddressTableDirectory) },
                        header.ImportDirectory.Size == 0 ? null : new GenericProperty { Name = "Import Directory", Value = FromDirectory(header.ImportDirectory) },
                        header.ResourceDirectory.Size == 0 ? null : new GenericProperty { Name = "Resource Directory", Value = FromDirectory(header.ResourceDirectory) },
                        header.BaseRelocationDirectory.Size == 0 ? null : new GenericProperty { Name = "Base Relocation Directory", Value = FromDirectory(header.BaseRelocationDirectory) },
                        header.BoundImportDirectory.Size == 0 ? null : new GenericProperty { Name = "Bound Import Directory", Value = FromDirectory(header.BoundImportDirectory) },
                        header.ExceptionDirectory.Size == 0 ? null : new GenericProperty { Name = "Exception Directory", Value = FromDirectory(header.ExceptionDirectory) },
                        header.ExportDirectory.Size == 0 ? null : new GenericProperty { Name = "Export Directory", Value = FromDirectory(header.ExportDirectory) },
                        header.LoadConfigurationDirectory.Size == 0 ? null : new GenericProperty { Name = "Load Configuration Directory", Value = FromDirectory(header.LoadConfigurationDirectory) },
                        new GenericProperty { Name = "Global Pointer", Value = ToDecHex((uint)header.GlobalPointerDirectory.VirtualAddress) },
                        header.ComDescriptorDirectory.Size == 0 ? null : new GenericProperty { Name = "CLR Descriptor Directory", Value = FromDirectory(header.ComDescriptorDirectory) },
                        header.DebugDirectory.Size == 0 ? null : new GenericProperty { Name = "Debug Directory", Value = FromDirectory(header.DebugDirectory) },
                        header.DelayImportDirectory.Size == 0 ? null : new GenericProperty { Name = "Delay Import Directory", Value = FromDirectory(header.DelayImportDirectory) },
                        header.ArchitectureDirectory.Size == 0 ? null : new GenericProperty { Name = "Architecture Directory", Value = FromDirectory(header.ArchitectureDirectory) },
                        header.ThreadLocalStorageDirectory.Size == 0 ? null : new GenericProperty { Name = "Thread Storage Directory", Value = FromDirectory(header.ThreadLocalStorageDirectory) },
                        header.CertificatesDirectory.Size == 0 ? null : new GenericProperty { Name = "Certificate Directory", Value = FromDirectory(header.CertificatesDirectory) },
                    }.Where(p => p != null).OrderBy(p => p.Name);
                }
                return _headerProperties;
            }
        }

        private string FromDirectory(DataDirectory dir) {
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

        private string _searchText;

        public string SearchText {
            get { return _searchText; }
            set { if(SetProperty(ref _searchText, value)) {
                    var view = CollectionViewSource.GetDefaultView(HeaderProperties);
                    if(string.IsNullOrWhiteSpace(value))
                        view.Filter = null;
                    else {
                        var name = value.ToLower();
                        view.Filter = o => ((GenericProperty)o).Name.ToLower().Contains(name);
                    }
                }
            }
        }

    }
}
