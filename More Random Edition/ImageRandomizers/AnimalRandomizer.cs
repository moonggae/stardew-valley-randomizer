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
        /// The base color to use - this value hue-shifted doesn't result in really
        /// weird looking images when multiplied onto images
        /// </summary>
        private readonly Color PaleColor = new(138, 255 ,217);

        /// <summary>
        /// The type of animal this is randomizing
        /// </summary>
        private AnimalTypes AnimalTypeToRandomize { get; set; }

        public AnimalRandomizer(AnimalTypes animalTypeToRandomize)
        {
            AnimalTypeToRandomize = animalTypeToRandomize;
            SubDirectory = $"Animals/{animalTypeToRandomize}";

            var AnimalImages = Directory.GetFiles($"{ImageDirectory}")
                .Where(x => x.EndsWith(".png"))
                .Select(x => Path.GetFileName(x))
                .OrderBy(x => x)
                .ToList();

            BaseFileName = Globals.RNGGetRandomValueFromList(AnimalImages);
        }

        /// <summary>
        /// Build the image
        /// If the image is "horse.png", we'll shift its hue
        /// TODO: change this logic to be cleaner - probably based on where images are in directories
        /// </summary>
        public override void BuildImage()
        {
            using Texture2D replacingImage = Texture2D.FromFile(Game1.graphics.GraphicsDevice, BaseFileFullPath);
            Texture2D finalImage;

            if (BaseFileName == "horse.png")
            {
                Color shiftedPaleColor = ImageManipulator.IncreaseHueBy(PaleColor, Range.GetRandomValue(0, 359));
                finalImage = ImageManipulator.MultiplyImageByColor(replacingImage, shiftedPaleColor);
            }
            else
            {
                finalImage = replacingImage;
            }

            if (ShouldSaveImage())
            {
                using FileStream stream = File.OpenWrite(OutputFileFullPath);
                finalImage.SaveAsPng(stream, finalImage.Width, finalImage.Height);

                Globals.SpoilerWrite($"{AnimalTypeToRandomize} replaced with {BaseFileName[..^4]}");
            }

            finalImage.Dispose();

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
