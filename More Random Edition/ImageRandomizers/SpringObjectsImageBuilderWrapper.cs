using System.Collections.Generic;

namespace Randomizer
{
	/// <summary>
	/// Wraps together all the classes that modify the spring objects file
	/// and enables them all to write to the image file without overwriting 
	/// each other
	/// </summary>
	public class SpringObjectsImageBuilderWrapper
	{
		public CropGrowthImageBuilder CropGrowthBuilder;
		public FishImageBuilder FishBuilder;
		public CropImageBuilder CropBuilder;
		public SeedImageBuilder SeedBuilder;

		public string OutputFileFullPath;
		public string SMAPIOutputFilePath;

		/// <summary>
		/// Builds the image using all the image builders
		/// Note that it doesn't matter which one we use for the path info since they'll all return
		/// the same data
		/// </summary>
		public void BuildImage()
		{
			CropGrowthBuilder = new CropGrowthImageBuilder();
			CropGrowthBuilder.BuildImage();

			FishBuilder = new FishImageBuilder();
			CropBuilder = new CropImageBuilder(CropGrowthBuilder.CropIdsToImageNames);
			SeedBuilder = new SeedImageBuilder(CropGrowthBuilder.CropIdsToImageNames);

			OutputFileFullPath = FishBuilder.OutputFileFullPath;
			SMAPIOutputFilePath = FishBuilder.SMAPIOutputFilePath;

			ImageBuilder.BuildImage(new List<ImageBuilder>
				{
					FishBuilder,
					CropBuilder,
					SeedBuilder
				},
				FishBuilder.BaseFileFullPath,
				FishBuilder.OutputFileFullPath
			);
		}

		/// <summary>
		/// Returns whether we should save the spring objects image
		/// This is true if any of the three builders return true
		/// </summary>
		/// <returns />
		public bool ShouldSaveSpringObjectsImage()
		{
			if (FishBuilder == null || CropBuilder == null || SeedBuilder == null)
			{
				return false;
			}
			return FishBuilder.ShouldSaveImage() || CropBuilder.ShouldSaveImage() || SeedBuilder.ShouldSaveImage();
		}

		/// <summary>
		/// Returns whether we should save the crop growth image
		/// </summary>
		/// <returns />
		public bool ShouldSaveCropGrowthImage()
		{
			return CropGrowthBuilder.ShouldSaveImage();
		}
	}
}
