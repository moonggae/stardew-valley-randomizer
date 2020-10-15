using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Randomizer
{
	public class CropImageBuilder : SpringObjectsImageBuilder
	{
		/// <summary>
		/// Keeps track of mapped points to crop ids so that GetRandomFileName can grab the matching ID
		/// </summary>
		private Dictionary<Point, int> PointsToCropIds;

		/// <summary>
		/// Keeps track of crop ids mapped to image names so that all the crop images can be linked
		/// </summary>
		private readonly Dictionary<int, string> CropIdsToImageNames;

		/// <summary>
		/// Constructor
		/// </summary>
		public CropImageBuilder(Dictionary<int, string> cropIdsToImageNames) : base("Crops")
		{
			CropIdsToImageNames = cropIdsToImageNames;

			PointsToCropIds = GetPointsToIdsMapping(CropItem.Get().Cast<Item>().ToList());
			PositionsToOverlay = PointsToCropIds.Keys.ToList();
		}

		/// <summary>
		/// Gets a random file name from the files to pull from and removes the found entry from the list
		/// </summary>
		/// <param name="position">The position of the instrument - unused in this version of the function</param>
		/// <returns>The file name</returns>
		protected override string GetRandomFileName(Point position)
		{
			string directory = string.IsNullOrEmpty(CustomImageDirectory) ? ImageDirectory : CustomImageDirectory;

			int cropId = PointsToCropIds[position];
			CropItem cropItem = (CropItem)ItemList.Items[cropId];

			if (!CropIdsToImageNames.TryGetValue(cropItem.Id, out string fileName))
			{
				Globals.ConsoleWarn($"Could not find image for the matching seed of {cropItem.Name}; using default image instead.");
				return $"{directory}/default.png";
			}

			return $"{directory}/{fileName}";
		}

		/// <summary>
		/// Whether the settings premit random crop images
		/// </summary>
		/// <returns>True if so, false otherwise</returns>
		public override bool ShouldSaveImage()
		{
			return Globals.Config.Crops.Randomize && Globals.Config.Crops.UseCustomImages;
		}
	}
}
