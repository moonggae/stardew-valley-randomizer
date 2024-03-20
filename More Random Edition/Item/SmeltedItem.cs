namespace Randomizer
{
	/// <summary>
	/// Represents items requiring a furnace to easily obtain
	/// </summary>
	public class SmeltedItem : Item
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="index">The index of the item</param>
		/// <param name="difficultyToObtain">The difficulty to obtain this item - defaults to medium</param>
		public SmeltedItem(ObjectIndexes index, 
			ObtainingDifficulties difficultyToObtain = ObtainingDifficulties.MediumTimeRequirements) 
			: base(index)
		{
			DifficultyToObtain = difficultyToObtain;
			IsSmelted = true;
			ItemsRequiredForRecipe = new Range(1, 5);
		}
	}
}
