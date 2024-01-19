namespace Randomizer
{
    /// <summary>
    /// The indexes in the Data/CraftingRecipes.xnb dictionary
    /// Through they are both here, this is only for the crafted items, not cooked recipes
    /// Add to this enum as these are used
    /// </summary>
    public enum CraftingRecipeIndexes
    {
        Ingredients = 0,

        // e.g. "Farming 2" means farming level 2,
        // Can have another prefix, like "s Farming 2", so split and grab the indices from the end of the array instead
        UnlockConditions = 4, 
        DisplayName = 5,
    }
}
