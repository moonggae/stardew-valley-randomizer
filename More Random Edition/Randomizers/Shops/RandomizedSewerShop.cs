using Microsoft.Xna.Framework.Graphics.PackedVector;
using StardewValley;
using StardewValley.GameData.Shops;
using System;
using System.Linq;

namespace Randomizer
{
    public class RandomizedSewerShop : RandomizedShop
    {
        public RandomizedSewerShop() : base("ShadowShop") { }

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
        /// Replace the Monster Fireplace and Sign of the Vessel with daily equivalents
        /// Means a random Furniture and a random BigCraftable
        /// </summary>
        private void AdjustStock()
        {
            if (Globals.Config.Shops.RandomizerSewerShop)
            {
                return;
            }

            RNG shopRNG = RNG.GetDailyRNG(nameof(RandomizedSewerShop));

            string fireplaceId = FurnitureFunctions.GetQualifiedId(FurnitureIndexes.MonsterFireplace);
            ShopItemData firePlaceItem = GetShopItemById(fireplaceId);
            firePlaceItem.ItemId = FurnitureFunctions.GetRandomFurnitureQualifiedId(shopRNG);

            string vesselId = BigCraftableFunctions.GetQualifiedId(BigCraftableIndexes.SignOfTheVessel);
            ShopItemData vesselItem = GetShopItemById(vesselId);
            ISalable bigCraftableToSell = ItemList.GetRandomBigCraftablesToSell(shopRNG, numberToGet: 1).First();
            var bigCraftableSalePrice = GetAdjustedItemPrice(bigCraftableToSell, fallbackPrice: 500, multiplier: 3);
            vesselItem.Price = bigCraftableSalePrice;
            vesselItem.ItemId = BigCraftableFunctions.GetRandomBigCraftableQualifiedId(shopRNG);
        }
    }
}
