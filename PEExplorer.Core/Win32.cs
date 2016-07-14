using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PEExplorer.Core {
    [SuppressUnmanagedCodeSecurity]
    static class Win32 {
        [Flags]
        public enum LoadLibraryFlags : uint {
            LoadAsImageResource = 0x20,
            LoadAsDataFile = 0x2,
            LoadAsDataFileExclusive = 0x40
        }

        [return:MarshalAs(UnmanagedType.Bool)]
        public delegate bool EnumResTypeProc(IntPtr hModule, IntPtr typeName, IntPtr param);
        public delegate bool EnumResNameProc(IntPtr hModule, IntPtr typeName, IntPtr name, IntPtr param);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool EnumResourceTypes(IntPtr hModule, EnumResTypeProc enumProc, IntPtr param);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool EnumResourceNames(IntPtr hModule, IntPtr typeName, EnumResNameProc enumProc, IntPtr param);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool EnumResourceNames(IntPtr hModule, string typeName, EnumResNameProc enumProc, IntPtr param);

        [DllImport("user32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr FindResource(IntPtr hModule, string name, string typeName);

        [DllImport("user32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("user32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint SizeOfResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("user32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LockResource(IntPtr hResData);


        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibraryEx(string path, IntPtr handle, LoadLibraryFlags flags);

        [DllImport("kernel32")]
        public static extern bool FreeLibrary(IntPtr hModule);

    }
}
