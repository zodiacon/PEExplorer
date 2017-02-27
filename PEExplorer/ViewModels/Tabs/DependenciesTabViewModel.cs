using Microsoft.Diagnostics.Runtime.Utilities;
using PEExplorer.Core;
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
	public sealed class DependencyTreeItem {
		MemoryMappedViewAccessor _accessor;

		public DependencyTreeItem(string filename, MemoryMappedViewAccessor accessor = null) {
			FilePath = filename;
			_accessor = accessor;
		}

		public string Text { get; set; }
		public string Icon { get; set; }
		public string FilePath { get; set; }

		public IEnumerable<DependencyTreeItem> Items {
			get {
				PEFile pefile;
				try {
					pefile = new PEFile(FilePath);
				}
				catch(FileNotFoundException) {
					yield break;
				}
				using(var parser = new PEFileParser(pefile, FilePath, _accessor)) {
					var imports = parser.GetImports();
					if(imports == null)
						yield break;
					foreach(var library in imports) {
						var path = Environment.SystemDirectory + "\\" + library.LibraryName;

						yield return new DependencyTreeItem(path) {
							Text = library.LibraryName,
							Icon = "/icons/library.ico",
						};
					}
				}
			}
		}
	}

	[Export, PartCreationPolicy(CreationPolicy.NonShared)]
	sealed class DependenciesTabViewModel : TabViewModelBase {
		public IEnumerable<DependencyTreeItem> Dependencies => _root.Items;

		[ImportingConstructor]
		public DependenciesTabViewModel(MainViewModel vm) : base(vm) {
		}

		public override string Icon => "/icons/dependencies.ico";

		public override string Text => "Dependencies";

		DependencyTreeItem _root;

		public DependencyTreeItem PEImage => _root ?? (_root = new DependencyTreeItem(MainViewModel.PathName, MainViewModel.Accessor) {
			Text = MainViewModel.FileName,
			Icon = "/icons/data.ico"
		});

	}
}
