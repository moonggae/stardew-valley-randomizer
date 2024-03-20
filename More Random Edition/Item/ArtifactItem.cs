namespace Randomizer
{
	/// <summary>
	/// Represents all artifacts
	/// </summary>
	public class ArtifactItem : Item
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="index">The index of the item</param>
		/// <param name="difficultyToObtain">The difficulty to obtain this artifact - defaults to UncommonItem</param>
		public ArtifactItem(ObjectIndexes index,
			ObtainingDifficulties difficultyToObtain = ObtainingDifficulties.UncommonItem) 
			: base(index)
		{
			DifficultyToObtain = difficultyToObtain;
			IsArtifact = true;
		}
	}
}
