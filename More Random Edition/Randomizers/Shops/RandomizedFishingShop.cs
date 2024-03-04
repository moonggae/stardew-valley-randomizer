using StardewValley.GameData.Shops;
using System;
using System.Linq;

namespace Randomizer
{
    public class RandomizedFishingShop : RandomizedShop
    {
        public RandomizedFishingShop() : base("FishShop") { }

        /// <summary>
        /// Adds a catch of the day (1-3 of any random fish of this season)
        /// </summary>
        public override ShopData ModifyShop()
        {
            AddCatchOfTheDay();

            return CurrentShopData;
        }

        /// <summary>
        /// Adds a catch of the day (1-3 of any random fish of this season)
        /// </summary>
        private void AddCatchOfTheDay()
        {
            if (!Globals.Config.Shops.AddFishingShopCatchOfTheDay)
            {
                return;
            }

            Random shopRNG = Globals.GetDailyRNG(nameof(RandomizedFishingShop));

            var currentSeason = SeasonsExtensions.GetCurrentSeason();
            var possibleFish = FishItem.GetListAsFishItem(true)
                .Where(fish => fish.AvailableSeasons.Contains(currentSeason))
                .ToList();
            var catchOfTheDay = Globals.RNGGetRandomValueFromList(possibleFish, shopRNG);
            var stock = Range.GetRandomValue(1, 3, shopRNG);

            InsertStockAt(catchOfTheDay.QualifiedId, "CoTD", availableStock: stock);
        }
    }
}
