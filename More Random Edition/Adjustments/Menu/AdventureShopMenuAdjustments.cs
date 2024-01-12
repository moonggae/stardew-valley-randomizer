using StardewValley.Menus;
using System.Linq;

namespace Randomizer
{
    internal class AdventureShopMenuAdjustments : ShopMenuAdjustments
    {
        /// <summary>
        /// Fixes sale prices for randomized gear so that nothing sells for more than it's worth
        /// </summary>
        /// <param name="menu">The shop menu</param>
        public static void FixPrices(ShopMenu menu)
        {
            menu.itemPriceAndStock = menu.itemPriceAndStock.ToDictionary(
                item => item.Key,
                item => new[] { item.Key.salePrice(), _maxValue }
            );
        }
    }
}
