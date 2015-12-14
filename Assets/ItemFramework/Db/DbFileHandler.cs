using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Guid = System.Guid;
using InvalidOperationException = System.InvalidOperationException;

namespace ItemFramework.Db
{
	/// <summary>
	/// DbHandler for databases saved as JSON file
	/// </summary>
	public class DbFileHandler : IDbHandler
	{
		/// <summary>
		/// Container for all data
		/// </summary>
		[DbObject("data")]
		private class DataContainer
		{
			[DbProperty("containers")]
			internal List<Container> Containers = new List<Container>();
			[DbProperty("itemstacks")]
			public List<ItemStack> ItemStacks = new List<ItemStack>();

			public DataContainer() { }

			/// <summary>
			/// Add Container to DataContainer
			/// </summary>
			/// <param name="container">Container to be added</param>
			public void AddContainer(Container container)
			{
				Containers.Add(container);
				// Add the Container's content
				AddItemStacks(container.Items);
			}

			/// <summary>
			/// Add ItemStacks to DataContainer
			/// </summary>
			/// <param name="itemStacks">ItemStacks to be added</param>
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

			/// <summary>
			/// Add ItemStacks to DataContainer
			/// </summary>
			/// <param name="itemStacks">ItemStacks to be added</param>
			public void AddItemStacks(params ItemStack[] itemStacks)
			{
				foreach (ItemStack itemStack in itemStacks)
				{
					ItemStacks.Add(itemStack);
				}
			}

			/// <summary>
			/// Remove Container from DataContainer
			/// </summary>
			/// <param name="container">Container to be removed</param>
			internal void RemoveContainer(Container container)
			{
				Containers.Remove(container);
			}

			/// <summary>
			/// Remove ItemStack from DataContainer
			/// </summary>
			/// <param name="itemStack">ItemStack to be removed</param>
			internal void RemoveItemStack(ItemStack itemStack)
			{
				ItemStacks.Remove(itemStack);
			}
		}

		private static DataContainer dataContainer = new DataContainer();

		/// <summary>
		/// Path to file
		/// </summary>
		public string Path { get; private set; }

		/// <summary>
		/// Create new DbFileHandler and load data
		/// </summary>
		/// <param name="path">Path to file</param>
		public DbFileHandler(string path)
		{
			Path = path;
			Load();
		}

		/// <summary>
		/// Create DbObject in database
		/// </summary>
		/// <param name="obj">DbObject to create</param>
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

		/// <summary>
		/// Load data from database
		/// </summary>
		private void Load()
		{
			using (FileStream fs = new FileStream(Path, FileMode.OpenOrCreate))
			{
				// Read the file
				byte[] bytes = new byte[fs.Length];
				int numBytesToRead = (int)fs.Length;
				int numBytesRead = 0;

				while (numBytesToRead > 0)
				{
					int n = fs.Read(bytes, numBytesRead, numBytesToRead);

					// Break if file is read
					if (n == 0)
					{
						break;
					}

					numBytesRead += n;
					numBytesToRead -= n;
				}

				numBytesToRead = bytes.Length;

				// Deserialize the data
				DataContainer loadedData = JsonConvert.DeserializeObject<DataContainer>(
					Encoding.UTF8.GetString(bytes),
					new JsonSerializerSettings
					{
						DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
					});

				// If the data couldn't be deserialized
				if (loadedData == null)
				{
					return;
				}

				dataContainer = loadedData;
			}
		}

		/// <summary>
		/// Load a specific object from the database
		/// </summary>
		/// <param name="id">Id of object</param>
		/// <returns>Object if found; otherwise null</returns>
		public object Load(Guid id)
		{
			// Look for object in Containers
			Container c = dataContainer.Containers.FirstOrDefault(x => x.Id == id);
			
			// If object found, return
			if (c != null)
			{
				return c;
			}

			// Look and return for object in ItemStack
			return dataContainer.ItemStacks.FirstOrDefault(x => x.Id == id);
		}

		/// <summary>
		/// Save the database to file
		/// </summary>
		public void Save()
		{
			using (FileStream fs = new FileStream(Path, FileMode.Create))
			{
				string t = JsonConvert.SerializeObject(
					dataContainer,
					new JsonSerializerSettings
					{
						DefaultValueHandling = DefaultValueHandling.Ignore
					});

				fs.Write(Encoding.UTF8.GetBytes(t), 0, Encoding.UTF8.GetByteCount(t));
			}
		}

		/// <summary>
		/// Remove object from database
		/// </summary>
		/// <param name="obj">DbObject to remove</param>
		public void Remove(DbObject obj)
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

		/// <summary>
		/// Remove all objects from database
		/// </summary>
		public void Clear()
		{
			dataContainer = new DataContainer();
			Save();
		}
	}
}