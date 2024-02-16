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
		/// <param name="id">The item id</param>
		public CrabPotItem(int id) : base(id)
		{
			DifficultyToObtain = ObtainingDifficulties.MediumTimeRequirements;
		}
	}
}
