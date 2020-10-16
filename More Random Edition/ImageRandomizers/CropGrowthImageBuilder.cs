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
		private const string FlowersDirectory = "Flowers";

		private List<string> NormalImages { get; set; }
		private List<string> RegrowingImages { get; set; }
		private List<string> TrellisImages { get; set; }
		private List<string> FlowerImages { get; set; }

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
			OffsetHeightInPx = 32;
			OffsetWidthInPx = 128;

			NormalImages = Directory.GetFiles($"{ImageDirectory}/{NormalDirectory}")
				.Where(x => x.EndsWith("-4.png") || x.EndsWith("-5.png"))
				.Select(x => x.Replace("-4.png", "").Replace("-5.png", ""))
				.Distinct()
				.OrderBy(x => x)
				.ToList();

			RegrowingImages = Directory.GetFiles($"{ImageDirectory}/{RegrowingDirectory}").Where(x => x.EndsWith(".png")).OrderBy(x => x).ToList();
			TrellisImages = Directory.GetFiles($"{ImageDirectory}/{TrellisDirectory}").Where(x => x.EndsWith(".png")).OrderBy(x => x).ToList();
			FlowerImages = Directory.GetFiles($"{ImageDirectory}/{FlowersDirectory}").Where(x => x.EndsWith(".png")).OrderBy(x => x).ToList();

			ValidateCropImages();
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
				defaultFileName = "default-flower";
				fileName = Globals.RNGGetAndRemoveRandomValueFromList(FlowerImages);
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

			else
			{
				fileName = Globals.RNGGetAndRemoveRandomValueFromList(NormalImages);

				if (growthInfo.GrowthStages.Count <= 4)
				{
					defaultFileName = "default-4";
					fileName += "-4.png";
				}

				else
				{
					defaultFileName = "default-5";
					fileName += "-5.png";
				}
			}

			if (string.IsNullOrEmpty(fileName) || fileName == "-4.png" || fileName == "-5.png")
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

		/// <summary>
		/// Validates that the crop growth images map to the appropriate directories
		/// </summary>
		private void ValidateCropImages()
		{
			// Gather data for normal images
			string normalCropGrowthDirectory = $"{ImageDirectory}/{NormalDirectory}";
			List<string> normalImageNames = Directory.GetFiles(normalCropGrowthDirectory).ToList();

			List<string> normal4StageImages = normalImageNames
				.Where(x => x.EndsWith("-4.png"))
				.Select(x => Path.GetFileName(x.Replace("-4.png", "")))
				.ToList();

			List<string> normal5StageImages = normalImageNames
				.Where(x => x.EndsWith("-5.png"))
				.Select(x => Path.GetFileName(x.Replace("-5.png", "")))
				.ToList();

			// Validate that the stage 4 and 5 match
			if (normal4StageImages.Count != normal5StageImages.Count)
			{
				string missingNumber = normal5StageImages.Count > normal4StageImages.Count ? "5" : "4";
				Globals.ConsoleWarn($"Missing a stage {missingNumber} image at: {normalCropGrowthDirectory}");
			}

			// Gather the all of the crop growth images and validate whether their names are all unique
			List<string> normalCropGrowthImages = NormalImages.Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
			List<string> regrowingCropGrowthImages = RegrowingImages.Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
			List<string> trellisCropGrowthImages = TrellisImages.Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
			List<string> flowerCropGrowthImages = FlowerImages.Select(x => Path.GetFileNameWithoutExtension(x)).ToList();

			List<string> allCropGrowthImages = normalCropGrowthImages
				.Concat(regrowingCropGrowthImages)
				.Concat(trellisCropGrowthImages)
				.Concat(flowerCropGrowthImages)
				.Distinct()
				.ToList();

			int countOfEachPath = normalCropGrowthImages.Count + regrowingCropGrowthImages.Count + trellisCropGrowthImages.Count + flowerCropGrowthImages.Count;
			if (allCropGrowthImages.Count != countOfEachPath)
			{
				Globals.ConsoleWarn($"Duplicate image name detected in one of the folders at: {ImageDirectory}");
			}

			// Check that every crop growth image has a matching seed packet
			string seedImageDirectory = $"{CustomImagesPath}/SpringObjects/Seeds";
			List<string> seedImageNames = Directory.GetFiles(seedImageDirectory)
				.Where(x => x.EndsWith(".png"))
				.Select(x => Path.GetFileNameWithoutExtension(x))
				.ToList();

			foreach (string growthImageName in allCropGrowthImages)
			{
				if (!seedImageNames.Contains(growthImageName))
				{
					Globals.ConsoleWarn($"{growthImageName}.png not found at: {seedImageDirectory}");
				}
			}

			// Check that all crop growth images exist as a crop or flower
			string cropImageDirectory = $"{CustomImagesPath}/SpringObjects/Crops";
			List<string> cropImageNames = Directory.GetFiles(cropImageDirectory)
				.Where(x => x.EndsWith(".png"))
				.Select(x => Path.GetFileNameWithoutExtension(x))
				.ToList();

			foreach (string cropImageName in normalCropGrowthImages.Concat(regrowingCropGrowthImages).Concat(trellisCropGrowthImages))
			{
				if (!cropImageNames.Contains(cropImageName))
				{
					Globals.ConsoleWarn($"{cropImageName}.png not found at: {cropImageDirectory}");
				}
			}

			string flowerImageDirectory = $"{CustomImagesPath}/SpringObjects/Flowers";
			List<string> flowerImageNames = Directory.GetFiles(flowerImageDirectory)
				.Where(x => x.EndsWith(".png"))
				.Select(x => Path.GetFileNameWithoutExtension(x))
				.ToList();

			foreach (string flowerImageName in flowerCropGrowthImages)
			{
				if (!flowerImageNames.Contains(flowerImageName))
				{
					Globals.ConsoleWarn($"{flowerImageName}.png not found at: {flowerImageDirectory}");
				}
			}
		}
	}
}
