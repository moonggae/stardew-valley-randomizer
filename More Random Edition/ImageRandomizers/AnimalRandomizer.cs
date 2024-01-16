using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using More_Random_Edition.ImageRandomizers;
using StardewValley;

namespace Randomizer
{
    public class AnimalRandomizer : ImageBuilder
    {
        private readonly Color PaleColor = new Color(138,255,217);
        private AnimalTypes AnimalTypeToRandomize;

        public AnimalRandomizer(AnimalTypes animalTypeToRandomize)
        {
            AnimalTypeToRandomize = animalTypeToRandomize;
            SubDirectory = $"Animals/{animalTypeToRandomize}";

            var AnimalImages = Directory.GetFiles($"{ImageDirectory}")
            .Where(x => x.EndsWith(".png"))
            .Select(x => Path.GetFileName(x))
            .OrderBy(x => x).ToList();

            BaseFileName = Globals.RNGGetRandomValueFromList( AnimalImages );
        }

        new public void BuildImage()
        {
            using Texture2D replacingImage = Texture2D.FromFile(Game1.graphics.GraphicsDevice, BaseFileFullPath);
            Texture2D finalImage;

            if (BaseFileName == "horse.png")
            {
                Color shiftedPaleColor = ImageManipulator.IncreaseHueBy(PaleColor, Range.GetRandomValue(0, 359));
                finalImage = ImageManipulator.MultiplyImageByColor(replacingImage, shiftedPaleColor);
            }
            else finalImage = replacingImage;

            if (ShouldSaveImage())
            {
                using FileStream stream = File.OpenWrite(OutputFileFullPath);
                finalImage.SaveAsPng(stream, finalImage.Width, finalImage.Height);
            }

            finalImage.Dispose();

        }

        public override bool ShouldSaveImage()
        {
            switch (AnimalTypeToRandomize)
            {
                case AnimalTypes.Horses: 
                    return Globals.Config.Animals.RandomizeHorses;
                case AnimalTypes.Pets:
                    return Globals.Config.Animals.RandomizePets;
                default:
                    Globals.ConsoleError("Tried to save randomized image of unrecognized Animal type");
                    return false;

                    

            }

        }





    }
}
