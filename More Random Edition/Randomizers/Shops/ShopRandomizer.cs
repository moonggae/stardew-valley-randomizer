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
            return CreateReplacements(new List<RandomizedShop>()
            {
                new RandomizedSeedShop(),
                new RandomizedJojaMart(),
                new RandomizedBlacksmithShop(),
                new RandomizedAdventureShop(),
                new RandomizedCarpenterShop(),
                new RandomizedSaloonShop(),
                new RandomizedOasisShop()
            });
        }

        /// <summary>
        /// Creates the shop replacement dictionary from a list of randomized shops
        /// </summary>
        /// <param name="shopRandomizers">The list of randomized shops</param>
        /// <returns>The dictionary of replacement data</returns>
        public static Dictionary<string, ShopData> CreateReplacements(
            List<RandomizedShop> shopRandomizers)
        {
            Dictionary<string, ShopData> shopReplacements = new();
            foreach(RandomizedShop shopRandomizer in shopRandomizers)
            {
                shopReplacements[shopRandomizer.ShopId] = shopRandomizer.ModifyShop();
            }
            return shopReplacements;
        }
    }
}
