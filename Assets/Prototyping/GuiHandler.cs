using ItemFramework;
using UnityEngine;
using UnityEngine.UI;

public class GuiHandler : MonoBehaviour
{
    public Text TinText;
    public Text CopperText;
    public Text BronzeText;
    public Furnace FurnaceScript;

    public void AddItems()
    {
        FurnaceScript.input.Add(new ItemStack(new ItemCopperOre(), 7));
        FurnaceScript.input.Add(new ItemStack(new ItemTinOre(), 1));
    }

    void Update()
    {
        TinText.text = FurnaceScript.input.Contains(typeof(ItemTinOre)).ToString();
        CopperText.text = FurnaceScript.input.Contains(typeof(ItemCopperOre)).ToString();
        BronzeText.text = FurnaceScript.output.Contains(typeof(ItemBronzeIngot)).ToString();
    }
}
