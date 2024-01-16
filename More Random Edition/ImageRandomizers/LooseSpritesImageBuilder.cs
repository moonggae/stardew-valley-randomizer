using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Randomizer
{
    /// <summary>
    /// Modifies the graphics so the first pet image is a question mark to indicate
    /// that it will use a random pet
    /// </summary>
    public class LooseSpritesImageBuilder : ImageBuilder
    {
        public LooseSpritesImageBuilder()
        {
            SubDirectory = $"LooseSprites";

            var localeSuffix = "";
            var localeString = Globals.ModRef.Helper.Translation.Locale;
            if (localeString != string.Empty)
            {
                localeSuffix = $".{localeString}";
            }
            BaseFileName = $"Cursors{localeSuffix}.png";

            PositionsToOverlay = new List<Point>() {
                new(1, 1)
            };

            OffsetWidthInPx = 160;
            OffsetHeightInPx = 208;
        }

        /// <summary>
        /// Gets the file name to replace
        /// Nothing random here, just one replacement to make!
        /// </summary>
        /// <param name="position">Unused</param>
        /// <returns>The filename of the random pet icon</returns>
        protected override string GetRandomFileName(Point position)
        {
            return $"{ImageDirectory}/RandomPetIcon.png";
        }

        /// <summary>
        /// Whether the image should be saved
        /// For now, we only modify a cat picture, so this is based solely on the pet setting
        /// </summary>
        /// <returns>True if the image should be saved, false otherwise</returns>
        public override bool ShouldSaveImage()
        {
            return Globals.Config.Animals.RandomizePets;
        }
    }
}
