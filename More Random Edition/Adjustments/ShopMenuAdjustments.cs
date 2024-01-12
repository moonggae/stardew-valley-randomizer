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
                AddStock(menu, new(cookedItem.Id, 1))
            );

            // Random Cooking Recipes - pick 3-5 random recipes each week
            var numberOfRecipes = Range.GetRandomValue(3, 5, ShopRNG);
            List<Item> gusRecipeList = Globals.RNGGetRandomValuesFromList(ItemList.GetCookedItems(), numberOfRecipes, ShopRNG);
            gusRecipeList.ForEach(recipeItem =>
            {
                SVObject recipe = new(recipeItem.Id, 1, isRecipe: true);

                // Don't add if player already knows recipe
                string recipeName = recipe.Name[..(recipe.Name.IndexOf("Recipe") - 1)];
                if (!Game1.player.cookingRecipes.ContainsKey(recipeName))
                {
                    AddStock(menu, recipe, stock: 1);
                }
            });
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
            SVObject item, 
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
            SVObject item, 
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
            SVObject item, 
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
    }
}
