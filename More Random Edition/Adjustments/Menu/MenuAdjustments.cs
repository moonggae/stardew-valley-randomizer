using StardewModdingAPI.Events;
using StardewValley.Menus;

namespace Randomizer
{
    /// <summary>
    /// Makes menu adjustments to shops, etc
    /// </summary>
    public class MenuAdjustments
    {
        private static HatShopMenuAdjustments HatShop { get; } = new();
        private static ClubShopMenuAdjustments ClubShop { get; } = new();

        /// <summary>
        /// Makes the actual menu adjustments
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Contains the menu info - NewMenu means a menu was opened, OldMenu means one was closed</param>
        public static void AdjustMenus(object sender, MenuChangedEventArgs e)
        {
            // Quests - fix the name in multiplayer
            if (e.NewMenu is QuestLog questLog)
            {
                QuestLogAdjustments.FixQuestName(questLog);
            }

            // Pause menu - adjust the crab pot cost if the player has Trapper
            else if (e.NewMenu is GameMenu gameMenu)
            {
                CraftingMenuAdjustments.ReduceCrabPotCost(gameMenu);

#if DEBUG
                // This is to test shop menus quickly
                // Set a breakpoint here and move the code so CalculateAndInvalidateShopEdits is executed
                var test = false;
                if (test)
                {
                    Globals.ModRef.CalculateAndInvalidateShopEdits();
                }
#endif
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
        /// <param name="shopMenu">The shop menu to adjust</param>
        /// <param name="wasShopOpened">True if the shop was just opened, false if it was closed</param>
        private static void AdjustShopMenus(ShopMenu shopMenu, bool wasShopOpened)
        {
            switch (shopMenu.ShopId)
            {
                // Hat shop - will sell a random hat each week in addition to what you've already unlocked
                case "HatMouse":
                    HatShop.OnChange(shopMenu, wasShopOpened);
                    break;
                // Club shop sells random furniture/clothing items weekly
                case "Casino":
                    ClubShop.OnChange(shopMenu, wasShopOpened);
                    break;
            }
        }
    }
}
