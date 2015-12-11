using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ItemFramework
{
	public class FrameworkRegistry
	{
		/// <summary>
		/// Registry over registered Items
		/// </summary>
		private static Dictionary<string, Item> registryItems = new Dictionary<string, Item>();
		/// <summary>
		/// List of loaded mod types
		/// </summary>
		private static List<Type> loadedModTypes = new List<Type>();

		/// <summary>
		/// Get an Item by it's identifier
		/// </summary>
		/// <param name="identifier">Item identifier</param>
		/// <returns>Item if found; otherwise null</returns>
		public static Item GetItem(string identifier)
		{
			if (!registryItems.ContainsKey(identifier))
			{
				return null;
			}
			return registryItems[identifier];
		}

		/// <summary>
		/// Register mod, if not already registered
		/// </summary>
		/// <param name="mod">Mod to be registered</param>
		public static void RegisterMod(IMod mod)
		{
			if (loadedModTypes.Contains(mod.GetType()))
			{
				return;
			}

			RegisterItems(mod.RegisterItems());
			RegisterRecipes(mod.RegisterRecipes());
			loadedModTypes.Add(mod.GetType());
		}

		/// <summary>
		/// Register Items.
		/// Throws exception if identifier is already in use.
		/// </summary>
		/// <param name="items">Dictionary of identifiers and Items</param>
		private static void RegisterItems(Dictionary<string, Type> items)
		{
			if (items == null) return;
			var e = items.GetEnumerator();
			while (e.MoveNext())
			{
				if (typeof(Item).IsAssignableFrom(e.Current.Value))
				{
					if (registryItems.ContainsKey(e.Current.Key))
					{
						throw new Exception("Item identifier is already in use by " + registryItems[e.Current.Key].ToString());
					}
					Item i = (Item)Activator.CreateInstance(e.Current.Value,true);
					i.Identifier = e.Current.Key;
                    registryItems.Add(e.Current.Key, i);
				}
			}
		}

		private static void RegisterRecipes(Type[] recipes)
		{
			if (recipes == null) return;
			foreach (Type recipe in recipes) {
				if (typeof(Item).IsAssignableFrom(recipe))
				{
					CraftingManager.Instance.Register((CraftingRecipe)Activator.CreateInstance(recipe));
				}
			}
		}
	}
}
