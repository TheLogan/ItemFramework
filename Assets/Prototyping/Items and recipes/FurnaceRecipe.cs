using ItemFramework;

public abstract class FurnaceRecipe : CraftingRecipe
{
	private int progressTime;
	public int ProgressTime
	{
		get { return progressTime; }
		protected set { progressTime = value; }
	}
}