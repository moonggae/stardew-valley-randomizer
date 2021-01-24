using StardewValley;
using StardewModdingAPI.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	/// <summary>
	/// Randomizes the preferences of all NPCs
	/// </summary>
	public class PreferenceRandomizer
	{

		//TODO: 
		//  Fix Bundles generated based on preferences - this will need to run before bundles are generated
		//  Randomize Universal Preferences - need to work out numbers of items and categories to do

		static EditedObjectInformation _editedObjectInfo;

		/// <summary>
		/// Default data for universal preferences - these can be overridden by an NPC's individual preference
		/// </summary>
		private readonly static Dictionary<string, string> DefaultUniversalPreferenceData = new Dictionary<string, string>()
		{
			["Universal_Love"] = "74 446 797 373 279",
			["Universal_Like"] = "-2 -7 -26 -75 -80 72 395 613 634 635 636 637 638 724 459 873",
			["Universal_Neutral"] = "194 216 262 304 815",
			["Universal_Dislike"] = "-4 -8 -12 -15 -16 -19 -22 -24 -25 -28 -74 78 169 246 247 305 309 310 311 403 419 423 535 536 537 725 726 749 271",
			["Universal_Hate"] = "0 -20 -21 92 110 111 112 142 152 153 157 178 105 168 170 171 172 374 376 378 380 397 420 684 721 766 767 772 203 308 265 909 910",
		};

		/// <summary>
		/// Default data for an NPC's individual preferences - stores reactions and exceptions to universal preferences
		/// </summary>
		private readonly static Dictionary<string, string> DefaultNPCPreferenceData = new Dictionary<string, string>
		{
			["Robin"] = "This is for me? Wow, I absolutely love it!!/224 426 636/Thanks! This is really nice!/-6 -79 424 709/Um... why?/16 330/What the...? This is terrible!/2/Thank you. This might come in handy.//",
			["Demetrius"] = "You're giving this to me? This is amazing!/207 232 233 400/Thank you! This is a very interesting specimen./-5 -79 422/...What is this?/80 330/This is disgusting./2/That was very thoughtful of you./-4/",
			["Maru"] = "Is that...? Oh wow, @! This is spectacular!/72 197 190 215 222 243 336 337 400 787 910/This is a super gift! Thank you!/-260 62 64 66 68 70 334 335 725 726 909/Oh... That's for me? I'll just put it over here.../-4 330 414 410 404 724/Yuck! You thought I would like this?/340 342 2 430 416/Thanks.//",
			["Sebastian"] = "I really love this. How did you know?/84 227 236 575 305/Thanks, I like this./267 614 276/...?/-81 30 -80/...I hate this./-5 2 459 195 201 240 330 -26 873/...thanks./-4/",
			["Linus"] = "This is wonderful! You've really made my day special./88 90 234 242 280/This is a great gift. Thank you!/-5 -6 -79 -81/Hmm... This doesn't really do much for me./-2 330/Why would you give this to me? Do you think I like junk just because I live in a tent? That's terrible./2/A gift? How nice./-4/",
			["Pierre"] = "This is my all-time favorite! Thank you!/202/That's very kind of you. I like this./-5 -6 -7 18 22 402 418 259/This isn't exactly my favorite.../-2 -81 330/Please, never bring this to me again./167 -4 199 270 229 248/A present? Thanks!//",
			["Caroline"] = "You're giving this... to me? I'm speechless./213 614 593 907/Oh, goodness! Are you sure?/-7 18 815 402 418/No, no, no.../-81 330 300 306 307/This is absolute junk. I'm offended./80 296/Oh, that's sweet. Thank you.//",
			["Abigail"] = "I seriously love this! You're the best, @!/66 128 220 226 276 611 904/Hey, how'd you know I was hungry? This looks delicious!//What am I supposed to do with this?/-5 -75 -79 16 245 246/What were you thinking? This is awful!/330/You brought me a present? Thanks.//",
			["Alex"] = "Hey, awesome! I love this stuff!/201 212 662 664/This is cool! Thanks./-5/Um... Okay. Thanks./16 330/Are you serious? This is garbage./80/Thanks!//",
			["George"] = "This is my favorite thing! Thank you./20 205/Thanks./18 195 199 200 214 219 223 231 233/That's a terrible gift./-80 16/This is probably the worst gift I've ever seen. Thanks a lot./22 80 330/A gift? Hmm...#$e#Do you want something in return?//",
			["Evelyn"] = "*gasp*... This is absolutely marvelous!$u#$e#You've made an old lady very happy./72 220 239 284 591 595/Oh my, it looks wonderful! That's very kind of you./-6 18 402 418/Um, Where will I put this?/16 80/...it smells awful./225 226 227 228 219 396 397 393 372 248 296 -4 330/How nice. Thank you, dear.//",
			["Lewis"] = "Wow, this is my favorite! Thank you!/200 208 235 614 260/Thanks, this is great!/-80 24 88 90 192 258 264 272 274 278/Well, I guess it's the thought that counts.../-6 16 330/This makes me sick. What a horrendous gift./80/That's very nice of you. Thanks.//",
			["Clint"] = "Yes! This is exactly what I've been looking for!/60 62 64 66 68 70 336 337 605 649 749/This is a fun gift. Thanks!/334 335/*Sigh*.../-80 2 16 80 /This makes me depressed./4/Thanks./-15/",
			["Penny"] = "Thank you! I really love this!/60 376 651 72 164 218 230 244 254/Thank you! This looks special./-6 20 22/Uh, it's for me? ...Thanks./-4 80 330 456 457 444 440 422 420/Ugh...I'm sorry, but I absolutely hate this./446 346 348 303 304 398 459 873/Thanks, this looks nice.//",
			["Pam"] = "Hey, hey! Now this is really something! Thanks a million, kid./24 90 199 208 303 346 459 873/You did good with this one, kid. Thanks!/-6 -75 -79 18 227 228 231 232 233 234 235 236 238 402 418/This just ain't my thing./-5 16 80 330/Now this is just absolutely despicable.$u#$e#(Is this some kind of mean joke?)/149 151/Thanks, kid./-4/",
			["Emily"] = "This gift is fabulous! Thank you so much!/60 62 64 66 68 70 241 428 440/Thank you! I'm feeling a positive energy from this gift./18 82 84 86 196 200 207 230 235 402 418/Sorry, @. I don't like this./78 2 232 233 225 226/This gift has a strong negative energy. I can't stand it./212 213 227 228/Thanks!//",
			["Haley"] = "Oh my god, this is my favorite thing!!/221 421 610 88/*gasp*...for me? Thank you!/18 60 62 64 70 88 222 223 232 233 234 402 418/Ugh...that's such a stupid gift./-5 -6 -75 -79 -81 80 -27/Gross!/-4 2 16 330 74/Thank you. I love presents.//",
			["Jas"] = "*gasp*...Wow!! Thank you!$h/221 595 604/I love presents! Thank you!/18 60 64 70 88 232 233 234 222 223 340 344 402 418 903/Is this a gift? Oh...$s/-5 -81 -75 -79 80/Ew. This is icky.$s/253 395 459 -26 2 16 330 873/Thank you!//",
			["Vincent"] = "Wow, thank you so much! This is terrific!/221 398 612 721 903/For me? Wow, thanks!/18 60 64 70 88 232 233 234 222 223 340 344 402 418/This isn't very fun...$s/-5 -75 -79 -81 80/Yuck, what is this?$s/253 395 459 -26 2 16 330 873/Thanks!//",
			["Jodi"] = "Oh, you're such a sweetheart! I really love this!/72 200 211 214 220 222 225 231/Thank you! This makes my day really special./-5 -6 -79 18 402 418/Hmm, well I guess I could always put this in the compost.../-81 80 248 330/*Blech*... I hate this.../18 22 396/That's so nice of you! Thanks.//",
			["Kent"] = "Oh...! Mom used to give me this when I was a young boy. It brings back wonderful memories. Thank you./607 649/Hey, now this is a really great gift. Thanks./-5 -79 18 402 418/Ah... It's a... what is this, exactly? Hmm.../80 330 416 873/This... They gave this to me in Gotoro prison camp. I've been trying to forget about that. *shudder*/-6 2 227 229 456/That's kind of you. The family will like this.//",
			["Sam"] = "Aw, yea! This is my absolute favorite!/90 206 655 658 562 731/Thanks! I really like this./167 210 213 220 223 224 228 232 233 239 -5/Hmm.. this is... interesting./-81 80 152 330 -75/You really don't get it, huh?/306 307 342 -15 2/You got that for me? Thanks!//",
			["Leah"] = "Oh! This is exactly what I wanted! Thank you!/196 200 348 606 651 650 426 430/This is a really nice gift! Thank you!/-5 -6 -79 -81 18 402 169 406 408 418 86/Hmm... I guess everyone has different tastes./-2 2 152 330 221 223 229 232 233 241 209 194/This is a pretty terrible gift, isn't it?/305 211 210 206 216/Thank you.//",
			["Shane"] = "Oh wow, @! How'd you know this is my favorite?/206 215 260 346/This is great! Thanks!/-5 -79 303/I don't really like this./-81 152 330/Why are you giving me your garbage?/80 342/Oh, you got me something? Thanks!//",
			["Marnie"] = "This is an incredible gift! Thanks!!/72 221 240 608/Thank you!/-5 -6 402 418/Oh. I guess I'll take it./16 152/This is worthless. I don't understand you./330/Thank you! This looks nice.//",
			["Elliott"] = "@, this is a beautiful gift! Thank you!/715 732 218 444 637 814/This is for me? Marvelous!/727 728 -79 60 80 82 84 149 151 346 348 728/Hmm... I'm not a huge fan of this./16 206 330 -79 -81 -6/This item gives me a terrible feeling. I'll have to dispose of it./80 154 300 296/Oh, a present! Thank you!/-4/",
			["Gus"] = "You're giving this... to me? I'm speechless./72 213 635 729 907/Oh, goodness! Are you sure?/348 303 -7 18/No, no, no.../16 330/This is absolute junk. I'm offended./80 167 648/Oh, that's sweet. Thank you.//",
			["Dwarf"] = "Hey, I really love this stuff. You can find great things in the mines./554 60 62 64 66 68 70 749/Ah, this reminds me of home./78 82 84 86 96 97 98 99 121 122/Hmm... Is this what humans like?/-5 -6 16 -81 2 4 330/I don't care what species you are. This is worthless garbage.//An offering! Thank you./-28/",
			["Wizard"] = "Ahh, this is imbued with potent arcane energies. It's very useful for my studies. Thank you!/155 422 769 768/Many thanks. This item has some very interesting properties./-12 72 82 84/This?... I suppose I'll cast it into the flames and see what happens./-5 -6 -81 2 16 330 766/Ughh... These are utterly mundane. Please refrain from bothering me with this in the future.//Thank you. This will prove useful, I think.//",
			["Harvey"] = "It's for me? This is my favorite stuff! It's like you read my mind./348 237 432 395 342/That's such a nice gift. Thank you!/-81 -79 -7 402 614 418 422 436 438 442 444 422/Hmm... Are you sure this is healthy?/-4 424 426 2 330 233 232 238 234 223 222 221 220 216 211 210 208 206 205/...I think I'm allergic to this./296 245 397 396 394 393 392/Thanks. That's very kind of you.//",
			["Sandy"] = "Ooo! I absolutely love getting flowers from the valley. I'm so happy!~/18 402 418 905/This looks wonderful. Thanks so much!/-75 -79 88 428 436 438 440/Oh... Um. Thanks./-4 2 330/*sniff* ...what is it?//This is for me? Thank you!//",
			["Willy"] = "This is great! If only me ol' Pappy was around. He'd go nuts for this./72 143 149 154 276 337 698 459/This looks great. Thank you!/66 336 340 699 707 198 202 209 212 213 214 219 225 727 730 728 732 265/Hmmm... You like stuff like this? Huh./-7 -81 2 4 330/... *sniff*... Well I guess I can toss it into the chum bucket.//A gift! Thanks./-4 227 228 242/",
			["Krobus"] = "This is an amazing gift. For my people it is a great honor to receive something like this./72 16 276 337 305 308/Thank you very much./66 265 336 340/Humans have... interesting tastes./-7 -81 2 330/Oh... Um. I guess I'll accept it.//Thank you.//",
			["Leo"] = "I love this. I'm going to put it in my nest./444 289 834 906/Are you sure? This is so nice.../852 396 392 394 397/Oh... I don't know if I can digest this.../-7 -81 2 330 303 459 342 346 348/*sniff* *sniff*... Why are you giving this to me? Do you want me to bury it for you?/257 253 247 271 283 304 303 346 873 348 459 167/I accept your gift./-4/"
		};

		/// <summary>
		/// Item Category indexes - only includes categories which are giftable - full list available here: https://stardewcommunitywiki.com/Modding:Object_data#Categories
		/// </summary>
		private readonly static Dictionary<int, string> ItemCategoryIDs = new Dictionary<int, string>
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
		/// Initialize list of items which are giftable to NPCs.
		/// </summary>
		private static List<Item> InitializeGiftableItemsList()
        {
			List<Item> GiftableItems = new List<Item>(
				ItemList.GetAnimalProducts().Concat(ItemList.GetArtifacts())
											.Concat(ItemList.GetCookeditems())
											.Concat(ItemList.GetCrops())
											.Concat(ItemList.GetFlowers())
											.Concat(ItemList.GetForagables())
											.Concat(ItemList.GetFruit())
											.Concat(ItemList.GetGeodeMinerals())
											.Concat(ItemList.GetResources())
											.Concat(ItemList.GetSeeds())
											.Concat(ItemList.GetTrash())
											.Concat(ItemList.GetUniqueBeachForagables())
											.Concat(ItemList.GetUniqueDesertForagables())
											.Concat(ItemList.GetUniqueWoodsForagables())
											.ToList()
				);
			return GiftableItems;
		}

		// Set of indices to use when parsing npc prefstrings.
		private const int LovesIndex = 1;
		private const int LikesIndex = 3;
		private const int DislikesIndex = 5;
		private const int HatesIndex = 7;
		private const int NeutralIndex = 9;

		/// <summary>
		/// Does the preference randomization
		/// </summary>
		/// <param name="editedObjectInfo">Used to access the FishItem info if fish randomization is turned on.</param>
		/// <returns>The dictionary to use for replacements</returns>
		public static Dictionary<string, string> Randomize(EditedObjectInformation editedObjectInfo)
		{
			_editedObjectInfo = editedObjectInfo;
			Dictionary<string, string> replacements = new Dictionary<string, string>();

			List<int> universalUnusedCategories = new List<int>(ItemCategoryIDs.Keys);
			List<Item> universalUnusedItems = InitializeGiftableItemsList();
			Dictionary<string, string> universalPreferenceDataReplacements = new Dictionary<string, string>();

			// Generate randomized Universal Preferences strings even if not enabled - keeps RNG stable
			foreach (KeyValuePair<string, string> universalPrefs in DefaultUniversalPreferenceData)
            {
				universalPreferenceDataReplacements.Add(universalPrefs.Key, GetUniversalPreferenceString(universalUnusedCategories, universalUnusedItems));
			}

			// Add generated prefstrings only if config option enabled
			if (Globals.Config.NPCPreferences.RandomizeUniversalPreferences)
			{
				foreach (KeyValuePair<string, string> keyValuePair in universalPreferenceDataReplacements)
                {
					replacements.Add(keyValuePair.Key, keyValuePair.Value);
                }
			}

			// Randomize NPC Preferences
			foreach (KeyValuePair<string, string> npcPreferences in DefaultNPCPreferenceData)
			{
				List<int> unusedCategories = new List<int>(ItemCategoryIDs.Keys);
				List<Item> unusedItems = InitializeGiftableItemsList();

                //Add fish if fish randomization is turned on
                if (Globals.Config.Fish.Randomize)
                {
                    // Create dummy Item with Fish ID - ID is all that's needed
                    foreach (int fishID in _editedObjectInfo.FishReplacements.Keys)
                    {
                        Item dummyFishItem = new Item(fishID);
                        unusedItems.Add(dummyFishItem);
                    }
                }
                // If fish randomization is turned off, add vanilla fish
                else
                {
                    List<Item> fishList = FishItem.Get();
                    unusedItems = unusedItems.Concat(fishList).ToList();
                }


                string[] tokens = npcPreferences.Value.Split('/');
				string name = npcPreferences.Key;

				for (int index = 1; index <= 9; index += 2) { tokens[index] = GetPreferenceString(index, unusedCategories, unusedItems); }

				replacements.Add(name, string.Join("/", tokens));
			}

			WriteToSpoilerLog(replacements);
			return replacements;
		}

		/// <summary>
		/// Universal preference strings - no need to vary quantities per index. May end up with lots of loved items, lots of hated items, both, neither, etc.
		/// </summary>
		/// <param name="unusedCategories"></param>
		/// <param name="unusedItems"></param>
		/// <returns>Universal preference string</returns>
		private static string GetUniversalPreferenceString(List<int> unusedCategories, List<Item> unusedItems)
        {
			int catNum = Range.GetRandomValue(0, 10);
			int itemNum = Range.GetRandomValue(5, 30);

			string catString = "";
			string itemString = "";

			string universalPrefString = "";

			// If there are still categories to be added
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

			universalPrefString = catString + itemString;
			return universalPrefString.Trim();
		}

		/// <summary>
		/// Builds an NPC's preference string for a given index (loves, hates, etc.).
		/// </summary>
		/// <param name="index">the index of the NPC's prefstring.</param>
		/// <param name="unusedCategories">Holds list of categories which have not yet been assigned - prevents double-assignment.</param>
		/// <param name="unusedItems">Holds list of Items which have not yet been assigned - prevents double-assignment.</param>
		/// <returns>NPC's preference string for a given index.</returns>
		private static string GetPreferenceString(int index, List<int> unusedCategories, List<Item> unusedItems)
        {
			int minItems;
			int maxItems;

			// Should probably be moved into its own function
			// Determine how many items to add per category - data available here: https://pastebin.com/gFEduBVd
			// Basically, add more loved/liked items than hated/disliked, and few neutrals
			switch (index)
			{
				case 1:
				case 7:
					minItems = 1;
					maxItems = 11;
					break;

				case 3:
				case 5:
					minItems = 1;
					maxItems = 18;
					break;

				case 9:
					minItems = 1;
					maxItems = 3;
					break;

				default:
					minItems = 0;
					maxItems = 0;
					break;
			}

			int itemNum = Range.GetRandomValue(minItems, maxItems);
			int catNum = Range.GetRandomValue(1, 4);

			string itemString = GetRandomItemString(unusedItems, itemNum);
			string catString = GetRandomCategoryString(unusedCategories, catNum);

			string tokenString = catString + " " + itemString;
			return tokenString;
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
			List<int> catIDs = new List<int>(unusedCategoryIDs);
			string catString = "";

			for (int catQuantity = quantity; catQuantity > 0; catQuantity--)
			{
				catString += Globals.RNGGetAndRemoveRandomValueFromList(unusedCategoryIDs) + " ";
			}

			return catString.Trim();
		}

		/// <summary>
		/// Write to the spoiler log.
		/// </summary>
		private static void WriteToSpoilerLog(Dictionary<string, string> replacements)
		{
			if (!Globals.Config.NPCPreferences.Randomize) { return; }

			Globals.SpoilerWrite("===== NPC GIFT TASTES =====");
			foreach (KeyValuePair<string, string> npcPreferences in replacements)
			{
				if (DefaultUniversalPreferenceData.ContainsKey(npcPreferences.Key))
				{
					Globals.SpoilerWrite($"{npcPreferences.Key.Replace('_', ' ')}: {TranslateIDs(npcPreferences.Value)}");
					if (npcPreferences.Key == "Universal_Hate") { Globals.SpoilerWrite(""); }
				}
				else
				{
					string npcName = npcPreferences.Key;
					string[] tokens = npcPreferences.Value.Split('/');

					Globals.SpoilerWrite(npcName);

					Globals.SpoilerWrite($"    Loves: {TranslateIDs(tokens[LovesIndex])}");
					Globals.SpoilerWrite($"    Likes: {TranslateIDs(tokens[LikesIndex])}");
					Globals.SpoilerWrite($"    Dislikes: {TranslateIDs(tokens[DislikesIndex])}");
					Globals.SpoilerWrite($"    Hates: {TranslateIDs(tokens[HatesIndex])}");
					Globals.SpoilerWrite($"    Neutral: {TranslateIDs(tokens[NeutralIndex])}");
					Globals.SpoilerWrite("");
				}
			}
			Globals.SpoilerWrite("");
		}

		/// <summary>
		/// Returns string with names of items in a comma-separated list.
		/// </summary>
		/// <param name="ItemIDString">the list of item IDs to parse. Expected format: ID numbers separated by spaces.</param>
		/// <returns>String of item names in a comma-separated list.</returns>
		private static string TranslateIDs(string ItemIDString)
        {
			string[] IDStringArray= ItemIDString.Split(' ');
			string outputString = "";

			for (int arrayPos = 0; arrayPos < IDStringArray.Length; arrayPos++)
			{
				int ID;
				bool IDParsed = int.TryParse(IDStringArray[arrayPos], out ID);

				if (!IDParsed)
                {
					Globals.ConsoleWarn($"Input string was not in a correct format: '{IDStringArray[arrayPos]}'");
					continue;
				}

				// Positive numbers only - negative numbers represent categories
				// Not all positive numbers are represented in ItemList - fish IDs are excluded if randomized
				if (ID > 0)
				{

					if (!ItemList.Items.ContainsKey(ID))
					{
						// ID not in ItemList - is a Fish
						outputString += _editedObjectInfo.FishReplacements[ID];
					}
					else
					{
						outputString += ItemList.GetItemName(ID);
					}
				}
				else
				{
					outputString += "[" + ItemCategoryIDs[ID] + "]";
				}

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
