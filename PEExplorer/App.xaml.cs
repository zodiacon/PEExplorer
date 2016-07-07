using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using PEExplorer.ViewModels;
using Zodiacon.WPF;

namespace PEExplorer {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            var container = new CompositionContainer(
                new AggregateCatalog(
                    new AssemblyCatalog(Assembly.GetExecutingAssembly()),
                    new AssemblyCatalog(typeof(IDialogService).Assembly)));

            container.ComposeExportedValue(container);

            var vm = container.GetExportedValue<MainViewModel>();
            var win = new MainWindow { DataContext = vm };
            win.Show();
        }
    }
}
