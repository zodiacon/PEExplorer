using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PEExplorer.Core {

    public sealed class ResourceManager : IDisposable {
        IntPtr _hModule;

        public ResourceManager(string path) {
            _hModule = Win32.LoadLibraryEx(path, IntPtr.Zero, Win32.LoadLibraryFlags.LoadAsDataFile | Win32.LoadLibraryFlags.LoadAsImageResource);
            if(_hModule == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        void Dispose(bool disposing) {
            Win32.FreeLibrary(_hModule);
            if(disposing)
                GC.SuppressFinalize(this);
        }

        public void Dispose() {
            Dispose(true);
        }

        ~ResourceManager() {
            Dispose(false);
        }

        public unsafe ICollection<ResourceID> GetResourceTypes() {
            var types = new List<ResourceID>();
            Win32.EnumResourceTypes(_hModule, (handle, ptr, param) => {
                if((long)ptr < 0x10000)
                    types.Add(new ResourceID((int)ptr));
                else
                    types.Add(new ResourceID(new string((char*)ptr)));
                return true;
            }, IntPtr.Zero);
            return types;
        }

        public unsafe ICollection<ResourceID> GetResourceNames(ResourceID type) {
            var names = new List<ResourceID>();
            Win32.EnumResNameProc proc = (module, typeName, name, param) => {
                if((long)name < 0x10000)
                    names.Add(new ResourceID((int)name));
                else
                    names.Add(new ResourceID(new string((char*)name)));
                return true;
            };

            if(type.IsId)
                Win32.EnumResourceNames(_hModule, new IntPtr(type.Id), proc, IntPtr.Zero);
            else
                Win32.EnumResourceNames(_hModule, type.Name, proc, IntPtr.Zero);
            return names;
        }
    }
}
