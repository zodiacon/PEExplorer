using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace PEExplorer.ViewModels {
    abstract class TabViewModelBase : BindableBase {
        public abstract string Icon { get; }
        public abstract string Text { get; }

        public virtual bool CanClose => true;

        protected MainViewModel MainViewModel { get; }

        protected TabViewModelBase(MainViewModel vm) {
            MainViewModel = vm;
        }

		public string ToDecHex(ulong n) => $"{n} (0x{n:X})";

	}

}
