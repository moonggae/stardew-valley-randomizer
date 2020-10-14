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
		public string OutputFileFullPath;
		public string SMAPIOutputFullPath;

		/// <summary>
		/// Builds the image using all the image builders
		/// Note that it doesn't matter which one we use for the path info since they'll all return
		/// the same data
		/// </summary>
		public void BuildImage()
		{
			FishImageBuilder fishBuilder = new FishImageBuilder();
			CropImageBuilder cropBuilder = new CropImageBuilder();
			SeedImageBuilder seedBuilder = new SeedImageBuilder(cropBuilder.CropIdsToImageNames);

			OutputFileFullPath = fishBuilder.OutputFileFullPath;
			SMAPIOutputFullPath = fishBuilder.SMAPIOutputFilePath;

			ImageBuilder.BuildImage(new List<ImageBuilder>
				{
					fishBuilder,
					cropBuilder,
					seedBuilder
				},
				fishBuilder.BaseFileFullPath,
				fishBuilder.OutputFileFullPath
			);
		}
	}
}
