using Netcode;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using SVItem = StardewValley.Item;
using SVObject = StardewValley.Object;

namespace Randomizer
{
    public class BundleMenuAdjustments
	{
		/// <summary>
		/// Fixes the ability to highlight rings in the bundle menu
		/// </summary>
		public static void FixRingSelection(JunimoNoteMenu e)
		{
			if (!Globals.Config.Bundles.Randomize)
			{
				return;
			}

            e.inventory.highlightMethod = HighlightBundleCompatibleItems;
		}

		/// <summary>
		/// A copy of the Utlity.cs code for highlightSmallObjects, but with rings included
		/// </summary>
		/// <param name="item">The Stardew Valley item</param>
		/// <returns>True if the item should be draggable, false otherwise</returns>
		private static bool HighlightBundleCompatibleItems(SVItem item)
		{
			if (item is Ring)
			{
				return true;
			}
			else if (item is SVObject)
			{
				return !(bool)((NetFieldBase<bool, NetBool>)(item as SVObject).bigCraftable);
			}
			return false;
		}

		/// <summary>
		/// Adds tooltips for the bundle items so that you can see where to get fish
		/// </summary>
		public static void AddDescriptionsToBundleTooltips()
		{
			if (Game1.activeClickableMenu is not JunimoNoteMenu menu || 
				!Globals.Config.Bundles.Randomize || 
				!Globals.Config.Bundles.ShowDescriptionsInBundleTooltips)
			{
				return;
			}

			foreach (ClickableTextureComponent ingredient in menu.ingredientList)
			{
				ingredient.hoverText = $"{ingredient.item.DisplayName}:\n{ingredient.item.getDescription()}";
			}
		}
	}
}

