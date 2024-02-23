using StardewValley;
using StardewValley.GameData.FruitTrees;
using StardewValley.GameData.Objects;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
    /// <summary>
    /// Randomizes what fruit trees go
    /// 
    /// For testing, use debug command "fruittrees":
    /// - Instantly grows all fruit trees
    /// - Trees already grown will bear a new fruit if the current season is valid
    /// </summary>
    public class FruitTreeRandomizer
    {
        /// <summary>
        /// Randomize fruit tree information
        /// TODO 1.6: we can make fruit trees that grow multiple different things! Look into what we want to do.
        /// </summary>
        /// <param name="objectReplacements">The object information - for fruit tree names and prices</param>
        public static Dictionary<string, FruitTreeData> Randomize(
            Dictionary<string, ObjectData> objectReplacements)
        {
            Dictionary<string, FruitTreeData> fruitTreeReplacements = new();

            // We aren't randomizing all the fruit trees yet, so hard-code this list for now
            List<int> fruitTreeIds = new()
            {
                (int)ObjectIndexes.CherrySapling,
                (int)ObjectIndexes.ApricotSapling,
                (int)ObjectIndexes.OrangeSapling,
                (int)ObjectIndexes.PeachSapling,
                (int)ObjectIndexes.PomegranateSapling,
                (int)ObjectIndexes.AppleSapling
            };
            List<Item> allPotentialTreesItems = ItemList.Items.Values.Where(x =>
                fruitTreeIds.Contains(x.Id) || x.DifficultyToObtain < ObtainingDifficulties.Impossible
            ).ToList();

            List<Item> treeItems = Globals.RNGGetRandomValuesFromList(allPotentialTreesItems, 6);

            // Choose the fruit tree to make a winter tree
            int idToMakeWinter = Globals.RNGGetRandomValueFromList(fruitTreeIds);

            int[] prices = treeItems.Select(x => x.GetPriceForObtainingDifficulty(0.2)).ToArray();

            if (!Globals.Config.RandomizeFruitTrees) 
            { 
                return fruitTreeReplacements; 
            }

            // The trees are incremented starting with cherry
            // Note that "treeItems" refers to the item the fruit tree will grow
            for (int i = 0; i < treeItems.Count; i++)
            {
                int price = prices[i];
                int fruitTreeId = fruitTreeIds[i];
                Item treeItem = treeItems[i];

                string newDispName = treeItem.Id == fruitTreeIds[i]
                    ? Globals.GetTranslation("item-recursion-sapling-name")
                    : Globals.GetTranslation("sapling-text", new { itemName = treeItem.DisplayName });

                // Modify the fruit tree item with:
                // - the winter season (if applicable)
                // - the id of the grown item
                FruitTreeData fruitTreeToModify = Game1.fruitTreeData[fruitTreeId.ToString()];
                if (fruitTreeId == idToMakeWinter)
                {
                    fruitTreeToModify.Seasons = new List<Season>() { Season.Winter };
                }
                fruitTreeToModify.Fruit[0].ItemId = treeItem.QualifiedId; // This is where we can assign multiple items to grow!
                fruitTreeReplacements[fruitTreeId.ToString()] = fruitTreeToModify;

                // If we change fruit trees to be usable multiple seasons, we'll want to change this string accordingly!
                string season = fruitTreeToModify.Seasons[0].ToString().ToLower();
                string seasonDisplay = Globals.GetTranslation($"seasons-{season}");

                // Replace the fruit tree name/price/description
                ObjectData fruitTreeObject = EditedObjects.DefaultObjectInformation[fruitTreeId.ToString()];
                fruitTreeObject.Price = price / 2;
                fruitTreeObject.DisplayName = newDispName;
                fruitTreeObject.Description = Globals.GetTranslation(
                    "sapling-description",
                    new { itemName = newDispName, season = seasonDisplay });
                objectReplacements[fruitTreeId.ToString()] = fruitTreeObject;
            }

            return fruitTreeReplacements;
        }
    }
}
