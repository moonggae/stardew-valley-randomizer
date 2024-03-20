using StardewValley;
using System.Collections.Generic;

namespace Randomizer
{
    public class BootRandomizer
	{
		private readonly static Dictionary<string, BootItem> Boots = new();

        /// <summary>
        /// The data from Data/Boots.xnb
        /// </summary>
        public static Dictionary<string, string> BootData { get; private set; }

        /// <summary>
        /// Randomizes boots - currently only changes defense and immunity
        /// </summary>
        /// <returns />
        public static Dictionary<string, string> Randomize()
		{
            // Initialize boot data here so that it's reloaded in case of a locale change
			// This is also used by SpringObjectBuilder, so do this here even if we aren't randomizing boots
            BootData = DataLoader.Boots(Game1.content);

            Dictionary<string, string> bootReplacements = new();
            if (!Globals.Config.Boots.Randomize) 
			{ 
				return bootReplacements;
			}

            RNG rng = RNG.GetFarmRNG(nameof(BootRandomizer));
            Boots.Clear();

			WeaponAndArmorNameRandomizer nameRandomizer = new(nameof(BootRandomizer));
			List<string> descriptions = 
				NameAndDescriptionRandomizer.GenerateBootDescriptions(BootData.Count);
			List<BootItem> bootsToUse = new();

			int index = 0;
			foreach (KeyValuePair<string, string> bootData in BootData)
			{
				string[] bootStringData = bootData.Value.Split("/");
				int originalDefense = int.Parse(bootStringData[(int)BootIndexes.Defense]);
                int originalImmunity = int.Parse(bootStringData[(int)BootIndexes.Immunity]);

                int statPool = rng.NextIntWithinPercentage(originalDefense + originalImmunity, 30);
				int defense = rng.NextIntWithinRange(0, statPool);
				int immunity = statPool - defense;

				if ((defense + immunity) == 0)
				{
					if (rng.NextBoolean())
					{
						defense = 1;
					}
					else
					{
						immunity = 1;
					}
				}

				BootItem newBootItem = new(
					bootData.Key,
					nameRandomizer.GenerateRandomBootName(),
					descriptions[index],
					defense,
					immunity);

				bootsToUse.Add(newBootItem);
				Boots.Add(newBootItem.Id, newBootItem);

				index++;
			}

			foreach (BootItem bootToAdd in bootsToUse)
			{
				bootReplacements.Add(bootToAdd.Id.ToString(), bootToAdd.ToString());
			}

			WriteToSpoilerLog(bootsToUse);
			return bootReplacements;
		}

		/// <summary>
		/// Writes the boots to the spoiler log
		/// </summary>
		/// <param name="bootsToUse">The boot data that was used</param>
		public static void WriteToSpoilerLog(List<BootItem> bootsToUse)
		{
			Globals.SpoilerWrite("==== BOOTS ====");
			foreach (BootItem bootToAdd in bootsToUse)
			{
				Globals.SpoilerWrite($"{bootToAdd.Name}: +{bootToAdd.Defense} defense; +{bootToAdd.Immunity} immunity");
			}
			Globals.SpoilerWrite("");
		}
	}
}
