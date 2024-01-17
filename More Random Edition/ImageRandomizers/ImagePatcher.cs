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

        /// <summary>
        /// The assets folder name
        /// </summary>
        protected const string AssetsFolder = "Assets";

        /// <summary>
        /// The sub folder to use as the root for this patcher - located after Assets
        /// </summary>
        protected string SubFolder { get; set; }

        /// <summary>
        /// The folder to use for this patcher - equivalent to
        /// Assets/<SubFolder>
        /// </summary>
        protected string PatcherImageFolder => $"{AssetsFolder}/{SubFolder}";

        abstract public void OnAssetRequested(IAssetData asset);
    }
}