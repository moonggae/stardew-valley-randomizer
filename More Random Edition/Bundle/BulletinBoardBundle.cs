using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
	public class BulletinBoardBundle : Bundle
	{
		public static List<BundleTypes> RoomBundleTypes { get; set; }

		/// <summary>
		/// Populates the bundle with the name, required items, minimum required, and color
		/// </summary>
		protected override void Populate()
		{
			BundleType = Globals.RNGGetAndRemoveRandomValueFromList(RoomBundleTypes);
			List<RequiredItem> potentialItems = new List<RequiredItem>();

			switch (BundleType)
			{
				case BundleTypes.BulletinNews:
					SetBundleName("bundle-bulletin-news");
					potentialItems = new List<RequiredItem>()
					{
						new RequiredItem((int)ObjectIndexes.SoggyNewspaper),
						new RequiredItem((int)ObjectIndexes.SoggyNewspaper),
						new RequiredItem((int)ObjectIndexes.SoggyNewspaper),
						new RequiredItem((int)ObjectIndexes.SoggyNewspaper),
						new RequiredItem((int)ObjectIndexes.SoggyNewspaper),
						new RequiredItem((int)ObjectIndexes.SoggyNewspaper),
						new RequiredItem((int)ObjectIndexes.SoggyNewspaper),
						new RequiredItem((int)ObjectIndexes.SoggyNewspaper),
					};
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, Range.GetRandomValue(1, 8));
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BulletinCleanup:
					SetBundleName("bundle-bulletin-cleanup");
					RequiredItems = RequiredItem.CreateList(ItemList.GetTrash(), 1, 5);
					Color = BundleColors.Green;
					break;
				case BundleTypes.BulletinHated:
					SetBundleName("bundle-bulletin-hated"); 
					potentialItems = RequiredItem.CreateList(
						PreferenceRandomizer.GetUniversalPreferences(UniversalPreferencesIndexes.Hated));
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Range.GetRandomValue(4, 6);
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinLoved:
					SetBundleName("bundle-bulletin-loved");
					potentialItems = RequiredItems = RequiredItem.CreateList(
						PreferenceRandomizer.GetUniversalPreferences(UniversalPreferencesIndexes.Loved));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
                    MinimumRequiredItems = Range.GetRandomValue(2, 6);
                    Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinAbigail:
					SetBundleName("Abigail-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Abigail));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BulletinAlex:
					SetBundleName("Alex-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Alex));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Green;
					break;
				case BundleTypes.BulletinCaroline:
					SetBundleName("Caroline-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Caroline));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinClint:
					SetBundleName("Clint-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Clint));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BulletinDwarf:
					SetBundleName("Dwarf-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Dwarf));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BulletinDemetrius:
					SetBundleName("Demetrius-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Demetrius));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinElliott:
					SetBundleName("Elliott-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Elliott));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinEmily:
					SetBundleName("Emily-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Emily));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinEvelyn:
					SetBundleName("Evelyn-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Evelyn));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinGeorge:
					SetBundleName("George-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.George));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Green;
					break;
				case BundleTypes.BulletinGus:
					SetBundleName("Gus-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Gus));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BulletinHaley:
					SetBundleName("Haley-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Haley));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinHarvey:
					SetBundleName("Harvey-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Harvey));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Green;
					break;
				case BundleTypes.BulletinJas:
					SetBundleName("Jas-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Jas));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BulletinJodi:
					SetBundleName("Jodi-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Jodi));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BulletinKent:
					SetBundleName("Kent-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Kent));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Green;
					break;
				case BundleTypes.BulletinKrobus:
					SetBundleName("Krobus-name");
					potentialItems = RequiredItem.CreateList(
						PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Krobus));
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinLeah:
					SetBundleName("Leah-name");
					potentialItems = RequiredItem.CreateList(
						PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Leah));
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Green;
					break;
				case BundleTypes.BulletinLewis:
					SetBundleName("Lewis-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Lewis));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Green;
					break;
				case BundleTypes.BulletinLinus:
					SetBundleName("Linus-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Linus));
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BulletinMarnie:
					SetBundleName("Marnie-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Marnie));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BulletinMaru:
					SetBundleName("Maru-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Maru));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BulletinPam:
					SetBundleName("Pam-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Pam));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BulletinPenny:
					SetBundleName("Penny-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Penny));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Yellow;
					break;
				case BundleTypes.BulletinPierre:
					SetBundleName("Pierre-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Pierre));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BulletinRobin:
					SetBundleName("Robin-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Robin));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Yellow;
					break;
				case BundleTypes.BulletinSam:
					SetBundleName("Sam-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Sam));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinSandy:
					SetBundleName("Sandy-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Sandy));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinSebastian:
					SetBundleName("Sebastian-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Sebastian));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BulletinShane:
					SetBundleName("Shane-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Shane));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinVincent:
					SetBundleName("Vincent-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Vincent));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinWilly:
					SetBundleName("Willy-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Willy));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinWizard:
					SetBundleName("Wizard-name");
					potentialItems = RequiredItem.CreateList(
                        PreferenceRandomizer.GetLovedItems(GiftableNPCIndexes.Wizard));
                    RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8, forceNumberOfValuesRNGCalls: true);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BulletinColorPink:
					SetBundleName("bundle-bulletin-pink");
					potentialItems = RequiredItem.CreateList(new List<int>
					{
						(int)ObjectIndexes.Shrimp,
						(int)ObjectIndexes.StrangeBun,
						(int)ObjectIndexes.SalmonDinner,
						(int)ObjectIndexes.PinkCake,
						(int)ObjectIndexes.Sashimi,
						(int)ObjectIndexes.IceCream,
						(int)ObjectIndexes.Salmonberry,
						(int)ObjectIndexes.Coral,
						(int)ObjectIndexes.Dolomite,
						(int)ObjectIndexes.Nekoite,
						(int)ObjectIndexes.StarShards,
						(int)ObjectIndexes.Peach,
						(int)ObjectIndexes.BugMeat,
						(int)ObjectIndexes.Bait
					});
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Range.GetRandomValue(3, 6);
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinColorWhite:
					SetBundleName("bundle-bulletin-white");
					potentialItems = RequiredItem.CreateList(new List<int>
					{
						(int)ObjectIndexes.Leek,
						(int)ObjectIndexes.Quartz,
						(int)ObjectIndexes.OrnamentalFan,
						(int)ObjectIndexes.DinosaurEgg,
						(int)ObjectIndexes.ChickenStatue,
						(int)ObjectIndexes.WhiteAlgae,
						(int)ObjectIndexes.WhiteEgg,
						(int)ObjectIndexes.LargeWhiteEgg,
						(int)ObjectIndexes.Milk,
						(int)ObjectIndexes.LargeMilk,
						(int)ObjectIndexes.FriedEgg,
						(int)ObjectIndexes.RicePudding,
						(int)ObjectIndexes.IceCream,
						(int)ObjectIndexes.Mayonnaise,
						(int)ObjectIndexes.IronBar,
						(int)ObjectIndexes.RefinedQuartz,
						(int)ObjectIndexes.IronOre,
						(int)ObjectIndexes.SpringOnion,
						(int)ObjectIndexes.SnowYam,
						(int)ObjectIndexes.Rice,
						(int)ObjectIndexes.GoatCheese,
						(int)ObjectIndexes.Cloth,
						(int)ObjectIndexes.GoatMilk,
						(int)ObjectIndexes.LargeGoatMilk,
						(int)ObjectIndexes.Wool,
						(int)ObjectIndexes.DuckEgg,
						(int)ObjectIndexes.RabbitsFoot,
						(int)ObjectIndexes.PaleBroth,
						(int)ObjectIndexes.Esperite,
						(int)ObjectIndexes.Lunarite,
						(int)ObjectIndexes.Marble,
						(int)ObjectIndexes.PrehistoricScapula,
						(int)ObjectIndexes.PrehistoricTibia,
						(int)ObjectIndexes.PrehistoricSkull,
						(int)ObjectIndexes.SkeletalHand,
						(int)ObjectIndexes.PrehistoricRib,
						(int)ObjectIndexes.PrehistoricVertebra,
						(int)ObjectIndexes.SkeletalTail,
						(int)ObjectIndexes.NautilusFossil,
						(int)ObjectIndexes.Trilobite,
						(int)ObjectIndexes.ArtichokeDip,
						(int)ObjectIndexes.LeadBobber,
						(int)ObjectIndexes.Chowder
					});
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Range.GetRandomValue(3, 6);
					Color = BundleColors.Yellow;
					break;
			}
		}

		/// <summary>
		/// Generates the reward for the bundle
		/// </summary>
		protected override void GenerateReward()
		{
			if (Globals.RNGGetNextBoolean(1))
			{
				Reward = new RequiredItem((int)ObjectIndexes.PrismaticShard);
			}

			else if (Globals.RNGGetNextBoolean(5))
			{
				// The idea is that we want a GOOD reward, so use the original data in case the
				// generated love list is really bad
				List<Item> universalLoves = PreferenceRandomizer
					.GetUniversalPreferences(UniversalPreferencesIndexes.Loved, forceOriginalData: true)
					.Where(x => x.Id != (int)ObjectIndexes.PrismaticShard)
					.ToList();

				Reward = Globals.RNGGetRandomValueFromList(RequiredItem.CreateList(universalLoves, 5, 10));
			}

			List<RequiredItem> potentialRewards = new()
			{
				new((int)ObjectIndexes.JunimoKartArcadeSystem),
				new((int)ObjectIndexes.PrairieKingArcadeSystem),
				new((int)ObjectIndexes.SodaMachine),
				new((int)ObjectIndexes.Beer, 43),
				new((int)ObjectIndexes.Salad, Range.GetRandomValue(5, 25)),
				new((int)ObjectIndexes.Bread, Range.GetRandomValue(5, 25)),
				new((int)ObjectIndexes.Spaghetti, Range.GetRandomValue(5, 25)),
				new((int)ObjectIndexes.Pizza, Range.GetRandomValue(5, 25)),
				new((int)ObjectIndexes.Coffee, Range.GetRandomValue(5, 25))
			};

			Reward = Globals.RNGGetRandomValueFromList(potentialRewards);
		}
	}
}
