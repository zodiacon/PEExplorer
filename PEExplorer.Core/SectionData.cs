using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExplorer.Core {
	public class SectionData {
		SectionHeader _header;
		internal SectionData(SectionHeader header) {
			_header = header;
		}

		public string Name => _header.Name;
		public uint VirtualSize => _header.VirtualSize;
		public uint VirtualAddress => _header.VirtualAddress;
		public uint SizeOfRawData => _header.SizeOfRawData;
		public uint PointerToRawData => _header.PointerToRawData;
		public uint PointerToRelocations => _header.PointerToRelocations;
		public uint PointerToLineNumbers => _header.PointerToLinenumbers;

		public ushort NumberOfRelocations => _header.NumberOfRelocations;
		public ushort NumberOfLineNumbers => _header.NumberOfLinenumbers;
		public SectionFlags Characteristics => _header.Characteristics;

	}
}
