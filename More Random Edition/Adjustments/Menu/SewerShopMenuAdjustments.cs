using StardewValley.Menus;
using System;
using System.Linq;

namespace Randomizer
{
    internal class SewerShopMenuAdjustments : ShopMenuAdjustments
    {
        /// <summary>
        /// Replaces a couple items in the shop
        /// </summary>
        /// <param name="menu">The shop menu</param>
        public override void Adjust(ShopMenu menu)
        {
            if (!ShouldChangeShop)
            {
                RestoreShopState(menu);
                return;
            }

            if (Globals.Config.Shops.RandomizerSewerShop)
            {
                AdjustStock(menu);
            }
        }

        /// <summary>
        /// Replace the Monster Fireplace and Sign of the Vessel with daily equivalents
        /// Means a random BigCraftable and a random Furniture
        /// </summary>
        /// <param name="menu">The shop menu</param>
        private static void AdjustStock(ShopMenu menu)
        {
            Random shopRNG = Globals.GetDailyRNG();

            RemoveFromStock(menu, (int)FurnitureIndexes.MonsterFireplace);
            RemoveFromStock(menu, (int)BigCraftableIndexes.SignOfTheVessel);

            var bigCraftableToSell = ItemList.GetRandomBigCraftablesToSell(shopRNG, numberToGet: 1).First();
            var bigCraftableSalePrice = GetAdjustedItemPrice(bigCraftableToSell, fallbackPrice: 500, multiplier: 3);
            InsertStockAt(menu, bigCraftableToSell, salePrice: bigCraftableSalePrice);
            InsertStockAt(menu, ItemList.GetRandomFurnitureToSell(shopRNG, numberToGet: 1).First());
        }
    }
}
