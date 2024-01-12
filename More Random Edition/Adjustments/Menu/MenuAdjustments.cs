using StardewModdingAPI.Events;
using StardewValley.Menus;
using StardewValley.Objects;
using System.Linq;

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
                    // Sewer shop - randomizes the furniture and big craftable items daily
                    case "Krobus":
                        SewerShopAdjustments.AdjustStock(shopMenu);
                        break;
                    default:
                        // The hat shop doesn't have a portrait, so well check it this way!
                        if (shopMenu.storeContext == "Forest" && shopMenu.itemPriceAndStock.Keys.All(item => item is Hat))
                        {
                            // Hat shop - will sell a random hat each week in addition to what you've already unlocked
                            HatShopAdjustments.AddHatOfTheWeek(shopMenu);
                        }
                        break;

                        // Shops TODO
                        // Wandering Traveler
                        // Joja Mart
                        // Desert Outpost
                        // Qi
                        // Easter egg/h'ween event shops?
                }
            }
        }
    }
}
