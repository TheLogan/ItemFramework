using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Guid = System.Guid;
using InvalidOperationException = System.InvalidOperationException;
using NotImplementedException = System.NotImplementedException;
using String = System.String;
using System;

namespace ItemFramework.Db
{
	public class DbFileHandler : IDbHandler
	{
		[DbObject("data")]
		private class DataContainer
		{
			[DbProperty("containers")]
			internal List<Container> Containers = new List<Container>();
			[DbProperty("itemstacks")]
			public List<ItemStack> ItemStacks = new List<ItemStack>();

			public DataContainer() { }

			public void AddContainer(Container container)
			{
				Containers.Add(container);
				AddItemStacks(container.Items);
			}

			public void AddItemStacks(params Guid?[] itemStacks)
			{
				foreach (Guid? itemStack in itemStacks)
				{
					if (itemStack.HasValue && itemStack.Value != Guid.Empty)
					{
						ItemStacks.Add(ItemStack.GetById(itemStack.Value));
					}
				}
			}

			public void AddItemStacks(params ItemStack[] itemStacks)
			{
				foreach (ItemStack itemStack in itemStacks)
				{
					ItemStacks.Add(itemStack);
				}
			}

			internal void RemoveContainer(Container obj)
			{
				Containers.Remove(obj);
			}

			internal void RemoveItemStack(ItemStack obj)
			{
				ItemStacks.Remove(obj);
			}
		}

		private static DataContainer dataContainer = new DataContainer();

		public string Path { get; private set; }

		public DbFileHandler(string path)
		{
			Path = path;
			Load();
		}

		public void Create(DbObject obj)
		{
			if (obj == null)
			{
				throw new InvalidOperationException("Object is null");
			}

			var dataContract = obj.GetType().GetCustomAttributes(typeof(DbObjectAttribute), true).FirstOrDefault();

			if (dataContract == null)
			{
				throw new InvalidOperationException("Object is missing DbObject attribute");
			}

			obj.Id = Guid.NewGuid();

			if (obj is ItemStack)
			{
				dataContainer.AddItemStacks((ItemStack)obj);
			}

			if (obj is Container)
			{
				dataContainer.AddContainer((Container)obj);
			}

			Save();
		}

		private void Load()
		{
			using (FileStream fs = new FileStream(Path, FileMode.OpenOrCreate))
			{
				// Deserialize the data and read it from the instance.
				byte[] bytes = new byte[fs.Length];
				int numBytesToRead = (int)fs.Length;
				int numBytesRead = 0;
				while (numBytesToRead > 0)
				{
					// Read may return anything from 0 to numBytesToRead.
					int n = fs.Read(bytes, numBytesRead, numBytesToRead);

					// Break when the end of the file is reached.
					if (n == 0)
						break;

					numBytesRead += n;
					numBytesToRead -= n;
				}
				numBytesToRead = bytes.Length;
				DataContainer loadedData = JsonConvert.DeserializeObject<DataContainer>(
					Encoding.UTF8.GetString(bytes),
					new JsonSerializerSettings
					{
						DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
					});

				if (loadedData == null)
				{
					return;
				}

				dataContainer = loadedData;
			}
		}

		public object Load(Guid id)
		{
			Container c = dataContainer.Containers.FirstOrDefault(x => x.Id == id);

			if (c != null)
			{
				return c;
			}

			return dataContainer.ItemStacks.FirstOrDefault(x => x.Id == id);
		}

		public void Save()
		{
			using (FileStream fs = new FileStream(Path, FileMode.Create))
			{
				string t = JsonConvert.SerializeObject(
					dataContainer,
					Formatting.Indented,
					new JsonSerializerSettings
					{
						DefaultValueHandling = DefaultValueHandling.Ignore
					});
				fs.Write(Encoding.UTF8.GetBytes(t), 0, Encoding.UTF8.GetByteCount(t));
			}
		}

		public void Delete(DbObject obj)
		{
			if (obj == null)
			{
				throw new InvalidOperationException("Object is null");
			}

			var dataContract = obj.GetType().GetCustomAttributes(typeof(DbObjectAttribute), true).FirstOrDefault();

			if (dataContract == null)
			{
				throw new InvalidOperationException("Object is missing DbObject attribute");
			}

			if (obj is ItemStack)
			{
				dataContainer.RemoveItemStack((ItemStack)obj);
			}

			if (obj is Container)
			{
				dataContainer.RemoveContainer((Container)obj);
			}

			Save();
		}
	}
}