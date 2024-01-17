using StardewModdingAPI;

namespace Randomizer
{
    public abstract class ImagePatcher
    {
        /// <summary>
        /// The image width in px - used when determining whether to crop and when drawing the image itself
        /// </summary>
        protected int ImageWidthInPx = 16;

        /// <summary>
        /// The image height in px - used when determining whether to crop and when drawing the image itself
        /// </summary>
        protected int ImageHeightInPx = 16;

        protected const string AssetsFolder = "Assets";
        protected string SubFolder { get; set; }
        protected string PatcherImageFolder => $"{AssetsFolder}/{SubFolder}";
        protected string StardewAssetPath { get; set; }

        abstract public void OnAssetRequested(IAssetData asset);
    }
}