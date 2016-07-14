using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PEExplorer.Helpers {
    static class Serializer {
        public static void Save<T>(T obj, Stream stm) where T : class {
            var writer = new DataContractSerializer(typeof(T));
            writer.WriteObject(stm, obj);
        }

        public static void Save<T>(T obj, string filename) where T : class {
            var path = GetPath(filename);

            try {
                using(var stm = File.Open(path, FileMode.Create))
                    Save(obj, stm);
            }
            catch {
            }
        }

        public static T Load<T>(Stream stm) where T : class {
            var reader = new DataContractSerializer(typeof(T));
            return reader.ReadObject(stm) as T;
        }

        public static T Load<T>(string filename) where T : class {
            var path = GetPath(filename);
            try {
                using(var stm = File.Open(path, FileMode.Open))
                    return Load<T>(stm);
            }
            catch {
                return null;
            }
        }

        private static string GetPath(string filename) {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create) + @"\PEExplorer";
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path += "\\" + filename;
            return path;
        }
    }
}

