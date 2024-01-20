using Microsoft.Xna.Framework;
using StardewModdingAPI;
using System.IO;

namespace Randomizer
{
    /// <summary>
    /// Modifies the graphics so the first pet image is a question mark to indicate
    /// that it will use a random pet
    /// 
    /// Also modifies the little icons of the pet and horse icon in the game's pause menu
    /// 
    /// Handles restoring the original image should the setting change
    /// </summary>
    public class AnimalIconPatcher : ImagePatcher
    {
        public static string StardewAssetPath => 
            Globals.GetLocalizedFileName("LooseSprites/Cursors");

        public AnimalIconPatcher()
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
            PatchAnimalIcon(asset, AnimalTypes.Pets, x: 160, y: 208);
            PatchAnimalIcon(asset, AnimalTypes.Horses, x: 192, y: 192);
        }

        /// <summary>
        /// Patches the animal over the Cursors.png sprite page at the specified coords
        /// </summary>
        /// <param name="asset">The image</param>
        /// <param name="animalType">The animal type, pet or horse expected</param>
        /// <param name="x">The x-origin of the icon</param>
        /// <param name="y">The y-origin of the icon</param>
        private void PatchAnimalIcon(IAssetData asset, AnimalTypes animalType, int x, int y)
        {
            string animalNamePath = GetCustomAssetPath(animalType);
            if (string.IsNullOrEmpty(animalNamePath))
            {
                return;
            }

            IRawTextureData overlay = Globals.ModRef.Helper.ModContent
                .Load<IRawTextureData>(animalNamePath);

            asset.AsImage().PatchImage(
                overlay,
                targetArea: new Rectangle(x, y, ImageWidthInPx, ImageHeightInPx));
        }

        /// <summary>
        /// Gets the pet icon to replace on the new game screen
        /// Gets the pause manu pet icon if we are in the game
        /// </summary>
        /// <returns>The full path of the icon, starting at the root of the mod</returns>
        private string GetCustomAssetPath(AnimalTypes animalType)
        {
            bool isRandomized = IsAnimalRandomized(animalType);
            if (isRandomized && Context.IsWorldReady)
            {
                string animalImage = AnimalRandomizer.GetRandomAnimalName(animalType, getOriginalName: true);
                string animalIconPath = Globals.GetFilePath(
                    $"{AssetsFolder}/CustomImages/Animals/{animalType}/Icons/{animalImage}");

                if (File.Exists(animalIconPath))
                {
                    return animalIconPath;
                }
            }

            var animalSpriteImage = isRandomized
                ? $"UnknownAnimalIcon.png"
                : $"Original{animalType}Icon.png";

            return $"{PatcherImageFolder}/{animalSpriteImage}";
        }

        /// <summary>
        /// Returns whether the given animal type is actually being randomized
        /// </summary>
        /// <param name="animalType">The animal type</param>
        /// <returns></returns>
        private static bool IsAnimalRandomized(AnimalTypes animalType)
        {
            return animalType switch
            {
                AnimalTypes.Pets => Globals.Config.Animals.RandomizePets,
                AnimalTypes.Horses => Globals.Config.Animals.RandomizeHorses,
                _ => false,
            };
        }
    }
}
