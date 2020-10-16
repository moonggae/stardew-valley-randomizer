using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Randomizer
{
	public class BundleImageBuilder : ImageBuilder
	{
		/// <summary>
		/// A map of the bundle position in the dictionary to the id it belongs to
		/// </summary>
		private Dictionary<Point, Bundle> PointsToBundlesMap;

		/// <summary>
		/// The list of bundle image names - without the file extention
		/// </summary>
		private readonly List<string> BundleImageNames;

		public BundleImageBuilder() : base()
		{
			BaseFileName = Globals.GetTranslation("junimo-note-graphic");
			SubDirectory = "Bundles";
			SetUpPointsToBundlesMap();
			PositionsToOverlay = PointsToBundlesMap.Keys.ToList();

			BundleImageNames = Directory.GetFiles($"{ImageDirectory}")
				.Where(x => !x.EndsWith("default.png"))
				.Where(x => x.EndsWith(".png"))
				.Select(x => Path.GetFileNameWithoutExtension(x))
				.OrderBy(x => x).ToList();

			ImageHeightInPx = 32;
			ImageWidthInPx = 32;
			OffsetWidthInPx = 32;
			OffsetWidthInPx = 32;
			InitialHeightOffetInPx = 180;
		}

		/// <summary>
		/// Sets up the map to link bundle points to their IDs
		/// </summary>
		private void SetUpPointsToBundlesMap()
		{
			const int ItemsPerRow = 20;
			PointsToBundlesMap = new Dictionary<Point, Bundle>();
			foreach (RoomInformation room in BundleRandomizer.Rooms)
			{
				foreach (Bundle bundle in room.Bundles)
				{
					PointsToBundlesMap[new Point(bundle.Id % ItemsPerRow, bundle.Id / ItemsPerRow)] = bundle;
				}
			}
		}

		/// <summary>
		/// Gets a random file name that matches the weapon type at the given position
		/// Will remove the name found from the list
		/// </summary>
		/// <param name="position">The position</param>
		/// <returns>The selected file name</returns>
		protected override string GetRandomFileName(Point position)
		{
			Bundle bundle = PointsToBundlesMap[position];
			return $"{ImageDirectory}/{bundle.ImageName}.png";
		}

		/// <summary>
		/// Whether the settings premit random weapon images
		/// </summary>
		/// <returns>True if so, false otherwise</returns>
		public override bool ShouldSaveImage()
		{
			return Globals.Config.RandomizeBundles;
		}

		/// <summary>
		/// Whether the settings premit random weapon images
		/// </summary>
		/// <returns>True if so, false otherwise</returns>
		protected override bool ShouldSaveImage(Point position)
		{
			if (!Globals.Config.RandomizeBundles) { return false; }

			Bundle bundle = PointsToBundlesMap[position];

			if (BundleImageNames.Contains(bundle.ImageName))
			{
				return true;
			}

			Globals.ConsoleWarn($"Could not find bundle image: {ImageDirectory}/{bundle.ImageName}");
			return false;
		}
	}
}
