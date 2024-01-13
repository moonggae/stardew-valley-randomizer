using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using SVObject = StardewValley.Object;

namespace Randomizer
{
    internal class OasisShopMenuAdjustments : ShopMenuAdjustments
    {
        /// <summary>
        /// Adjust the oasis shop stock to sell more exotic items, including furnature
        /// Has some logic based on the day of the week
        /// </summary>
        /// <param name="menu">The shop menu</param>
        public static void AdjustStock(ShopMenu menu)
        {
            if (!Globals.Config.Shops.RandomizeOasisShop)
            {
                return;
            }

            // Track the seeds so we can add them back and add the matching crop every Tuesday
            var desertShopSeeds = menu.itemPriceAndStock.Keys
                .Where(item =>
                    item is SVObject &&
                    ItemList.Items.ContainsKey((item as SVObject).ParentSheetIndex) &&
                    ItemList.Items[(item as SVObject).ParentSheetIndex].IsSeed)
                .Select(item => ItemList.Items[(item as SVObject).ParentSheetIndex] as SeedItem)
                .ToList();

            EmptyStock(menu);

            // Most of the stock will change every Monday, with a couple exceptions
            Random weeklyShopRNG = Globals.GetWeeklyRNG();
            Random dailyShopRNG = Globals.GetDailyRNG();

            AddStock(menu, desertShopSeeds.Cast<Item>().ToList());
            AddDaySpecificItems(menu, desertShopSeeds, weeklyShopRNG);
            AddRandomItems(menu, weeklyShopRNG);
            AddClothingAndFurnatureItems(menu, weeklyShopRNG, dailyShopRNG);
        }

        /// <summary>
        /// Adds... 
        /// - a random desert foragable every weekday
        /// - a crop corresponding to the seeds sold here every Tuesday
        /// - a random cooked item every weekend
        /// All refreshing on Monday
        /// </summary>
        /// <param name="menu">The menu</param>
        /// <param name="desertShopSeeds">The list of seeds normally sold in this shop - used to get the corresponding crop</param>
        /// <param name="weeklyShopRNG">The weekly RNG</param>
        private static void AddDaySpecificItems(ShopMenu menu, List<SeedItem> desertShopSeeds, Random weeklyShopRNG)
        {
            var desertShopCrops = desertShopSeeds
                .Select(item => ItemList.Items[item.CropGrowthInfo.CropId])
                .ToList();

            // Perform these first so the seed doesn't change on different days of the week
            var desertForagable = Globals.RNGGetRandomValueFromList(ItemList.GetUniqueDesertForagables(), weeklyShopRNG);
            var desertCrop = Globals.RNGGetRandomValueFromList(desertShopCrops, weeklyShopRNG);
            var cookedItem = Globals.RNGGetRandomValuesFromList(ItemList.GetCookedItems(), 1, weeklyShopRNG);

            var gameDay = DayFunctions.GetCurrentDay();
            if (DayFunctions.IsWeekday(gameDay)) {
                
                int foragablePrice = GetAdjustedItemPrice(desertForagable, fallbackPrice: 50, multiplier: 4);
                AddStock(menu, desertForagable, salePrice: foragablePrice);
            }

            if (gameDay == Days.Tuesday)
            {
                int salePrice = GetAdjustedItemPrice(desertCrop, fallbackPrice: 50, multiplier: 4);
                AddStock(menu, desertCrop, salePrice: salePrice);
            }

            if (DayFunctions.IsWeekend(gameDay))
            {
                AddStock(menu, cookedItem);
            }
        }

        /// <summary>
        /// Adds...
        /// - a random craftable item in the moderate category
        /// - a random recource item
        /// </summary>
        /// <param name="menu">The menu</param>
        /// <param name="weeklyShopRNG">The weekly RNG</param>
        private static void AddRandomItems(ShopMenu menu, Random weeklyShopRNG)
        {
            var existingItems = menu.itemPriceAndStock.Keys
               .Where(item => item is SVObject)
               .Select(item => (item as SVObject).ParentSheetIndex)
               .ToList();
            var craftableItems = ItemList.GetCraftableItems(CraftableCategories.Moderate, existingItems)
                .ToList();
            var craftableItem = Globals.RNGGetRandomValueFromList(craftableItems, weeklyShopRNG);
            var resourceItem = ItemList.GetRandomResourceItem(
                existingItems.Concat(craftableItems.Select(item => item.Id)).ToArray(), weeklyShopRNG);

            int craftableSalePrice = GetAdjustedItemPrice(craftableItem, fallbackPrice: 50, multiplier: 2);
            int resourceSalePrice = GetAdjustedItemPrice(resourceItem, fallbackPrice: 30, multiplier: 4);
            AddStock(menu, craftableItem, salePrice: craftableSalePrice);
            AddStock(menu, resourceItem, salePrice: resourceSalePrice);
        }

        /// <summary>
        /// Adds...
        /// - a daily clothing item
        /// - a daily furnature item
        /// - 4 weekly furniture items
        /// </summary>
        /// <param name="menu">The menu</param>
        /// <param name="weeklyShopRNG">The weekly RNG</param>
        /// <param name="dailyShopRNG">The daily RNG</param>
        private static void AddClothingAndFurnatureItems(ShopMenu menu, Random weeklyShopRNG, Random dailyShopRNG)
        {
            var dailyClothingItem = ItemList.GetRandomClothingToSell(dailyShopRNG, numberToGet: 1);
            var dailyFurnitureItem = ItemList.GetRandomFurnitureToSell(dailyShopRNG, numberToGet: 1);
            var weeklyFurnitureItems = ItemList.GetRandomFurnitureToSell(
                weeklyShopRNG,
                numberToGet: 4,
                new List<int>() { (dailyFurnitureItem.First() as SVObject).ParentSheetIndex }
            );

            AddStock(menu, dailyClothingItem.Concat(dailyFurnitureItem).Concat(weeklyFurnitureItems).ToList());
        }
    }
}
