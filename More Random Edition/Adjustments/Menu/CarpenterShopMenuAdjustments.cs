using StardewValley.Menus;
using System;
using SVObject = StardewValley.Object;

namespace Randomizer.Menu
{
    internal class CarpenterShopMenuAdjustments : ShopMenuAdjustments
    {
        /// <summary>
        /// Adds Clay to Robin's shop since it's really grindy to get
        /// Add some randomness to the price each day (between 25-75 coins each)
        /// </summary>
        /// <param name="menu">The shop menu</param>
        public static void AddClay(ShopMenu menu)
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
    }
}
