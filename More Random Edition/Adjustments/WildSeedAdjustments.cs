using StardewValley;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SVCrop = StardewValley.Crop;
using SVSeason = StardewValley.Season;

namespace Randomizer
{
    public class WildSeedAdjustments
	{
		/// <summary>
		/// This is the method to repalce the existing Crop.getRandomWildCropForSeason
		/// This will make the seed grow a crop of an actual appropriate type
		/// </summary>
		/// <param name="season">The relevant season</param>
		/// <returns>The ID of the random wild crop</returns>
		public virtual string GetRandomWildCropForSeason(SVSeason season)
		{
            List<string> wildCropIDs;
			switch (season)
			{
				case SVSeason.Spring:
					wildCropIDs = ItemList.GetForagables(Seasons.Spring)
						.Where(x => x.ShouldBeForagable).Select(x => x.QualifiedId).ToList();
					break;
				case SVSeason.Summer:
					wildCropIDs = ItemList.GetForagables(Seasons.Summer)
						.Where(x => x.ShouldBeForagable).Select(x => x.QualifiedId).ToList();
					break;
				case SVSeason.Fall:
					wildCropIDs = ItemList.GetForagables(Seasons.Fall)
						.Where(x => x.ShouldBeForagable).Select(x => x.QualifiedId).ToList();
					break;
				case SVSeason.Winter:
					wildCropIDs = ItemList.GetForagables(Seasons.Winter)
						.Where(x => x.ShouldBeForagable).Select(x => x.QualifiedId).ToList();
					break;
				default:
					Globals.ConsoleWarn($"GetRandomWildCropForSeason was passed an unexpected season value: {season}. Returning the ID for horseradish.");
					return ItemList.GetQualifiedId(ObjectIndexes.WildHorseradish);
			}

			return Globals.RNGGetRandomValueFromList(wildCropIDs, Game1.random);
		}

		/// <summary>
		/// Replaces the Crop.getRandomWildCropForSeason method in Stardew Valley's Crop.cs 
		/// with this file's GetRandomWildCropForSeason method
		/// NOTE: THIS IS UNSAFE CODE, CHANGE WITH EXTREME CAUTION
		/// </summary>
		public static void ReplaceGetRandomWildCropForSeason()
		{
            /// TODO 1.6: This seems to crash games on load - something is wrong!
            MethodInfo methodToReplace = typeof(SVCrop).GetMethod("getRandomWildCropForSeason");
			MethodInfo methodToInject = typeof(WildSeedAdjustments).GetMethod("GetRandomWildCropForSeason");
			Globals.RepointMethod(methodToReplace, methodToInject);
		}
	}
}
