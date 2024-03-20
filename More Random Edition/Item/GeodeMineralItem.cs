namespace Randomizer
{
	/// <summary>
	/// Minerals you can only get from geodes
	/// </summary>
	public class GeodeMineralItem : Item
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="index">The id of the item</param>
		public GeodeMineralItem(ObjectIndexes index) : base(index)
		{
			IsGeodeMineral = true;
			DifficultyToObtain = ObtainingDifficulties.LargeTimeRequirements;
		}
	}
}
