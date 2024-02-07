using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Randomizer
{
    /// <summary>The mod entry point</summary>
    public class ModEntry : Mod
	{
		private AssetLoader _modAssetLoader;
		private AssetEditor _modAssetEditor;
		static IGenericModConfigMenuApi api;

		/// <summary>The mod entry point, called after the mod is first loaded</summary>
		/// <param name="helper">Provides simplified APIs for writing mods</param>
		public override void Entry(IModHelper helper)
		{
			Globals.ModRef = this;
			Globals.Config = helper.ReadConfig<ModConfig>();

            _modAssetLoader = new AssetLoader(this);
            _modAssetEditor = new AssetEditor(this);

            helper.Events.Content.AssetRequested += OnAssetRequested;

			PreLoadReplacments();
			helper.Events.GameLoop.GameLaunched += (sender, args) => _modAssetLoader.ReplaceTitleScreenAssets();
			helper.Events.GameLoop.GameLaunched += (sender, args) => TryLoadModConfigMenu();
			helper.Events.Content.LocaleChanged += (sender, args) => _modAssetLoader.ReplaceTitleScreenAssets();
			helper.Events.GameLoop.ReturnedToTitle += (sender, args) => _modAssetEditor.ResetValuesAndInvalidateCache();
			helper.Events.GameLoop.ReturnedToTitle += (sender, args) => _modAssetLoader.ReplaceTitleScreenAssets();
            helper.Events.GameLoop.SaveLoaded += (sender, args) => CalculateAllReplacements();
			helper.Events.Display.MenuChanged += MenuAdjustments.AdjustMenus;
			helper.Events.GameLoop.DayEnding += (sender, args) => MenuAdjustments.ResetShopStates();

			if (Globals.Config.Music.Randomize) 
			{ 
				helper.Events.GameLoop.UpdateTicked += (sender, args) => MusicRandomizer.TryReplaceSong(); 
			}

			if (Globals.Config.RandomizeRain)
			{
				helper.Events.GameLoop.DayStarted += (sender, args) => _modAssetLoader.ReplaceRain();
			}

			if (Globals.Config.Crops.Randomize)
			{
				helper.Events.Multiplayer.PeerContextReceived += (sender, args) => WorldAdjustments.FixParsnipSeedBox();
			}

			if (Globals.Config.Crops.Randomize || Globals.Config.Fish.Randomize)
			{
				helper.Events.Display.RenderingActiveMenu += (sender, args) => CraftingRecipeAdjustments.HandleCraftingMenus();
			}

			//TODO 1.6: the repoint hack has a myserious crash now... we should probably try to override it instead or something
			//if (Globals.Config.RandomizeForagables)
			//{
			//	helper.Events.GameLoop.GameLaunched += (sender, args) => WildSeedAdjustments.ReplaceGetRandomWildCropForSeason();
			//}

			if (Globals.Config.Bundles.Randomize && Globals.Config.Bundles.ShowDescriptionsInBundleTooltips)
			{
				helper.Events.Display.RenderedActiveMenu += (sender, args) => BundleMenuAdjustments.AddDescriptionsToBundleTooltips();
			}
		}

		/// <summary>
		/// When an asset is requested, attempt to replace it
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            _modAssetLoader.OnAssetRequested(sender, e);
            _modAssetEditor.OnAssetRequested(sender, e);
        }

		/// <summary>
		/// Set up the mod config menu
		/// Exits early if it is not installed
		/// </summary>
        private void TryLoadModConfigMenu()
		{
			if (!Helper.ModRegistry.IsLoaded("spacechase0.GenericModConfigMenu"))
			{
				Globals.ConsoleTrace("GenericModConfigMenu not present");
				return;
			}

			api = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
			api.Register(ModManifest, () => Globals.Config = new ModConfig(), () => Helper.WriteConfig(Globals.Config));

			ModConfigMenuHelper menuHelper = new(api, ModManifest);
			menuHelper.RegisterModOptions();

		}

		/// <summary>
		/// Loads the replacements that can be loaded before a game is selected
		/// </summary>
		public void PreLoadReplacments()
		{
			_modAssetEditor.CalculateEditsBeforeLoad();
		}

		/// <summary>
		/// Does all the randomizer replacements that take place after a game is loaded
		/// </summary>
		public void CalculateAllReplacements()
		{
			// Seed is pulled from farm name
			byte[] seedvar = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(Game1.player.farmName.Value));
			int seed = BitConverter.ToInt32(seedvar, 0);

			Monitor.Log($"Seed Set: {seed}");

			Globals.RNG = new SaveLoadRNG(seed);
			Globals.SpoilerLog = new SpoilerLogger(Game1.player.farmName.Value);

            // Make replacements and edits
            _modAssetLoader.CalculateReplacements();
			_modAssetEditor.CalculateEdits();
			_modAssetLoader.RandomizeImages();
            MuseumRewardMenuAdjustments.PopulateRewardMap();

			// Invalidate all replaced and edited assets so they are reloaded
			_modAssetLoader.InvalidateCache();
			_modAssetEditor.InvalidateCache();

			// Ensure that the bundles get changed if they're meant to
			Game1.GenerateBundles(Game1.bundleType, true);

            WorldAdjustments.ChangeDayOneForagables();
            WorldAdjustments.FixParsnipSeedBox();

			// We should now be done with Globals.RNG
            Globals.RNG.IsPostFileLoad = true;
            Globals.SpoilerLog.WriteFile();
        }

		/// <summary>
		/// A passthrough to calculate amd invalidate UI edits
		/// Used when the lanauage is changed
		/// </summary>
		public void CalculateAndInvalidateUIEdits()
		{
			_modAssetEditor.CalculateAndInvalidateUIEdits();
		}
	}
}