using StardewValley;

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

	public class SeasonFunctions
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
	}
}
