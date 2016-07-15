using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEExplorer.Core;

namespace PEExplorer.ViewModels {
    class ResourceTypeViewModel {
        public ResourceID ResourceType { get; internal set; }
        public ICollection<ResourceViewModel> Resources { get; } = new List<ResourceViewModel>();

        public ResourceManager ResourceManager { get; }

        public ResourceTypeViewModel(ResourceManager mgr) {
            ResourceManager = mgr;
        }
    }
}
