using StardewValley.GameData.Shops;
using System;
using System.Linq;

namespace Randomizer
{
    public class RandomizedJojaMart : RandomizedShop
    {
        public RandomizedJojaMart() : base("Joja") { }

        /// <summary>
        /// Adjusts fruit tree prices and adds the item of the week
        /// </summary>
        /// <returns>The modified shop data</returns>
        public override ShopData ModifyShop()
        {
            AddItemOfTheWeek();

            return CurrentShopData;
        }

        /// <summary>
        /// Adds an item of the week to Pierre's shop, refreshing every Monday
        /// Consists of a more expensive than usual item has a small chance of being hard to get
        /// </summary>
        private void AddItemOfTheWeek()
        {
            if (!Globals.Config.Shops.AddJojaMartItemOfTheWeek)
            {
                return;
            }

            Random shopRNG = Globals.GetWeeklyRNG(nameof(RandomizedJojaMart));

            // Don't choose pre-existing items
            var itemsAlreadyInStock = CurrentShopData.Items
                .Select(item => item.ItemId)
                .ToList();

            // 1/10 chance of there being a better item in stock
            var validItems = Globals.RNGGetNextBoolean(10, shopRNG)
                ? ItemList.GetItemsAtDifficulty(ObtainingDifficulties.MediumTimeRequirements)
                    .Concat(ItemList.GetItemsAtDifficulty(ObtainingDifficulties.LargeTimeRequirements))
                    .Where(x => !itemsAlreadyInStock.Contains(x.QualifiedId))
                    .Distinct()
                    .ToList()
                : ItemList.GetCraftableItems(CraftableCategories.Easy)
                    .Concat(ItemList.GetCraftableItems(CraftableCategories.EasyAndNeedMany))
                    .Concat(ItemList.GetItemsBelowDifficulty(ObtainingDifficulties.MediumTimeRequirements))
                    .Where(x => !itemsAlreadyInStock.Contains(x.QualifiedId))
                    .Distinct()
                    .ToList();

            // Select an item to be the item of the week, and then add it
            Item itemOfTheWeek = validItems[shopRNG.Next(validItems.Count)];
            int salePrice = GetAdjustedItemPrice(itemOfTheWeek, fallbackPrice: 20, multiplier: 2);
            int stock = itemOfTheWeek.IsCraftable &&
                    ((itemOfTheWeek as CraftableItem).Category == CraftableCategories.EasyAndNeedMany)
                ? Range.GetRandomValue(30, 50, shopRNG)
                : Range.GetRandomValue(3, 15, shopRNG);

            InsertStockAt(itemOfTheWeek.QualifiedId, "IoTW", salePrice, stock);
        }
    }
}
