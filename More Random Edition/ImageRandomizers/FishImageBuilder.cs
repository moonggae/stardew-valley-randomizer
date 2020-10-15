namespace Randomizer
{
	/// <summary>
	/// The image builder for fish icons - inherits from the spring objects builder because
	/// it modifies the springobjects.xnb image file
	/// </summary>
	public class FishImageBuilder : SpringObjectsImageBuilder
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public FishImageBuilder() : base("Fish")
		{
			PositionsToOverlay = GetAllPoints(FishItem.Get(true));
		}

		/// <summary>
		/// Whether the settings premit random fish images
		/// </summary>
		/// <returns>True if so, false otherwise</returns>
		public override bool ShouldSaveImage()
		{
			return Globals.Config.Fish.Randomize && Globals.Config.Fish.UseCustomImages;
		}
	}
}
