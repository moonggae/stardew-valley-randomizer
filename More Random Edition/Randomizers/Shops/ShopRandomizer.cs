using StardewValley.GameData.Shops;
using System.Collections.Generic;

namespace Randomizer
{
    public class ShopRandomizer
    {
        /// <summary>
        /// Gets the daily shop repalcements - intended to be called at the
        /// start of every day to modify shop stock on a given day
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, ShopData> GetDailyShopReplacements()
        {
            var seedShop = new SeedShopRandomizer();

            Dictionary<string, ShopData> shopReplacements = new()
            {
                [seedShop.ShopId] = seedShop.ModifyShop()
            };

            return shopReplacements;
        }
    }
}
