using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
    /// <summary>
    /// An enum for the seasons
    /// </summary>
    public enum Seasons
	{
		Spring,
		Summer,
		Fall,
		Winter
	}

	public static class SeasonsExtensions
	{
        /// <summary>
        /// Gets the season of the game as one of the enum values
        /// </summary>
        /// <returns>The current season</returns>
        public static Seasons GetCurrentSeason()
		{
			string currentSeason = Game1.currentSeason.ToLower();
			switch(currentSeason)
			{
				case "spring": return Seasons.Spring;
                case "summer": return Seasons.Summer;
                case "fall": return Seasons.Fall;
                case "winter": return Seasons.Winter;
				default:
					Globals.ConsoleError($"Tried to parse unexpected season string: {currentSeason}!");
					return Seasons.Spring; // Default to something
            }
		}

        /// <summary>
        /// Gets a random color that fits the given season
        /// </summary>
        /// <param name="season">The season</param>
        /// <returns>The chosen color</returns>
        public static Color GetRandomColorForSeason(this Seasons season)
        {
            Range SpringHueRange = new(100, 155);
            Range SummerHueRange = new(50, 65);
            Range FallHueRange = new(10, 40);
            Range WinterHueRange = new(180, 260);

            return season switch
            {
                Seasons.Spring => ImageManipulator.GetRandomColor(SpringHueRange),
                Seasons.Summer => ImageManipulator.GetRandomColor(SummerHueRange),
                Seasons.Fall => ImageManipulator.GetRandomColor(FallHueRange),
                _ => ImageManipulator.GetRandomColor(WinterHueRange)
            };
        }

        /// <summary>
        /// Gets a random color for a list of given seasons
        /// 1 in list: calls the normal function for it
        /// 2 in list: averages them out
        /// 3+ in list: uses CyanAndBlue (passes Winter over)
        /// </summary>
        /// <param name="season">The season</param>
        /// <returns>The chosen color</returns>
        public static Color GetRandomColorForSeasons(List<Seasons> seasons)
        {
            switch (seasons.Count)
            {
                case 1:
                    return seasons[0].GetRandomColorForSeason();
                case 2:
                    var color1 = seasons[0].GetRandomColorForSeason();
                    var color2 = seasons[1].GetRandomColorForSeason();
                    return ImageManipulator.AverageColors(color1, color2);
                default:
                    return GetRandomColorForSeason(Seasons.Winter);
            }
        }

        /// <summary>
        /// Gets a random season from the enum
        /// </summary>
        /// <returns>The random season</returns>
        public static Seasons GetRandomSeason()
        {
            var allSeasons = Enum.GetValues(typeof(Seasons))
                .Cast<Seasons>()
                .ToList();

            return Globals.RNGGetRandomValueFromList(allSeasons);
        }
    }
}
