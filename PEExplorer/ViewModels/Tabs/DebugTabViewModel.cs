using PEExplorer.Helpers;
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

        public IEnumerable<GenericProperty> Properties {
            get {
                var debug = MainViewModel.PEParser.GetDebugInformation();
                yield return new GenericProperty {
                    Name = "Time Stamp",
                    Value = ToDecHex(debug.TimeDateStamp),
                    Info = DateTimeHelper.ToDateTime(debug.TimeDateStamp).ToString("F")
                };
                yield return new GenericProperty {
                    Name = "Debug Type",
                    Value = debug.DebugType.ToString()
                };
                yield return new GenericProperty {
                    Name = "major version",
                    Value = debug.MajorVersion.ToString()
                };
                yield return new GenericProperty {
                    Name = "Minor Version",
                    Value = debug.MinorVersion.ToString()
                };
                yield return new GenericProperty {
                    Name = "Size of Data",
                    Value = ToDecHex(debug.SizeOfData)
                };
                yield return new GenericProperty {
                    Name = "Address of Raw Data",
                    Value = ToDecHex(debug.AddressOfRawData)
                };
                yield return new GenericProperty {
                    Name = "Pointer to Raw Data",
                    Value = ToDecHex(debug.PointerToRawData)
                };

            }
        }
	}
}
