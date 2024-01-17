using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Randomizer
{
    public class AssetLoader
	{
		private readonly ModEntry _mod;

		private readonly Dictionary<string, string> _customAssetReplacements = new();
        private readonly Dictionary<string, Texture2D> _editedAssetReplacements = new();

        /// <summary>Constructor</summary>
        /// <param name="mod">A reference to the ModEntry</param>
        public AssetLoader(ModEntry mod)
		{
			_mod = mod;
		}

        /// <summary>
        /// When an asset is requested, execute the approriate patcher's code, or replace
		/// the value from our dictionary
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void OnAssetRequested(object sender, AssetRequestedEventArgs e)
		{
            if (e.NameWithoutLocale.IsEquivalentTo(RainPatcher.StardewAssetPath))
            {
                e.Edit(new RainPatcher().OnAssetRequested);
            }
            else if (e.Name.IsEquivalentTo(PetIconPatcher.StardewAssetPath))
            {
                e.Edit(new PetIconPatcher().OnAssetRequested);
            }
            else if (e.NameWithoutLocale.IsEquivalentTo(TitleScreenPatcher.StardewAssetPath))
            {
                e.Edit(new TitleScreenPatcher().OnAssetRequested);
            }

			// Files that come from our own images: we're replacing an xnb asset with one on our filesystem
            else if (_customAssetReplacements.TryGetValue(e.Name.BaseName, out string customAsset))
            {
                e.LoadFromModFile<Texture2D>(customAsset, AssetLoadPriority.Medium);
				_customAssetReplacements.Remove(e.Name.BaseName);
            } 

			// Files that we have in memory: we're replacing an xnb asset with a Texture2D object
			else if (_editedAssetReplacements.TryGetValue(e.Name.BaseName, out Texture2D editedAsset))
			{
				e.Edit(asset =>
				{
					var editor = asset.AsImage();
					editor.PatchImage(editedAsset);
					_editedAssetReplacements.Remove(e.Name.BaseName);
                });
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
			_customAssetReplacements[normalizedAssetName.BaseName] = replacementAsset;
		}

        /// <summary>
        /// Adds a replacement to our internal dictionary
        /// </summary>
        /// <param name="originalAsset">The original asset</param>
        /// <param name="replacementAsset">The asset to replace it with</param>
        private void AddReplacement(string originalAsset, Texture2D replacementAsset)
        {
            IAssetName normalizedAssetName = _mod.Helper.GameContent.ParseAssetName(originalAsset);
            _editedAssetReplacements[normalizedAssetName.BaseName] = replacementAsset;
        }

        /// <summary>
        /// Adds a set of replacements to our internal dictionary for assets coming from our own files
        /// </summary>
        /// <param name="replacements">Key: the original asset; Value: the asset to replace it with</param>
        private void AddCustomAssetReplacements(Dictionary<string, string> replacements)
		{
			foreach (string key in replacements.Keys)
			{
				AddReplacement(key, replacements[key]);
			}
		}

        /// <summary>
        /// Adds a set of replacements to our internal dictionary for assets that we have texture data for
        /// </summary>
        /// <param name="replacements">Key: the original asset; Value: the asset to replace it with</param>
        private void AddEditedAssetReplacements(Dictionary<string, Texture2D> replacements)
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
			foreach (string assetName in _customAssetReplacements.Keys)
			{
				_mod.Helper.GameContent.InvalidateCache(assetName);
			}

            foreach (string assetName in _editedAssetReplacements.Keys)
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
            _mod.Helper.GameContent.InvalidateCache(PetIconPatcher.StardewAssetPath);
        }

		/// <summary>
		/// Replaces the title screen graphics and refreshes the settings UI page
		/// </summary>
		private void ReplaceTitleScreen()
		{
            _mod.Helper.GameContent.InvalidateCache(TitleScreenPatcher.StardewAssetPath);
        }

        /// <summary>
        /// Replaces the rain - intended to be called once per day start
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        /// </summary>
        public void ReplaceRain()
        {
            if (Globals.Config.RandomizeRain)
            {
                _mod.Helper.GameContent.InvalidateCache("TileSheets/rain");
            }
        }

        /// <summary>Asset replacements to load when the farm is loaded</summary>
        public void CalculateReplacements()
		{
			_customAssetReplacements.Clear();

			AddCustomAssetReplacements(AnimalSkinRandomizer.Randomize());

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
	}
}