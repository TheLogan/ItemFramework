using System;
using System.Collections.Generic;

namespace ItemFramework
{
	public interface IMod
	{
		Dictionary<string, Type> RegisterItems();
		Type[] RegisterRecipes();
	}
}
