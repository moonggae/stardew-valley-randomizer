using StardewValley.GameData.Shops;
using System;
using System.Collections.Generic;

namespace Randomizer
{
    public class RandomizedBlacksmithShop : RandomizedShop
    {
        private const string UniqueItemId = "RandomizedBlacksmithItem";
        
        public RandomizedBlacksmithShop() : base("Blacksmith") { }

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
        /// Adjusts the stock
        /// - 50% of discounting an item
        /// - 35% chance of adding some metal bars to the stock 
        ///   - 95% from that for 4 other bar types, means 8.3125% for any of those
        ///   - 5% from there for iridium  1.75% chance overall
        /// - 12% chance to add any random artifact
        /// - 3% chance to add iridium ore
        /// </summary>
        /// <param name="menu">The shop menu</param>
        private void AdjustStock()
        {
            Random shopRNG = Globals.GetDailyRNG(nameof(RandomizedBlacksmithShop));

            int rolledValue = Range.GetRandomValue(0, 99, shopRNG);
            if (rolledValue < 50) // 50%
            {
                DiscountAnItem(shopRNG);
            }
            else if (rolledValue < 85) // 35%
            {
                AddMetalBars(shopRNG);
            }
            else if (rolledValue < 97) // 12%
            {
                AddAnArtifact(shopRNG);
            }
            else // 3%
            {
                AddIridiumOre(shopRNG);
            }
        }

        /// <summary>
        /// Discounts an existing item in the shop from between 10-25%
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="shopRNG"></param>
        private void DiscountAnItem(Random shopRNG)
        {
            // Choose an ore and discount multiplier
            string itemToDiscount = 
                Globals.RNGGetRandomValueFromList(CurrentShopData.Items, shopRNG).ItemId;
            var priceMultiplier = 1 - (Range.GetRandomValue(10, 25, shopRNG) / 100f);

            // This shop has two sets of prices based on the year; we will discount both
            GetShopItemsByItemIds(new List<string> { itemToDiscount })
                .ForEach(itemToDiscount => {
                    itemToDiscount.Price = (int)(itemToDiscount.Price * priceMultiplier);
                });
        }

        /// <summary>
        /// Adds any random artifact with a stock of 1
        /// </summary>
        /// <param name="shopRNG"></param>
        private void AddAnArtifact(Random shopRNG)
        {
            var artifact = Globals.RNGGetRandomValueFromList(ItemList.GetArtifacts(), shopRNG);
            var salePrice = GetAdjustedItemPrice(artifact, fallbackPrice: 50, multiplier: 3);
            AddStock(artifact.QualifiedId, UniqueItemId, salePrice, availableStock: 1);
        }

        /// <summary>
        /// Adds 5-15 iridium ores
        /// </summary>
        /// <param name="shopRNG"></param>
        private void AddIridiumOre(Random shopRNG)
        {
            var iridiumOre = ItemList.Items[ObjectIndexes.IridiumOre];
            var stock = Range.GetRandomValue(5, 15, shopRNG);
            var salePrice = GetAdjustedItemPrice(iridiumOre, fallbackPrice: 50, multiplier: 5);
            AddStock(iridiumOre.QualifiedId, UniqueItemId, salePrice, stock);
        }

        /// <summary>
        /// Adds a random bar to the shop
        /// - 5% chance of 2-4 iridium
        /// - 3-8 of lesser bars
        /// Maple bars are here on purpose because lol
        /// </summary>
        /// <param name="shopRNG"></param>
        private void AddMetalBars(Random shopRNG)
        {
            var getIridiumBar = Globals.RNGGetNextBoolean(5, shopRNG);
            if (getIridiumBar)
            {
                var iridiumBar = ItemList.Items[ObjectIndexes.IridiumBar];
                var stock = Range.GetRandomValue(2, 4, shopRNG);
                var salePrice = GetAdjustedItemPrice(iridiumBar, fallbackPrice: 50, multiplier: 3);
                AddStock(iridiumBar.QualifiedId, UniqueItemId, salePrice, stock);
            }

            else
            {
                var commonMetalBars = new List<Item>()
                {
                    ItemList.Items[ObjectIndexes.CopperBar],
                    ItemList.Items[ObjectIndexes.IronBar],
                    ItemList.Items[ObjectIndexes.GoldBar],
                    ItemList.Items[ObjectIndexes.MapleBar]
                };

                var stock = Range.GetRandomValue(3, 8, shopRNG);
                var bar = Globals.RNGGetRandomValueFromList(commonMetalBars, shopRNG);
                var salePrice = bar.Id == (int)ObjectIndexes.MapleBar
                    ? GetAdjustedItemPrice(bar, fallbackPrice: 50, multiplier: 1)
                    : GetAdjustedItemPrice(bar, fallbackPrice: 50, multiplier: 3);
                AddStock(bar.QualifiedId, UniqueItemId, salePrice, stock);
            }
        }
    }
}
