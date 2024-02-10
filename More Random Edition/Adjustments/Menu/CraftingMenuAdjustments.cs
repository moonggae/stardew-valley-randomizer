using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
    public class CraftingMenuAdjustments
	{
		/// <summary>
		/// Reduces the cost of the crab pot
		/// Intended to be used if the player has the Trapper profession
		/// </summary>
		/// <param name="gameMenu">The game menu that needs its cost adjusted</param>
		public static void ReduceCrabPotCost(GameMenu gameMenu)
		{
            const int TrapperProfession = 7;
            if (!Globals.Config.CraftingRecipes.Randomize || 
				!Game1.player.professions.Contains(TrapperProfession)) 
			{ 
				return; 
			}

			CraftingPage craftingPage = (CraftingPage)gameMenu.pages[GameMenu.craftingTab];
			foreach (var page in craftingPage.pagesOfCraftingRecipes)
			{
				foreach (ClickableTextureComponent key in page.Keys)
				{
					CraftingRecipe recipe = page[key];
					if (recipe.name == "Crab Pot")
					{
						CraftableItem crabPot = (CraftableItem)ItemList.Items[ObjectIndexes.CrabPot];
						Dictionary<ObjectIndexes, int> randomizedRecipe = crabPot.LastRecipeGenerated;
						ReduceRecipeCost(page[key], randomizedRecipe);
					}
				}
			}
		}

		/// <summary>
		/// Reduces a recipe's cost
		/// - if everything only needs one of each item, remove the cheapest item
		/// - otherwise - halve the amounts of all items, rounding down (with a min of 1 required)
		/// </summary>
		/// <param name="inGameRecipe">The recipe as stored by Stardew Valley</param>
		/// <param name="randomizedRecipe">The recipe as stored by this mod</param>
		private static void ReduceRecipeCost(
			CraftingRecipe inGameRecipe, 
			Dictionary<ObjectIndexes, int> randomizedRecipe)
		{
            Dictionary<string, int> recipeList = inGameRecipe.recipeList;
			recipeList.Clear();
			if (randomizedRecipe.Values.All(x => x < 2))
			{
				ObjectIndexes firstKeyOfEasiestItem = randomizedRecipe.Keys
					.Select(x => ItemList.Items[x])
					.OrderBy(x => x.DifficultyToObtain)
					.Select(x => (ObjectIndexes)x.Id)
					.First();

				foreach (ObjectIndexes id in randomizedRecipe.Keys.Where(x => x != firstKeyOfEasiestItem))
				{
					var idAsString = ((int)id).ToString();
					recipeList.Add(idAsString, 1);
				}
			}
			else
			{
				foreach (ObjectIndexes id in randomizedRecipe.Keys)
				{
                    var idAsString = ((int)id).ToString();
                    int numberRequired = randomizedRecipe[id];
					recipeList.Add(idAsString, Math.Max(numberRequired / 2, 1));
				}
			}
		}
	}
}