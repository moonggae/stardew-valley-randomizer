using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;

namespace Randomizer
{
    /// <summary>
    /// Modifies the graphics so the first pet image is a question mark to indicate
    /// that it will use a random pet
    /// 
    /// Handles restoring the original image should the setting change
    /// </summary>
    public class LooseSpritesImagePatcher : ImagePatcher
    {
        public LooseSpritesImagePatcher()
        {
            StardewAssetPath = "LooseSprites/Cursors";
            SubFolder = "LooseSprites";
        }

        public override void OnAssetRequested(IAssetData asset)
        {
            var editor = asset.AsImage();
            IRawTextureData overlay = Globals.ModRef.Helper.ModContent
                .Load<IRawTextureData>(GetCustomAssetPath());

            editor.PatchImage(overlay, targetArea: new Rectangle(160, 208, ImageWidthInPx, ImageHeightInPx));
        }

        public string GetCustomAssetPath()
        {
            var petSpriteImage = Globals.Config.Animals.RandomizePets
                ? "RandomPetIcon.png"
                : "OriginalPetIcon.png";

            return $"{PatcherImageFolder}/{petSpriteImage}";
        }
    }
}
