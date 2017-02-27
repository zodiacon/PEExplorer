using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDisasm;

namespace PEExplorer.ViewModels {
	class InstructionViewModel {
		public Instruction Instruction { get; }

		public InstructionViewModel(Instruction instruction) {
			Instruction = instruction;
		}

		public string Bytes => string.Join(" ", Instruction.Bytes);

		public string Address => Instruction.Offset.ToString("X8");

		public string Text => Instruction.ToString().Substring(8);

	}
}
