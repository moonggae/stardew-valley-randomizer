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
					potentialItems = RequiredItem.CreateList(NPC.UniversalHates);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Range.GetRandomValue(4, 6);
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinLoved:
					SetBundleName("bundle-bulletin-loved");
					RequiredItems = RequiredItem.CreateList(NPC.UniversalLoves);
					MinimumRequiredItems = 2;
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinAbigail:
					SetBundleName("Abigail-name");
					potentialItems = RequiredItem.CreateList(Abigail.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BulletinAlex:
					SetBundleName("Alex-name");
					potentialItems = RequiredItem.CreateList(Alex.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Green;
					break;
				case BundleTypes.BulletinCaroline:
					SetBundleName("Caroline-name");
					potentialItems = RequiredItem.CreateList(Caroline.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinClint:
					SetBundleName("Clint-name");
					potentialItems = RequiredItem.CreateList(Clint.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BulletinDwarf:
					SetBundleName("Dwarf-name");
					potentialItems = RequiredItem.CreateList(Dwarf.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BulletinDemetrius:
					SetBundleName("Demetrius-name");
					potentialItems = RequiredItem.CreateList(Demetrius.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinElliott:
					SetBundleName("Elliott-name");
					potentialItems = RequiredItem.CreateList(Elliott.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinEmily:
					SetBundleName("Emily-name");
					potentialItems = RequiredItem.CreateList(Emily.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinEvelyn:
					SetBundleName("Evelyn-name");
					potentialItems = RequiredItem.CreateList(Evelyn.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinGeorge:
					SetBundleName("George-name");
					potentialItems = RequiredItem.CreateList(George.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Green;
					break;
				case BundleTypes.BulletinGus:
					SetBundleName("Gus-name");
					potentialItems = RequiredItem.CreateList(Gus.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BulletinHaley:
					SetBundleName("Haley-name");
					potentialItems = RequiredItem.CreateList(Haley.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinHarvey:
					SetBundleName("Harvey-name");
					potentialItems = RequiredItem.CreateList(Harvey.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Green;
					break;
				case BundleTypes.BulletinJas:
					SetBundleName("Jas-name");
					potentialItems = RequiredItem.CreateList(Jas.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BulletinJodi:
					SetBundleName("Jodi-name");
					potentialItems = RequiredItem.CreateList(Jodi.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BulletinKent:
					SetBundleName("Kent-name");
					potentialItems = RequiredItem.CreateList(Kent.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Green;
					break;
				case BundleTypes.BulletinKrobus:
					SetBundleName("Krobus-name");
					potentialItems = RequiredItem.CreateList(Krobus.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinLeah:
					SetBundleName("Leah-name");
					potentialItems = RequiredItem.CreateList(Leah.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Green;
					break;
				case BundleTypes.BulletinLewis:
					SetBundleName("Lewis-name");
					potentialItems = RequiredItem.CreateList(Lewis.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Green;
					break;
				case BundleTypes.BulletinLinus:
					SetBundleName("Linus-name");
					potentialItems = RequiredItem.CreateList(Linus.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BulletinMarnie:
					SetBundleName("Marnie-name");
					potentialItems = RequiredItem.CreateList(Marnie.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BulletinMaru:
					SetBundleName("Maru-name");
					potentialItems = RequiredItem.CreateList(Maru.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BulletinPam:
					SetBundleName("Pam-name");
					potentialItems = RequiredItem.CreateList(Pam.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BulletinPenny:
					SetBundleName("Penny-name");
					potentialItems = RequiredItem.CreateList(Penny.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Yellow;
					break;
				case BundleTypes.BulletinPierre:
					SetBundleName("Pierre-name");
					potentialItems = RequiredItem.CreateList(Pierre.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Orange;
					break;
				case BundleTypes.BulletinRobin:
					SetBundleName("Robin-name");
					potentialItems = RequiredItem.CreateList(Robin.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Yellow;
					break;
				case BundleTypes.BulletinSam:
					SetBundleName("Sam-name");
					potentialItems = RequiredItem.CreateList(Sam.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinSandy:
					SetBundleName("Sandy-name");
					potentialItems = RequiredItem.CreateList(Sandy.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinSebastian:
					SetBundleName("Sebastian-name");
					potentialItems = RequiredItem.CreateList(Sebastian.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Purple;
					break;
				case BundleTypes.BulletinShane:
					SetBundleName("Shane-name");
					potentialItems = RequiredItem.CreateList(Shane.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Blue;
					break;
				case BundleTypes.BulletinVincent:
					SetBundleName("Vincent-name");
					potentialItems = RequiredItem.CreateList(Vincent.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinWilly:
					SetBundleName("Willy-name");
					potentialItems = RequiredItem.CreateList(Willy.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
					MinimumRequiredItems = Math.Min(Math.Max(RequiredItems.Count - 2, 3), RequiredItems.Count);
					Color = BundleColors.Red;
					break;
				case BundleTypes.BulletinWizard:
					SetBundleName("Wizard-name");
					potentialItems = RequiredItem.CreateList(Wizard.Loves);
					RequiredItems = Globals.RNGGetRandomValuesFromList(potentialItems, 8);
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
				List<Item> universalLoves = NPC.UniversalLoves.Where(x =>
					x.Id != (int)ObjectIndexes.PrismaticShard).ToList();

				Reward = Globals.RNGGetRandomValueFromList(RequiredItem.CreateList(universalLoves, 5, 10));
			}

			List<RequiredItem> potentialRewards = new List<RequiredItem>
			{
				new RequiredItem((int)ObjectIndexes.JunimoKartArcadeSystem),
				new RequiredItem((int)ObjectIndexes.PrairieKingArcadeSystem),
				new RequiredItem((int)ObjectIndexes.SodaMachine),
				new RequiredItem((int)ObjectIndexes.Beer, 43),
				new RequiredItem((int)ObjectIndexes.Salad, Range.GetRandomValue(5, 25)),
				new RequiredItem((int)ObjectIndexes.Bread, Range.GetRandomValue(5, 25)),
				new RequiredItem((int)ObjectIndexes.Spaghetti, Range.GetRandomValue(5, 25)),
				new RequiredItem((int)ObjectIndexes.Pizza, Range.GetRandomValue(5, 25)),
				new RequiredItem((int)ObjectIndexes.Coffee, Range.GetRandomValue(5, 25))
			};

			Reward = Globals.RNGGetRandomValueFromList(potentialRewards);
		}
	}
}
