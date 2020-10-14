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
		/// Keeps track of crop ids mapped to image names so that it can be used by the seed and
		/// crop growth image builders
		/// </summary>
		public Dictionary<int, string> CropIdsToImageNames;

		/// <summary>
		/// Constructor
		/// </summary>
		public CropImageBuilder() : base("Crops")
		{
			CropIdsToImageNames = new Dictionary<int, string>();
			PointsToCropIds = GetPointsToIdsMapping(CropItem.Get().Cast<Item>().ToList());
			PositionsToOverlay = PointsToCropIds.Keys.ToList();
		}

		/// <summary>
		/// Gets a random file name from the files to pull from and removes the found entry from the list
		/// </summary>
		/// <param name="position">The position of the instrument - unused in this version of the function</param>
		/// <returns>The random file name</returns>
		protected override string GetRandomFileName(Point position)
		{
			string retrievedFileName = base.GetRandomFileName(position);
			if (!retrievedFileName.Contains("default.png"))
			{

				int cropId = PointsToCropIds[position];
				CropIdsToImageNames[cropId] = retrievedFileName;
			}

			return retrievedFileName;
		}

		/// <summary>
		/// Whether the settings premit random crop images
		/// </summary>
		/// <returns>True if so, false otherwise</returns>
		public override bool ShouldSaveImage()
		{
			return Globals.Config.Crops.Randomize && Globals.Config.Crops.RandomizeImages;
		}
	}
}
