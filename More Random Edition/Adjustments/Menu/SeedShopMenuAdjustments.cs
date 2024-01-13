using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using SVObject = StardewValley.Object;

namespace Randomizer
{
    internal class SeedShopMenuAdjustments : ShopMenuAdjustments
    {
        /// <summary>
        /// Adjusts fruit tree prices and adds the item of the week
        /// </summary>
        /// <param name="menu">The shop menu</param>
        protected override void Adjust(ShopMenu menu)
        {
            if (!ShouldChangeShop) { 
                RestoreShopState(menu); 
                return;
            }

            if (Globals.Config.RandomizeFruitTrees)
            {
                FixSaplingPrices(menu);
            }

            if (Globals.Config.Shops.AddSeedShopItemOfTheWeek)
            {
                AddItemOfTheWeek(menu);
            }
        }

        /// <summary>
        /// Fix the sapling prices to reflect the actual fruit tree prices
        /// </summary>
        /// <param name="menu">The shop menu</param>
        private static void FixSaplingPrices(ShopMenu menu)
        {
            var saplingMenuItems = menu.itemPriceAndStock
                .Where(keyValuePair => keyValuePair.Key.Name.Contains("Sapling"))
                .ToList();
            foreach (KeyValuePair<ISalable, int[]> sapling in saplingMenuItems)
            {
                menu.itemPriceAndStock[sapling.Key] = new[] { sapling.Key.salePrice(), _maxValue };
            }
        }

        /// <summary>
        /// Adds an item of the week to Pierre's shop, refershing every Monday
        /// Consists of a more expensive than usual item has a small chance of being hard to get
        /// </summary>
        /// <param name="menu">The shop menu</param>
        private static void AddItemOfTheWeek(ShopMenu menu)
        {
            Random shopRNG = Globals.GetWeeklyRNG();

            // Build list of possible items
            var itemsAlreadyInStock = menu.itemPriceAndStock.Keys
                .Where(shopKey => shopKey is SVObject)
                .Select(item => (item as SVObject).ParentSheetIndex)
                .ToList();

            var validItems = ItemList.GetCraftableItems(CraftableCategories.Easy)
                .Concat(ItemList.GetCraftableItems(CraftableCategories.EasyAndNeedMany))
                .Concat(ItemList.GetItemsBelowDifficulty(ObtainingDifficulties.MediumTimeRequirements))
                .Where(x => !itemsAlreadyInStock.Contains(x.Id))
                .Distinct()
                .ToList();

            // 1/10 chance of there being a better item in stock
            if (Globals.RNGGetNextBoolean(10, shopRNG))
            {
                validItems = ItemList.GetItemsAtDifficulty(ObtainingDifficulties.MediumTimeRequirements)
                    .Concat(ItemList.GetItemsAtDifficulty(ObtainingDifficulties.LargeTimeRequirements))
                    .Where(x => !itemsAlreadyInStock.Contains(x.Id))
                    .Distinct()
                    .ToList();
            }

            // Select an item to be the item of the week
            // Do this here so the next check
            Item itemOfTheWeek = validItems[shopRNG.Next(validItems.Count)];

            // Certain items don't have a salePrice or it is too low
            // Triple that price because there's infinite stock
            int salePrice = GetAdjustedItemPrice(itemOfTheWeek, fallbackPrice: 20, multiplier: 3);
            InsertStockAt(menu, itemOfTheWeek, salePrice: salePrice);
        }
    }
}