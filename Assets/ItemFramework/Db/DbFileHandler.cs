using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using String = System.String;
using Guid = System.Guid;

namespace ItemFramework.Db {
    public class DbFileHandler : IDbHandler {
        [DbObject("data")]
        private class DataContainer {
            [DbProperty("containers")]
            internal List<Container> Containers = new List<Container>();
            [DbProperty("itemstacks")]
            public List<ItemStack> ItemStacks = new List<ItemStack>();

            public DataContainer() { }

            public void AddContainer(Container container) {
                Containers.Add(container);
                AddItemStacks(container.Items);
            }

            public void AddItemStacks(params Guid?[] itemStacks) {
                foreach (Guid? itemStack in itemStacks) {
                    if (itemStack.HasValue && itemStack.Value != Guid.Empty) {
                        ItemStacks.Add(ItemStack.GetItemStackById(itemStack.Value));
                    }
                }
            }
        }

        private static DataContainer dataContainer = new DataContainer();
        
        public string Path { get; private set; }

        public DbFileHandler(string path) {
            Path = path;
        }

        public void Load() {
            using (FileStream fs = new FileStream(Path, FileMode.OpenOrCreate)) {
                // Deserialize the data and read it from the instance.
                byte[] bytes = new byte[fs.Length];
                int numBytesToRead = (int)fs.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0) {
                    // Read may return anything from 0 to numBytesToRead.
                    int n = fs.Read(bytes, numBytesRead, numBytesToRead);

                    // Break when the end of the file is reached.
                    if (n == 0)
                        break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }
                numBytesToRead = bytes.Length;
                DataContainer loadedData = JsonConvert.DeserializeObject<DataContainer>(Encoding.UTF8.GetString(bytes));
                if (loadedData == null) {
                    Debug.Log("No database found");
                    return;
                }
                dataContainer = loadedData;
                Debug.Log(String.Format("Found {0} container(s) and {1} itemstack(s)", dataContainer.Containers.Count, dataContainer.ItemStacks.Count));
            }
        }

        public void Save() {
            using (FileStream fs = new FileStream(Path, FileMode.Create)) {
                string t = JsonConvert.SerializeObject(dataContainer);
                fs.Write(Encoding.UTF8.GetBytes(t), 0, Encoding.UTF8.GetByteCount(t));
                Debug.Log("Written to database.");
            }
        }
    }
}