using Microsoft.Diagnostics.Runtime.Utilities;
using PEExplorer.Core;
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

namespace PEExplorer.ViewModels.Tabs {
	public sealed class DependencyTreeItem : BindableBase {
		MemoryMappedViewAccessor _accessor;

		public DependencyTreeItem(string filename, bool apiSet, MemoryMappedViewAccessor accessor = null) {
			FilePath = filename;
			_accessor = accessor;
            IsApiSet = apiSet;
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

        IEnumerable<ExportedSymbol> _exports;
        public IEnumerable<ExportedSymbol> Exports {
            get {
                if(_exports == null) {
                    try {
                        using(var pe = new PEFile(FilePath)) {
                            using(var parser = new PEFileParser(pe, FilePath)) {
                                _exports = parser.GetExports();
                            }
                        }
                    }
                    catch { }
                }
                return _exports;
            }
        }

		List<DependencyTreeItem> _items;
		public IEnumerable<DependencyTreeItem> Items {
			get {
                if(IsApiSet)
                    return null;

				if(_items == null) {
					_items = new List<DependencyTreeItem>(8);
					PEFile pefile;
					try {
						pefile = new PEFile(FilePath);
					}
					catch(FileNotFoundException) {
						return null;
					}
					using(var parser = new PEFileParser(pefile, FilePath, _accessor)) {
						var imports = parser.GetImports();
						if(imports == null)
							return _items;
						foreach(var library in imports) {
							var path = Environment.SystemDirectory + "\\" + library.LibraryName;

                            bool apiSet = library.LibraryName.StartsWith("api-ms-");
							_items.Add(new DependencyTreeItem(path, apiSet) {
								Text = library.LibraryName,
								Icon = apiSet ? "/icons/apiset.ico" : "/icons/library.ico",
							});
						}
                        pefile.Dispose();
					}
				}
				return _items;
			}
		}
	}

	[Export, PartCreationPolicy(CreationPolicy.NonShared)]
	sealed class DependenciesTabViewModel : TabViewModelBase {
		DependencyTreeItem[] _items;
		public DependencyTreeItem[] Dependencies => _items ?? (_items = _root.Items.ToArray());

		[ImportingConstructor]
		public DependenciesTabViewModel(MainViewModel vm) : base(vm) {
		}

		public override string Icon => "/icons/dependencies.ico";

		public override string Text => "Dependencies";

		DependencyTreeItem _root;

		public DependencyTreeItem PEImage => _root ?? (_root = new DependencyTreeItem(MainViewModel.PathName, false, MainViewModel.Accessor) {
			Text = MainViewModel.FileName,
			Icon = "/icons/data.ico",
		});

        private DependencyTreeItem _selectedItem;

        public DependencyTreeItem SelectedItem {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

    }
}
