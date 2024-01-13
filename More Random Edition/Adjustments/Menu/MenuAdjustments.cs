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
        private static SeedShopMenuAdjustments SeedShop { get; } = new();
        private static AdventureShopMenuAdjustments AdventureShop { get; } = new();
        private static CarpenterShopMenuAdjustments CarpenterShop { get; } = new();
        private static SaloonShopMenuAdjustments SaloonShop { get; } = new();
        private static OasisShopMenuAdjustments OasisShop { get; } = new();
        private static SewerShopMenuAdjustments SewerShop { get; } = new();
        private static HatShopMenuAdjustments HatShop { get; } = new();
        private static ClubShopMenuAdjustments ClubShop { get; } = new();

        /// <summary>
        /// Reset all the shop states
        /// Intended to be called at the end of every day so shops can reset
        /// </summary>
        public static void ResetShopStates()
        {
            SeedShop.ResetShopState();
            // Adventure shop is skipped as there's nothing to restore
            CarpenterShop.ResetShopState();
            SaloonShop.ResetShopState();
            OasisShop.ResetShopState();
            SewerShop.ResetShopState();
            // Hat shop is skipped as there's nothing to restore
            // Club shop is skipped as there's nothing to restore
        }

        /// <summary>
        /// Makes the actual menu adjustments
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Contains the menu info - NewMenu means a menu was opened, OldMenu means one was closed</param>
        public static void AdjustMenus(object sender, MenuChangedEventArgs e)
        {
            // Bundle menu - fix ring deposit
            if (e.NewMenu is JunimoNoteMenu openedBundleMenu)
            {
                BundleMenuAdjustments.FixRingSelection(openedBundleMenu);
            }

            // Shops - adjust on open
            else if (e.NewMenu is ShopMenu openedShopMenu)
            {
                AdjustShopMenus(openedShopMenu, wasShopOpened: true);
            }

            // Shops - adjust on close
            else if (e.OldMenu is ShopMenu closedShopMenu)
            {
                AdjustShopMenus(closedShopMenu, wasShopOpened: false);
            }
        }

        /// <summary>
        /// Adjust shops on menu open
        /// Modifies the stock if it was the first time they were open, or restores it from the state
        /// it was at when it was last closed
        /// </summary>
        /// <param name="shopMenu"></param>
        /// <param name="wasShopOpen">True if the shop was just opened, false if it was closed</param>
        private static void AdjustShopMenus(ShopMenu shopMenu, bool wasShopOpened)
        {
            switch (shopMenu.portraitPerson?.Name)
            {
                // Seed shop
                case "Pierre":
                    SeedShop.OnChange(shopMenu, wasShopOpened);
                    break;
                // Adventure shop - fix weapon prices so infinite money can't be made
                case "Marlon":
                    AdventureShop.OnChange(shopMenu, wasShopOpened);
                    break;
                // Carpenter shop - add clay to prevent long grinds
                case "Robin":
                    CarpenterShop.OnChange(shopMenu, wasShopOpened);
                    break;
                // Saloon shop - will sell random foods/recipes each day
                case "Gus":
                    SaloonShop.OnChange(shopMenu, wasShopOpened);
                    break;
                // Oasis shop - randomizes its foragable/crop/furniture stock each week
                case "Sandy":
                    OasisShop.OnChange(shopMenu, wasShopOpened);
                    break;
                // Sewer shop - randomizes the furniture and big craftable items daily
                case "Krobus":
                    SewerShop.OnChange(shopMenu, wasShopOpened);
                    break;
                default:
                    // The hat/club shops don't have portraits
                    if (shopMenu.storeContext == "Forest" && shopMenu.itemPriceAndStock.Keys.All(item => item is Hat))
                    {
                        // Hat shop - will sell a random hat each week in addition to what you've already unlocked
                        HatShop.OnChange(shopMenu, wasShopOpened);
                    }

                    else if (shopMenu.storeContext == "Club")
                    {
                        // Club shop sells random furniture/clothing items weekly
                        ClubShop.OnChange(shopMenu, wasShopOpened);
                    }
                    break;

                    // Shops TODO
                    // Joja Mart
                    // Willy's fishing shop
                    // Easter egg/h'ween event shops?
            }
        }
    }
}
