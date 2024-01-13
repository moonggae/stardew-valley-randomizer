using StardewValley.Menus;
using StardewValley.Objects;
using System;
using System.Linq;
using SVObject = StardewValley.Object;

namespace Randomizer
{
    internal class CarpenterShopMenuAdjustments : ShopMenuAdjustments
    {
        /// <summary>
        /// Adds Clay to Robin's shop since it's really grindy to get
        /// Add some randomness to the price each day (between 25-75 coins each)
        /// </summary>
        /// <param name="menu">The shop menu</param>
        public static void AddClay(ShopMenu menu)
        {
            if (!Globals.Config.Shops.AddClayToRobinsShop)
            {
                return;
            }

            Random shopRNG = Globals.GetDailyRNG();
            var basePrice = 50;
            var clayPrice = Globals.RNGGetIntWithinPercentage(basePrice, 50, shopRNG);

            SVObject clay = new((int)ObjectIndexes.Clay, 1);
            InsertStockAt(menu, clay, salePrice: clayPrice, index: 2);
        }

        /// <summary>
        /// Adds a random item to the shop that is required to make a tapper - changes daily
        /// </summary>
        /// <param name="menu">The shop menu</param>
        public static void AddRandomTapperCraftItem(ShopMenu menu)
        {
            if (!Globals.Config.Shops.AddTapperCraftItemsToRobinsShop)
            {
                return;
            }

            var exitingStockIds = menu.itemPriceAndStock.Keys
                .Where(item => item is SVObject)
                .Select(item => (item as SVObject).ParentSheetIndex)
                .ToList();

            Random shopRNG = Globals.GetDailyRNG();
            var tapperItemIds = ((CraftableItem)ItemList.Items[(int)ObjectIndexes.Tapper]).LastRecipeGenerated.Keys;
            var tapperItems = tapperItemIds
                .Select(id => ItemList.Items[id])
                .Where(item => !exitingStockIds.Contains(item.Id))
                .ToList();

            if (tapperItems.Any())
            {
                var tapperItemToSell = Globals.RNGGetRandomValueFromList(tapperItems, shopRNG);
                var price = GetAdjustedItemPrice(tapperItemToSell, 50, 5);
                InsertStockAt(menu, tapperItemToSell, salePrice: price, index: 2);
            }
        }
    }
}
