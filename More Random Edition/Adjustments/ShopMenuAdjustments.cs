using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using SVObject = StardewValley.Object;

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
                var test = sapling.Key as SVObject;
                menu.itemPriceAndStock[sapling.Key] = new[] { sapling.Key.salePrice(), _maxValue };
            }
        }

        /// <summary>
        /// Adds an item of the week to Pierre's shop, refershing every Monday
        /// Consists of a more expensive than usual item has a small chance of being hard to get
        /// </summary>
        /// <param name="menu">The shop menu</param>
        public static void AddSeedShopItemOfTheWeek(ShopMenu menu)
        {
            if (!Globals.Config.Shops.AddSeedShopItemOfTheWeek)
            {
                return;
            }
                
            Random ShopRNG = Globals.GetWeeklyRNG();

            // Build list of possible items
            var itemsAlreadyInStock = menu.itemPriceAndStock.Keys
                .Where(shopKey => shopKey is SVObject)
                .Select(item => (item as SVObject).ParentSheetIndex)
                .ToList();

            var validItems = ItemList.GetCraftableItems(CraftableCategories.Easy)
                .Concat(ItemList.GetCraftableItems(CraftableCategories.EasyAndNeedMany))
                .Concat(ItemList.GetItemsBelowDifficulty(ObtainingDifficulties.MediumTimeRequirements))
                .Where(x => x.Id >= 0 && !itemsAlreadyInStock.Contains(x.Id)) // Filters out BigCraftables and those that are already in stock
                .Distinct()
                .ToList();

            // 1/10 chance of there being a better item in stock
            if (Globals.RNGGetNextBoolean(10, ShopRNG)) 
            {
                validItems = ItemList.GetItemsAtDifficulty(ObtainingDifficulties.MediumTimeRequirements)
                    .Concat(ItemList.GetItemsAtDifficulty(ObtainingDifficulties.LargeTimeRequirements))
                    .Where(x => x.Id >= 0 && !itemsAlreadyInStock.Contains(x.Id))
                    .Distinct()
                    .ToList();
            }

            // Select an item to be the item of the week
            // Do this here so the next check
            Item item = validItems[ShopRNG.Next(validItems.Count)];
            SVObject itemOfTheWeek = new(item.Id, 1);

            // Certain items don't have a salePrice or it is too low
            // Triple that price because there's infinite stock
            int salePrice = Math.Max(20, (int)(itemOfTheWeek.salePrice() * Game1.MasterPlayer.difficultyModifier)) * 3;

            menu.itemPriceAndStock.Add(itemOfTheWeek, new[] { salePrice, _maxValue });
            menu.forSale.Insert(0, itemOfTheWeek);
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

        /// <summary>
        /// Adds Clay to Robin's shop since it's really grindy to get
        /// Add some randomness to the price each day (between 25-75 coins each)
        /// </summary>
        /// <param name="menu">The shop menu</param>
        public static void AddClayToCarpenterShop(ShopMenu menu)
        {
            if (!Globals.Config.Shops.AddClayToRobinsShop)
            {
                return;
            }

            Random ShopRNG = Globals.GetDailyRNG();
            var basePrice = 50;
            var clayPrice = Globals.RNGGetIntWithinPercentage(basePrice, 50, ShopRNG);

            SVObject clay = new(Vector2.Zero, (int)ObjectIndexes.Clay, _maxValue);
            menu.itemPriceAndStock.Add(clay, new[] { clayPrice, _maxValue });
            menu.forSale.Insert(2, clay);
        }
    }
}
