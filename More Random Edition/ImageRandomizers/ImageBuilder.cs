using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Linq;
using StardewValley;

namespace Randomizer
{
    public abstract class ImageBuilder
    {
        /// <summary>
        /// The image width in px - used when determining whether to crop and when drawing the image itself
        /// </summary>
        protected int ImageWidthInPx = 16;

        /// <summary>
        /// The image height in px - used when determining whether to crop and when drawing the image itself
        /// </summary>
        protected int ImageHeightInPx = 16;

        /// <summary>
        /// The offset width in px - used when positioning the image
        /// </summary>
        protected int OffsetWidthInPx = 16;

        /// <summary>
        /// The image height in px - used when positioning the image
        /// </summary>
        protected int OffsetHeightInPx = 16;

        /// <summary>
        /// The image height in px - this is the initial height to start drawing at
        /// </summary>
        protected int InitialHeightOffetInPx = 0;

        /// <summary>
        /// The name of the output file
        /// </summary>
        protected const string OutputFileName = "randomizedImage.png";

        /// <summary>
        /// The path for all the custom images
        /// </summary>
        protected readonly string CustomImagesPath = Globals.GetFilePath("Assets/CustomImages");

        /// <summary>
        /// The path to the custom images
        /// </summary>
        protected string ImageDirectory
        {
            get
            {
                return $"{CustomImagesPath}/{SubDirectory}";
            }
        }

        /// <summary>
        /// The path to the custom images
        /// </summary>
        public string BaseFileFullPath
        {
            get
            {
                return $"{ImageDirectory}/{BaseFileName}";
            }
        }

        /// <summary>
        /// The path to the custom images
        /// </summary>
        public string OutputFileFullPath
        {
            get
            {
                return $"{ImageDirectory}/{OutputFileName}";
            }
        }

        /// <summary>
        /// The output path as needed by SMAPI
        /// </summary>
        public string SMAPIOutputFilePath
        {
            get
            {
                return $"Assets/CustomImages/{SubDirectory}/{OutputFileName}";
            }
        }

        /// <summary>
        /// The name of the base file
        /// </summary>
        protected string BaseFileName { get; set; }

        /// <summary>
        /// The subdirectory where the base file and replacements are located
        /// </summary>
        protected string SubDirectory { get; set; }

        /// <summary>
        /// A list of positions in the file that will be overlayed to
        /// </summary>
        protected List<Point> PositionsToOverlay { get; set; }

        /// <summary>
        /// The files to pull from - gets all images in the directory that don't include the base file
        /// </summary>
        protected List<string> FilesToPullFrom { get; set; }

        /// <summary>
        /// ONLY set this if your base image is coming directly from an XNB file,
        /// in which case, set it to that path
        /// </summary>
        protected string StardewAssetPath { get; set; }

        /// <summary>
        /// Returns true if we're loading the base image from Stardew rather than
        /// providing our own
        /// </summary>
        private bool IsLoadingBaseImageFromXNB
        {
            get { return !string.IsNullOrWhiteSpace(StardewAssetPath); }
        }

        /// <summary>
        /// Builds the image and saves the result into randomizedImage.png
        /// </summary>
        public virtual void BuildImage()
        {
            Texture2D finalImage = IsLoadingBaseImageFromXNB
                ? Globals.ModRef.Helper.GameContent.Load<Texture2D>(StardewAssetPath)
                : Texture2D.FromFile(Game1.graphics.GraphicsDevice, BaseFileFullPath);

            FilesToPullFrom = GetAllCustomImages();
            foreach (Point position in PositionsToOverlay)
            {
                string randomFileName = GetRandomFileName(position);
                if (string.IsNullOrWhiteSpace(randomFileName) || !ShouldSaveImage(position))
                {
                    continue;
                }

                if (!File.Exists(randomFileName))
                {
                    Globals.ConsoleError($"File {randomFileName} does not exist! Using default image instead.");
                    continue;
                }

                using Texture2D originalRandomImage = Texture2D.FromFile(Game1.graphics.GraphicsDevice, randomFileName);
                using Texture2D randomImage = ManipulateImage(originalRandomImage, randomFileName);
                CropAndOverlayImage(position, randomImage, finalImage);
            }

            if (ShouldSaveImage())
            {
                using FileStream stream = File.OpenWrite(OutputFileFullPath);
                finalImage.SaveAsPng(stream, finalImage.Width, finalImage.Height);
            }

            // Do NOT dispose of an in game image, unless you want NullReferenceExceptions
            // when the game tries to use the image again!
            if (!IsLoadingBaseImageFromXNB)
            {
                finalImage.Dispose();
            }
        }

        /// <summary>
        /// Manipulate the image in some way - will be left to the parent classes to decide
        /// </summary>
        /// <param name="image"></param>
        /// <returns>The manipulated image (the input, in this case)</returns>
        protected virtual Texture2D ManipulateImage(Texture2D image, string fileName)
        {
            return image;
        }

        /// <summary>
        /// Crops out an image from the position and the random image given to us and places it
        /// onto a final image that we will use as the output
        /// Based on https://stackoverflow.com/questions/16137500/cropping-texture2d-on-all-sides-in-xna-c-sharp
        /// </summary>
        /// <param name="position">The position to start the cropping</param>
        /// <param name="randomImage">The image to crop from</param>
        /// <param name="finalImage">The image to crop to</param>
        private void CropAndOverlayImage(Point position, Texture2D randomImage, Texture2D finalImage)
        {
            int xOffset = position.X * OffsetWidthInPx;
            int yOffset = position.Y * OffsetHeightInPx + InitialHeightOffetInPx;

            Rectangle sourceRect = new Rectangle(0, 0, ImageWidthInPx, ImageHeightInPx);
            Color[] data = new Color[sourceRect.Width * sourceRect.Height];
            randomImage.GetData(0, sourceRect, data, 0, sourceRect.Width * sourceRect.Height);

            Rectangle destRect = new Rectangle(xOffset, yOffset, ImageWidthInPx, ImageHeightInPx);
            finalImage.SetData(0, destRect, data, 0, destRect.Width * destRect.Height);
        }

        /// <summary>
        /// Gets all the custom images from the given directory excluding the base file name
        /// </summary>
        /// <returns></returns>
        private List<string> GetAllCustomImages()
        {
            List<string> files = Directory.GetFiles(ImageDirectory).ToList();
            return files.Where(x =>
                !x.EndsWith(OutputFileName) &&
                (IsLoadingBaseImageFromXNB || !x.EndsWith(BaseFileName)) &&
                x.EndsWith(".png"))
            .ToList();
        }

        /// <summary>
        /// Gets a random file name from the files to pull from and removes the found entry from the list
        /// </summary>
        /// <param name="position">The position of the instrument - unused in this version of the function</param>
        /// <returns></returns>
        protected virtual string GetRandomFileName(Point position)
        {
            string fileName = Globals.RNGGetAndRemoveRandomValueFromList(FilesToPullFrom);

            if (string.IsNullOrEmpty(fileName))
            {
                Globals.ConsoleWarn($"Not enough images at directory (need more images, using default image): {ImageDirectory}");
                return null;
            }

            return fileName;
        }

        /// <summary>
        /// Whether we should actually save the image file, or if the setting is off
        /// </summary>
        /// <returns />
        public abstract bool ShouldSaveImage();

        /// <summary>
        /// Whether we should actually save the image file, or if the setting is off
        /// This is used to check individual images - default is to check for the entire image builder
        /// </summary>
        /// <param name="point">The point to check at</param>
        /// <returns />
        protected virtual bool ShouldSaveImage(Point point)
        {
            return ShouldSaveImage();
        }

        /// <summary>
        /// Cleans up all replacement files
        /// Called whenever a farm or the game is loaded
        /// </summary>
        public static void CleanUpReplacementFiles()
        {
            File.Delete(Globals.GetFilePath("Assets/CustomImages/Bundles/randomizedImage.png"));
            File.Delete(Globals.GetFilePath("Assets/CustomImages/Weapons/randomizedImage.png"));
            File.Delete(Globals.GetFilePath("Assets/CustomImages/SpringObjects/randomizedImage.png"));
            File.Delete(Globals.GetFilePath("Assets/CustomImages/CropGrowth/randomizedImage.png"));
            File.Delete(Globals.GetFilePath("Assets/CustomImages/Animals/Horses/randomizedImage.png"));
            File.Delete(Globals.GetFilePath("Assets/CustomImages/Animals/Pets/randomizedImage.png"));
        }
    }
}
