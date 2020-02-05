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
		/// The size of the width/height in px
		/// </summary>
		protected const int SizeInPx = 16;

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
		/// The path to the custom images
		/// </summary>
		protected string BaseFileFullPath
		{
			get
			{
				return $"{ImageDirectory}/{BaseFileName}";
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

		public void BuildImage()
		{
			Bitmap finalImage = null;

			try
			{
				finalImage = new Bitmap(BaseFileFullPath);
				Graphics graphics = Graphics.FromImage(finalImage);

				List<string> filesToPullFrom = GetAllCustomImages();
				foreach (Point position in PositionsToOverlay)
				{
					string randomFileName = Globals.RNGGetAndRemoveRandomValueFromList(filesToPullFrom);
					Bitmap bitmap = new Bitmap(randomFileName);
					int xOffset = position.X * SizeInPx;
					int yOffset = position.Y * SizeInPx;
					graphics.DrawImage(bitmap, new Rectangle(yOffset, xOffset, SizeInPx, SizeInPx));
				}

				finalImage.Save($"{ImageDirectory}/randomizedImage.png");
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

		private List<string> GetAllCustomImages()
		{
			List<string> files = Directory.GetFiles(ImageDirectory).ToList();
			return files.Where(x => !x.Contains(BaseFileName)).ToList();
		}
	}
}
