using StardewModdingAPI.Events;
using StardewValley.Menus;

namespace Randomizer
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
                        SeedShopMenuAdjustments.FixSaplingPrices(shopMenu);
                        SeedShopMenuAdjustments.AddItemOfTheWeek(shopMenu);
                        break;
                    // Adventure shop (the items can be resold for infinite money sometimes
                    case "Marlon":
                        AdventureShopMenuAdjustments.FixPrices(shopMenu);
                        break;
                    // Carpenter shop - add clay to prevent long grinds
                    case "Robin":
                        CarpenterShopMenuAdjustments.AddClay(shopMenu);
                        break;
                    // Saloon shop - will sell random foods/recipes each day
                    case "Gus":
                        SaloonShopAdjustments.AdjustStock(shopMenu);
                        break;
                    // Oasis shop - randomizes its foragable/crop/furniture stock each week
                    case "Sandy":
                        OasisShopAdjustments.AdjustStock(shopMenu);
                        break;

                        // Shops TODO
                        // Wandering Traveler
                        // Joja Mart
                        // Sewer Shop
                        // Desert Outpost
                        // Qi
                        // Hat Shop?
                        // Easter egg/h'ween event shops?
                }
            }
        }
    }
}
