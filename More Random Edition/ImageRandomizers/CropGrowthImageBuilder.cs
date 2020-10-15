using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Randomizer
{
	public class CropGrowthImageBuilder : ImageBuilder
	{
		private const string NormalDirectory = "NormalCrops";
		private const string RegrowingDirectory = "RegrowingCrops";
		private const string TrellisDirectory = "TrellisCrops";
		private const string FlowersWithHuesDirectory = "FlowersWithHues";
		private const string FlowersWithoutHuesDirectory = "FlowersWithoutHues";

		private List<string> Normal4StageImages { get; set; }
		private List<string> Normal5StageImages { get; set; }
		private List<string> RegrowingImages { get; set; }
		private List<string> TrellisImages { get; set; }
		private List<string> FlowersWithHuesImages { get; set; }
		private List<string> FlowersWithoutHuesImages { get; set; }

		/// <summary>
		/// Keeps track of crop ids mapped to image names so that all the crop images can be linked
		/// </summary>
		public Dictionary<int, string> CropIdsToImageNames;

		/// <summary>
		/// Keeps track of crop growth images to crop ids
		/// </summary>
		private readonly Dictionary<Point, int> CropGrowthImagePointsToIds;

		public CropGrowthImageBuilder()
		{
			CropIdsToImageNames = new Dictionary<int, string>();
			BaseFileName = "Crops.png";
			SubDirectory = "CropGrowth";
			CropGrowthImagePointsToIds = GetCropGrowthImageMap();
			PositionsToOverlay = CropGrowthImagePointsToIds.Keys.ToList();

			ImageHeightInPx = 32;
			ImageWidthInPx = 128;

			Normal4StageImages = Directory.GetFiles($"{ImageDirectory}/{NormalDirectory}").Where(x => x.EndsWith("-4.png")).OrderBy(x => x).ToList();
			Normal5StageImages = Directory.GetFiles($"{ImageDirectory}/{NormalDirectory}").Where(x => x.EndsWith("-5.png")).OrderBy(x => x).ToList();
			RegrowingImages = Directory.GetFiles($"{ImageDirectory}/{RegrowingDirectory}").Where(x => x.EndsWith(".png")).OrderBy(x => x).ToList();
			TrellisImages = Directory.GetFiles($"{ImageDirectory}/{TrellisDirectory}").Where(x => x.EndsWith(".png")).OrderBy(x => x).ToList();
			FlowersWithHuesImages = Directory.GetFiles($"{ImageDirectory}/{FlowersWithHuesDirectory}").Where(x => x.EndsWith(".png")).OrderBy(x => x).ToList();
			FlowersWithoutHuesImages = Directory.GetFiles($"{ImageDirectory}/{FlowersWithoutHuesDirectory}").Where(x => x.EndsWith(".png")).OrderBy(x => x).ToList();
		}

		/// <summary>
		/// Gets the map of crop growth images to their ids
		/// Excludes coffee (TODO) and Ancient Seeds, as they aren't randomized
		/// </summary>
		/// <returns></returns>
		private Dictionary<Point, int> GetCropGrowthImageMap()
		{
			const int itemsPerRow = 2;

			Dictionary<Point, int> imageMap = new Dictionary<Point, int>();
			List<int> seedIdsToExclude = new List<int>
			{
				(int)ObjectIndexes.CoffeeBean,
				(int)ObjectIndexes.AncientSeeds
			};
			foreach (SeedItem seedItem in ItemList.GetSeeds().Where(x => !seedIdsToExclude.Contains(x.Id)).Cast<SeedItem>())
			{
				int sheetIndex = seedItem.CropGrowthInfo.GraphicId;
				CropItem cropItem = (CropItem)ItemList.Items[seedItem.CropGrowthInfo.CropId];
				imageMap[new Point(sheetIndex % itemsPerRow, sheetIndex / itemsPerRow)] = cropItem.Id;
			}

			return imageMap;
		}

		/// <summary>
		/// Gets a random file name that matches the crop growth image at the given position
		/// Will remove the name found from the list
		/// </summary>
		/// <param name="position">The position</param>
		/// <returns>The selected file name</returns>
		protected override string GetRandomFileName(Point position)
		{
			string fileName = "";
			string defaultFileName = "default";

			int cropId = CropGrowthImagePointsToIds[position];
			CropItem cropItem = (CropItem)ItemList.Items[cropId];
			CropGrowthInformation growthInfo = cropItem.MatchingSeedItem.CropGrowthInfo;
			SeedItem seedItem = cropItem.MatchingSeedItem;

			FixWidthValue(seedItem.CropGrowthInfo.GraphicId);

			if (cropItem.IsFlower)
			{
				if (seedItem.CropGrowthInfo.TintColorInfo.HasTint)
				{
					defaultFileName = "default-flower-hue";
					fileName = Globals.RNGGetAndRemoveRandomValueFromList(FlowersWithHuesImages);
				}

				else
				{
					defaultFileName = "default-flower-no-hue";
					fileName = Globals.RNGGetAndRemoveRandomValueFromList(FlowersWithoutHuesImages);
				}
			}

			else if (growthInfo.IsTrellisCrop)
			{
				defaultFileName = "default-trellis";
				fileName = Globals.RNGGetAndRemoveRandomValueFromList(TrellisImages);
			}

			else if (growthInfo.RegrowsAfterHarvest)
			{
				defaultFileName = "default-regrows";
				fileName = Globals.RNGGetAndRemoveRandomValueFromList(RegrowingImages);
			}

			else if (growthInfo.GrowthStages.Count <= 4)
			{
				defaultFileName = "default-4";
				fileName = Globals.RNGGetAndRemoveRandomValueFromList(Normal4StageImages);
			}

			else
			{
				defaultFileName = "default-5";
				fileName = Globals.RNGGetAndRemoveRandomValueFromList(Normal5StageImages);
			}

			if (string.IsNullOrEmpty(fileName))
			{
				Globals.ConsoleWarn($"Using default image for crop growth - you may not have enough crop growth images: {position.X}, {position.Y}");
				return $"{ImageDirectory}/{defaultFileName}.png";
			}


			CropIdsToImageNames[cropId] = Path.GetFileName(fileName).Replace("-4.png", ".png").Replace("-5.png", ".png");
			return fileName;
		}


		/// <summary>
		/// Fix the width value given the graphic id
		/// This is to prevent the giant cauliflower from being cut off
		/// </summary>
		/// <param name="graphicId">The graphic ID to check</param>
		private void FixWidthValue(int graphicId)
		{
			List<int> graphicIndexesWithSmallerWidths = new List<int> { 32, 34 };
			if (graphicIndexesWithSmallerWidths.Contains(graphicId))
			{
				ImageWidthInPx = 112;
			}
			else
			{
				ImageWidthInPx = 128;
			}
		}

		/// <summary>
		/// Whether the settings premit random crop growth images
		/// </summary>
		/// <returns>True if so, false otherwise</returns>
		public override bool ShouldSaveImage()
		{
			return Globals.Config.Crops.Randomize && Globals.Config.Crops.UseCustomImages;
		}
	}
}
