using StardewValley.GameData.Shops;
using System;
using System.Linq;

namespace Randomizer
{
    public class RandomizedFishingShop : RandomizedShop
    {
        public RandomizedFishingShop() : base("FishShop") { }

		public override bool ShouldModifyShop()
	        => Globals.Config.Shops.AddFishingShopCatchOfTheDay;

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
            RNG shopRNG = RNG.GetDailyRNG(nameof(RandomizedFishingShop));

            var currentSeason = SeasonsExtensions.GetCurrentSeason();
            var possibleFish = FishItem.GetListAsFishItem(true)
                .Where(fish => fish.AvailableSeasons.Contains(currentSeason))
                .ToList();
            var catchOfTheDay = shopRNG.GetRandomValueFromList(possibleFish);
            var stock = shopRNG.NextIntWithinRange(1, 3);

            InsertStockAt(catchOfTheDay.QualifiedId, "CoTD", availableStock: stock);
        }
    }
}
