using StardewValley.Menus;
using StardewValley.Objects;
using System;
using System.Linq;

namespace Randomizer
{
    internal class HatShopMenuAdjustments : ShopMenuAdjustments
    {
        public HatShopMenuAdjustments() : base()
        {
            SkipShopSave = true;
        }

        /// <summary>
        /// Callthrough to AddHatOfTheWeek, if the setting is on
        /// This shop doesn't need to be restored or anything, as there will never be limited stock
        /// </summary>
        /// <param name="menu">The shop menu</param>
        protected override void Adjust(ShopMenu menu)
        {
            if (Globals.Config.Shops.AddHatShopHatOfTheWeek)
            {
                AddHatOfTheWeek(menu);
            }
        }

        /// <summary>
        /// TODO 1.6: test this change because ItemId is now used
        /// Adds an hat of the week to the shop
        /// </summary>
        /// <param name="menu">The shop menu</param>
        private static void AddHatOfTheWeek(ShopMenu menu)
        {
            // Stock will change every Monday
            Random shopRNG = Globals.GetWeeklyRNG(nameof(HatShopMenuAdjustments));

            var existingHatIds = menu.itemPriceAndStock.Keys
                .Where(item => item is Hat)
                .Select(item => (item as Hat).ItemId)
                .ToList();

            var hatOfTheWeek = ItemList.GetRandomHatsToSell(shopRNG, numberToGet: 1, existingHatIds).FirstOrDefault();
            if (hatOfTheWeek != null)
            {
                InsertStockAt(menu, hatOfTheWeek, salePrice: 1000);
            }
        }
    }
}
