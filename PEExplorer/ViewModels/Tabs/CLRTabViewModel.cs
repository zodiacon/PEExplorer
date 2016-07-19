using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PEExplorer.ViewModels.Tabs {
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    class CLRTabViewModel : TabViewModelBase {
        [ImportingConstructor]
        public CLRTabViewModel(MainViewModel vm) : base(vm) {
        }

        public override string Icon => "/icons/cpu.ico";

        public override string Text => "CLR";
    }

}
