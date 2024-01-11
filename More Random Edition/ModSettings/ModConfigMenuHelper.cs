using StardewModdingAPI;
using System;

namespace Randomizer
{
    public class ModConfigMenuHelper
	{
		public IGenericModConfigMenuApi api;
		public IManifest ModManifest;

		public ModConfigMenuHelper(IGenericModConfigMenuApi api, IManifest ModManifest)
		{
			this.api = api;
			this.ModManifest = ModManifest;
		}

		public void RegisterModOptions()
		{
            AddCheckbox("Create Spoiler Log", "Create a text file which contains all of the randomized elements when a new farm is created. Highly recommended to leave on.", () => Globals.Config.CreateSpoilerLog, (bool val) => Globals.Config.CreateSpoilerLog = val);

			AddSectionTitle("---RANDOMIZATION OPTIONS---", "Toggle on or off the various aspects of the game which can be randomized.");

			AddSectionTitle("Bundle Options");
			AddCheckbox("Community Center Bundles", "Generate new bundles for each room which select a random number of items from a themed pool.", () => Globals.Config.Bundles.Randomize, (bool val) => Globals.Config.Bundles.Randomize = val);
			AddCheckbox("Show Helpful Tooltips", "When this option is enabled, mouse over the items in a bundle to get a helpful description of where to locate them.", () => Globals.Config.Bundles.ShowDescriptionsInBundleTooltips, (bool val) => Globals.Config.Bundles.ShowDescriptionsInBundleTooltips = val);

			AddSectionTitle("Crafting Recipe Options");
			AddCheckbox("Crafting Recipes", "Create recipes using randomly selected items from a pool. Uses rules for balanced difficulty.", () => Globals.Config.CraftingRecipes.Randomize, (bool val) => Globals.Config.CraftingRecipes.Randomize = val);
			AddCheckbox("Skill Level Requirements", "Randomize levels at which the recipes are learned. Recipe randomization must be turned on for this to take effect.", () => Globals.Config.CraftingRecipes.RandomizeLevels, (bool val) => Globals.Config.CraftingRecipes.RandomizeLevels = val);

			AddSectionTitle("NPC Options");
			AddCheckbox("Swap NPC Skins", "Switch NPC's skins arounds. Does not change names or schedules.", () => Globals.Config.NPCs.RandomizeSkins, (bool val) => Globals.Config.NPCs.RandomizeSkins = val);
			AddCheckbox("NPC Birthdays", "Moves each NPC's birthday to a random day in the year.", () => Globals.Config.NPCs.RandomizeBirthdays, (bool val) => Globals.Config.NPCs.RandomizeBirthdays = val);
			AddCheckbox("Individual Item Preferences", "Generates a new set of loved items, hated items, and so on for each NPC.", () => Globals.Config.NPCs.RandomizeIndividualPreferences, (bool val) => Globals.Config.NPCs.RandomizeIndividualPreferences = val);
			AddCheckbox("Universal Item Preferences", "Generates new sets of universally loved, liked, disliked, hated, and neutral items.", () => Globals.Config.NPCs.RandomizeUniversalPreferences, (bool val) => Globals.Config.NPCs.RandomizeUniversalPreferences = val);

			AddSectionTitle("Crop Options");
			AddCheckbox("Crops", "Randomize crop names, growing schedules, and attributes (trellis, scythe needed, etc.).", () => Globals.Config.Crops.Randomize, (bool val) => Globals.Config.Crops.Randomize = val);
			AddCheckbox("Use Custom Crop Images", "Use custom images for seeds and crops at each growth stage.", () => Globals.Config.Crops.UseCustomImages, (bool val) => Globals.Config.Crops.UseCustomImages = val);
			AddCheckbox("Fruit Trees", "Generates Item saplings that grow a random item. Prices are loosely balanced based on the item grown.", () => Globals.Config.RandomizeFruitTrees, (bool val) => Globals.Config.RandomizeFruitTrees = val);

			AddSectionTitle("Fish Options");
			AddCheckbox("Fish", "Randomize fish names, difficulty and behaviors, as well as locations, times of days and seasons.", () => Globals.Config.Fish.Randomize, (bool val) => Globals.Config.Fish.Randomize = val);
			AddCheckbox("Use Custom Fish Images", "Use custom images for the fish.", () => Globals.Config.Fish.UseCustomImages, (bool val) => Globals.Config.Fish.UseCustomImages = val);

			AddSectionTitle("Monster Options");
			AddCheckbox("Monster Stats", "Randomize monster stats, behaviors, and non-unique item drops.", () => Globals.Config.Monsters.Randomize, (bool val) => Globals.Config.Monsters.Randomize = val);
			AddCheckbox("Shuffle Monster Drops", "Shuffle unique monster drops between all monsters.", () => Globals.Config.Monsters.SwapUniqueDrops, (bool val) => Globals.Config.Monsters.SwapUniqueDrops = val);

			AddSectionTitle("Weapon Options");
			AddCheckbox("Weapons", "Randomize weapon stats, types, and drop locations.", () => Globals.Config.Weapons.Randomize, (bool val) => Globals.Config.Weapons.Randomize = val);
			AddCheckbox("Use Custom Weapon Images", "Use custom images for weapons.", () => Globals.Config.Weapons.UseCustomImages, (bool val) => Globals.Config.Weapons.UseCustomImages = val);
			AddCheckbox("Galaxy Sword Name", "Disable to have the Galaxy Sword keep its name. There is a hard-coded check to spawn a high level bat on Wilderness Farm at night if the player has a Galaxy Sword in their inventory.", () => Globals.Config.Weapons.RandomizeGalaxySwordName, (bool val) => Globals.Config.Weapons.RandomizeGalaxySwordName = val);

			AddSectionTitle("Boot Options");
			AddCheckbox("Boots", "Randomize boots stats, names, descriptions.", () => Globals.Config.Boots.Randomize, (bool val) => Globals.Config.Boots.Randomize = val);
			AddCheckbox("Use Custom Boot Images", "Use custom images for boots.", () => Globals.Config.Boots.UseCustomImages, (bool val) => Globals.Config.Boots.UseCustomImages = val);

			AddSectionTitle("Music Options");
			AddCheckbox("Music", "Shuffle most songs and ambience.", () => Globals.Config.Music.Randomize, (bool val) => Globals.Config.Music.Randomize = val);
			AddCheckbox("Random Song on Area Change", "Plays a new song each time the loaded area changes.", () => Globals.Config.Music.RandomSongEachTransition, (bool val) => Globals.Config.Music.RandomSongEachTransition = val);

			AddSectionTitle("Shop Options");
            AddCheckbox("Seed Shop Item of the Week", "Adds an expensive item to Pierre's shop that changes every Monday.", () => Globals.Config.Shops.AddSeedShopItemOfTheWeek, (bool val) => Globals.Config.Shops.AddSeedShopItemOfTheWeek = val);
            AddCheckbox("Add Clay to Robin's", "Adds clay to Robin's shop, costing between 25-75 coins each day", () => Globals.Config.Shops.AddClayToRobinsShop, (bool val) => Globals.Config.Shops.AddClayToRobinsShop = val);

            AddSectionTitle("Misc Options");
			AddCheckbox("Building Costs", "Farm buildings that Robin can build for the player choose from a random pool of resources.", () => Globals.Config.RandomizeBuildingCosts, (bool val) => Globals.Config.RandomizeBuildingCosts = val);
			AddCheckbox("Animal Skins", "You might get a surprise from Marnie.", () => Globals.Config.RandomizeAnimalSkins, (bool val) => Globals.Config.RandomizeAnimalSkins = val);
			AddCheckbox("Forageables", "Forageables for every season and location are now randomly selected. Every forageable appears at least once per year.", () => Globals.Config.RandomizeForagables, (bool val) => Globals.Config.RandomizeForagables = val);
			AddCheckbox("Intro Text", "Replace portions of the intro cutscene with Mad Libs style text.", () => Globals.Config.RandomizeIntroStory, (bool val) => Globals.Config.RandomizeIntroStory = val);
			AddCheckbox("Quests", "Randomly select quest givers, required items, and rewards.", () => Globals.Config.RandomizeQuests, (bool val) => Globals.Config.RandomizeQuests = val);
			AddCheckbox("Rain", "Replace rain with a variant (Skulls/Junimos/Cats and Dogs/etc).", () => Globals.Config.RandomizeRain, (bool val) => Globals.Config.RandomizeRain = val);
		}

        /// <summary>
        /// A wrapper for the AddBoolOption functionality for readability
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="optionName"></param>
        /// <param name="optionTooltip"></param>
        /// <param name="optionGet"></param>
        /// <param name="optionSet"></param>
        private void AddCheckbox(
			string optionName, 
			string optionTooltip, 
			Func<bool> optionGet, 
			Action<bool> optionSet)
		{
            api.AddBoolOption(
				mod: ModManifest,
				name: () => optionName,
				tooltip: () => optionTooltip,
				getValue: optionGet,
				setValue: optionSet
            );
        }

		private void AddSectionTitle(string text, string tooltip = "")
		{
            api.AddSectionTitle(ModManifest, () => text, () => tooltip);
		}
	}

}
