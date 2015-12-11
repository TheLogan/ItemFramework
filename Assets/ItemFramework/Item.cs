namespace ItemFramework
{
	public abstract class Item
	{
		/// <summary>
		/// Name of Item
		/// </summary>
		public string Name { get; set; }

		private int stackSize = 64;

		/// <summary>
		/// Max ItemStack Amount of Item
		/// </summary>
		public int StackSize
		{
			get
			{
				return stackSize;
			}
			set
			{
				if (value <= 0)
				{
					throw new System.Exception("Trying to set Item StackSize to 0 or less!");
				}

				unchecked
				{
					if (value > System.Int32.MaxValue)
					{
						throw new System.Exception("Trying to set Item StackSize higher than Int32.MaxValue!");
					}
				}

				stackSize = value;
			}
		}

		/// <summary>
		/// Debugable string for Item
		/// </summary>
		/// <returns>Debugable string</returns>
		public override string ToString()
		{
			return "Item[Name=" + Name + "]";
		}

	}
}