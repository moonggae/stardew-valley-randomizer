using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Randomizer
{
	public abstract class SpringObjectsImageBuilder : ImageBuilder
	{
		/// <summary>
		/// The number of items per row in the spring objects file
		/// </summary>
		protected const int ItemsPerRow = 24;

		/// <summary>
		/// The constructor
		/// </summary>
		/// <param name="customFolderName">The folder name of the image type being built</param>
		public SpringObjectsImageBuilder(string customFolderName) : base()
		{
			BaseFileName = "springobjects.png";
			SubDirectory = "Shared";
			CustomImageDirectory = $"{ImageDirectory}/{customFolderName}";
		}

		/// <summary>
		/// Gets all points from a list of items, based on the item id
		/// </summary>
		/// <param name="items"></param>
		/// <returns></returns>
		protected List<Point> GetAllPoints(List<Item> items)
		{
			return items.Select(item => GetPointFromItem(item)).ToList();
		}


		/// <summary>
		/// Gets a mapping of points to ids in the springobjects file given a list of items
		/// </summary>
		/// <param name="items"></param>
		/// <returns></returns>
		protected Dictionary<Point, int> GetPointsToIdsMapping(List<Item> items)
		{
			Dictionary<Point, int> pointsToIdsMapping = new Dictionary<Point, int>();
			foreach (Item item in items)
			{
				pointsToIdsMapping[GetPointFromItem(item)] = item.Id;
			}

			return pointsToIdsMapping;
		}

		/// <summary>
		/// Gets the point in the springobjects file that belongs to the given item
		/// </summary>
		/// <param name="item">The item</param>
		/// <returns />
		protected Point GetPointFromItem(Item item)
		{
			return new Point(item.Id % ItemsPerRow, item.Id / ItemsPerRow);
		}
	}
}
