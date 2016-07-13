using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SharpDisasm;
using Zodiacon.WPF;

namespace PEExplorer.ViewModels {
    class DisassemblyViewModel : DialogViewModelBase {
        public DisassemblyViewModel(Window dialog) : base(dialog) {
        }

        public void Disassemble(byte[] code, int address, bool x64) {
            Disassembler.Translator.IncludeAddress = true;
            Disassembler.Translator.IncludeBinary = true;
            var disassem = new Disassembler(code, x64 ? ArchitectureMode.x86_64 : ArchitectureMode.x86_32, (ulong)address, true);
            Instructions = disassem.Disassemble().TakeWhileIncluding(i => i.Mnemonic != SharpDisasm.Udis86.ud_mnemonic_code.UD_Iret).
                Select(i => new InstructionViewModel(i));
        }

        private IEnumerable<InstructionViewModel> _instructions;

        public IEnumerable<InstructionViewModel> Instructions {
            get { return _instructions; }
            set { SetProperty(ref _instructions, value); }
        }

    }
}
