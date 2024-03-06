using System;

namespace Randomizer
{
	/// <summary>
	/// Contains one set of items and their rarity
	/// </summary>
	public class ForagableData
	{
		public string QualifiedItemId { get; }
        public int ItemId { get; }
        public double ItemRarity { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="item">The item that the data is for</param>
		/// <param name="rng">The RNG to use</param>
		public ForagableData(Item item, RNG rng)
		{
			QualifiedItemId = item.QualifiedId;
			ItemId = item.Id;

			bool useNormalDistribution = rng.Next(0, 2) == 0;
			ItemRarity = useNormalDistribution ? (double)rng.Next(4, 8) / 10 : (double)rng.Next(1, 9) / 10;
		}
	}
}
