using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.PEParsing;

namespace PEExplorer.ViewModels.Tabs {
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    class ImportAddressTableTabViewModel : TabViewModelBase {
        [ImportingConstructor]
        public ImportAddressTableTabViewModel(MainViewModel vm) : base(vm) {
        }

        public override string Icon => "/icons/iat.ico";

        public override string Text => "IAT";

        ICollection<ImportedSymbol> _imports;

        public ICollection<ImportedSymbol> Imports => _imports ?? (_imports = MainViewModel.Parser.GetImportAddressTable());
    }
}
