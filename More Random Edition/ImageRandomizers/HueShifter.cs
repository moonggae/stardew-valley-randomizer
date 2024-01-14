using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace More_Random_Edition.ImageRandomizers
{
    public class HueShifter
    {
        public static Texture2D ShiftImageHue(Texture2D image, float amountToShift)
        {

            Color[] imageColors = GetImageColorData(image);
            Color[] alteredImageColors = new Color[image.Width * image.Height];


            for (int i = 0; i < imageColors.Length; i++)
            {

                alteredImageColors[i] = IncreaseHueBy(imageColors[i], amountToShift);

            }

            Texture2D newImage = new Texture2D(Game1.graphics.GraphicsDevice, image.Width, image.Height);
            newImage.SetData(alteredImageColors);

            return newImage;

        }

        // It shifts any color within the inputted range by more or less than colors not in the range,
        // if zero it won't shift the color at all
        // H values mapped to colors for ref:
        //
        // Red falls between 0 and 60 degrees.
        // Yellow falls between 61 and 120 degrees.
        // Green falls between 121 and 180 degrees.
        // Cyan falls between 181 and 240 degrees.
        // Blue falls between 241 and 300 degrees.
        // Magenta falls between 301 and 360 degrees.

        public static Texture2D ShiftImageHue(Texture2D image, float amountToShift, float lowerBound, float upperBound, float multiplier)
        {

            Color[] imageColors = GetImageColorData(image);
            Color[] alteredImageColors = new Color[image.Width * image.Height];


            for (int i = 0; i < imageColors.Length; i++)
            {
                Color currentColor = imageColors[i];
                float currentHue = RgbToHsv(currentColor.R, currentColor.G, currentColor.B)[0];

                if (upperBound <= currentHue || currentHue <= lowerBound) alteredImageColors[i] = IncreaseHueBy(imageColors[i], amountToShift);
                else if (multiplier == 0) alteredImageColors[i] = imageColors[i];
                else alteredImageColors[i] = IncreaseHueBy(imageColors[i], amountToShift * multiplier);

            }

            Texture2D newImage = new Texture2D(Game1.graphics.GraphicsDevice, image.Width, image.Height);
            newImage.SetData(alteredImageColors);

            return newImage;

        }

        public static Texture2D MultiplyImages(Texture2D FirstImage, Texture2D SecondImage)
        {

            Color[] imageColors = GetImageColorData(FirstImage);
            Color[] image2Colors = GetImageColorData(SecondImage);
            Color[] alteredImageColors = new Color[FirstImage.Width * FirstImage.Height];


            for (int i = 0; i < imageColors.Length; i++)
            {
                alteredImageColors[i] = multiplyColors(imageColors[i], image2Colors[i]);

            }

            Texture2D newImage = new Texture2D(Game1.graphics.GraphicsDevice, FirstImage.Width, FirstImage.Height);
            newImage.SetData(alteredImageColors);

            return newImage;

        }

        public static Texture2D MultiplyImageByColor(Texture2D FirstImage, Color ColorToBeMultipliedBy)
        {

            Color[] imageColors = GetImageColorData(FirstImage);
            Color[] alteredImageColors = new Color[FirstImage.Width * FirstImage.Height];


            for (int i = 0; i < imageColors.Length; i++)
            {
                alteredImageColors[i] = multiplyColors(imageColors[i], ColorToBeMultipliedBy);

            }

            Texture2D newImage = new Texture2D(Game1.graphics.GraphicsDevice, FirstImage.Width, FirstImage.Height);
            newImage.SetData(alteredImageColors);

            return newImage;

        }

        private static Color[] GetImageColorData(Texture2D image)
        {
            Color[] colors = new Color[image.Width * image.Height];
            image.GetData<Color>(colors);
            return colors;

        }
        public static Color IncreaseHueBy(Color originalColor, float valueToIncrease)
        {
            float h;
            Color alteredColor = new Color();
            float[] HSV = new float[3];
            float[] RGB = new float[3];

            HSV = RgbToHsv(originalColor.R, originalColor.G, originalColor.B);
            h = HSV[0];
            h += valueToIncrease;

            RGB = HsvToRgb(h, HSV[1], HSV[2]);

            alteredColor.R = (byte)(RGB[0]);
            alteredColor.G = (byte)(RGB[1]);
            alteredColor.B = (byte)(RGB[2]);
            alteredColor.A = originalColor.A;

            return alteredColor;
        }

        public static Color multiplyColors(Color firstColor, Color secondColor)
        {
            Color multipliedColor = new Color();
            multipliedColor.R = (byte)((firstColor.R * secondColor.R) / 255);
            multipliedColor.G = (byte)((firstColor.G * secondColor.G) / 255);
            multipliedColor.B = (byte)((firstColor.B * secondColor.B) / 255);
            multipliedColor.A = Math.Min(firstColor.A, secondColor.A);

            return multipliedColor;
        }

        static float[] RgbToHsv(float r, float g, float b)
        {
            float[] output = new float[3];
            float h, s, v, min, max, delta;
            min = System.Math.Min(System.Math.Min(r, g), b);
            max = System.Math.Max(System.Math.Max(r, g), b);
            v = max;               // v
            delta = max - min;
            if (max != 0)
            {
                s = delta / max;       // s

                if (r == max)
                    h = (g - b) / delta;       // between yellow & magenta
                else if (g == max)
                    h = 2 + (b - r) / delta;   // between cyan & yellow
                else
                    h = 4 + (r - g) / delta;   // between magenta & cyan
                h *= 60;               // degrees
                if (h < 0)
                    h += 360;
            }
            else
            {
                // r = g = b = 0       // s = 0, v is undefined
                s = 0;
                h = -1;
            }

            output[0] = h;
            output[1] = s;
            output[2] = v;

            return output;

        }
        static float[] HsvToRgb(float h, float s, float v)
        {
            float[] output = new float[3];
            // Keeps h from going over 360
            h = h - ((int)(h / 360) * 360);

            int i;
            float r, g, b, f, p, q, t;
            if (s == 0)
            {
                // achromatic (grey)
                r = g = b = v;

                output[0] = r;
                output[1] = g;
                output[2] = b;

                return output;
            }
            h /= 60;           // sector 0 to 5

            i = (int)h;
            f = h - i;         // factorial part of h
            p = v * (1 - s);
            q = v * (1 - s * f);
            t = v * (1 - s * (1 - f));
            switch (i)
            {
                case 0:
                    r = v;
                    g = t;
                    b = p;
                    break;
                case 1:
                    r = q;
                    g = v;
                    b = p;
                    break;
                case 2:
                    r = p;
                    g = v;
                    b = t;
                    break;
                case 3:
                    r = p;
                    g = q;
                    b = v;
                    break;
                case 4:
                    r = t;
                    g = p;
                    b = v;
                    break;
                default:       // case 5:
                    r = v;
                    g = p;
                    b = q;
                    break;
            }

            output[0] = r;
            output[1] = g;
            output[2] = b;

            return output;
        }
    }
}

