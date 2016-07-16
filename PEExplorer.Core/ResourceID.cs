using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PEExplorer.Core {
    public sealed class ResourceID : IEquatable<ResourceID> {
        static ResourceID[] _standardResources;

        static ResourceID() {
            _standardResources = typeof(ResourceID).GetFields(BindingFlags.Public | BindingFlags.Static).Select(f => f.GetValue(null)).Cast<ResourceID>().ToArray();
        }

        public int Id { get; }
        public string Name { get; }
        public ResourceID(int id, string typename = null) {
            Id = id;
            if(typename == null) {
                var res = _standardResources.FirstOrDefault(r => r.IsId && r.Id == id);
                if(res != null) {
                    typename = res.TypeName;
                    IsStandard = true;
                }
            }
            else {
                IsStandard = true;
            }
            TypeName = typename;
        }

        public ResourceID(string name) {
            Name = TypeName = name;
        }

        public bool IsId => Name == null;

        public bool IsStandard { get; }

        public override string ToString() => IsId ? $"#{Id}" : Name;

        public bool Equals(ResourceID other) {
            return other.Id == Id && other.Name == Name;
        }

        public override int GetHashCode() {
            return IsId ? Id.GetHashCode() : Name.GetHashCode();
        }

        public override bool Equals(object obj) {
            if(obj is ResourceID)
                return Equals((ResourceID)obj);
            return false;
        }

        unsafe struct FixedDisposable : IDisposable {
            internal GCHandle _handle;
            public FixedDisposable(string value = null) {
                _handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            }
            public void Dispose() {
                _handle.Free();
            }
        }

        public IDisposable GetAsIntPtr(out IntPtr ptr) {
            if(IsId) {
                ptr = new IntPtr(Id);
                return null;
            }
            var fix = new FixedDisposable(Name);
            ptr = fix._handle.AddrOfPinnedObject();
            return fix;
        }

        public string TypeName { get; }

        public static readonly ResourceID Icon = new ResourceID(3, "Icon");
        public static readonly ResourceID String = new ResourceID(6, "String");
        public static readonly ResourceID Accelerator = new ResourceID(9, "Accelerator");
        public static readonly ResourceID AnimatedCursor = new ResourceID(21, "Animated Cursor");
        public static readonly ResourceID Bitmap = new ResourceID(2, "Bitmap");
        public static readonly ResourceID Dialog = new ResourceID(5, "Dialog");
        public static readonly ResourceID Cursor = new ResourceID(1, "Cursor");
        public static readonly ResourceID Font = new ResourceID(2, "Font");
        public static readonly ResourceID Manifest = new ResourceID(24, "Manifest");
        public static readonly ResourceID AnimatedIcon = new ResourceID(22, "Animated Icon");
        public static readonly ResourceID FontDir = new ResourceID(7, "Font Directory");
        public static readonly ResourceID GroupIcon = new ResourceID(14, "Icon Group");
        public static readonly ResourceID GroupCursor = new ResourceID(12, "Cursor Group");
        public static readonly ResourceID Menu = new ResourceID(4, "Menu");
        public static readonly ResourceID Html = new ResourceID(23, "Html");
        public static readonly ResourceID MessageTable = new ResourceID(11, "Message Table");
        public static readonly ResourceID PlugPlay = new ResourceID(19, "Plug & PLay");
        public static readonly ResourceID Version = new ResourceID(16, "Version");
        public static readonly ResourceID RCData = new ResourceID(10, "RC Data");
    }
}
