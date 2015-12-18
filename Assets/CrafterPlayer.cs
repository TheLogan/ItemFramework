using ItemFramework;

namespace Assets
{
	class CrafterPlayer : Crafter
	{
		public CrafterPlayer()
		{
			input = new Container(4, 2);
			output = new Container(1, 1);
		}
	}
}
