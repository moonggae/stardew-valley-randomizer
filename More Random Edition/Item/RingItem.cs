using StardewValley;
using SVRing = StardewValley.Objects.Ring;

namespace Randomizer
{
	/// <summary>
	/// Represents a fish
	/// </summary>
	public class RingItem : Item
	{
		public RingItem(int id) : base(id)
		{
			DifficultyToObtain = ObtainingDifficulties.NonCraftingItem;
			IsRing = true;
			CanStack = false;
		}

        public override ISalable GetSaliableObject(int initialStack = 1, bool isRecipe = false, int price = -1)
        {
			return new SVRing(Id)
			{
				Stack = initialStack
			};
        }
    }
}
