using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
    /// <summary>
    /// Randomizes the preferences of all NPCs
    /// </summary>
    public class PreferenceRandomizer
	{
		/// <summary>
		/// Default data for universal preferences - these can be overridden by an NPC's individual preference
		/// </summary>
		private readonly static List<string> UniversalPreferenceKeys = new()
		{
			"Universal_Love",
			"Universal_Like",
			"Universal_Neutral",
			"Universal_Dislike",
			"Universal_Hate"
		};

		/// <summary>
		/// Item Category indexes - only includes categories which are giftable
		/// https://stardewcommunitywiki.com/Modding:Object_data#Categories
		/// </summary>
		private readonly static Dictionary<int, string> GiftableItemCategories = new()
		{
			[-2] = "Gems",
			[-4] = "Fish",
			[-5] = "Eggs",
			[-6] = "Milk",
			[-7] = "Cooking",
			[-8] = "Crafting",
			[-12] = "Minerals",
			[-15] = "Metals",
			[-16] = "Resources",
			[-20] = "Trash",
			[-21] = "Bait",
			[-22] = "Tackles",
			[-24] = "Decor",
			[-26] = "Artisan Goods",
			[-27] = "Tree Products",
			[-28] = "Monster Loot",
			[-74] = "Seeds",
			[-75] = "Vegetables",
			[-79] = "Fruit",
			[-80] = "Flowers",
			[-81] = "Foragables"
		};

        /// <summary>
        /// The data from Data/NPCGiftTastes.xnb
        /// </summary>
        public static Dictionary<string, string> GiftTasteData { get; private set; }

        /// <summary>
        /// Randomize NPC Preferences information.
        /// </summary>
        /// <returns>Dictionary&lt;string, string&gt; which holds the replacement prefstrings for the enabled preferences (NPC/Universal).</returns>
        public static Dictionary<string, string> Randomize()
        {   // Initialize gift taste data here so that it's reloaded in case of a locale change
            GiftTasteData = Globals.ModRef.Helper.GameContent
                .Load<Dictionary<string, string>>("Data/NPCGiftTastes");
            Dictionary<string, string> replacements = new();

			List<int> universalUnusedCategories = new(GiftableItemCategories.Keys);
			List<Item> universalUnusedItems = ItemList.GetGiftables();

			// Generate the universal preferences
			foreach (string key in UniversalPreferenceKeys)
			{
				replacements.Add(
                    key, 
					GetUniversalPreferenceString(universalUnusedCategories, universalUnusedItems));
			}

			// Generate randomized NPC Preferences strings
			foreach (string npcName in NPC.GiftableNPCs)
			{
				List<int> unusedCategories = new(GiftableItemCategories.Keys);
				List<Item> unusedItems = ItemList.GetGiftables();

				string[] giftTasteData = GiftTasteData[npcName].Split('/');
				foreach (NPCGiftTasteIndexes index in Enum.GetValues(typeof(NPCGiftTasteIndexes)))
				{
                    giftTasteData[(int)index] = GetPreferenceString(index, unusedCategories, unusedItems);
				}

				replacements.Add(npcName, string.Join("/", giftTasteData));
			}

			// Update Loves/Hates for Bundle reqs
			UpdateBundlePrefs(replacements);

			WriteToSpoilerLog(replacements);
			return replacements;
		}

		/// <summary>
		/// Builds universal preference string
		/// </summary>
		/// <param name="unusedCategories">Keeps track of which categories have not yet been assigned</param>
		/// <param name="unusedItems">Keeps track of which items have not yet been assigned.</param>
		/// <returns><c>string</c> containing IDs of categories and items</returns>
		private static string GetUniversalPreferenceString(List<int> unusedCategories, List<Item> unusedItems)
		{
			// No need to vary quantities per index.May end up with lots of loved items, lots of hated items, both, neither, etc.
			int catNum = Range.GetRandomValue(0, 10);
			int itemNum = Range.GetRandomValue(5, 30);

			string catString = "";
			string itemString = "";

			while (unusedCategories.Any() && catNum > 0)
			{
				catString += Globals.RNGGetAndRemoveRandomValueFromList(unusedCategories) + " ";
				catNum--;
			}

			while (unusedItems.Any() && itemNum > 0)
			{
				itemString += Globals.RNGGetAndRemoveRandomValueFromList(unusedItems).Id + " ";
				itemNum--;
			}

			return (catString + itemString).Trim();
		}

		/// <summary>
		/// Builds an NPC's preference string for a given index (loves, hates, etc.).
		/// </summary>
		/// <param name="index">the index of the NPC's prefstring.</param>
		/// <param name="unusedCategories">Holds list of categories which have not yet been assigned - prevents double-assignment.</param>
		/// <param name="unusedItems">Holds list of Items which have not yet been assigned - prevents double-assignment.</param>
		/// <returns>NPC's preference string for a given index.</returns>
		private static string GetPreferenceString(NPCGiftTasteIndexes index, List<int> unusedCategories, List<Item> unusedItems)
		{
			Range numberOfPrefs = new(0, 0);

			// Should probably be moved into its own function
			// Determine how many items to add per category - data available here: https://pastebin.com/gFEduBVd
			// Basically, add more loved/liked items than hated/disliked, and few neutrals
			switch (index)
			{
				// Loved Items - higher minimum, so generated bundles have more items to draw from
				case NPCGiftTasteIndexes.Loves:
					numberOfPrefs = new(6, 11);
					break;
				case NPCGiftTasteIndexes.Likes:
				case NPCGiftTasteIndexes.Dislikes:
                    numberOfPrefs = new(1, 18);
					break;
				case NPCGiftTasteIndexes.Hates:
                    numberOfPrefs = new(1, 11);
					break;
				case NPCGiftTasteIndexes.Neutral:
                    numberOfPrefs = new(1, 3);
					break;
			}

			int numberOfItems = numberOfPrefs.GetRandomValue();
			int numberOfCategories = Range.GetRandomValue(0, 2);

			string itemString = GetRandomItemString(unusedItems, numberOfItems);
			string catString = GetRandomCategoryString(unusedCategories, numberOfCategories);
			return $"{catString} {itemString}";
		}

		/// <summary>Builds a string consisting of <paramref name="quantity"/> randomly selected IDs from <paramref name="unusedItems"/>.</summary>
		/// <param name="quantity">the number of IDs to add.</param>
		/// <param name="unusedItems"> the list of IDs to pull from.</param>
		/// <returns>A string of Item IDs with no leading/trailing whitespace.</returns>
		private static string GetRandomItemString(List<Item> unusedItems, int quantity)
		{
			List<Item> giftableItems = new List<Item>(unusedItems);
			string itemString = "";

			for (int itemQuantity = quantity; itemQuantity > 0; itemQuantity--)
			{
				itemString += Globals.RNGGetAndRemoveRandomValueFromList(giftableItems).Id + " ";
			}

			return itemString.Trim();
		}

		/// <summary>Builds a string consisting of <paramref name="quantity"/> randomly selected IDs from <paramref name="unusedCategoryIDs"/>.</summary>
		/// <param name="quantity">the number of IDs to add.</param>
		/// <param name="unusedCategoryIDs"> the list of IDs to pull from.</param>
		/// <returns>A string of Category IDs with no leading/trailing whitespace.</returns>
		private static string GetRandomCategoryString(List<int> unusedCategoryIDs, int quantity)
		{
			string catString = "";

			for (int catQuantity = quantity; catQuantity > 0; catQuantity--)
			{
				catString += Globals.RNGGetAndRemoveRandomValueFromList(unusedCategoryIDs) + " ";
			}

			return catString.Trim();
		}

		/// <summary>
		/// Updates Universal Loves, Universal Hates, and all NPC Loves for Bundles.
		/// </summary>
		private static void UpdateBundlePrefs(Dictionary<string, string> replacements)
		{
			List<Item> newLovesList = ItemList.GetItemListFromString(replacements["Universal_Love"], ' ');
			List<Item> newHatesList = ItemList.GetItemListFromString(replacements["Universal_Hate"], ' ');

			NPC.UpdateUniversalLoves(newLovesList);
			NPC.UpdateUniversalHates(newHatesList);

			foreach (KeyValuePair<string, string> NPCPreferences in replacements)
			{
				// If Universal Preference, skip
				if (UniversalPreferenceKeys.Contains(NPCPreferences.Key)) { continue; }

				string NPCLoves = NPCPreferences.Value.Split('/')[(int)NPCGiftTasteIndexes.Loves];
				NPC.UpdateNPCLoves(NPCPreferences.Key, ItemList.GetItemListFromString(NPCLoves, ' '));
			}
		}

		/// <summary>
		/// Write to the spoiler log.
		/// </summary>
		private static void WriteToSpoilerLog(Dictionary<string, string> replacements)
		{
			if (!Globals.Config.NPCs.RandomizeIndividualPreferences && !Globals.Config.NPCs.RandomizeUniversalPreferences) { return; }

			Globals.SpoilerWrite("===== NPC GIFT TASTES =====");
			foreach (KeyValuePair<string, string> NPCPreferences in replacements)
			{
				if (UniversalPreferenceKeys.Contains(NPCPreferences.Key))
				{
					Globals.SpoilerWrite($"{NPCPreferences.Key.Replace('_', ' ')}: {TranslateIDs(NPCPreferences.Value)}");
					Globals.SpoilerWrite("");
				}
				else
				{
					string npcName = NPCPreferences.Key;
					string[] tokens = NPCPreferences.Value.Split('/');

					Globals.SpoilerWrite(npcName);
					Globals.SpoilerWrite($"\tLoves: {TranslateIDs(tokens[(int)NPCGiftTasteIndexes.Loves])}");
					Globals.SpoilerWrite($"\tLikes: {TranslateIDs(tokens[(int)NPCGiftTasteIndexes.Likes])}");
					Globals.SpoilerWrite($"\tDislikes: {TranslateIDs(tokens[(int)NPCGiftTasteIndexes.Dislikes])}");
					Globals.SpoilerWrite($"\tHates: {TranslateIDs(tokens[(int)NPCGiftTasteIndexes.Hates])}");
					Globals.SpoilerWrite($"\tNeutral: {TranslateIDs(tokens[(int)NPCGiftTasteIndexes.Neutral])}");
					Globals.SpoilerWrite("");
				}
			}
			Globals.SpoilerWrite("");
		}

		/// <summary>
		/// Returns string with names of items in a comma-separated list.
		/// </summary>
		/// <param name="itemIDString">the list of item IDs to parse. Expected format: ID numbers separated by spaces.</param>
		/// <returns>String of item names in a comma-separated list.</returns>
		private static string TranslateIDs(string itemIDString)
		{
			string[] IDStringArray = itemIDString.Trim().Split(' ');
			string outputString = "";

			for (int arrayPos = 0; arrayPos < IDStringArray.Length; arrayPos++)
			{
				bool IDParsed = int.TryParse(IDStringArray[arrayPos], out int ID);
				if (!IDParsed)
				{
					Globals.ConsoleWarn($"Input string was not in a correct format: '{IDStringArray[arrayPos]}'");
					continue;
				}

				// Add the stirng based on whether it's a category
				outputString += ID > 0
					? ItemList.GetItemName(ID)
					: $"[{GiftableItemCategories[ID]}]";

				// Not last item - put comma after
				if (arrayPos != IDStringArray.Length - 1)
				{
					outputString += ", ";
				}
			}

			return outputString;
		}
	}
}
