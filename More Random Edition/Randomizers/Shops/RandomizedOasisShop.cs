using StardewValley.GameData.Shops;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
    public class RandomizedOasisShop : RandomizedShop
    {
        public RandomizedOasisShop() : base("Sandy") { }

        /// <summary>
        /// Modifies the shop stock - see AdjustStock for details
        /// </summary>
        /// <returns>The modified shop data</returns>
        public override ShopData ModifyShop()
        {
            AdjustStock();

            return CurrentShopData;
        }

        /// <summary>
        /// Adjust the oasis shop stock to sell more exotic items, including furnature
        /// Has some logic based on the day of the week
        /// </summary>
        private void AdjustStock()
        {
            if (!Globals.Config.Shops.RandomizeOasisShop)
            {
                return;
            }

            // Track the seeds so we can add them back and add the matching crop every Tuesday
            var desertShopSeeds = CurrentShopData.Items
                .Select(shopData => ItemList.GetItemFromStringId(shopData.ItemId))
                .Where(item => item is SeedItem)
                .Cast<SeedItem>()
                .ToList();

            CurrentShopData.Items.Clear();

            // Most of the stock will change every Monday, with a couple exceptions
            Random weeklyShopRNG = Globals.GetWeeklyRNG(nameof(RandomizedOasisShop));
            Random dailyShopRNG = Globals.GetDailyRNG(nameof(RandomizedOasisShop));

            desertShopSeeds.ForEach(seed => AddStock(seed.QualifiedId, $"SeedItem-{seed.QualifiedId}"));
            AddDaySpecificItems(desertShopSeeds, weeklyShopRNG);
            AddRandomItems(weeklyShopRNG);
            AddClothingAndFurnatureItems(weeklyShopRNG, dailyShopRNG);
        }

        /// <summary>
        /// Adds... 
        /// - a random desert foragable every weekday
        /// - a crop corresponding to the seeds sold here every Tuesday
        /// - a random cooked item every weekend
        /// All refreshing on Monday
        /// </summary>
        /// <param name="desertShopSeeds">The list of seeds normally sold in this shop - used to get the corresponding crop</param>
        /// <param name="weeklyShopRNG">The weekly RNG</param>
        private void AddDaySpecificItems(List<SeedItem> desertShopSeeds, Random weeklyShopRNG)
        {
            // Every weekday, add a desert foragable
            var desertForagable = Globals.RNGGetRandomValueFromList(ItemList.GetUniqueDesertForagables(), weeklyShopRNG);
            int foragablePrice = GetAdjustedItemPrice(desertForagable, fallbackPrice: 50, multiplier: 2);
            int foragableStock = Range.GetRandomValue(1, 5, weeklyShopRNG);
            AddStock(desertForagable.QualifiedId, 
                "WeekdayForagable", 
                foragablePrice,
                foragableStock,
                condition: DayFunctions.GetConditionForWeekday());

            // Every Tuesday, add a crop corresponding to the seeds sold here
            var desertShopCrops = desertShopSeeds
                .Select(item => ItemList.Items[(ObjectIndexes)item.CropId])
                .ToList();
            var desertCrop = Globals.RNGGetRandomValueFromList(desertShopCrops, weeklyShopRNG);
            int desertCropPrice = GetAdjustedItemPrice(desertCrop, fallbackPrice: 50, multiplier: 2);
            int desertCropStock = Range.GetRandomValue(3, 8, weeklyShopRNG);
            AddStock(desertCrop.QualifiedId, 
                "TuesdayCrop", 
                desertCropPrice, 
                desertCropStock,
                condition: DayFunctions.GetCondition(Days.Tuesday));

            // Every weekend,, add a random cooked item
            var cookedItem = Globals.RNGGetRandomValueFromList(ItemList.GetCookedItems(), weeklyShopRNG);
            AddStock(cookedItem.QualifiedId, 
                "WeekendFood", 
                condition: DayFunctions.GetConditionForWeekend());
        }

        /// <summary>
        /// Adds...
        /// - a random craftable item in the moderate category
        /// - a random recource item
        /// </summary>
        /// <param name="weeklyShopRNG">The weekly RNG</param>
        private void AddRandomItems(Random weeklyShopRNG)
        {
            var craftableItem = Globals.RNGGetRandomValueFromList(
                ItemList.GetCraftableItems(CraftableCategories.Moderate), 
                weeklyShopRNG);
            int craftableSalePrice = GetAdjustedItemPrice(craftableItem, fallbackPrice: 50, multiplier: 2);
            AddStock(craftableItem.QualifiedId, "CraftableItem", price: craftableSalePrice);

            var resourceItem = ItemList.GetRandomResourceItem(rng: weeklyShopRNG);
            int resourceSalePrice = GetAdjustedItemPrice(resourceItem, fallbackPrice: 30, multiplier: 4);
            AddStock(resourceItem.QualifiedId, "ResourceItem", price: resourceSalePrice);
        }

        /// <summary>
        /// Adds...
        /// - a daily clothing item
        /// - a daily furnature item
        /// - 4 weekly furniture items
        /// </summary>
        /// <param name="weeklyShopRNG">The weekly RNG</param>
        /// <param name="dailyShopRNG">The daily RNG</param>
        private void AddClothingAndFurnatureItems(Random weeklyShopRNG, Random dailyShopRNG)
        {
            var dailyClothingItemId = ClothingFunctions.GetRandomClothingQualifiedId(rng: dailyShopRNG);
            var dailyFurnitureItemId = FurnitureFunctions.GetRandomFurnitureQualifiedId(rng: dailyShopRNG);
            var weeklyFurnitureItemIds = FurnitureFunctions.GetRandomFurnitureQualifiedIds(
                numberToGet: 4,
                new List<string>() { dailyClothingItemId, dailyFurnitureItemId },
                weeklyShopRNG);

            AddStock(dailyClothingItemId, "DailyClothing");
            AddStock(dailyFurnitureItemId, "DailyFurniture");
            weeklyFurnitureItemIds.ForEach(itemId =>
                AddStock(itemId, $"WeeklyFurniture-{itemId}"));
        }
    }
}
