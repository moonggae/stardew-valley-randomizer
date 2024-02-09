using StardewValley.Menus;
using System;
using System.Linq;

namespace Randomizer
{
    internal class FishingShopMenuAdjustments : ShopMenuAdjustments
    {
        /// <summary>
        /// Adds a catch of the day (1-3 of any random fish of this season)
        /// </summary>
        /// <param name="menu">The shop menu</param>
        protected override void Adjust(ShopMenu menu)
        {
            if (!ShouldChangeShop)
            {
                RestoreShopState(menu);
                return;
            }

            if (Globals.Config.Shops.AddFishingShopCatchOfTheDay)
            {
                AddCatchOfTheDay(menu);
            }
        }

        /// <summary>
        /// Adds a catch of the day (1-3 of any random fish of this season)
        /// </summary>
        /// <param name="menu">The shop menu</param>
        private static void AddCatchOfTheDay(ShopMenu menu)
        {
            Random shopRNG = Globals.GetDailyRNG(nameof(FishingShopMenuAdjustments));

            var currentSeason = SeasonFunctions.GetCurrentSeason();
            var possibleFish = FishItem.GetListAsFishItem(true)
                .Where(fish => fish.AvailableSeasons.Contains(currentSeason))
                .ToList();
            var catchOfTheDay = Globals.RNGGetRandomValueFromList(possibleFish, shopRNG);
            var stock = Range.GetRandomValue(1, 3, shopRNG);

            InsertStockAt(menu, catchOfTheDay.GetSaliableObject(), stock);
        }
    }
}
