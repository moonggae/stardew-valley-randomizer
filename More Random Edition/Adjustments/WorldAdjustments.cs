using Microsoft.Xna.Framework;
using Netcode;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Monsters;
using System.Collections.Generic;
using System.Linq;
using RandomizerItem = Randomizer.Item;
using StardewValleyNpc = StardewValley.NPC;

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
            if (!Context.IsWorldReady)
            {
                // In this case, it's probably the new player connecting - their box will be taken care
                // of in CalculateAllReplacements
                return;
            }

            GameLocation farmHouse = Game1.getLocationFromName("FarmHouse");
            if (farmHouse?.Objects != null)
            {
                List<StardewValley.Objects.Chest> chestsInRoom =
                    farmHouse.Objects.Values.Where(x =>
                        x.DisplayName == "Chest")
                        .Cast<StardewValley.Objects.Chest>()
                        .Where(x => x.giftbox.Value)
                    .ToList();

                if (chestsInRoom.Count > 0)
                {
                    string parsnipSeedsName = ItemList.GetItemName(ObjectIndexes.ParsnipSeeds);
                    var chest = chestsInRoom[0];
                    if (chest.items.Count > 0)
                    {
                        StardewValley.Item itemInChest = chest.items[0];
                        if (itemInChest.ParentSheetIndex == (int)ObjectIndexes.ParsnipSeeds)
                        {
                            itemInChest.DisplayName = parsnipSeedsName;
                        }
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
                        RandomizerItem newForagable = Globals.RNGGetRandomValueFromList(newForagables, Game1.random);
                        location.Objects[oldForagableKey].ParentSheetIndex = newForagable.Id;
                        location.Objects[oldForagableKey].Name = newForagable.Name;
                    }
                }
            }
        }

        /// <summary>
        /// Identical to the logic in Farm.cs (spawnFlyingMonstersOffscreen) for the giant bat spawn, 
        /// but we check for the randomized weapon instead
        /// </summary>
        public static void TrySpawnGalaxySwordBat()
        {
            var currentLocation = Game1.currentLocation;
            if (!Globals.Config.Weapons.Randomize ||
                currentLocation.Name != "Farm" ||
                !(Game1.random.NextDouble() < 0.25))
            {
                return;
            }

            if (WeaponRandomizer.Weapons.TryGetValue((int)WeaponIndexes.GalaxySword, out var galaxySword)) {
                if (Game1.player.CombatLevel >= 10 &&
                    Game1.random.NextDouble() < 0.01 &&
                    Game1.player.hasItemInInventoryNamed(galaxySword.Name))
                {
                    NetCollection<StardewValleyNpc> characters = currentLocation.characters;
                    Bat bat = new(Vector2.Zero * 64f, 9999)
                    {
                        focusedOnFarmers = true,
                        wildernessFarmMonster = true
                    };
                    characters.Add(bat);
                }
            }
        }
    }
}
