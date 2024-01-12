using StardewValley.Menus;
using StardewValley;
using System;
using System.Linq;
using SVObject = StardewValley.Object;

namespace Randomizer
{
    internal class OasisShopAdjustments : ShopMenuAdjustments
    {
        /// <summary>
        /// Adjust the oasis shop stock to sell late game items, cycling every Monday
        /// </summary>
        /// <param name="menu">The shop menu</param>
        public static void AdjustStock(ShopMenu menu)
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
    }
}
