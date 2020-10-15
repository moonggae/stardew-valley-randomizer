using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Randomizer
{
	public class SeedImageBuilder : SpringObjectsImageBuilder
	{
		/// <summary>
		/// Keeps track of mapped points to crop ids so that GetRandomFileName can grab the matching ID
		/// </summary>
		private readonly Dictionary<Point, int> PointsToSeedIds;

		/// <summary>
		/// Keeps track of crop ids mapped to image names so that all the crop images can be linked
		/// </summary>
		private readonly Dictionary<int, string> CropIdsToImageNames;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="cropIdsToImageNames">The mapping of crops to images so we can link images together</param>
		public SeedImageBuilder(Dictionary<int, string> cropIdsToImageNames) : base("Seeds")
		{
			CropIdsToImageNames = cropIdsToImageNames;

			List<Item> seedItems = GetAllSeedItems();
			PointsToSeedIds = GetPointsToIdsMapping(seedItems);
			PositionsToOverlay = GetAllPoints(seedItems);

			//TODO: deal with coffee - it doesn't have a matching crop since it's marked as a seed!
			// coffee's growth index is 40, if it matters
		}

		/// <summary>
		/// Get all seed items to replace
		/// </summary>
		/// <returns>The list of items</returns>
		private List<Item> GetAllSeedItems()
		{
			return CropItem.Get().Select(x => x.MatchingSeedItem).Cast<Item>().ToList();
		}

		/// <summary>
		/// Gets a random file name from the files to pull from and removes the found entry from the list
		/// </summary>
		/// <param name="position">The position of the instrument - unused in this version of the function</param>
		/// <returns>The file name</returns>
		protected override string GetRandomFileName(Point position)
		{
			string directory = string.IsNullOrEmpty(CustomImageDirectory) ? ImageDirectory : CustomImageDirectory;

			int seedId = PointsToSeedIds[position];
			SeedItem seedItem = (SeedItem)ItemList.Items[seedId];

			int cropId = seedItem.CropGrowthInfo.CropId;
			if (!CropIdsToImageNames.TryGetValue(cropId, out string fileName))
			{
				Item cropItem = ItemList.Items[cropId];
				Globals.ConsoleWarn($"Could not find image for the matching seed of {cropItem.Name}; using default image instead.");
				return $"{directory}/default.png";
			}

			return $"{directory}/{fileName}";
		}

		/// <summary>
		/// Whether the settings premit random seed images
		/// </summary>
		/// <returns>True if so, false otherwise</returns>
		public override bool ShouldSaveImage()
		{
			return Globals.Config.Crops.Randomize && Globals.Config.Crops.UseCustomImages;
		}
	}
}
