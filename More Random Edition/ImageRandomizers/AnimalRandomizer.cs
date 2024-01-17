using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System.IO;
using System.Linq;

namespace Randomizer
{
    /// <summary>
    /// Creates an image file for the given type of animal
    /// </summary>
    public class AnimalRandomizer : ImageBuilder
    {
        /// <summary>
        /// The type of animal this is randomizing
        /// </summary>
        private AnimalTypes AnimalTypeToRandomize { get; set; }

        public AnimalRandomizer(AnimalTypes animalTypeToRandomize)
        {
            AnimalTypeToRandomize = animalTypeToRandomize;
            SubDirectory = $"Animals/{animalTypeToRandomize}";
            StardewAssetPath = $"Animals/{animalTypeToRandomize}"; ;
        }

        /// <summary>
        /// Build the image - hue shift it if the base file name ends with "-hue-shift"
        /// </summary>
        protected override Texture2D BuildImage()
        {
            string randomAnimalFileName = GetRandomAnimalFileName();
            string imageLocation = $"{ImageDirectory}/{randomAnimalFileName}";
            using Texture2D replacingImage = Texture2D.FromFile(Game1.graphics.GraphicsDevice, imageLocation);
            Texture2D finalImage;

            if (randomAnimalFileName[..^4].EndsWith("-hue-shift"))
            {
                int hueShiftValue = Range.GetRandomValue(0, 359);
                Color shiftedPaleColor = ImageManipulator.IncreaseHueBy(ImageManipulator.PaleColor, hueShiftValue);
                finalImage = ImageManipulator.MultiplyImageByColor(replacingImage, shiftedPaleColor);
            }
            else
            {
                finalImage = replacingImage;
            }

            if (ShouldSaveImage() && Globals.Config.SaveRandomizedImages)
            {
                using FileStream stream = File.OpenWrite(OutputFileFullPath);
                finalImage.SaveAsPng(stream, finalImage.Width, finalImage.Height);

                Globals.SpoilerWrite($"{AnimalTypeToRandomize} replaced with {randomAnimalFileName[..^4]}");
            }

            return finalImage;
        }

        /// <summary>
        /// Gets a random animal file name from the randomizers current directory
        /// </summary>
        /// <returns></returns>
        private string GetRandomAnimalFileName()
        {
            var animalImages = Directory.GetFiles($"{ImageDirectory}")
                .Where(x => x.EndsWith(".png"))
                .Select(x => Path.GetFileName(x))
                .OrderBy(x => x)
                .ToList();

            return Globals.RNGGetRandomValueFromList(animalImages);

        }

        /// <summary>
        /// Whether we should save the image
        /// Based on the appriate Animal randomize setting
        /// </summary>
        /// <returns>True if we should save; false otherwise</returns>
        public override bool ShouldSaveImage()
        {
            switch (AnimalTypeToRandomize)
            {
                case AnimalTypes.Horses: 
                    return Globals.Config.Animals.RandomizeHorses;
                case AnimalTypes.Pets:
                    return Globals.Config.Animals.RandomizePets;
                default:
                    Globals.ConsoleError($"Tried to save randomized image of unrecognized Animal type: {AnimalTypeToRandomize}");
                    return false;
            }
        }
    }
}
