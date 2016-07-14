using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEExplorer.Core;

namespace PEExplorer.ViewModels.Tabs {
    [Export]
    class ResourcesTabViewModel : TabViewModelBase {
        ResourceManager _resourceManager;

        [ImportingConstructor]
        public ResourcesTabViewModel(MainViewModel vm) : base(vm) {
            //_resourceManager = new ResourceManager(vm.PathName);
            //foreach(var type in _resourceManager.GetResourceTypes()) {
            //    Debug.WriteLine(type);
            //    var names = _resourceManager.GetResourceNames(type);

            //}
        }

        public override string Icon => "/icons/resources.ico";

        public override string Text => "Resources";
    }
}
