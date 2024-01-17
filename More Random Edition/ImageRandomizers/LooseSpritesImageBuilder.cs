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
        /// <summary>
        /// Get the SMAPI original asset path so that we can restore the
        /// pet image if the setting changes
        /// </summary>
        public string SMAPIOriginalAssetPath 
        { 
            get { return $"Assets/CustomImages/{SubDirectory}/{BaseFileName}"; } 
        }
        
        public LooseSpritesImageBuilder()
        {
            SubDirectory = "LooseSprites";
            BaseFileName = Globals.GetLocalizedFileName("Cursors", "png");
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
