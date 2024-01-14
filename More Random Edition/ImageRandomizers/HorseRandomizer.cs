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
            SubDirectory = "Animals";
            BaseFileName = "horse.png";

        }

        new public void BuildImage()
        {
            Texture2D horseImage = Texture2D.FromFile(Game1.graphics.GraphicsDevice, BaseFileFullPath);

            Color shiftedPaleColor = HueShifter.IncreaseHueBy(PaleColor, Range.GetRandomValue(0, 359));
            Texture2D finalImage = HueShifter.MultiplyImageByColor(horseImage, shiftedPaleColor);

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
