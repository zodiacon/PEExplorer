using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEExplorer.Core;

namespace PEExplorer.ViewModels.Tabs {
    [Export]
    class ImportsTabViewModel : TabViewModelBase {
        [ImportingConstructor]
        public ImportsTabViewModel(MainViewModel vm) : base(vm) {
        }

        public override string Icon => "/icons/import2.ico";

        public override string Text => "Imports";

        IEnumerable<ImportedLibrary> _imports;
        public IEnumerable<ImportedLibrary> Imports => _imports ?? (_imports = MainViewModel.PEFile.GetImports());        
    }
}
