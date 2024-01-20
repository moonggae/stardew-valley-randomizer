using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using System.IO;

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
        public static string StardewAssetPath => 
            Globals.GetLocalizedFileName("LooseSprites/Cursors");

        public PetIconPatcher()
        {
            SubFolder = "LooseSprites";
        }

        /// <summary>
        /// Called when the asset is requested
        /// Patches the image into the game
        /// If a farm is loaded, will load the picture of the randomized pet
        /// </summary>
        /// <param name="asset">The equivalent asset from Stardew to modify</param>
        public override void OnAssetRequested(IAssetData asset)
        {
            var editor = asset.AsImage();
            string petNamePath = GetCustomAssetPath();

            IRawTextureData overlay = Globals.ModRef.Helper.ModContent
                .Load<IRawTextureData>(petNamePath);

            editor.PatchImage(
                overlay, 
                targetArea: new Rectangle(160, 208, ImageWidthInPx, ImageHeightInPx));
        }

        /// <summary>
        /// Gets the pet icon to replace on the new game screen
        /// Gets the pause manu pet icon if we are in the game
        /// </summary>
        /// <returns>The full path of the icon, starting at the root of the mod</returns>
        public string GetCustomAssetPath()
        {
            if (Globals.Config.Animals.RandomizePets && Context.IsWorldReady)
            {
                string petImage = AnimalRandomizer.GetRandomPetName(getOriginalName: true);
                string petIconPath = Globals.GetFilePath(
                    $"{AssetsFolder}/CustomImages/Animals/Pets/Icons/{petImage}");

                if (File.Exists(petIconPath))
                {
                    return petIconPath;
                }
            }

            var petSpriteImage = Globals.Config.Animals.RandomizePets
                ? "RandomPetIcon.png"
                : "OriginalPetIcon.png";

            return $"{PatcherImageFolder}/{petSpriteImage}";
        }
    }
}
