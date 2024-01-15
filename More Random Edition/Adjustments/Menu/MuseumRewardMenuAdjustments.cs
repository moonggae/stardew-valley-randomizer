using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;

namespace Randomizer
{
    internal class MuseumRewardMenuAdjustments
    {
        public static void AdjustMenu(ItemGrabMenu itemGrabMenu)
        {
            // the menu will fire off an event each time an object is grabbed out of it
            // we have it defined as "Testing" here
            itemGrabMenu.behaviorOnItemGrab = new ItemGrabMenu.behaviorOnItemSelect(Testing);

            // TODO for this:
            // - make a map of all of the old reward items to the new ones
            // - all the new ones NEED TO BE UNIQUE IDS due to how this works

            // This is the UI for the menu
            var actualMenu = itemGrabMenu.ItemsToGrabMenu;

            // Here, we would want to loop through the actualInventory and use our map to make a list of
            // rewards that should be there instead
            // Then clear the inventory like we do here, and add our rewards instead
            actualMenu.actualInventory.Clear();
            actualMenu.actualInventory.Add(new StardewValley.Object((int)ObjectIndexes.Acorn, 1));
        }

        // This is the event that would fire when an item is grabbed from the museum reward UI
        private static void Testing(StardewValley.Item item, Farmer who)
        {
            // here we would use our map to look up the associated item
            // For example, we replaced the first reward with the Acorn here
            // - before it was Cauliflower Seeds
            if (item.ParentSheetIndex == (int)ObjectIndexes.Acorn)
            {
                // call the ORIGINAL callback with the ORIGINAL item reward so that the mail gets set properly
                // and the rewards count as if they were never modified!
                (Game1.currentLocation as LibraryMuseum).collectedReward(new StardewValley.Object((int)ObjectIndexes.CauliflowerSeeds, 1), who);
            }
        }
    }
}
