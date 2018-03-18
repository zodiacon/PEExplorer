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

        public enum ImageType {
            Bitmap,
            Icon,
            Cursor
        }

        [Flags]
        public enum LoadImageFlags {
            None = 0,
            DefaultSize = 0x40,
            CreateDibSection = 0x2000
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

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr FindResource(IntPtr hModule, string name, string typeName);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint SizeofResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LockResource(IntPtr hResData);


        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibraryEx(string path, IntPtr handle, LoadLibraryFlags flags);

        [DllImport("kernel32")]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindResource(IntPtr hModule, IntPtr name, IntPtr type);

        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadIcon(IntPtr hModule, IntPtr name);

        [DllImport("user32")]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("user32")]
        public static extern bool DestroyCursor(IntPtr hCursor);

        [DllImport("user32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr LoadImage(IntPtr hModule, IntPtr name, ImageType type, int width, int height, LoadImageFlags flags);

        [DllImport("user32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr LoadImage(IntPtr hModule, string name, ImageType type, int width, int height, LoadImageFlags flags);

        [DllImport("gdi32")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32")]
        public static extern IntPtr CreateIconFromResource([MarshalAs(UnmanagedType.LPArray)] byte[] bits, int size, bool isIcon, uint version);

        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int LoadString(IntPtr hModule, int id, StringBuilder buffer, int maxSize);
    }
}
