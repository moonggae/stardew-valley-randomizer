namespace Randomizer
{
	/// <summary>
	/// Represents an item that is normally foragable
	/// </summary>
	public class ForagableItem : Item
	{
		public ForagableItem(ObjectIndexes index) : base(index)
		{
			ShouldBeForagable = true;
			DifficultyToObtain = ObtainingDifficulties.LargeTimeRequirements;
			ItemsRequiredForRecipe = new Range(1, 3);
		}
	}
}
