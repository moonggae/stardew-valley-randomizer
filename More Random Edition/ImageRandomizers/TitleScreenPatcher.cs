using StardewModdingAPI;

namespace Randomizer
{
    /// <summary>
    /// Modifies the title screen with our randomizer graphics
    /// </summary>
    public class TitleScreenPatcher : ImagePatcher
    {
        public const string StardewAssetPath = "Minigames/TitleButtons";

        public TitleScreenPatcher()
        {
            SubFolder = "Minigames";
        }

        /// <summary>
        /// Called when the asset is requested
        /// Patches the image into the game
        /// </summary>
        /// <param name="asset">The equivalent asset from Stardew to modify</param>
        public override void OnAssetRequested(IAssetData asset)
        {
            var editor = asset.AsImage();
            IRawTextureData overlay = Globals.ModRef.Helper.ModContent
                .Load<IRawTextureData>(GetCustomAssetPath());

            editor.PatchImage(overlay);
        }

        /// <summary>
        /// Gets the pet icon to replace
        /// </summary>
        /// <returns>The full path of the icon, starting at the root of the mod</returns>
        public string GetCustomAssetPath()
        {
            return $"{PatcherImageFolder}/{Globals.GetLocalizedFileName("TitleButtons", "png")}";
        }
    }
}
