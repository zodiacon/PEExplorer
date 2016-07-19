using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEExplorer.Core;

namespace PEExplorer.ViewModels.Resources {
    class ResourceTypeViewModel {
        public ResourceID ResourceType { get; internal set; }
        public ICollection<ResourceViewModel> Resources { get; } = new List<ResourceViewModel>();

        public ResourceManager ResourceManager { get; }

        [Import]
        public MainViewModel MainViewModel;

        public ResourceTypeViewModel(ResourceManager mgr) {
            ResourceManager = mgr;
        }

        static readonly Dictionary<ResourceID, Type> _viewModels = new Dictionary<ResourceID, Type> {
            { ResourceID.Icon, typeof(ImageResourceViewModel) },
            { ResourceID.Cursor, typeof(ImageResourceViewModel) },
            { ResourceID.Bitmap, typeof(ImageResourceViewModel) },
            { ResourceID.GroupCursor, typeof(ImageResourceViewModel) },
            { ResourceID.GroupIcon, typeof(ImageResourceViewModel) },
            { ResourceID.StringTable, typeof(StringResourceViewModel) },
            { ResourceID.Manifest, typeof(TextResourceViewModel) },
        };

        internal ResourceViewModel CreateResourceViewModel(ResourceID resource) {
            if(!ResourceType.IsStandard)
                return new ResourceViewModel(resource, this);

            Type viewModelType;
            if(!_viewModels.TryGetValue(ResourceType, out viewModelType))
                return new ResourceViewModel(resource, this);

            return (ResourceViewModel)Activator.CreateInstance(viewModelType, resource, this);
        }

        public bool CustomViewPossible => false;

    }
}
