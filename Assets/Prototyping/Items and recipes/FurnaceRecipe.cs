using ItemFramework;

public abstract class FurnaceRecipe : CraftingRecipe
{
	private float progressTime;
	public float ProgressTime
	{
		get { return progressTime; }
		protected set { progressTime = value; }
	}
}