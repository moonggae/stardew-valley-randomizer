using StardewValley.Menus;

namespace Randomizer
{
    // TODO 1.6: Check if this is actually necessary anymore
    internal class AdventureShopMenuAdjustments : ShopMenuAdjustments
    {
        public AdventureShopMenuAdjustments() : base()
        {
            SkipShopSave = true;
        }

        /// <summary>
        /// Callthrough to FixPrices
        /// This shop doesn't need to be restored or anything, as we're only modifying prices and nothing else
        /// </summary>
        /// <param name="menu">The shop menu</param>
        protected override void Adjust(ShopMenu menu)
        {
            FixPrices(menu);
        }

        /// <summary>
        /// Fixes sale prices for randomized gear so that nothing sells for more than it's worth
        /// </summary>
        /// <param name="menu">The shop menu</param>
        private static void FixPrices(ShopMenu menu)
        {
            foreach(var itemData in menu.itemPriceAndStock)
            {
                var item = itemData.Key;
                var saleInfo = itemData.Value;
                saleInfo.Price = item.salePrice();
            }
        }
    }
}
