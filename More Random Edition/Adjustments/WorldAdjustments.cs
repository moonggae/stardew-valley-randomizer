using Microsoft.Xna.Framework;
using StardewModdingAPI.Utilities;
using StardewValley;
using System.Collections.Generic;
using System.Linq;
using RandomizerItem = Randomizer.Item;
using SVChest = StardewValley.Objects.Chest;
using SVItem = StardewValley.Item;

//TODO: see if this is necessary anymore after fixing the items, etc
namespace Randomizer
{
    /// <summary>
    /// Adjustments that are for the state of the game world in general
    /// </summary>
    public class WorldAdjustments
    {
        /// <summary>
        /// Fixes the item name that you get at the start of the game
        /// </summary>
        public static void FixParsnipSeedBox()
        {
            // We don't want to do this before we're initialized
            if (ItemList.Items == null)
            {
                return;
            }

            GameLocation farmHouse = Game1.getLocationFromName("FarmHouse");
            if (farmHouse?.Objects != null)
            {
                SVChest chest =
                    farmHouse.Objects.Values.Where(x =>
                        x.DisplayName == "Chest")
                        .Cast<SVChest>()
                        .Where(x => x.giftbox.Value)
                    .FirstOrDefault();

                if (chest != null && chest.Items.Count == 1)
                {
                    SVItem itemInChest = chest.Items[0];

                    //TODO 1.6: update ParentSheetIndex - this is NOT the right way to compare things anymore
                    if (itemInChest.ParentSheetIndex == (int)ObjectIndexes.ParsnipSeeds)
                    {
                        //TODO 1.6: this is a readonly property now... figure out how to fix it
                        // (or maybe this will just work without any changes now?
                        //itemInChest.DisplayName = ItemList.GetItemName(ObjectIndexes.ParsnipSeeds);
                    }
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
                        from location in Game1.locations
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
                    List<string> foragableIds = ItemList.GetForagables().Select(x => x.QualifiedId).ToList();
                    List<Vector2> tiles = location.Objects.Pairs
                        .Where(x => foragableIds.Contains(x.Value.QualifiedItemId))
                        .Select(x => x.Key)
                        .ToList();

                    var dailyRNG = Globals.GetDailyRNG(nameof(WorldAdjustments));
                    foreach (Vector2 oldForagableKey in tiles)
                    {
                        RandomizerItem newForagable = 
                            Globals.RNGGetRandomValueFromList(newForagables, dailyRNG);
                        location.Objects[oldForagableKey] = (Object)newForagable.GetSaliableObject();
                        location.Objects[oldForagableKey].IsSpawnedObject = true;
                    }
                }
            }
        }
    }
}
