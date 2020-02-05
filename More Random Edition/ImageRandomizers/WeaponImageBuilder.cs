using System.Collections.Generic;
using System.Drawing;

namespace Randomizer
{
	public class WeaponImageBuilder : ImageBuilder
	{
		public WeaponImageBuilder() : base()
		{
			BaseFileName = "weapons.png";
			SubDirectory = "weapons";
			PositionsToOverlay = new List<Point>()
			{
				new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(0, 3), new Point(0, 4), new Point(0, 5), new Point(0, 6), new Point(0, 7),
				new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(1, 3), new Point(1, 4), new Point(1, 5), new Point(1, 6), new Point(1, 7),
				new Point(2, 0), new Point(2, 1), new Point(2, 2), new Point(2, 3), new Point(2, 4), new Point(2, 5), new Point(2, 6), new Point(2, 7),
				new Point(3, 0), new Point(3, 1), new Point(3, 2), new Point(3, 3), new Point(3, 4), new Point(3, 5), new Point(3, 6), new Point(3, 7),
				new Point(4, 0), new Point(4, 1), new Point(4, 2), new Point(4, 3), new Point(4, 4), new Point(4, 5), new Point(4, 6), new Point(4, 7),
				new Point(5, 0), new Point(5, 1), new Point(5, 2), new Point(5, 3), new Point(5, 4), new Point(5, 5), new Point(5, 6),
				new Point(6, 0), new Point(6, 1), new Point(6, 2), new Point(6, 3), new Point(6, 4),
			};
		}
	}
}
