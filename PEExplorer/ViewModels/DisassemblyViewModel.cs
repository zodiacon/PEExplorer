using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SharpDisasm;
using Zodiacon.WPF;
using SharpDisasm.Udis86;

namespace PEExplorer.ViewModels {
    class DisassemblyViewModel : DialogViewModelBase {
        public string SymbolName { get; }

        public DisassemblyViewModel(Window dialog, string symbolName) : base(dialog) {
            SymbolName = symbolName;
        }

        public void Disassemble(byte[] code, int address, bool x64) {
            Disassembler.Translator.IncludeAddress = true;
            Disassembler.Translator.IncludeBinary = true;
            using(var disassem = new Disassembler(code, x64 ? ArchitectureMode.x86_64 : ArchitectureMode.x86_32, (ulong)address, true)) {
                Instructions = disassem.Disassemble().TakeWhileIncluding((i, c) => c < 1000 && 
				i.Mnemonic != ud_mnemonic_code.UD_Iret && i.Mnemonic != ud_mnemonic_code.UD_Iint3).
                    Select(i => new InstructionViewModel(i));
            }
        }

        private IEnumerable<InstructionViewModel> _instructions;

        public IEnumerable<InstructionViewModel> Instructions {
            get { return _instructions; }
            set { SetProperty(ref _instructions, value); }
        }

        public string Title => $"Disassembly: {SymbolName}";
    }
}
