using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace PEExplorer.ViewModels {
    class TreeViewItemViewModel : BindableBase {
        private string _text;

        public string Text {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        private string _icon;

        public string Icon {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }

        ObservableCollection<TreeViewItemViewModel> _items;

        public IList<TreeViewItemViewModel> Items => _items ?? (_items = new ObservableCollection<TreeViewItemViewModel>());
    }
}
