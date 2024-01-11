using Microsoft.Xna.Framework;
using Randomizer;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Locations;
using System.Collections.Generic;
using System.Linq;
using RandomizerItem = Randomizer.Item;

namespace Randomizer
{
    /// <summary>
    /// Adjustments that are more meant for the first day of play and aren't actually menus
    /// </summary>
    public class DayOneAdjustments
    {
        /// <summary>
        /// Fixes the item name that you get at the start of the game
        /// </summary>
        public static void FixParsnipSeedBox()
        {
            GameLocation farmHouse = Game1.getLocationFromName("FarmHouse");

            List<StardewValley.Objects.Chest> chestsInRoom =
                farmHouse.Objects.Values.Where(x =>
                    x.DisplayName == "Chest")
                    .Cast<StardewValley.Objects.Chest>()
                    .Where(x => x.giftbox.Value)
                .ToList();

            if (chestsInRoom.Count > 0)
            {
                string parsnipSeedsName = ItemList.GetItemName((int)ObjectIndexes.ParsnipSeeds);
                StardewValley.Item itemInChest = chestsInRoom[0].items[0];
                if (itemInChest.ParentSheetIndex == (int)ObjectIndexes.ParsnipSeeds)
                {
                    itemInChest.Name = parsnipSeedsName;
                    itemInChest.DisplayName = parsnipSeedsName;
                }
            }
        }

        /// <summary>
        /// Fixes the foragables on day 1 - the save file is created too quickly for it to be
        /// randomized right away, so we'll change them on the spot on the first day
        /// </summary>
        public static void ChangeDayOneForagables()
        {
            SDate currentDate = SDate.Now();
            if (currentDate.DaysSinceStart < 2)
            {
                List<GameLocation> locations = Game1.locations
                    .Concat(
                        from location in Game1.locations.OfType<BuildableGameLocation>()
                        from building in location.buildings
                        where building.indoors.Value != null
                        select building.indoors.Value
                    ).ToList();

                List<RandomizerItem> newForagables =
                    ItemList.GetForagables(Seasons.Spring)
                        .Where(x => x.ShouldBeForagable) // Removes the 1/1000 items
                        .Cast<RandomizerItem>().ToList();

                foreach (GameLocation location in locations)
                {
                    List<int> foragableIds = ItemList.GetForagables().Select(x => x.Id).ToList();
                    List<Vector2> tiles = location.Objects.Pairs
                        .Where(x => foragableIds.Contains(x.Value.ParentSheetIndex))
                        .Select(x => x.Key)
                        .ToList();

                    foreach (Vector2 oldForagableKey in tiles)
                    {
                        RandomizerItem newForagable = Globals.RNGGetRandomValueFromList(newForagables, true);
                        location.Objects[oldForagableKey].ParentSheetIndex = newForagable.Id;
                        location.Objects[oldForagableKey].Name = newForagable.Name;
                    }
                }
            }
        }
    }
}
