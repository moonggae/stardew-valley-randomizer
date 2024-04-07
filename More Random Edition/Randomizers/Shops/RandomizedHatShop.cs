using StardewValley.GameData.Shops;
using System;
using System.Linq;

namespace Randomizer
{
    public class RandomizedHatShop : RandomizedShop
    {
        public RandomizedHatShop() : base("HatMouse") { }

        /// <summary>
        /// Adds an hat of the week to the shop
        /// </summary>
        public override ShopData ModifyShop()
        {
            AddHatOfTheWeek();

            return CurrentShopData;
        }

        /// <summary>
        /// Adds an hat of the week to the shop
        /// </summary>
        private void AddHatOfTheWeek()
        {
            if (!Globals.Config.Shops.AddHatShopHatOfTheWeek)
            {
                return;
            }

            // Stock will change every Monday
            RNG shopRNG = RNG.GetWeeklyRNG(nameof(RandomizedHatShop));

            var existingHatIds = CurrentShopData.Items
                .Select(item => item.ItemId)
                .ToList();

            string hatOfTheWeek = HatFunctions.GetRandomHatQualifiedId(
                shopRNG,
                idsToExclude: existingHatIds);
            InsertStockAt(hatOfTheWeek, "HoTW", price: 1000);
        }
    }
}
