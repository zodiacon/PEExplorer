using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExplorer.ViewModels.Tabs {
	[Export, PartCreationPolicy(CreationPolicy.NonShared)]
	class DebugTabViewModel : TabViewModelBase {
		[ImportingConstructor]
		public DebugTabViewModel(MainViewModel vm) : base(vm) {
		}

		public override string Icon => "/icons/debug.ico";

		public override string Text => "Debug";
	}
}
