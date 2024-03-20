namespace Randomizer
{
    /// <summary>
    /// Represents a fish
    /// </summary>
    public class RingItem : Item
	{
		public RingItem(ObjectIndexes index) : base(index)
		{
			DifficultyToObtain = ObtainingDifficulties.NonCraftingItem;
			IsRing = true;
			CanStack = false;
		}
    }
}
