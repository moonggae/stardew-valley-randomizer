namespace Randomizer
{
	/// <summary>
	/// Represents a trash item that you can get while fishing, for example
	/// </summary>
	public class CrabPotItem : Item
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="index">The item index</param>
		public CrabPotItem(ObjectIndexes index) : base(index)
		{
			DifficultyToObtain = ObtainingDifficulties.MediumTimeRequirements;
		}
	}
}
