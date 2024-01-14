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
    public class HorseRandomizer : ImageBuilder
    {
        private readonly Color PaleColor = new Color(138,255,217);

        public HorseRandomizer()
        {
            SubDirectory = "Animals/Horses";

            var HorseImages = Directory.GetFiles($"{ImageDirectory}")
            .Where(x => x.EndsWith(".png"))
            .Select(x => Path.GetFileName(x))
            .OrderBy(x => x).ToList();

            BaseFileName = Globals.RNGGetRandomValueFromList( HorseImages );
        }

        new public void BuildImage()
        {
            Texture2D horseImage = Texture2D.FromFile(Game1.graphics.GraphicsDevice, BaseFileFullPath);
            Texture2D finalImage;

            if (BaseFileName == "horse.png")
            {
                Color shiftedPaleColor = HueShifter.IncreaseHueBy(PaleColor, Range.GetRandomValue(0, 359));
                finalImage = HueShifter.MultiplyImageByColor(horseImage, shiftedPaleColor);
            }
            else finalImage = horseImage;

            if (ShouldSaveImage())
            {
                using FileStream stream = File.OpenWrite(OutputFileFullPath);
                finalImage.SaveAsPng(stream, finalImage.Width, finalImage.Height);
            }

        }

        public override bool ShouldSaveImage()
        {
            return Globals.Config.Animals.RandomizeHorses;
        }





    }
}
