using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using PEExplorer.Core;

namespace PEExplorer.ViewModels.Tabs {
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    class ImportsTabViewModel : TabViewModelBase {
        [ImportingConstructor]
        public ImportsTabViewModel(MainViewModel vm) : base(vm) {
        }

        public override string Icon => "/icons/import2.ico";

        public override string Text => "Imports";

        IEnumerable<ImportedLibrary> _imports;
        public IEnumerable<ImportedLibrary> Imports => _imports ?? (_imports = MainViewModel.PEFile.GetImports());

        private string _searchLibrariesText;

        public string SearchLibrariesText {
            get { return _searchLibrariesText; }
            set {
                if(SetProperty(ref _searchLibrariesText, value)) {
                    var view = CollectionViewSource.GetDefaultView(Imports);
                    if(string.IsNullOrWhiteSpace(value))
                        view.Filter = null;
                    else {
                        var text = value.ToLower();
                        view.Filter = o => ((ImportedLibrary)o).LibraryName.ToLower().Contains(text);
                    }
                }
            }
        }

        private ImportedLibrary _selectedLibrary;

        public ImportedLibrary SelectedLibrary {
            get { return _selectedLibrary; }
            set {
                if(SetProperty(ref _selectedLibrary, value)) {
                    SearchImportsText = string.Empty;
                }
            }
        }

        private string _searchImportsText;

        public string SearchImportsText {
            get { return _searchImportsText; }
            set {
                if(SetProperty(ref _searchImportsText, value)) {
                    if(SelectedLibrary != null) {
                        var view = CollectionViewSource.GetDefaultView(SelectedLibrary.Symbols);
                        if(string.IsNullOrWhiteSpace(value))
                            view.Filter = null;
                        else {
                            var text = value.ToLower();
                            view.Filter = o => ((ImportedSymbol)o).Name.ToLower().Contains(text);
                        }
                    }
                }
            }
        }

    }
}
