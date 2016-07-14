using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PEExplorer.Core {
    public sealed class ResourceID : IEquatable<ResourceID> {
        static ResourceID[] _standardResources = typeof(ResourceID).GetFields(BindingFlags.Public | BindingFlags.Static).Select(f => f.GetValue(null)).Cast<ResourceID>().ToArray();

        public int Id { get; }
        public string Name { get; }
        public ResourceID(int id) {
            Id = id;
        }

        public ResourceID(string name) {
            Name = name;
        }

        public bool IsId => Name == null;

        public bool IsStandard => IsId && _standardResources.Any(res => res.Equals(this));

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

        public static readonly ResourceID Icon = new ResourceID(5);
        public static readonly ResourceID Accelerator = new ResourceID(9);
        public static readonly ResourceID AnimatedCursor = new ResourceID(21);
        public static readonly ResourceID Bitmap = new ResourceID(2);
    }
}
