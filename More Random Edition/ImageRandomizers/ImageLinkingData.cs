namespace Randomizer
{
    /// <summary>
    /// Linking data to be used by the image builders to link data together
    /// Currently used by the SpringObjects and CropGrowth image builders so that the images get manipulated 
    /// in the same way (matching crop/seed/plant graphics, etc)
    /// </summary>
    public class ImageLinkingData
    {
        /// <summary>
        /// What image was just selected to be pasted into the base image
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// What value was set for the hue shift - used to make the linked images get the same value
        /// </summary>
        public int HueShiftValue { get; set; }

        public ImageLinkingData(string imageName)
        {
            ImageName = imageName;
        }
    }
}
