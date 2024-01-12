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
                .Where(x => !itemsAlreadyInStock.Contains(x.Id))
                .Distinct()
                .ToList();

            // 1/10 chance of there being a better item in stock
            if (Globals.RNGGetNextBoolean(10, ShopRNG)) 
            {
                validItems = ItemList.GetItemsAtDifficulty(ObtainingDifficulties.MediumTimeRequirements)
                    .Concat(ItemList.GetItemsAtDifficulty(ObtainingDifficulties.LargeTimeRequirements))
                    .Where(x => !itemsAlreadyInStock.Contains(x.Id))
                    .Distinct()
                    .ToList();
            }

            // Select an item to be the item of the week
            // Do this here so the next check
            Item item = validItems[ShopRNG.Next(validItems.Count)];
            ISalable itemOfTheWeek = item.GetSaliableObject();

            // Certain items don't have a salePrice or it is too low
            // Triple that price because there's infinite stock
            int salePrice = GetAdjustedItemPrice(itemOfTheWeek, fallbackPrice: 20, multiplier: 3);
            InsertStockAt(menu, itemOfTheWeek, salePrice: salePrice);
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

            SVObject clay = new((int)ObjectIndexes.Clay, 1);
            InsertStockAt(menu, clay, salePrice: clayPrice, index: 2);
        }

        /// <summary>
        /// The saloon shop will be mostly random now - cycling every Monday
        /// - Beer and Coffee will still be available
        /// - 3-5 random cooked foods will be sold
        /// - 3-5 random recipes will be sold (not shown if the player has them)
        /// </summary>
        /// <param name="menu">The shop menu</param>
        public static void AdjustSaloonShopStock(ShopMenu menu)
        {
            if (!Globals.Config.Shops.RandomizeSaloonShop)
            {
                return;
            }

            // Stock will change every Monday
            Random ShopRNG = Globals.GetWeeklyRNG();
            EmptyStock(menu);

            // Beer and coffee will always be available
            SVObject beer = new((int)ObjectIndexes.Beer, 1);
            SVObject coffee = new((int)ObjectIndexes.Coffee, 1);

            AddStock(menu, beer);
            AddStock(menu, coffee);

            // Random Cooked Items - pick 3-5 random dishes each week
            var numberOfCookedItems = Range.GetRandomValue(3, 5, ShopRNG);
            List<Item> gusFoodList = Globals.RNGGetRandomValuesFromList(ItemList.GetCookedItems(), numberOfCookedItems, ShopRNG);
            gusFoodList.ForEach(cookedItem => 
                AddStock(menu, cookedItem.GetSaliableObject())
            );

            // Random Cooking Recipes - pick 3-5 random recipes each week
            var numberOfRecipes = Range.GetRandomValue(3, 5, ShopRNG);
            List<Item> gusRecipeList = Globals.RNGGetRandomValuesFromList(ItemList.GetCookedItems(), numberOfRecipes, ShopRNG);
            gusRecipeList.ForEach(recipeItem =>
            {
                ISalable recipe = recipeItem.GetSaliableObject(isRecipe: true);

                // Don't add if player already knows recipe
                string recipeName = recipe.Name[..(recipe.Name.IndexOf("Recipe") - 1)];
                if (!Game1.player.cookingRecipes.ContainsKey(recipeName))
                {
                    AddStock(menu, recipe, stock: 1);
                }
            });
        }

        /// <summary>
        /// Adjust the oasis shop stock to sell late game items, cycling every Monday
        /// </summary>
        /// <param name="menu">The shop menu</param>
        public static void AdjustOasisShopStock(ShopMenu menu)
        {
            if (!Globals.Config.Shops.RandomizeOasisShop)
            {
                return;
            }

            // Stock will change every Monday
            Random ShopRNG = Globals.GetWeeklyRNG();

            // If a coconut was stocked, replace it with a random desert foragable
            if (RemoveFromStock(menu, (int)ObjectIndexes.Coconut))
            {
                var desertForagable = Globals.RNGGetRandomValueFromList(ItemList.GetUniqueDesertForagables(), ShopRNG);
                ISalable svDesertForagable = desertForagable.GetSaliableObject();
                int salePrice = GetAdjustedItemPrice(svDesertForagable, fallbackPrice: 50, multiplier: 4);

                InsertStockAt(menu, svDesertForagable, salePrice: salePrice, index: 4);
            }

            // If a cactus fruit was stocked, replace it with a random food that she sells the seeds for
            if (RemoveFromStock(menu, (int)ObjectIndexes.CactusFruit))
            {
                var desertShopCrops = menu.itemPriceAndStock.Keys
                    .Where(item =>
                        item is SVObject &&
                        ItemList.Items.ContainsKey((item as SVObject).ParentSheetIndex) &&
                        ItemList.Items[(item as SVObject).ParentSheetIndex].IsSeed)
                    .Select(item => ItemList.Items[(item as SVObject).ParentSheetIndex] as SeedItem)
                    .Select(item => ItemList.Items[item.CropGrowthInfo.CropId])
                    .ToList();

                var desertCrop = Globals.RNGGetRandomValueFromList(desertShopCrops, ShopRNG);
                ISalable svDesertCrop = desertCrop.GetSaliableObject();
                int salePrice = GetAdjustedItemPrice(svDesertCrop, fallbackPrice: 50, multiplier: 4);

                InsertStockAt(menu, svDesertCrop, salePrice: salePrice, index: 4);
            }

            // Also include a couple of random useful items
            var existingItems = menu.itemPriceAndStock.Keys
                .Where(item => item is SVObject)
                .Select(item => (item as SVObject).ParentSheetIndex)
                .ToList();
            var craftableItems = ItemList.GetCraftableItems(CraftableCategories.Moderate, existingItems)
                .ToList();
            var craftableItem = Globals.RNGGetRandomValueFromList(craftableItems, ShopRNG);
            var resourceItem = ItemList.GetRandomResourceItem(
                existingItems.Concat(craftableItems.Select(item => item.Id)).ToArray(), ShopRNG);

            ISalable svCraftableItem = craftableItem.GetSaliableObject();
            int craftableSalePrice = GetAdjustedItemPrice(svCraftableItem, fallbackPrice: 50, multiplier: 2);
            InsertStockAt(menu, svCraftableItem, salePrice: craftableSalePrice, index: 5);

            ISalable svResourceItem = resourceItem.GetSaliableObject();
            int resourceSalePrice = GetAdjustedItemPrice(svResourceItem, fallbackPrice: 30, multiplier: 2);
            InsertStockAt(menu, svResourceItem, salePrice: resourceSalePrice, index: 6);
        }

        /// <summary>
        /// Adds the given item to the given shop menu
        /// </summary>
        /// <param name="menu">The shop menu<param>
        /// <param name="item">The item to add</param>
        /// <param name="stock">The amount to sell - defaults to max</param>
        /// <param name="salePrice">The amount to sell at - defaults to the item's sale price</param>
        private static void AddStock(
            ShopMenu menu,
            ISalable item, 
            int stock = int.MaxValue,
            int? salePrice = null)
        {
            AddToItemPriceAndStock(menu, item, stock, salePrice);
            menu.forSale.Add(item);
        }

        /// <summary>
        /// Inserts the given item at the given position - defaults to the first index (0)
        /// </summary>
        /// <param name="menu">The shop menu<param>
        /// <param name="item">The item to add</param>
        /// <param name="stock">The amount to sell - defaults to max</param>
        /// <param name="salePrice">The amount to sell at - defaults to the item's sale price</param>
        /// <param name="index">The index to insert the item to (where it will show up in the shop menu)</param>
        private static void InsertStockAt(
            ShopMenu menu,
            ISalable item, 
            int stock = int.MaxValue,
            int? salePrice = null,
            int index = 0)
        {
            AddToItemPriceAndStock(menu, item, stock, salePrice);
            menu.forSale.Insert(index, item);
        }

        /// <summary>
        /// Adds the item to the menu's itemPriceAndStock dictionary
        /// </summary>
        /// <param name="menu">The shop menu<param>
        /// <param name="item">The item to add</param>
        /// <param name="stock">The amount to sell - defaults to max</param>
        /// <param name="salePrice">The amount to sell at - defaults to the item's sale price</param>
        private static void AddToItemPriceAndStock(
            ShopMenu menu, 
            ISalable item, 
            int stock = int.MaxValue, 
            int? salePrice = null)
        {
            var price = salePrice ?? item.salePrice();
            menu.itemPriceAndStock.Add(item, new[] { price, stock });
        }

        /// <summary>
        /// Empty a given shop's stock
        /// </summary>
        /// <param name="menu">The menu of the shop</param>
        private static void EmptyStock(ShopMenu menu)
        {
            while (menu.itemPriceAndStock.Any())
            {
                ISalable obj = menu.itemPriceAndStock.Keys.ElementAt(0);
                menu.itemPriceAndStock.Remove(obj);
                menu.forSale.Remove(obj);
            }
        }

        /// <summary>
        /// Removes an item fom the shop stock
        /// </summary>
        /// <param name="menu">The menu of the shop</param>
        /// <param name="itemId">The item to remove</param>
        /// <returns>Whether an item was removed</returns>
        private static bool RemoveFromStock(ShopMenu menu, int itemId)
        {
            var itemToRemove = menu.itemPriceAndStock.Keys
                .Where(item => item is SVObject && (item as SVObject).ParentSheetIndex == itemId)
                .FirstOrDefault();

            if (itemToRemove != null)
            {
                menu.itemPriceAndStock.Remove(itemToRemove);
                menu.forSale.Remove(itemToRemove);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a new price for the item
        /// Takes the greater of the item's price and the fallback price and multiplies them by
        /// the multiplier and the difficulty level
        /// </summary>
        /// <param name="item">The item to get the price for</param>
        /// <param name="fallbackPrice">The price to use if the item costs too little</param>
        /// <param name="multiplier">The multiplier</param>
        /// <returns>The new item price</returns>
        private static int GetAdjustedItemPrice(ISalable item, int fallbackPrice, int multiplier)
        {
            return Math.Max(fallbackPrice, (int)(item.salePrice() * Game1.MasterPlayer.difficultyModifier)) * multiplier;
        }
    }
}
