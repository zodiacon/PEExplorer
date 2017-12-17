using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zodiacon.PEParsing;

namespace PEExplorer.ViewModels.Tabs {
	[Export, PartCreationPolicy(CreationPolicy.NonShared)]
	class SectionsTabViewModel : TabViewModelBase {
		[ImportingConstructor]
		public SectionsTabViewModel(MainViewModel vm) : base(vm) {
		}

		public override string Icon => "/icons/sections.ico";
		public override string Text => "Sections";

		ICollection<SectionHeader> _sections;
		public ICollection<SectionHeader> Sections => _sections ?? (_sections = MainViewModel.PEParser.GetSectionHeaders());

	}
}
