using StardewValley;
using StardewValley.GameData.Objects;
using System.Collections.Generic;

namespace Randomizer
{
    /// <summary>
    /// Contains information required for editing object data
    /// </summary>
    public class EditedObjects
	{
		public static IDictionary<string, ObjectData> DefaultObjectInformation;

		// TODO 1.6: remove this in favor of the below!
		public Dictionary<int, string> ObjectInformationReplacements = new();

        public Dictionary<string, ObjectData> ObjectsReplacements = new();
        public Dictionary<int, string> FruitTreeReplacements = new();
		public Dictionary<int, string> CropsReplacements
		{
			get
			{
				Dictionary<int, string> cropsReplacements = new();
				foreach (KeyValuePair<int, CropGrowthInformation> cropInfo in CropGrowthInformation.CropIdsToInfo)
				{
					cropsReplacements.Add(cropInfo.Key, cropInfo.Value.ToString());
				}
				return cropsReplacements;
			}
		}
		public Dictionary<string, string> FishReplacements = new();

		/// <summary>
		/// Refresh the object data each randomization run in case we have stale data
		/// </summary>
		public EditedObjects()
		{
			DefaultObjectInformation = Game1.objectData;
        }
	}
}
