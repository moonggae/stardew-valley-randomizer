using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.GameData.Characters;
using System.Collections.Generic;

namespace Randomizer
{
    /// <summary>
    /// Randomizes the birthdays of all NPCs
    /// </summary>
    public class BirthdayRandomizer
	{
		/// <summary>
		/// TODO 1.6: When the date of the new festival is known, we need to add to this list!
		/// Holidays - don't assign birthdays to these days!
		/// </summary>
		private readonly static List<SDate> Holidays = new()
		{
			new(13, "spring", 1),
			new(24, "spring", 1),
			new(11, "summer", 1),
			new(16, "fall", 1),
			new(27, "fall", 1),
			new(8, "winter", 1),
			new(25, "winter", 1),
			new(15, "winter", 1),
			new(16, "winter", 1),
			new(17, "winter", 1)
		};

		/// <summary>
		/// The string to use for holidays in the birthdays in use list
		/// </summary>
		private const string HolidayString = "HOLIDAY";

		/// <summary>
		/// Does the birthday randomization
		/// </summary>
		/// <returns>The dictionary to use for replacements</returns>
		public static Dictionary<string, CharacterData> Randomize()
		{
			Dictionary<SDate, string> birthdaysInUse = InitBirthdaysInUse();
			Dictionary<string, CharacterData> replacements = new();
			Dictionary<string, CharacterData> characterData = DataLoader.Characters(Game1.content);

            foreach (KeyValuePair<string, CharacterData> dispositionData in characterData)
			{
				string npcName = dispositionData.Key;
				CharacterData npc = dispositionData.Value;

				// Don't replace a non-existant birthday
				if (npc.BirthSeason == null)
				{
					continue;
				}

                AddRandomBirthdayToNPC(npcName, npc, birthdaysInUse);
				replacements.Add(npcName, npc);
			}

			WriteToSpoilerLog(birthdaysInUse);
			return replacements;
		}

		/// <summary>
		/// Initializes the birthdays in use - adds all the holidays to it so that they can't
		/// be picked
		/// </summary>
		/// <returns />
		private static Dictionary<SDate, string> InitBirthdaysInUse()
		{
			Dictionary<SDate, string> birthdaysInUse = new();
			foreach (SDate holidayDate in Holidays)
			{
				birthdaysInUse.Add(holidayDate, HolidayString);
			}
			return birthdaysInUse;
		}

		/// <summary>
		/// Gets a random Stardew Valley date - excludes the holidays and birthdays already in use
		/// </summary>
		/// <param name="npcName">The name of the NPC - used as the key</param>
		/// <param name="npc">The npc to modify</param>
		/// <param name="birthdaysInUse">The birthdays in use - this function adds the date to it</param>
		private static void AddRandomBirthdayToNPC(
            string npcName,
            CharacterData npc,
            Dictionary<SDate, string> birthdaysInUse)
		{
			if (npcName == "Wizard")
			{
				SetWizardBirthday(npc, birthdaysInUse);
				return;
			}

			List<string> seasonStrings = new() { "spring", "summer", "fall", "winter" };
			string season = Globals.RNGGetRandomValueFromList(seasonStrings);
			bool dateRetrieved = false;
			SDate date;

			do
			{
				date = new SDate(Range.GetRandomValue(1, 28), season, 1);
				if (!birthdaysInUse.ContainsKey(date))
				{
					birthdaysInUse.Add(date, npcName);
                    npc.BirthSeason = date.Season;
					npc.BirthDay = date.Day;
                    dateRetrieved = true;
				}
			} while (!dateRetrieved);
		}

		/// <summary>
		/// Sets the wizard's birthday - must be from the 15-17, as the game hard-codes the "Night Market" text
		/// to the billboard
		/// </summary>
		/// <param name="wizard">The wizard to modify</param>
		/// <param name="birthdaysInUse">The birthdays in use - this function adds the date to it</param>
		/// <returns />
		private static void SetWizardBirthday(CharacterData wizard, Dictionary<SDate, string> birthdaysInUse)
		{
			int day = Range.GetRandomValue(15, 17);
			SDate wizardBirthday = new(day, "winter", 1);

			birthdaysInUse.Remove(wizardBirthday);
			birthdaysInUse.Add(wizardBirthday, "Wizard");

			wizard.BirthSeason = wizardBirthday.Season;
			wizard.BirthDay = wizardBirthday.Day;
		}

		/// <summary>
		/// Write to the spoiler log
		/// </summary>
		/// <param name="replacements">The replacements made - need to filter out the "HOLIDAY" entries</param>
		private static void WriteToSpoilerLog(Dictionary<SDate, string> replacements)
		{
			if (!Globals.Config.NPCs.RandomizeBirthdays) { return; }

			Globals.SpoilerWrite("===== NPC BIRTHDAYS =====");
			foreach (SDate date in replacements.Keys)
			{
				string npcName = replacements[date];
				if (npcName == HolidayString) { continue; }

				Globals.SpoilerWrite($"{npcName}: {date.Season} {date.Day}");
			}
			Globals.SpoilerWrite("");
		}
	}
}
