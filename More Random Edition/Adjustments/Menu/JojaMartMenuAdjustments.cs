using StardewValley.Menus;
using System;
using System.Linq;
using SVObject = StardewValley.Object;

namespace Randomizer
{
    internal class JojaMartMenuAdjustments : ShopMenuAdjustments
    {
        protected override void Adjust(ShopMenu menu)
        {
            if (!ShouldChangeShop)
            {
                RestoreShopState(menu);
                return;
            }

            if (Globals.Config.Shops.AddJojaMartItemOfTheWeek)
            {
                AddItemOfTheWeek(menu);
            }
        }

        private static void AddItemOfTheWeek(ShopMenu menu)
        {
            Random shopRNG = Globals.GetWeeklyRNG(nameof(JojaMartMenuAdjustments));

            // Build list of possible items
            var itemsAlreadyInStock = menu.itemPriceAndStock.Keys
                .Where(shopKey => shopKey is SVObject)
                .Select(item => (item as SVObject).ParentSheetIndex)
                .ToList();

            // 1/10 chance of there being a better item in stock
            var validItems = Globals.RNGGetNextBoolean(10, shopRNG)
                ? ItemList.GetItemsAtDifficulty(ObtainingDifficulties.MediumTimeRequirements)
                    .Concat(ItemList.GetItemsAtDifficulty(ObtainingDifficulties.LargeTimeRequirements))
                    .Where(x => !itemsAlreadyInStock.Contains(x.Id))
                    .Distinct()
                    .ToList()
                : ItemList.GetCraftableItems(CraftableCategories.Easy)
                    .Concat(ItemList.GetCraftableItems(CraftableCategories.EasyAndNeedMany))
                    .Concat(ItemList.GetItemsBelowDifficulty(ObtainingDifficulties.MediumTimeRequirements))
                    .Where(x => !itemsAlreadyInStock.Contains(x.Id))
                    .Distinct()
                    .ToList();

            // Select an item to be the item of the week
            Item itemOfTheWeek = validItems[shopRNG.Next(validItems.Count)];
            int stock = itemOfTheWeek.IsCraftable &&
                    ((itemOfTheWeek as CraftableItem).Category == CraftableCategories.EasyAndNeedMany)
                ? Range.GetRandomValue(30, 50, shopRNG)
                : Range.GetRandomValue(3, 15, shopRNG);
            int salePrice = GetAdjustedItemPrice(itemOfTheWeek, fallbackPrice: 15, multiplier: 2);
            InsertStockAt(menu, itemOfTheWeek.GetSaliableObject(initialStack: stock), stock, salePrice);
        }
    }
}
