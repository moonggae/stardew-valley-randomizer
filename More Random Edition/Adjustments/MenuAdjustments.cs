using StardewModdingAPI.Events;
using StardewValley.Menus;

namespace Randomizer.Adjustments
{
    /// <summary>
    /// Makes menu adjustments to shops, etc
    /// </summary>
    public class MenuAdjustments
    {
        /// <summary>
        /// Makes the actual menu adjustments
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void TryAdjustMenu(object sender, MenuChangedEventArgs e)
        {
            // Bundle menu - fix ring deposit
            if (e.NewMenu is JunimoNoteMenu bundleMenu)
            {
                BundleMenuAdjustments.FixRingSelection(bundleMenu);
            }

            // Shops
            else if (e.NewMenu is ShopMenu shopMenu)
            {
                switch (shopMenu.portraitPerson?.Name)
                {
                    // Seed shop
                    case "Pierre":
                        ShopMenuAdjustments.FixSaplingPrices(shopMenu);
                        ShopMenuAdjustments.AddSeedShopItemOfTheWeek(shopMenu);
                        break;
                    // Adventure shop (the items can be resold for infinite money sometimes
                    case "Marlon":
                        ShopMenuAdjustments.FixAdventureShopPrices(shopMenu);
                        break;
                }
            }
        }
    }
}
