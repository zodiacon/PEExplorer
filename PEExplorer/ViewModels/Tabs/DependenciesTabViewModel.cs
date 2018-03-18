using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.PEParsing;

namespace PEExplorer.ViewModels.Tabs {
    sealed class DependencyTreeItem : BindableBase {
        DependenciesTabViewModel _tab;

        public DependencyTreeItem(DependenciesTabViewModel tab, string filename, bool apiSet, IEnumerable<object> exports = null) {
            FilePath = filename;
            IsApiSet = apiSet;
            _tab = tab;
            _exports = exports;
        }

        public string Text { get; set; }
        public string Icon { get; set; }
        public string FilePath { get; set; }
        public bool IsApiSet { get; }

        private bool _isExpanded;

        public bool IsExpanded {
            get { return _isExpanded; }
            set { SetProperty(ref _isExpanded, value); }
        }

        private bool _isSelected;

        public bool IsSelected {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        IEnumerable<object> _exports;
        public IEnumerable<object> Exports {
            get {
                if (_exports == null) {
                    if (IsApiSet) {
                        if (_tab.Imports.TryGetValue(FilePath, out var library))
                            _exports = library.Symbols;
                    }
                    else {
                        try {
                            using (var parser = new PEParser(FilePath)) {
                                _exports = parser.GetExports();
                            }
                        }
                        catch { }
                    }
                }
                return _exports;
            }
        }

        List<DependencyTreeItem> _items;
        public IEnumerable<DependencyTreeItem> Items {
            get {
                if (IsApiSet)
                    return null;

                if (_items == null) {
                    _items = new List<DependencyTreeItem>(8);
                    using (var parser = new PEParser(FilePath)) {
                        var imports = parser.GetImports();
                        if (imports == null)
                            return _items;
                        foreach (var library in imports) {
                            var path = Environment.SystemDirectory + "\\" + library.LibraryName;

                            bool apiSet = library.LibraryName.StartsWith("api-ms-");
                            _items.Add(new DependencyTreeItem(_tab, apiSet ? library.LibraryName : path, apiSet) {
                                Text = library.LibraryName,
                                Icon = apiSet ? "/icons/apiset.ico" : "/icons/library.ico",
                            });
                        }
                    }
                }
                return _items;
            }
        }
    }

    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    sealed class DependenciesTabViewModel : TabViewModelBase, IPartImportsSatisfiedNotification {
        DependencyTreeItem[] _items;
        public DependencyTreeItem[] Dependencies => _items ?? (_items = _root.Items.ToArray());

        [ImportingConstructor]
        public DependenciesTabViewModel(MainViewModel vm) : base(vm) {
        }

        public override string Icon => "/icons/dependencies.ico";

        public override string Text => "Dependencies";

        DependencyTreeItem _root;

        public DependencyTreeItem PEImage => _root ?? (_root = new DependencyTreeItem(this, MainViewModel.PathName, false,
            MainViewModel.Parser.GetExports()) {
            Text = MainViewModel.FileName,
            Icon = "/icons/data.ico",
        });

        private DependencyTreeItem _selectedItem;

        public DependencyTreeItem SelectedItem {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        public Dictionary<string, ImportedLibrary> Imports { get; private set; }

        public void OnImportsSatisfied() {
            var imports = MainViewModel.Parser.GetImports();
            if (imports != null) {
                Imports = imports.ToDictionary(library => library.LibraryName);
            }
        }
    }
}
