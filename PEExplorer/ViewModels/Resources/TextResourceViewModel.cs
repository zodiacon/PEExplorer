using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEExplorer.Core;

namespace PEExplorer.ViewModels.Resources {
    class TextResourceViewModel : ResourceViewModel {
        public TextResourceViewModel(ResourceID id, ResourceTypeViewModel type) : base(id, type) {
        }

        public string ManifestText => Type.MainViewModel.PEFile.GetSxSManfest();

        public override bool CustomViewPossible => true;
    }
}
