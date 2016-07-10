using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
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

        IEnumerable<ExportedSymbol> _exports;

        public unsafe IEnumerable<ExportedSymbol> Exports {
            get {
                if(_exports == null) {
                    var header = MainViewModel.PEHeader;
                    _exports = PEHelper.GetExports(header, MainViewModel.Accessor);
                }
                return _exports;
            }
        }

        private string _searchText;

        public string SearchText {
            get { return _searchText; }
            set {
                if(SetProperty(ref _searchText, value)) {
                    var view = CollectionViewSource.GetDefaultView(Exports);
                    if(string.IsNullOrWhiteSpace(value))
                        view.Filter = null;
                    else {
                        var lower = value.ToLower();
                        view.Filter = o => {
                            var symbol = (ExportedSymbol)o;
                            return symbol.Name.ToLower().Contains(lower) || (symbol.ForwardName != null && symbol.ForwardName.ToLower().Contains(lower));
                        };
                    }
                }
            }
        }

    }
}
