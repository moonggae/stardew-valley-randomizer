using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	/// <summary>
	/// Randomizes the crafting recipes
	/// </summary>
	public class CraftingRecipeRandomizer
	{
		/// <summary>
		/// Randomizes the crafting recipes
		/// Includes two non-item recipes to transmute metals
		/// </summary>
		/// <returns>The dictionary of changes to make to the asset</returns>
		public static Dictionary<string, string> Randomize()
		{
			if (Globals.Config.CraftingRecipes.Randomize)  
			{ 
				Globals.SpoilerWrite($"==== CRAFTING RECIPES ===="); 
			}

			Dictionary<string, string> replacements = new();
			var allCraftableItems = ItemList.Items.Values
				.Concat(ItemList.BigCraftableItems.Values)
				.Where(x => x.IsCraftable)
				.Cast<CraftableItem>()
				.ToList();
			foreach (CraftableItem item in allCraftableItems)
			{
                replacements[item.CraftingRecipeKey] = item.GetCraftingString();
			}

			// These two are not actually items, but we want to randomize their recipes anwyway
			// The IDs passed in don't really matter
			const string TransmuteIronName = "Transmute (Fe)";
            const string TransmuteGoldName = "Transmute (Au)";
            replacements[TransmuteIronName] = new CraftableItem(
				-1000, 
				CraftableCategories.Moderate, 
				dataKey: TransmuteIronName).GetCraftingString(TransmuteIronName);
            replacements[TransmuteGoldName] = new CraftableItem(
				-1000, 
				CraftableCategories.Moderate, 
				dataKey: TransmuteGoldName).GetCraftingString(TransmuteGoldName);

            if (Globals.Config.CraftingRecipes.Randomize) 
			{ 
				Globals.SpoilerWrite(""); 
			}

			return replacements;
		}
	}
}
