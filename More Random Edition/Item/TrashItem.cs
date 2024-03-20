namespace Randomizer
{
	/// <summary>
	/// Represents a trash item that you can get while fishing, for example
	/// </summary>
	public class TrashItem : Item
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="index">The item index</param>
		public TrashItem(ObjectIndexes index) : base(index)
		{
			DifficultyToObtain = ObtainingDifficulties.NoRequirements;
			IsTrash = true;
		}
	}
}
