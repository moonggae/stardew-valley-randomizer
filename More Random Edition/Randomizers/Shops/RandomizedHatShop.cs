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
            Random shopRNG = Globals.GetWeeklyRNG(nameof(RandomizedHatShop));

            var existingHatIds = CurrentShopData.Items
                .Select(item => item.ItemId)
                .ToList();

            string hatOfTheWeek = HatFunctions.GetRandomHatQualifiedId(
                idsToExclude: existingHatIds, shopRNG);
            InsertStockAt(hatOfTheWeek, "HoTW", price: 1000);
        }
    }
}
