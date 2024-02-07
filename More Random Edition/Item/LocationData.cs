using Force.DeepCloner;
using StardewValley;
using StardewValley.GameData.Locations;
using System.Collections.Generic;
using static StardewValley.GameData.QuantityModifier;
using SVLocationData = StardewValley.GameData.Locations.LocationData;

namespace Randomizer
{
    /// <summary>
    /// Contains information about foragable items and fish per season, as well as diggable items
    /// </summary>
    public class LocationData
	{
		public Locations Location { get; set; }
        public string LocationName { get { return Location.ToString(); } }
        public SVLocationData StardewLocationData { get; private set; }
		public List<ForagableData> SpringForagables { get; } = new List<ForagableData>();
		public List<ForagableData> SummerForagables { get; } = new List<ForagableData>();
		public List<ForagableData> FallForagables { get; } = new List<ForagableData>();
		public List<ForagableData> WinterForagables { get; } = new List<ForagableData>();

		// TODO 1.6: Deal with this part when we do artifacts
		public Item ExtraDiggingItem { get; set; }
		public double ExtraDiggingItemRarity { get; set; }


		public LocationData(Locations location) 
		{ 
			Location = location;
			StardewLocationData = DataLoader.Locations(Game1.content)[LocationName];
		}

        /// <summary>
        /// Gets the location data populated with the foragable info in this class
        /// </summary>
        /// <returns>The modified data</returns>
		public SVLocationData GetLocationDataWithModifiedForagableData()
		{
            var modifiedData = StardewLocationData.DeepClone();
            modifiedData.Forage.Clear();

            AddForagableDataForSeason(modifiedData, Season.Spring, SpringForagables);
            AddForagableDataForSeason(modifiedData, Season.Summer, SummerForagables);
            AddForagableDataForSeason(modifiedData, Season.Fall, FallForagables);
            AddForagableDataForSeason(modifiedData, Season.Winter, WinterForagables);

            return modifiedData;
        }

        /// <summary>
        /// Adds the foragable data for the given season
        /// </summary>
        /// <param name="locationData">The data to add the foragables to</param>
        /// <param name="season">The season</param>
        /// <param name="data">The list of foragables to add</param>
        private void AddForagableDataForSeason(
            SVLocationData locationData,
            Season season, 
            List<ForagableData> data)
        {
            data.ForEach(foragableData =>
                locationData.Forage.Add(GetSpawnForageData(foragableData, season)));
        }

        /// <summary>
        /// Gets the spawn forage data to set in the location
        /// this is mostly default values, but with the season and our own
        /// foragable data values added on
        /// </summary>
        /// <param name="foragableData">Our foragable data to include</param>
        /// <param name="season">The season of the foragable</param>
        /// <returns>The created spawn forage data</returns>
		private static SpawnForageData GetSpawnForageData(ForagableData foragableData, Season season)
		{
            return new SpawnForageData
            {
                Chance = foragableData.ItemRarity,
                Season = season,
                Condition = null,
                Id = foragableData.QualifiedItemId,
                ItemId = foragableData.QualifiedItemId,
                RandomItemId = null,
                MaxItems = null,
                MinStack = -1,
                MaxStack = -1,
                Quality = -1,
                ObjectInternalName = null,
                ObjectDisplayName = null,
                ToolUpgradeLevel = -1,
                IsRecipe = false,
                StackModifiers = null,
                StackModifierMode = QuantityModifierMode.Stack,
                QualityModifiers = null,
                QualityModifierMode = QuantityModifierMode.Stack,
                PerItemCondition = null
            };
		}
    }
}
