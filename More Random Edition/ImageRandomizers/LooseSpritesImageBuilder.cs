using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

namespace Randomizer
{
    /// <summary>
    /// Modifies the graphics so the first pet image is a question mark to indicate
    /// that it will use a random pet
    /// 
    /// Handles restoring the original image should the setting change
    /// </summary>
    public class LooseSpritesImageBuilder : ImageBuilder
    {
        public LooseSpritesImageBuilder()
        {
            // This needs to be done on demand here since it's called every time the locale is changed
            File.Delete(Globals.GetFilePath("Assets/CustomImages/LooseSprites/randomizedImage.png"));

            SubDirectory = "LooseSprites";
            StardewAssetPath = $"LooseSprites/{Globals.GetLocalizedFileName("Cursors")}";
            OffsetWidthInPx = 160;
            OffsetHeightInPx = 208;
            PositionsToOverlay = new List<Point>() { new(1, 1) };
        }

        /// <summary>
        /// Gets the file name to replace
        /// Nothing random here, just one replacement to make!
        /// </summary>
        /// <param name="position">Unused</param>
        /// <returns>The filename of the random pet icon</returns>
        protected override string GetRandomFileName(Point position)
        {
            return Globals.Config.Animals.RandomizePets
                ? $"{ImageDirectory}/RandomPetIcon.png"
                : $"{ImageDirectory}/OriginalPetIcon.png";
        }

        /// <summary>
        /// The image will always be saved, as we're either modifing it, or restoring it back to normal
        /// </summary>
        /// <returns>True</returns>
        public override bool ShouldSaveImage()
        {
            return true;
        }
    }
}
