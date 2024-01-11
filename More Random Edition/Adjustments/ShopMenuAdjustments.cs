using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer.Adjustments
{
    public class ShopMenuAdjustments
    {
        private static readonly int _maxValue = int.MaxValue;

        /// <summary>
        /// Fix the sapling prices to reflect the actual fruit tree prices
        /// </summary>
        /// <param name="menu">The shop menu</param>
        public static void FixSaplingPrices(ShopMenu menu)
        {
            if (!Globals.Config.RandomizeFruitTrees)
            {
                return;
            }

            var saplingMenuItems = menu.itemPriceAndStock
                .Where(keyValuePair => keyValuePair.Key.Name.Contains("Sapling"))
                .ToList();
            foreach (KeyValuePair<ISalable, int[]> sapling in saplingMenuItems)
            {
                menu.itemPriceAndStock[sapling.Key] = new[] { sapling.Key.salePrice(), _maxValue };
            }
        }

        /// <summary>
        /// Fixes sale prices for randomized gear so that nothing sells for more than it's worth
        /// </summary>
        /// <param name="menu">The shop menu</param>
        public static void FixAdventureShopPrices(ShopMenu menu)
        {
            menu.itemPriceAndStock = menu.itemPriceAndStock.ToDictionary(
                item => item.Key,
                item => new[] { item.Key.salePrice(), _maxValue }
            );
        }
    }
}
