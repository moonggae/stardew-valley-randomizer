namespace Randomizer
{
	public class CropGrowthImageBuilder : ImageBuilder
	{
		public CropGrowthImageBuilder()
		{
			HeightInPx = 32;
			WidthInPx = 128; // TODO 112 for normal crops - IMPORTANT so we don't delete part of the giant cauliflower!
		}

		//TODO: for every seed (crop?), include the index into the crops.png image


		/// <summary>
		/// Whether the settings premit random crop growth images
		/// </summary>
		/// <returns>True if so, false otherwise</returns>
		public override bool ShouldSaveImage()
		{
			return Globals.Config.Crops.Randomize && Globals.Config.Crops.RandomizeImages;
		}
	}
}
