using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PEExplorer.Core;

namespace PEExplorer.ViewModels.Tabs {
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    class ExportsTabViewModel : TabViewModelBase {
        [ImportingConstructor]
        public ExportsTabViewModel(MainViewModel vm) : base(vm) {
            var temp = Exports;
        }

        public override string Icon => "/icons/export1.ico";

        public override string Text => "Exports";

        IEnumerable<ExportItemViewModel> _exports;

        public unsafe IEnumerable<ExportItemViewModel> Exports {
            get {
                if(_exports == null) {
                    var header = MainViewModel.PEHeader;
                    var dir = header.ExportDirectory;
                    var offset = header.RvaToFileOffset(dir.VirtualAddress);

                    IMAGE_EXPORT_DIRECTORY exportDirectory;
                    MainViewModel.Accessor.Read(offset, out exportDirectory);
                }
                return _exports;
            }
        }
    }
}
