namespace Randomizer
{
    /// <summary>
    /// The indexes in the Data/ObjectInformation.xnb dictionary
    /// Add to this enum as these are used
    /// </summary>
    public enum ObjectInformationIndexes
    {
        Name = 0,
        Price = 1,
        DisplayName = 4,
        Description = 5,

        /// <summary>
        /// This is at the end of the object information - unsure if it's actually used anywhere
        /// See FishItem.ObjectInformationSuffix
        /// </summary>
        AdditionalFishInfo = 6
    }
}
