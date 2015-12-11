using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ItemFramework
{
	public class FrameworkRegistry
	{
		private static Dictionary<string, Item> registryItems = new Dictionary<string, Item>();
		private static List<Type> loadedMods = new List<Type>();

		public static Item GetItem(string identifier)
		{
			if (!registryItems.ContainsKey(identifier))
			{
				Debug.WriteLine("Item is not registered - " + identifier);
				return null;
			}
			return registryItems[identifier];
		}

		public static void RegisterMod(IMod mod)
		{
			if (loadedMods.Contains(mod.GetType()))
			{
				Debug.WriteLine("Mod already loaded - " + mod.ToString());
				return;
			}

			RegisterItems(mod.RegisterItems());
			RegisterRecipes(mod.RegisterRecipes());
			loadedMods.Add(mod.GetType());
		}

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
