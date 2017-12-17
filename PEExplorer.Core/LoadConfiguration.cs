using DebugHelp;
using Microsoft.Diagnostics.Runtime.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PEExplorer.Core {
	public sealed class LoadConfiguration {
		IMAGE_LOAD_CONFIG_DIRECTORY32 _config32;
		IMAGE_LOAD_CONFIG_DIRECTORY64 _config64;
		PEParser _pefile;
		bool _is32bit;

		internal LoadConfiguration(PEParser pefile, ref IMAGE_LOAD_CONFIG_DIRECTORY32 config32) {
			_config32 = config32;
			_is32bit = true;
			_pefile = pefile;
		}

		internal LoadConfiguration(PEParser pefile, ref IMAGE_LOAD_CONFIG_DIRECTORY64 config64) {
			_config64 = config64;
			_pefile = pefile;
		}

		public uint TimeDateStamp => _is32bit ? _config32.TimeDateStamp : _config64.TimeDateStamp;
		public ushort MajorVersion => _is32bit ? _config32.MajorVersion : _config64.MajorVersion;
		public ushort MinorVersion => _is32bit ? _config32.MinorVersion : _config64.MinorVersion;

		public ulong CFGCheckFunctionPointer => _is32bit ? _config32.GuardCFCheckFunctionPointer : _config64.GuardCFCheckFunctionPointer;
		public ulong CFGDispatchFunctionPointer => _is32bit ? _config32.GuardCFDispatchFunctionPointer : _config64.GuardCFDispatchFunctionPointer;

		public ControlFlowGuardFlags GuardFlags => _is32bit ? _config32.GuardFlags : _config64.GuardFlags;

		public uint GlobalFlagsClear => _is32bit ? _config32.GlobalFlagsClear : _config64.GlobalFlagsClear;
		public uint GlobalFlagsSet => _is32bit ? _config32.GlobalFlagsSet : _config64.GlobalFlagsSet;

		public ulong CFGFunctionTable => _is32bit ? _config32.GuardCFFunctionTable : _config64.GuardCFFunctionTable;
		public ulong CFGFunctionCount => _is32bit ? _config32.GuardCFFunctionCount : _config64.GuardCFFunctionCount;

		public async Task<IEnumerable<ExportedSymbol>> GetCFGFunctions() {
			var va = CFGFunctionTable - _pefile.Header.ImageBase;
			int count = (int)CFGFunctionCount;
			var symbols = new ExportedSymbol[count];
			var offset = _pefile.Header.RvaToFileOffset((int)va);
			int lastIndex = -1;

			using(var handler = SymbolHandler.Create(SymbolOptions.UndecorateNames)) {
				var dllBase = await handler.TryLoadSymbolsForModuleAsync(_pefile.FileName);

				ulong disp;
				var symbol = new SymbolInfo();
				for(int i = 0; i < count; i++) {
					var address = _pefile.Accessor.ReadUInt32(offset);
					string name = null;
					if(dllBase != 0) {
						if(handler.TryGetSymbolFromAddress(address + dllBase, ref symbol, out disp) && (lastIndex < 0 || symbol.Name != symbols[lastIndex].Name)) {
							name = symbol.Name;
							lastIndex = i;
						}
					}
					symbols[i] = new ExportedSymbol { Address = address, Name = name };
					offset += 5;
				}
			}

			return symbols;
		}
	}
}

