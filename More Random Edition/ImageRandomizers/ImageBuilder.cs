using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Randomizer
{
	public abstract class ImageBuilder
	{
		/// <summary>
		/// The size of the width in px
		/// </summary>
		protected int WidthInPx = 16;

		/// <summary>
		/// The size of the height in px
		/// </summary>
		protected int HeightInPx = 16;

		/// <summary>
		/// The name of the output file
		/// </summary>
		private const string OutputFileName = "randomizedImage.png";

		/// <summary>
		/// The path to the custom images
		/// </summary>
		protected string ImageDirectory
		{
			get
			{
				return $"Mods/Randomizer/Assets/CustomImages/{SubDirectory}";
			}
		}

		/// <summary>
		/// The directory to place custom images
		/// If this is not assigned, will use ImageDirectory instead
		/// </summary>
		protected string CustomImageDirectory;


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
		/// The name of the default image
		/// </summary>
		protected const string DefaultFileName = "default.png";

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
		/// Builds the image and saves the result into randomizedImage.png
		/// </summary>
		public void BuildImage()
		{
			BuildImage(new List<ImageBuilder> { this }, BaseFileFullPath, OutputFileFullPath);
		}

		/// <summary>
		/// Builds the image and saves the result into randomizedImage.png
		/// </summary>
		/// <param name="imageBuilders">
		/// A list of image builders - will write to the same file and save it one time
		/// at the end so that all image builders can do their work on the same image
		/// </param>
		/// <param name="imageBuilders">The image builders to use</param>
		/// <param name="baseFileFullPath">The full path to the base file we're using</param>
		/// <param name="outputPath">The full path to the output file we're writing to</param>
		public static void BuildImage(List<ImageBuilder> imageBuilders, string baseFileFullPath, string outputPath)
		{
			Bitmap finalImage = null;

			try
			{
				finalImage = new Bitmap(baseFileFullPath);
				Graphics graphics = Graphics.FromImage(finalImage);

				foreach (ImageBuilder imageBuilder in imageBuilders)
				{
					imageBuilder.FilesToPullFrom = imageBuilder.GetAllCustomImages();
					foreach (Point position in imageBuilder.PositionsToOverlay)
					{
						string randomFileName = imageBuilder.GetRandomFileName(position);
						if (!imageBuilder.ShouldSaveImage()) { continue; }

						Bitmap bitmap = new Bitmap(randomFileName);
						int xOffset = position.X * imageBuilder.WidthInPx;
						int yOffset = position.Y * imageBuilder.HeightInPx;

						graphics.FillRectangle(
							new SolidBrush(Color.FromArgb(0, 0, 1)),
							new Rectangle(xOffset, yOffset, imageBuilder.WidthInPx, imageBuilder.HeightInPx));
						graphics.DrawImage(bitmap, new Rectangle(xOffset, yOffset, imageBuilder.WidthInPx, imageBuilder.HeightInPx));
					}
				}

				finalImage.MakeTransparent(Color.FromArgb(0, 0, 1));
				if (imageBuilders.Any(x => x.ShouldSaveImage()))
				{
					finalImage.Save(outputPath);
				}
			}
			catch (Exception ex)
			{
				if (finalImage != null)
				{
					finalImage.Dispose();
				}
				throw ex;
			}
		}

		/// <summary>
		/// Gets all the custom images from the given directory excluding the base file name
		/// </summary>
		/// <returns></returns>
		private List<string> GetAllCustomImages()
		{
			string directory = string.IsNullOrEmpty(CustomImageDirectory) ? ImageDirectory : CustomImageDirectory;
			List<string> files = Directory.GetFiles(directory).ToList();
			return files.Where(x =>
				!x.EndsWith(DefaultFileName) &&
				!x.EndsWith(OutputFileName) &&
				!x.EndsWith(BaseFileName) &&
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
			string directory = string.IsNullOrEmpty(CustomImageDirectory) ? ImageDirectory : CustomImageDirectory;
			string fileName = Globals.RNGGetAndRemoveRandomValueFromList(FilesToPullFrom);

			if (string.IsNullOrEmpty(fileName))
			{
				Globals.ConsoleWarn($"Not enough images at directory (need more images, using default image): {directory}");
				return $"{directory}/default.png";
			}

			return fileName;
		}

		/// <summary>
		/// Whether we should actually save the image file, or if the setting is off
		/// </summary>
		/// <returns />
		public abstract bool ShouldSaveImage();

		/// <summary>
		/// Cleans up all replacement files
		/// Should be called when the game is first loaded
		/// </summary>
		public static void CleanUpReplacementFiles()
		{
			File.Delete($"Mods/Randomizer/Assets/CustomImages/Weapons/randomizedImage.png");
			File.Delete($"Mods/Randomizer/Assets/CustomImages/Shared/randomizedImage.png");
			File.Delete($"Mods/Randomizer/Assets/CustomImages/CropGrowth/randomizedImage.png");
		}
	}
}
