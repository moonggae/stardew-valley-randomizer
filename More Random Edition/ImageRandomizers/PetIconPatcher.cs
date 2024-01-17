using Microsoft.Xna.Framework;
using StardewModdingAPI;

namespace Randomizer
{
    /// <summary>
    /// Modifies the graphics so the first pet image is a question mark to indicate
    /// that it will use a random pet
    /// 
    /// Handles restoring the original image should the setting change
    /// </summary>
    public class PetIconPatcher : ImagePatcher
    {
        public static string StardewAssetPath => Globals.GetLocalizedFileName("LooseSprites/Cursors");

        public PetIconPatcher()
        {
            SubFolder = "LooseSprites";
        }

        /// <summary>
        /// Called when the asset is requested
        /// Patches the image into the game
        /// </summary>
        /// <param name="asset">The equivalent asset from Stardew to modify</param>
        public override void OnAssetRequested(IAssetData asset)
        {
            var editor = asset.AsImage();
            IRawTextureData overlay = Globals.ModRef.Helper.ModContent
                .Load<IRawTextureData>(GetCustomAssetPath());

            editor.PatchImage(overlay, targetArea: new Rectangle(160, 208, ImageWidthInPx, ImageHeightInPx));
        }

        /// <summary>
        /// Gets the pet icon to replace
        /// </summary>
        /// <returns>The full path of the icon, starting at the root of the mod</returns>
        public string GetCustomAssetPath()
        {
            var petSpriteImage = Globals.Config.Animals.RandomizePets
                ? "RandomPetIcon.png"
                : "OriginalPetIcon.png";

            return $"{PatcherImageFolder}/{petSpriteImage}";
        }
    }
}
