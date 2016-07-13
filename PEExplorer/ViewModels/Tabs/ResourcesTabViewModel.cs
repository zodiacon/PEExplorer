using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExplorer.ViewModels.Tabs {
    [Export]
    class ResourcesTabViewModel : TabViewModelBase {
        [ImportingConstructor]
        public ResourcesTabViewModel(MainViewModel vm) : base(vm) {
        }

        public override string Icon => "/icons/resources.ico";

        public override string Text => "Resources";
    }
}
