using StardewModdingAPI;
using StardewModdingAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	public class AssetEditor
	{
		private readonly ModEntry _mod;
		private Dictionary<string, string> _recipeReplacements = new Dictionary<string, string>();
		private Dictionary<string, string> _bundleReplacements = new Dictionary<string, string>();
		private Dictionary<string, string> _blueprintReplacements = new Dictionary<string, string>();
		private Dictionary<string, string> _uiStringReplacements = new Dictionary<string, string>();
		private Dictionary<string, string> _grandpaStringReplacements = new Dictionary<string, string>();
		private Dictionary<string, string> _stringReplacements = new Dictionary<string, string>();
		private Dictionary<string, string> _locationStringReplacements = new Dictionary<string, string>();
		private Dictionary<int, string> _fishReplacements = new Dictionary<int, string>();
		private Dictionary<int, string> _questReplacements = new Dictionary<int, string>();
		private Dictionary<string, string> _mailReplacements = new Dictionary<string, string>();
		private Dictionary<string, string> _locationsReplacements = new Dictionary<string, string>();
		private Dictionary<int, string> _objectInformationReplacements = new Dictionary<int, string>();
		private Dictionary<int, string> _fruitTreeReplacements = new Dictionary<int, string>();
		private Dictionary<int, string> _cropReplacements = new Dictionary<int, string>();
		private Dictionary<string, string> _cookingChannelReplacements = new Dictionary<string, string>();
		private Dictionary<int, string> _weaponReplacements = new Dictionary<int, string>();
		private Dictionary<int, string> _bootReplacements = new Dictionary<int, string>();
		private Dictionary<string, string> _monsterReplacements = new Dictionary<string, string>();
		private Dictionary<string, string> _birthdayReplacements = new Dictionary<string, string>();
		private Dictionary<string, string> _preferenceReplacements = new Dictionary<string, string>();
		private Dictionary<int, string> _secretNotesReplacements = new Dictionary<int, string>();


		/// <summary>
		/// Whether we're currently ignoring replacing object information
		/// This is done between day loads to prevent errors with the Special Orders
		/// Eventually this can be removed when we modify the orders themselves
		/// </summary>
		private bool IgnoreObjectInformationReplacements { get; set; }

		public AssetEditor(ModEntry mod)
		{
			_mod = mod;
		}

		/// <summary>
		/// Called when requesting new assets - will replace them with our version
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        public void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
			if (TryReplaceAsset(e, "Data/CraftingRecipes", _recipeReplacements) ||
                TryReplaceAsset(e, "Data/Bundles", _bundleReplacements) ||
                TryReplaceAsset(e, "Data/Blueprints", _blueprintReplacements) ||
                TryReplaceAsset(e, "Strings/UI", _uiStringReplacements) ||
                TryReplaceAsset(e, "Data/Fish", _fishReplacements) ||
                TryReplaceAsset(e, "Data/Quests", _questReplacements) ||
                TryReplaceAsset(e, "Data/mail", _mailReplacements) ||
                TryReplaceAsset(e, "Data/Locations", _locationsReplacements) ||
                TryReplaceAsset(e, "Strings/Locations", _locationStringReplacements) ||
                TryReplaceAsset(e, "Data/fruitTrees", _fruitTreeReplacements) ||
                TryReplaceAsset(e, "Data/Crops", _cropReplacements) ||
                TryReplaceAsset(e, "Data/TV/CookingChannel", _cookingChannelReplacements) ||
                TryReplaceAsset(e, "Data/weapons", _weaponReplacements) ||
                TryReplaceAsset(e, "Data/Boots", _bootReplacements) ||
                TryReplaceAsset(e, "Data/Monsters", _monsterReplacements) ||
                TryReplaceAsset(e, "Data/NPCDispositions", _birthdayReplacements) ||
                TryReplaceAsset(e, "Data/NPCGiftTastes", _preferenceReplacements) ||
                TryReplaceAsset(e, "Data/SecretNotes", _secretNotesReplacements))
			{
				return;
			}

            if (ShouldReplaceAsset(e, "Strings/StringsFromCSFiles"))
            {
                e.Edit((asset) => ApplyEdits(asset, _grandpaStringReplacements));
                e.Edit((asset) => ApplyEdits(asset, _stringReplacements));
            }
            else if (ShouldReplaceAsset(e, "Data/ObjectInformation"))
            {
                if (IgnoreObjectInformationReplacements)
                {
                    e.Edit((asset) => ApplyEdits(asset, new Dictionary<int, string>()));
                }
                else
                {
                    e.Edit((asset) => ApplyEdits(asset, _objectInformationReplacements));
                }
            }
        }

        /// <summary>
        /// Whether we should replace the asset based on the setting
        /// </summary>
        /// <param name="e">The requested asset, so we can grab the name off of it</param>
        /// <param name="assetName">The name that were currently checking - if they don't match, exit early</param>
        /// <returns>True if we should replace it; false otherwise</returns>
        private bool ShouldReplaceAsset(AssetRequestedEventArgs e, string assetName)
        {
            if (!e.Name.IsEquivalentTo(assetName))
            {
                return false;
            }

            if (e.Name.IsEquivalentTo("Data/CraftingRecipes")) { return Globals.Config.CraftingRecipes.Randomize; }
            if (e.Name.IsEquivalentTo("Data/Bundles")) { return Globals.Config.Bundles.Randomize; }
            if (e.Name.IsEquivalentTo("Data/Blueprints")) { return Globals.Config.RandomizeBuildingCosts; }
            if (e.Name.IsEquivalentTo("Strings/StringsFromCSFiles")) { return true; }
            if (e.Name.IsEquivalentTo("Strings/UI")) { return true; }
            if (e.Name.IsEquivalentTo("Data/ObjectInformation")) { return true; }
            if (e.Name.IsEquivalentTo("Data/Fish")) { return Globals.Config.Fish.Randomize; }
            if (e.Name.IsEquivalentTo("Data/Quests") || e.Name.IsEquivalentTo("Data/mail")) { return Globals.Config.RandomizeQuests; }
            if (e.Name.IsEquivalentTo("Data/Locations")) { return Globals.Config.Fish.Randomize || Globals.Config.RandomizeForagables || Globals.Config.AddRandomArtifactItem; }
            if (e.Name.IsEquivalentTo("Strings/Locations")) { return Globals.Config.Crops.Randomize; } // For now, as the only thing is the sweet gem berry text
            if (e.Name.IsEquivalentTo("Data/fruitTrees")) { return Globals.Config.RandomizeFruitTrees; }
            if (e.Name.IsEquivalentTo("Data/Crops")) { return Globals.Config.Crops.Randomize; }
            if (e.Name.IsEquivalentTo("Data/TV/CookingChannel")) { return Globals.Config.Crops.Randomize || Globals.Config.Fish.Randomize; }
            if (e.Name.IsEquivalentTo("Data/weapons")) { return Globals.Config.Weapons.Randomize; }
            if (e.Name.IsEquivalentTo("Data/Boots")) { return Globals.Config.Boots.Randomize; }
            if (e.Name.IsEquivalentTo("Data/Monsters")) { return Globals.Config.Monsters.Randomize; }
            if (e.Name.IsEquivalentTo("Data/NPCDispositions")) { return Globals.Config.NPCs.RandomizeBirthdays; }
            if (e.Name.IsEquivalentTo("Data/NPCGiftTastes")) { return Globals.Config.NPCs.RandomizeIndividualPreferences || Globals.Config.NPCs.RandomizeUniversalPreferences; }
            if (e.Name.IsEquivalentTo("Data/SecretNotes")) { return Globals.Config.NPCs.RandomizeIndividualPreferences; }
            return false;
        }

		/// <summary>
		/// Tries to replace the asset with the one with the given name
		/// </summary>
		/// <param name="e"></param>
		/// <param name="assetName"></param>
		/// <param name="replacement"></param>
		/// <returns>True if successful, false otherwise</returns>
        private bool TryReplaceAsset(AssetRequestedEventArgs e, string assetName, Dictionary<string, string> replacement)
        {
            if (ShouldReplaceAsset(e, assetName))
            {
                e.Edit((asset) => ApplyEdits(asset, replacement));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to replace the asset with the one with the given name
        /// </summary>
        /// <param name="e"></param>
        /// <param name="assetName"></param>
        /// <param name="replacement"></param>
        /// <returns>True if successful, false otherwise</returns>
        private bool TryReplaceAsset(AssetRequestedEventArgs e, string assetName, Dictionary<int, string> replacement)
        {
            if (ShouldReplaceAsset(e, assetName))
            {
                e.Edit((asset) => ApplyEdits(asset, replacement));
                return true;
            }
            return false;
        }

        private static void ApplyEdits<TKey, TValue>(IAssetData asset, IDictionary<TKey, TValue> edits)
        {
            IAssetDataForDictionary<TKey, TValue> assetDict = asset.AsDictionary<TKey, TValue>();
            foreach (KeyValuePair<TKey, TValue> edit in edits)
            {
                assetDict.Data[edit.Key] = edit.Value;
            }
        }

        /// <summary>
        /// Invalidates the cache for all the assets
        /// </summary>
        public void InvalidateCache()
		{
			_mod.Helper.GameContent.InvalidateCache("Data/CraftingRecipes");
			_mod.Helper.GameContent.InvalidateCache("Data/Bundles");
			_mod.Helper.GameContent.InvalidateCache("Data/Blueprints");
			_mod.Helper.GameContent.InvalidateCache("Strings/StringsFromCSFiles");
			_mod.Helper.GameContent.InvalidateCache("Strings/UI");
			_mod.Helper.GameContent.InvalidateCache("Data/ObjectInformation");
			_mod.Helper.GameContent.InvalidateCache("Data/Events/Farm");
			_mod.Helper.GameContent.InvalidateCache("Data/Fish");
			_mod.Helper.GameContent.InvalidateCache("Data/Quests");
			_mod.Helper.GameContent.InvalidateCache("Data/mail");
			_mod.Helper.GameContent.InvalidateCache("Data/Locations");
			_mod.Helper.GameContent.InvalidateCache("Strings/Locations");
			_mod.Helper.GameContent.InvalidateCache("Data/fruitTrees");
			_mod.Helper.GameContent.InvalidateCache("Data/Crops");
			_mod.Helper.GameContent.InvalidateCache("Data/TV/CookingChannel");
			_mod.Helper.GameContent.InvalidateCache("Data/weapons");
			_mod.Helper.GameContent.InvalidateCache("Data/Boots");
			_mod.Helper.GameContent.InvalidateCache("Data/Monsters");
			_mod.Helper.GameContent.InvalidateCache("Data/NPCDispositions");
			_mod.Helper.GameContent.InvalidateCache("Data/NPCGiftTastes");
            _mod.Helper.GameContent.InvalidateCache("Data/SecretNotes");
		}

		/// <summary>
		/// Calculates edits that need to happen before a save file is loaded
		/// </summary>
		public void CalculateEditsBeforeLoad()
		{
			CalculateAndInvalidateUIEdits();
			_grandpaStringReplacements = StringsAdjustments.RandomizeGrandpasStory();
		}

		/// <summary>
		/// Calculates the UI string replacements and invalidates the cache so it can be updated
		/// Should be called on game load and after a language change
		/// </summary>
		public void CalculateAndInvalidateUIEdits()
		{
			_uiStringReplacements = StringsAdjustments.ModifyRemixedBundleUI();
			_mod.Helper.GameContent.InvalidateCache("Strings/UI");
		}

		/// <summary>
		/// Calculates all the things to edit and creates the replacement dictionaries
		/// </summary>
		public void CalculateEdits()
		{
			ItemList.Initialize();
			ValidateItemList();

			EditedObjectInformation editedObjectInfo = new EditedObjectInformation();
			FishRandomizer.Randomize(editedObjectInfo);
			_fishReplacements = editedObjectInfo.FishReplacements;

			CropRandomizer.Randomize(editedObjectInfo);
			_fruitTreeReplacements = editedObjectInfo.FruitTreeReplacements;
			_cropReplacements = editedObjectInfo.CropsReplacements;
			_objectInformationReplacements = editedObjectInfo.ObjectInformationReplacements;

			_blueprintReplacements = BlueprintRandomizer.Randomize();
			_monsterReplacements = MonsterRandomizer.Randomize(); // Must be done before recipes since rarities of drops change
			_locationsReplacements = LocationRandomizer.Randomize(); // Must be done before recipes because of wild seeds
			_recipeReplacements = CraftingRecipeRandomizer.Randomize();
			_stringReplacements = StringsAdjustments.GetCSFileStringReplacements();
			_locationStringReplacements = StringsAdjustments.GetLocationStringReplacements();
			CraftingRecipeAdjustments.FixCookingRecipeDisplayNames();
			_cookingChannelReplacements = CookingChannelAdjustments.GetTextEdits();

			// Needs to run after Cooking Recipe fix so that cooked items are properly named,
			// and needs to run before bundles so that NPC Loved Item bundles are properly generated
			_preferenceReplacements = PreferenceRandomizer.Randomize();
			_secretNotesReplacements = SecretNotesRandomizer.FixSecretNotes(_preferenceReplacements);

			_bundleReplacements = BundleRandomizer.Randomize();
			MusicRandomizer.Randomize();

			QuestInformation questInfo = QuestRandomizer.Randomize();
			_questReplacements = questInfo.QuestReplacements;
			_mailReplacements = questInfo.MailReplacements;

			_weaponReplacements = WeaponRandomizer.Randomize();
			_bootReplacements = BootRandomizer.Randomize();
			_birthdayReplacements = BirthdayRandomizer.Randomize();
		}

		/// <summary>
		/// Turns on the flag to ignore object information replacements and invalidates the cache
		/// so that the original values are reloaded
		/// </summary>
		public void UndoObjectInformationReplacements()
		{
			IgnoreObjectInformationReplacements = true;
			_mod.Helper.GameContent.InvalidateCache("Data/ObjectInformation");
		}

		/// <summary>
		/// Turns off the flag to ignore object information replacements and invalidates the cache
		/// so that the randomized values are reloaded
		/// </summary>
		public void RedoObjectInformationReplacements()
		{
			IgnoreObjectInformationReplacements = false;
			_mod.Helper.GameContent.InvalidateCache("Data/ObjectInformation");
		}

		/// <summary>
		/// Validates that all the items in the ObjectIndexes exist in the main item list
		/// </summary>
		private static void ValidateItemList()
		{
			foreach (ObjectIndexes index in Enum.GetValues(typeof(ObjectIndexes)).Cast<ObjectIndexes>())
			{
				if (!ItemList.Items.ContainsKey((int)index))
				{
					Globals.ConsoleWarn($"Missing item: {(int)index}: {index}");
				}
			}
		}
	}
}