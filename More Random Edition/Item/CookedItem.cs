using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
    /// <summary>
    /// Represents an item you make in your kitchen
    /// </summary>
    public class CookedItem : Item
	{
		/// <summary>
		/// The id of the special ingredient used to cook this item
		/// </summary>
		public int? IngredientId { get; set; } = null;

        /// <summary>
        /// The special ingredient used to cook this item
        /// </summary>
        public string IngredientName { 
			get 
			{
				return IngredientId == null
					? string.Empty
					: ItemList.Items[IngredientId.Value].Name;
			}
		}

		/// <summary>
		/// The original name of the item
		/// </summary>
		public string OriginalName { get; set; }

		/// <summary>
		/// Whether this is a dish where the name was changed
		/// </summary>
		public bool IsCropOrFishDish { get; set; }

		/// <summary>
		/// True if it's a fish dish
		/// </summary>
		public bool IsFishDish { get; set; }

        /// <summary>
        /// True if it's a crop dish - based on the fish dish value
        /// </summary>
        public bool IsCropDish { 
			get
			{
				return IsCropOrFishDish && !IsFishDish;
			}
		}

        /// <summary>
        /// The object text between the id name and display name
        /// </summary>
        private string TextBetweenNames { get; set; }

        /// <summary>
        /// The ending object text on the item
        /// </summary>
        private string EndingObjectText { get; set; }

		public CookedItem(int id) : base(id)
		{
			IsCooked = true;
			OriginalName = Name;
			DifficultyToObtain = ObtainingDifficulties.LargeTimeRequirements;
		}

		public CookedItem(
			int id, 
			int? ingredientId, 
			string textBetweenNames, 
			string endingObjectText, 
			bool isFishDish = false): this(id)
		{
			IsCropOrFishDish = true;
			IngredientId = ingredientId;
            TextBetweenNames = textBetweenNames;
			EndingObjectText = endingObjectText;
			IsFishDish = isFishDish;
        }

		/// <summary>
		/// We can't do this on construction, since the randomized names have not happened yet
		/// </summary>
		public void CalculateOverrideName()
		{
			if (IsCropOrFishDish)
            {
                string nameAndDescription = Globals.GetTranslation($"item-{Id}-name-and-description", new { itemName = IngredientName });
                string name = nameAndDescription.Split("/")[0];
                OverrideName = string.Format(name, IngredientName);
            }
        }

        /// <summary>
        /// Gets all the fish dishes
        /// </summary>
        /// <returns />
		public static List<CookedItem> GetAllFishDishes()
		{
			return ItemList.Items.Values
				.Where(item => item is CookedItem cookedItem && cookedItem.IsFishDish)
				.Cast<CookedItem>()
				.ToList();
		}

        /// <summary>
        /// Gets all the crop dishes
        /// </summary>
        /// <returns />
        public static List<CookedItem> GetAllCropDishes()
        {
            return ItemList.Items.Values
                .Where(item => item is CookedItem cookedItem && cookedItem.IsCropDish)
                .Cast<CookedItem>()
                .ToList();
        }

        /// <summary>
        /// Returns the ToString representation to be used for the cooked item object
        /// </summary>
        /// <returns />
        public override string ToString()
		{
			if (!IsCropOrFishDish)
			{
				Globals.ConsoleWarn($"Unexpected ToString call of cooked item: {OriginalName}");
				return "";
			}

			string NameAndDescription = Globals.GetTranslation($"item-{Id}-name-and-description", new { itemName = IngredientName });
            return $"{OriginalName}/{TextBetweenNames}/{NameAndDescription}/{EndingObjectText}";
		}
    }
}
