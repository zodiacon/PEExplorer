using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PEExplorer.Core;

namespace PEExplorer.ViewModels {
    class ResourceViewModel {
        public ResourceID ResourceId { get; }
        public ResourceTypeViewModel Type { get; }

        public ResourceViewModel(ResourceID id, ResourceTypeViewModel type) {
            ResourceId = id;
            Type = type;
        }

        ImageSource _image;
        public ImageSource Icon => _image ?? (_image = Type.ResourceManager.GetIconImage(ResourceId, true));
        public ImageSource Cursor => _image ?? (_image = Type.ResourceManager.GetIconImage(ResourceId, false));

        public ImageSource Bitmap => _image ?? (_image = Type.ResourceManager.GetBitmapImage(ResourceId));

        string _text;
        public string Text => _text ?? (_text = Type.ResourceManager.GetResourceString(ResourceId));
    }
}
