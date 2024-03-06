using System.Collections.Generic;

namespace Randomizer
{
	public class JojaBundle : Bundle
	{
		public static List<BundleTypes> RoomBundleTypes { get; set; }

		/// <summary>
		/// Creates a bundle for the Joja mart
		/// </summary>
		protected override void Populate()
		{
			RNG rng = BundleRandomizer.Rng;

			BundleType = rng.GetAndRemoveRandomValueFromList(RoomBundleTypes);

			switch (BundleType)
			{
				case BundleTypes.JojaMissing:
					SetBundleName("bundle-joja-missing");

					RequiredItems = new List<RequiredBundleItem>
					{
						new(ItemList.GetRandomItemAtDifficulty(rng, ObtainingDifficulties.EndgameItem)),
						new(ItemList.GetRandomItemAtDifficulty(rng, ObtainingDifficulties.RareItem)),
						new(ItemList.GetRandomItemAtDifficulty(rng, ObtainingDifficulties.LargeTimeRequirements)),
						new(ItemList.GetRandomItemAtDifficulty(rng, ObtainingDifficulties.MediumTimeRequirements)),
						new(
							rng.GetRandomValueFromList(
								ItemList.GetItemsBelowDifficulty(
									ObtainingDifficulties.Impossible,
									new List<int> { (int)ObjectIndexes.AnyFish }))
						)
					};
					MinimumRequiredItems = 5;
					Color = BundleColors.Blue;
					break;
			}
		}

		/// <summary>
		/// There actually no item reward for the Joja bundle, so leaving blank
		/// </summary>
		protected override void GenerateReward()
		{
		}
	}
}
