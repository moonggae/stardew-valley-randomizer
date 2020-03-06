using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Randomizer
{
	public class FishImageBuilder : ImageBuilder
	{
		private const int ItemsPerRow = 24;

		public FishImageBuilder() : base()
		{
			BaseFileName = "springobjects.png";
			SubDirectory = "Fish";
			PositionsToOverlay = GetAllFishPoints();
		}

		/// <summary>
		/// Gets a list of all points to replace with fish in the default file
		/// These are all based on the id of the fish
		/// </summary>
		/// <returns>A list of points</returns>
		private List<Point> GetAllFishPoints()
		{
			return FishItem.Get(true)
				.Select(x => new Point(x.Id % ItemsPerRow, x.Id / ItemsPerRow))
				.ToList();
		}

		/// <summary>
		/// Whether the settings premit random fish images
		/// </summary>
		/// <returns>True if so, false otherwise</returns>
		public override bool ShouldSaveImage()
		{
			return Globals.Config.RandomizeFish && Globals.Config.UseCustomFishImages_Needs_Above_Setting_On;
		}
	}
}
