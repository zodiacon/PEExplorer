using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEExplorer.Core;

namespace PEExplorer.ViewModels.Resources {
    class StringResourceViewModel : ResourceViewModel {
        public StringResourceViewModel(ResourceID id, ResourceTypeViewModel type) : base(id, type) {
        }

        ICollection<StringResource> _strings;

        public ICollection<StringResource> Strings => _strings ?? (_strings = Type.ResourceManager.GetStringTableContent(ResourceId));

        public override bool CustomViewPossible => true;

    }
}
