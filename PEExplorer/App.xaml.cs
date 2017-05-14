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
using System.Runtime.CompilerServices;

namespace PEExplorer {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        MainViewModel _mainViewModel;
        readonly Dictionary<string, Assembly> _assemblies = new Dictionary<string, Assembly>(4);

        public App() {
            LoadAssemblies();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void LoadAssemblies() {
            var appAssembly = typeof(App).Assembly;
            foreach (var resourceName in appAssembly.GetManifestResourceNames()) {
                if (resourceName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase)) {
                    using (var stream = appAssembly.GetManifestResourceStream(resourceName)) {
                        var assemblyData = new byte[(int)stream.Length];
                        stream.Read(assemblyData, 0, assemblyData.Length);
                        var assembly = Assembly.Load(assemblyData);
                        _assemblies.Add(assembly.GetName().Name, assembly);
                    }
                }
            }
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }

        Assembly OnAssemblyResolve(object sender, ResolveEventArgs args) {
            var shortName = new AssemblyName(args.Name).Name;
            if (_assemblies.TryGetValue(shortName, out var assembly)) {
                return assembly;
            }
            return null;
        }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            var container = new CompositionContainer(
                new AggregateCatalog(
                    new AssemblyCatalog(Assembly.GetExecutingAssembly()),
                    new AssemblyCatalog(typeof(IDialogService).Assembly)));

			var defaults = new UIServicesDefaults();

			container.ComposeExportedValue(container);
			container.ComposeExportedValue(defaults.DialogService);
			container.ComposeExportedValue(defaults.FileDialogService);
			container.ComposeExportedValue(defaults.MessageBoxService);

			var vm = _mainViewModel = container.GetExportedValue<MainViewModel>();
            var win = new MainWindow { DataContext = vm };
            win.Show();
			if (e.Args.Length > 0)
				vm.OpenInternal(e.Args[0], false);
        }

        protected override void OnExit(ExitEventArgs e) {
            _mainViewModel.Close();
            base.OnExit(e);
        }
    }
}
