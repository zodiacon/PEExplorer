using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEExplorer.Core;

namespace PEExplorer.ViewModels.Tabs {
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    class ResourcesTabViewModel : TabViewModelBase, IDisposable {
        ResourceManager _resourceManager;

        [ImportingConstructor]
        public ResourcesTabViewModel(MainViewModel vm) : base(vm) {
        }

        public override string Icon => "/icons/resources.ico";

        public override string Text => "Resources";

        ICollection<ResourceTypeViewModel> _resources;

        public ICollection<ResourceTypeViewModel> Resources => _resources ?? (_resources = GetResources());

        private ICollection<ResourceTypeViewModel> GetResources() {
            _resourceManager = new ResourceManager(MainViewModel.PathName);
            var resources = new List<ResourceTypeViewModel>();
            foreach(var type in _resourceManager.GetResourceTypes()) {
                var resourceType = new ResourceTypeViewModel(_resourceManager) { ResourceType = type };
                foreach(var resource in _resourceManager.GetResourceNames(type)) {
                    resourceType.Resources.Add(new ResourceViewModel(resource, resourceType));
                }
                resources.Add(resourceType);
            }
            return resources;
        }

        public void Dispose() {
            _resourceManager.Dispose();
        }
    }
}
