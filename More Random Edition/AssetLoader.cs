using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using static StardewValley.LocalizedContentManager;

namespace Randomizer
{
    public class AssetLoader
	{
		private readonly ModEntry _mod;
		private readonly Dictionary<string, string> _replacements = new Dictionary<string, string>();

		/// <summary>Constructor</summary>
		/// <param name="mod">A reference to the ModEntry</param>
		public AssetLoader(ModEntry mod)
		{
			_mod = mod;
		}

		/// <summary>
		/// When an asset is requested, load it from the replacements dictionary if
		/// there is actually an entry in it
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <exception cref="InvalidOperationException"></exception>
		public void OnAssetRequested(object sender, AssetRequestedEventArgs e)
		{
            if (_replacements.TryGetValue(e.Name.BaseName, out string replacementAsset))
            {
                e.LoadFromModFile<Texture2D>(replacementAsset, AssetLoadPriority.Medium);
            }
        }

		/// <summary>
		/// Adds a replacement to our internal dictionary
		/// </summary>
		/// <param name="originalAsset">The original asset</param>
		/// <param name="replacementAsset">The asset to replace it with</param>
		private void AddReplacement(string originalAsset, string replacementAsset)
		{
			IAssetName normalizedAssetName = _mod.Helper.GameContent.ParseAssetName(originalAsset);
			_replacements[normalizedAssetName.BaseName] = replacementAsset;
		}

		/// <summary>
		/// Adds a set of replacements to our internal dictionary
		/// </summary>
		/// <param name="replacements">Key: the original asset; Value: the asset to replace it with</param>
		private void AddReplacements(Dictionary<string, string> replacements)
		{
			foreach (string key in replacements.Keys)
			{
				AddReplacement(key, replacements[key]);
			}
		}

		/// <summary>
		/// Invalidate all replaced assets so that the changes are reapplied
		/// </summary>
		public void InvalidateCache()
		{
			foreach (string assetName in _replacements.Keys)
			{
				_mod.Helper.GameContent.InvalidateCache(assetName);
			}
		}

		/// <summary>
		/// Nothing to do here at the moment
		/// </summary>
		public void CalculateReplacementsBeforeLoad()
		{
		}

		/// <summary>
		/// The current locale
		/// </summary>
		private string _currentLocale = "default";

		/// <summary>
		/// Replaces the title scrren graphics - done whenever the locale is changed or the game is first loaded
		/// Won't actually replace it if it already did
		/// </summary>
		public void TryReplaceTitleScreen()
		{
			IClickableMenu genericMenu = Game1.activeClickableMenu;
			if (genericMenu is null || !(genericMenu is TitleMenu)) { return; }

			if (_currentLocale != _mod.Helper.Translation.Locale)
			{
				ReplaceTitleScreen((TitleMenu)genericMenu);
			}
		}

		/// <summary>
		/// Replaces the title screen after returning from a game - called by the appropriate event handler
		/// </summary>
		public void ReplaceTitleScreenAfterReturning()
		{
			ReplaceTitleScreen();
		}

		/// <summary>
		/// Replaces the title screen graphics and refreshes the settings UI page
		/// </summary>
		/// <param name="titleMenu">The title menu - passed if we're already on the title screen</param>
		private void ReplaceTitleScreen(TitleMenu titleMenu = null)
		{
			_currentLocale = _mod.Helper.Translation.Locale;
			AddReplacement("Minigames/TitleButtons", $"Assets/Minigames/{Globals.GetTranslation("title-graphic")}");
			_mod.Helper.GameContent.InvalidateCache("Minigames/TitleButtons");

			if (titleMenu != null)
			{
				LanguageCode code = _mod.Helper.Translation.LocaleEnum;
				_mod.Helper.Reflection.GetMethod(titleMenu, "OnLanguageChange", true).Invoke(code);
			}

			_mod.CalculateAndInvalidateUIEdits();
		}

		/// <summary>Asset replacements</summary>
		public void CalculateReplacements()
		{
			_replacements.Clear();

			AddReplacements(AnimalSkinRandomizer.Randomize());
			AddReplacements(NPCSkinRandomizer.Randomize());
			ReplaceRain();
		}

		/// <summary>
		/// Randomizes the images - depending on what settings are on
		/// It's still important to build the images to make sure seeds are consistent
		/// </summary>
		public void RandomizeImages()
		{
			WeaponImageBuilder weaponImageBuilder = new();
			weaponImageBuilder.BuildImage();
			HandleImageReplacement(weaponImageBuilder, "TileSheets/weapons");

			CropGrowthImageBuilder cropGrowthImageBuilder = new();
			cropGrowthImageBuilder.BuildImage();
			HandleImageReplacement(cropGrowthImageBuilder, "TileSheets/crops");

			SpringObjectsImageBuilder springObjectsImageBuilder = new(cropGrowthImageBuilder.CropIdsToLinkingData);
			springObjectsImageBuilder.BuildImage();
			HandleImageReplacement(springObjectsImageBuilder, "Maps/springobjects");

			BundleImageBuilder bundleImageBuilder = new();
			bundleImageBuilder.BuildImage();
			HandleImageReplacement(bundleImageBuilder, "LooseSprites/JunimoNote");

            AnimalRandomizer horseImageBuilder = new(AnimalTypes.Horses);
            horseImageBuilder.BuildImage();
            HandleImageReplacement(horseImageBuilder, "Animals/horse");

            AnimalRandomizer petImageBuilder = new(AnimalTypes.Pets);
            petImageBuilder.BuildImage();
            HandleImageReplacement(petImageBuilder, "Animals/cat");
        }

		/// <summary>
		/// Handles actually adding the image replacement
		/// If the image doesn't exist, sleep for 0.1 second increments until it does
		/// </summary>
		/// <param name="imageBuilder">The image builder</param>
		/// <param name="xnbPath">The path to the xnb image to replace</param>
		private void HandleImageReplacement(ImageBuilder imageBuilder, string xnbPath)
		{
			if (imageBuilder.ShouldSaveImage())
			{
				while (!File.Exists(imageBuilder.OutputFileFullPath))
				{
					Thread.Sleep(100);
				}
				AddReplacement(xnbPath, imageBuilder.SMAPIOutputFilePath);
			}
		}

		/// <summary>
		/// Replaces the rain - intended to be called once per day start
		/// <param name="sender">The event sender</param>
		/// <param name="e">The event arguments</param>
		/// </summary>
		public void ReplaceRain(object sender = null, DayEndingEventArgs e = null)
		{
			RainTypes rainType = Globals.RNGGetRandomValueFromList(
				Enum.GetValues(typeof(RainTypes)).Cast<RainTypes>().ToList(),
				Globals.GetDailyRNG("rain"));

			if (!Globals.Config.RandomizeRain) { return; }

			AddReplacement("TileSheets/rain", $"Assets/TileSheets/{rainType}Rain");
			_mod.Helper.GameContent.InvalidateCache("TileSheets/rain");
		}
	}
}