using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PEExplorer.ViewModels.Resources;
using Prism.Commands;
using Zodiacon.PEParsing;
using Zodiacon.WPF;

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
                MainViewModel.Container.SatisfyImportsOnce(resourceType);
                foreach(var resource in _resourceManager.GetResourceNames(type)) {
                    var vm = resourceType.CreateResourceViewModel(resource);
                    resourceType.Resources.Add(vm);
                }
                resources.Add(resourceType);
            }
            return resources;
        }

        public void Dispose() {
            _resourceManager.Dispose();
        }

        [Import]
        IFileDialogService FileDialogService;

        ICommand _exportCommand;
        public ICommand ExportCommand => _exportCommand ?? (_exportCommand = new DelegateCommand<object>(res => {
            var file = FileDialogService.GetFileForSave();
            if(file == null) return;

            File.WriteAllBytes(file, ((ResourceViewModel)res).GetContents());
        }, res => res is ResourceViewModel).ObservesProperty(() => SelectedTreeItem));

        private object _selectedTreeItem;

        public object SelectedTreeItem {
            get { return _selectedTreeItem; }
            set { SetProperty(ref _selectedTreeItem, value); }
        }
    }
}
