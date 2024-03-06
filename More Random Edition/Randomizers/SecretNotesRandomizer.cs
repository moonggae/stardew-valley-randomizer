using StardewValley;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	public class SecretNotesRandomizer
	{
        private static RNG Rng { get; set; }
        private static Dictionary<string, string> prefs;

		/// <summary>
		/// For each secret note, generate a random number of NPCs for whom to reveal loved items.
		/// </summary>
		/// <returns><c>Dictionary&lt;int, string&gt;</c> containing the secret note IDs and strings to replace.</returns>
		public static Dictionary<int, string> FixSecretNotes(Dictionary<string, string> preferenceReplacements)
		{
            Rng = RNG.GetFarmRNG(nameof(SecretNotesRandomizer));

            prefs = preferenceReplacements;
			Dictionary<int, string> _replacements = new();
			Dictionary<int, string> secretNoteData = DataLoader.SecretNotes(Game1.content);

			// The data dictionary has more entries than we want to change - we do only want indexes 1-9
            for (int noteIndex = 1; noteIndex < 9; noteIndex++)
			{
				// Pick from 1-3 random characters to reveal loved items for, choosing more items for fewer characters
				int numberOfCharactersToRevealFor = Rng.NextIntWithinRange(1, 3);
				int numberOfItemsToReveal = Rng.NextIntWithinRange(5, 6) - numberOfCharactersToRevealFor;

				List<string> charactersToRevealFor = Rng.GetRandomValuesFromList(
					PreferenceRandomizer.GiftableNPCs.Values.ToList(), numberOfCharactersToRevealFor);
				string npcLovesString = FormatRevealString(charactersToRevealFor, numberOfItemsToReveal);

				string dataWithoutReveal = secretNoteData[noteIndex].Split("%revealtaste")[0];
                string noteText = $"{dataWithoutReveal}{npcLovesString}";
				_replacements.Add(noteIndex, noteText);
			}

			WriteToSpoilerLog(_replacements);

			return _replacements;
		}

		/// <summary>
		/// Formats set of reveal commands (e.g. <c>"%revealtaste:Sam:270%revealtaste:Maru:113..."</c>).
		/// </summary>
		/// <param name="npcs">NPCs to reveal items for</param>
		/// <param name="numberToReveal">Number of items to reveal for each NPC</param>
		/// <returns><c>String</c> containing NPCs' preferences to reveal.</returns>
		private static string FormatRevealString(List<string> npcs, int numberToReveal)
		{
			string lovesString = "";

			foreach (string npc in npcs)
			{
				string[] tokens = prefs[npc].Split('/');
				List<string> items = tokens[1].Trim().Split(' ') 
					.Where(x => int.Parse(x) > 0)            
					.ToList();                               

				for (int num = numberToReveal; num > 0; num--)
				{
					lovesString += GetItemRevealString(npc, items);
				}
			}

			return lovesString;
		}

		/// <summary>
		/// Builds a single item reveal string (e.g. <c>%revealtaste:Abigail:206</c>)
		/// </summary>
		/// <param name="npc">Name of the NPC to build the item reveal string for</param>
		/// <param name="items">The list of items to select from - will remove the chosen entry from it!</param>
		/// <returns>String representing item reveal command</returns>
		private static string GetItemRevealString(string npc, List<string> items)
		{
			return items.Any()
				? $"%revealtaste:{npc}:{Rng.GetAndRemoveRandomValueFromList(items)}"
				: "";
		}

		/// <summary>
		/// Log the results
		/// </summary>
		/// <param name="replacements">The results</param>
		private static void WriteToSpoilerLog(Dictionary<int, string> replacements)
		{
			if (!Globals.Config.NPCs.RandomizeIndividualPreferences) 
			{ 
				return; 
			}

			Globals.SpoilerWrite("===== SECRET NOTES =====");
			foreach (KeyValuePair<int, string> pair in replacements)
			{
				Globals.SpoilerWrite($"{pair.Key}: {pair.Value}");
			}
			Globals.SpoilerWrite("");
		}
	}
}
