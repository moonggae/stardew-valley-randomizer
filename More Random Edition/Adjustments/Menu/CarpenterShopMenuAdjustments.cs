using StardewValley.Menus;
using System;
using System.Linq;
using SVObject = StardewValley.Object;

namespace Randomizer
{
    internal class CarpenterShopMenuAdjustments : ShopMenuAdjustments
    {
        /// <summary>
        /// Adds clay and tapper craft items
        /// </summary>
        /// <param name="menu">The shop menu</param>
        protected override void Adjust(ShopMenu menu)
        {
            if (!ShouldChangeShop)
            {
                RestoreShopState(menu);
                return;
            }

            if (Globals.Config.Shops.AddClayToRobinsShop)
            {
                AddClay(menu);
            }

            if (Globals.Config.Shops.AddTapperCraftItemsToRobinsShop)
            {
                AddRandomTapperCraftItem(menu);
            }
        }

        /// <summary>
        /// Adds Clay to Robin's shop since it's really grindy to get
        /// Add some randomness to the price each day (between 25-75 coins each)
        /// </summary>
        /// <param name="menu">The shop menu</param>
        private static void AddClay(ShopMenu menu)
        {
            Random shopRNG = Globals.GetDailyRNG(nameof(CarpenterShopMenuAdjustments));
            var basePrice = 50;
            var clayStock = Range.GetRandomValue(20, 50);
            var clayPrice = Globals.RNGGetIntWithinPercentage(basePrice, 50, shopRNG);

            SVObject clay = new((int)ObjectIndexes.Clay, clayStock);
            InsertStockAt(menu, clay, stock: clayStock, salePrice: clayPrice, index: 2);
        }

        /// <summary>
        /// Adds a random item to the shop that is required to make a tapper - changes daily
        /// </summary>
        /// <param name="menu">The shop menu</param>
        private static void AddRandomTapperCraftItem(ShopMenu menu)
        {
            var exitingStockIds = menu.itemPriceAndStock.Keys
                .Where(item => item is SVObject)
                .Select(item => (item as SVObject).ParentSheetIndex)
                .ToList();

            Random shopRNG = Globals.GetDailyRNG(nameof(CarpenterShopMenuAdjustments));
            var tapperItemIdsAndStock = ((CraftableItem)ItemList.Items[(int)ObjectIndexes.Tapper]).LastRecipeGenerated;
            var tapperItems = tapperItemIdsAndStock.Keys
                .Select(id => ItemList.Items[id])
                .Where(item => !exitingStockIds.Contains(item.Id))
                .ToList();

            if (tapperItems.Any())
            {
                var tapperItemToSell = Globals.RNGGetRandomValueFromList(tapperItems, shopRNG);
                var stock = tapperItemIdsAndStock[tapperItemToSell.Id];
                var price = GetAdjustedItemPrice(tapperItemToSell, 50, 5);
                InsertStockAt(menu, tapperItemToSell.GetSaliableObject(stock), stock: stock, salePrice: price, index: 2);
            }
        }
    }
}
