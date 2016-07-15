using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExplorer.ViewModels.Tabs {
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    class ExceptionsTabViewModel : TabViewModelBase {
        [ImportingConstructor]
        public ExceptionsTabViewModel(MainViewModel vm) : base(vm) {
        }

        public override string Icon => "/icons/exceptions.ico";

        public override string Text => "Exceptions";
    }
}
