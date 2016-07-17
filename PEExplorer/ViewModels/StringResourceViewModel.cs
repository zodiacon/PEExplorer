using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEExplorer.Core;

namespace PEExplorer.ViewModels {
    class StringResourceViewModel : ResourceViewModel {
        public StringResourceViewModel(ResourceID id, ResourceTypeViewModel type) : base(id, type) {
        }

        string _text;
        public string Text => _text ?? (_text = Type.ResourceManager.GetResourceString(ResourceId));

        public override bool CustomViewPossible => true;
    }
}
