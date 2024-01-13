using StardewValley.Menus;
using StardewValley.Objects;
using System;
using System.Linq;
using SVObject = StardewValley.Object;

namespace Randomizer
{
    internal class HatShopMenuAdjustments : ShopMenuAdjustments
    {
        /// <summary>
        /// Adds an hat of the week to the shop
        /// </summary>
        /// <param name="menu">The shop menu</param>
        public static void AddHatOfTheWeek(ShopMenu menu)
        {
            if (!Globals.Config.Shops.AddHatShopHatOfTheWeek)
            {
                return;
            }

            // Stock will change every Monday
            Random shopRNG = Globals.GetWeeklyRNG();

            var existingHatIds = menu.itemPriceAndStock.Keys
                .Where(item => item is Hat)
                .Select(item => (item as Hat).which.Value)
                .ToList();

            var hatOfTheWeek = ItemList.GetRandomHatsToSell(shopRNG, numberToGet: 1, existingHatIds).FirstOrDefault();
            if (hatOfTheWeek != null)
            {
                InsertStockAt(menu, hatOfTheWeek, salePrice: 1000);
            }
        }
    }
}
