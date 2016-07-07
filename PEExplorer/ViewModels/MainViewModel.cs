using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Diagnostics.Runtime.Utilities;
using PEExplorer.ViewModels.Tabs;
using Prism.Commands;
using Prism.Mvvm;
using Zodiacon.WPF;

namespace PEExplorer.ViewModels {
    [Export]
    class MainViewModel : BindableBase {
        public string Title => "PE Explorer " + (FileName ?? string.Empty);

        ObservableCollection<TabViewModelBase> _tabs = new ObservableCollection<TabViewModelBase>();
        public IList<TabViewModelBase> Tabs => _tabs;

        private TabViewModelBase _selectedTab;

        public TabViewModelBase SelectedTab {
            get { return _selectedTab; }
            set { SetProperty(ref _selectedTab, value); }
        }

        private string _fileName;
        private PEFile _peFile;

        public string PathName { get; set; }
        public PEFile PEFile {
            get { return _peFile; }
            set { SetProperty(ref _peFile, value); }
        }


        public string FileName {
            get { return _fileName; }
            set { SetProperty(ref _fileName, value); }
        }

        [Import]
        IFileDialogService FileDialogService;

        [Import]
        IMessageBoxService MessageBoxService;

        [Import]
        CompositionContainer Container;

        ObservableCollection<TreeViewItemViewModel> _treeRoot = new ObservableCollection<TreeViewItemViewModel>();

        public IList<TreeViewItemViewModel> TreeRoot => _treeRoot;

        public ICommand OpenCommand => new DelegateCommand(() => {
            try {
                var filename = FileDialogService.GetFileForOpen("PE Files (*.exe;*.dll;*.ocx;*.obj;*.lib)|*.exe;*.dll;*.ocx;*.obj;*.lib", "Select File");
                if(filename == null) return;
                OpenInternal(filename);
                FileName = Path.GetFileName(filename);
                PathName = filename;
                OnPropertyChanged(nameof(Title));
                BuildTree();
            }
            catch(Exception ex) {
                MessageBoxService.ShowMessage(ex.Message, "PE Explorer");
            }
        });

        private void BuildTree() {
            TreeRoot.Clear();
            TreeRoot.Add(new TreeViewItemViewModel { Text = FileName, Icon = "/icons/pefile.ico" });

            var generalTab = Container.GetExportedValue<GeneralTabViewModel>();
            Tabs.Add(generalTab);
            SelectedTab = generalTab;
        }

        private void OpenInternal(string filename) {
            PEFile = new PEFile(filename);
        }

        public ICommand ExitCommand => new DelegateCommand(() => Application.Current.Shutdown());

        public ICommand CloseCommand => new DelegateCommand(() => {
            FileName = null;
            _tabs.Clear();
            _treeRoot.Clear();
            OnPropertyChanged(nameof(Title));
        });

    }
}
