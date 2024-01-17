using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Randomizer
{
    public class AssetLoader
	{
		private readonly ModEntry _mod;
		private readonly Dictionary<string, string> _replacements = new();

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
            if (e.NameWithoutLocale.IsEquivalentTo("LooseSprites/Cursors"))
            {
                e.Edit(new LooseSpritesImagePatcher().OnAssetRequested);
            }
            else if (_replacements.TryGetValue(e.Name.BaseName, out string replacementAsset))
            {
                e.LoadFromModFile<Texture2D>(replacementAsset, AssetLoadPriority.Medium);
				_replacements.Remove(e.Name.BaseName);
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
        /// Replace the assets on the title screen - includes the title screen menu
        /// and the new game menu
        /// </summary>
        public void ReplaceTitleScreenAssets()
		{
			ReplaceTitleScreen();
			ReplaceNewGameCatIcon();
        }

		/// <summary>
		/// Replaces the cat icon on the new game menu if pets are randomized
		/// Otherwise, restore the icon
		/// </summary>
		private void ReplaceNewGameCatIcon()
		{
			var xnbLocation = "LooseSprites/Cursors";
            _mod.Helper.GameContent.InvalidateCache(xnbLocation);
        }

		/// <summary>
		/// Replaces the title screen graphics and refreshes the settings UI page
		/// </summary>
		private void ReplaceTitleScreen()
		{
			string moddedAssetName = "TitleButtons";
			string stardewAssetName = "Minigames/TitleButtons";
            AddReplacement(stardewAssetName, $"Assets/Minigames/{Globals.GetLocalizedFileName(moddedAssetName, "png")}");
			_mod.Helper.GameContent.InvalidateCache(stardewAssetName);
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

            Globals.SpoilerWrite("==== ANIMALS ====");

            AnimalRandomizer horseImageBuilder = new(AnimalTypes.Horses);
            horseImageBuilder.BuildImage();
            HandleImageReplacement(horseImageBuilder, "Animals/horse");

            AnimalRandomizer petImageBuilder = new(AnimalTypes.Pets);
            petImageBuilder.BuildImage();
            HandleImageReplacement(petImageBuilder, "Animals/cat");

            Globals.SpoilerWrite("");
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