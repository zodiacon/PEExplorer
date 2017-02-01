using PEExplorer.Core;
using PEExplorer.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExplorer.ViewModels.Tabs {
	[Export, PartCreationPolicy(CreationPolicy.NonShared)]
	class LoadConfigTabViewModel : TabViewModelBase {
		LoadConfiguration _loadConfig;

		[ImportingConstructor]
		public LoadConfigTabViewModel(MainViewModel vm) : base(vm) {
		}

		public override string Icon => "/icons/config.ico";

		public override string Text => "Load Config";

		GenericProperty[] _properties;

		ExportedSymbol[] _cfgFunctions;
		public ExportedSymbol[] CFGFunctions {
			get {
				if (_cfgFunctions == null) {
					_loadConfig.GetCFGFunctions().ContinueWith(t => {
						_cfgFunctions = t.Result.ToArray();
						OnPropertyChanged(nameof(CFGFunctions));
					}).ConfigureAwait(true);
				}
				return _cfgFunctions;
			}
		}

		public GenericProperty[] Properties {
			get {
				if (_properties == null) {
					_loadConfig = MainViewModel.PEParser.GetLoadConfiguration();

					_properties = new GenericProperty[] {
						new GenericProperty { Name = "Time Stamp", Value = _loadConfig.TimeDateStamp.ToString(), Info = DateTimeHelper.FromSeconds(_loadConfig.TimeDateStamp).ToString() },
						new GenericProperty { Name = "Major Version", Value = _loadConfig.MajorVersion.ToString() },
						new GenericProperty { Name = "Minor Version", Value = _loadConfig.MinorVersion.ToString() },

						new GenericProperty { Name = "CFG Check Function Pointer", Value = ToDecHex(_loadConfig.CFGCheckFunctionPointer) },
						new GenericProperty { Name = "CFG Dispatch Function Pointer", Value = ToDecHex(_loadConfig.CFGDispatchFunctionPointer) },
						new GenericProperty { Name = "CFG Flags", Value = ToDecHex((ulong)_loadConfig.GuardFlags), Info = _loadConfig.GuardFlags.ToString() },
						new GenericProperty { Name = "CFG Function Table", Value = ToDecHex(_loadConfig.CFGFunctionTable) },
						new GenericProperty { Name = "CFG Function Count", Value = _loadConfig.CFGFunctionCount.ToString() },
					};

				}
				return _properties;

			}
		}
	}
}
