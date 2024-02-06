using StardewValley;
using StardewValley.GameData.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer
{
    /// <summary>
    /// Randomizes the building that Robin can build for you
    /// </summary>
    public class BuildingRandomizer
	{
        /// <summary>
        /// Randomize the buildings
        /// </summary>
        /// <returns>The dictionary to use to replace the assets</returns>
        public static Dictionary<string, BuildingData> Randomize()
		{
            Dictionary<string, BuildingData> buildingData = DataLoader.Buildings(Game1.content);
			Dictionary<string, BuildingData> buildingChanges = new();

            List<int> idsToDisallowForAnimalBuildings = ItemList.GetAnimalProducts().Select(x => x.Id).ToList();
			idsToDisallowForAnimalBuildings.AddRange(new List<int>
			{
				(int)ObjectIndexes.GreenSlimeEgg,
				(int)ObjectIndexes.BlueSlimeEgg,
				(int)ObjectIndexes.RedSlimeEgg,
				(int)ObjectIndexes.PurpleSlimeEgg
			});

			Item resource1, resource2;
			ItemAndMultiplier itemChoice;
			List<ItemAndMultiplier> buildingMaterials;

            List<Buildings> buildings = Enum.GetValues(typeof(Buildings)).Cast<Buildings>().ToList();
            foreach (Buildings buildingType in buildings)
			{
				resource1 = ItemList.GetRandomResourceItem();
				resource2 = ItemList.GetRandomResourceItem(new int[] { resource1.Id });

				switch (buildingType)
				{
					case Buildings.Silo:
						buildingMaterials = new List<ItemAndMultiplier>
                        {
                            new(resource1, Range.GetRandomValue(2, 3)),
                            new(resource2),
                            new(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements))
                        };
						RandomizeAndAddBuilding("Silo", buildingMaterials, buildingData, buildingChanges);
                        break;
					case Buildings.Mill:
						buildingMaterials = new List<ItemAndMultiplier>
						{
							new(resource1, 3),
							new(resource2, 2),
							new(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements))
						};
                        RandomizeAndAddBuilding("Mill", buildingMaterials, buildingData, buildingChanges);
                        break;
					case Buildings.ShippingBin:
						itemChoice = Globals.RNGGetRandomValueFromList(new List<ItemAndMultiplier>
						{
							new(resource1, 3),
							new(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements))
						});
                        buildingMaterials = new List<ItemAndMultiplier> { itemChoice };
                        RandomizeAndAddBuilding("Shipping Bin", buildingMaterials, buildingData, buildingChanges);
                        break;
					case Buildings.Coop:
						itemChoice = Globals.RNGGetRandomValueFromList(new List<ItemAndMultiplier>
						{
							new(resource1, 5),
							new(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements, idsToDisallowForAnimalBuildings.ToArray()))
						});
						buildingMaterials = new List<ItemAndMultiplier>
						{
							itemChoice,
							new(resource2, Range.GetRandomValue(2, 3))
						};
                        RandomizeAndAddBuilding("Coop", buildingMaterials, buildingData, buildingChanges);
                        break;
					case Buildings.BigCoop:
						itemChoice = Globals.RNGGetRandomValueFromList(new List<ItemAndMultiplier>
						{
							new(resource1, 3),
							new(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements, idsToDisallowForAnimalBuildings.ToArray()))
						});
						buildingMaterials = new List<ItemAndMultiplier>
						{
							itemChoice,
							new(resource2, 7)
						};
                        RandomizeAndAddBuilding("Big Coop", buildingMaterials, buildingData, buildingChanges);
                        break;
					case Buildings.DeluxeCoop:
						itemChoice = Globals.RNGGetRandomValueFromList(new List<ItemAndMultiplier>
						{
							new(resource1, 9),
							new(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.LargeTimeRequirements, idsToDisallowForAnimalBuildings.ToArray()))
						});
						buildingMaterials = new List<ItemAndMultiplier>
						{
							itemChoice,
							new(resource2, 4)
						};
                        RandomizeAndAddBuilding("Deluxe Coop", buildingMaterials, buildingData, buildingChanges);
                        break;
					case Buildings.Barn:
						itemChoice = Globals.RNGGetRandomValueFromList(new List<ItemAndMultiplier>
						{
							new(resource1, 5),
							new(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements, idsToDisallowForAnimalBuildings.ToArray()))
						});
						buildingMaterials = new List<ItemAndMultiplier>
						{
							itemChoice,
							new(resource2, Range.GetRandomValue(2, 3))
						};
                        RandomizeAndAddBuilding("Barn", buildingMaterials, buildingData, buildingChanges);
                        break;
					case Buildings.BigBarn:
						itemChoice = Globals.RNGGetRandomValueFromList(new List<ItemAndMultiplier>
						{
							new(resource1, 3),
							new(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements, idsToDisallowForAnimalBuildings.ToArray()))
						});
                        buildingMaterials = new List<ItemAndMultiplier>
						{
							itemChoice,
							new(resource2, 7)
						};
                        RandomizeAndAddBuilding("Big Barn", buildingMaterials, buildingData, buildingChanges);
                        break;
					case Buildings.DeluxeBarn:
						itemChoice = Globals.RNGGetRandomValueFromList(new List<ItemAndMultiplier>
						{
							new(resource1, 9),
							new(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.LargeTimeRequirements, idsToDisallowForAnimalBuildings.ToArray()))
						});
                        buildingMaterials = new List<ItemAndMultiplier>
						{
							itemChoice,
							new(resource2, 4)
						};
                        RandomizeAndAddBuilding("Deluxe Barn", buildingMaterials, buildingData, buildingChanges);
                        break;
					case Buildings.SlimeHutch:
						buildingMaterials = new List<ItemAndMultiplier>
						{
							new(resource1, 9),
							new(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements), 2),
							new(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.LargeTimeRequirements))
						};
                        RandomizeAndAddBuilding("Slime Hutch", buildingMaterials, buildingData, buildingChanges);
                        break;
					case Buildings.Shed:
                        buildingMaterials = Globals.RNGGetRandomValueFromList(new List<List<ItemAndMultiplier>> {
							new() { new ItemAndMultiplier(resource1, 5) },
							new() {
								new ItemAndMultiplier(resource1, 3),
								new ItemAndMultiplier(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements))
							}
						});
                        RandomizeAndAddBuilding("Shed", buildingMaterials, buildingData, buildingChanges);
                        break;
					case Buildings.Cabin:
                        RandomizeAndAddBuilding("Cabin", GetRequiredItemsForCabin(), buildingData, buildingChanges);
                        break;
					case Buildings.Well:
						itemChoice = Globals.RNGGetRandomValueFromList(new List<ItemAndMultiplier>
						{
							new(resource1, 3),
							new(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements))
						});
                        buildingMaterials = new List<ItemAndMultiplier> { itemChoice };
                        RandomizeAndAddBuilding("Well", buildingMaterials, buildingData, buildingChanges);
                        break;
					case Buildings.FishPond:
                        buildingMaterials = new List<ItemAndMultiplier>
						{
							new(resource1, 2),
							new(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.SmallTimeRequirements), 2),
							new(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.SmallTimeRequirements), 2)
						};
                        RandomizeAndAddBuilding("Fish Pond", buildingMaterials, buildingData, buildingChanges);
                        break;
					case Buildings.Stable:
                        buildingMaterials = new List<ItemAndMultiplier>
						{
							new(ItemList.GetRandomItemAtDifficulty(ObtainingDifficulties.MediumTimeRequirements), 2),
							new(resource1, 8),
						};
                        RandomizeAndAddBuilding("Stable", buildingMaterials, buildingData, buildingChanges);
                        break;
					default:
						Globals.ConsoleError($"Unhandled building: {buildingType}");
						continue;
				}
			}

			WriteToSpoilerLog(buildingChanges);
			return buildingChanges;
		}

		/// <summary>
		/// Randomizes the build cost and assigns the given build materials to the building
		/// </summary>
		/// <param name="buildingKey">The key of the building</param>
		/// <param name="buildMaterials">The materials to assign the building</param>
		/// <param name="buildingData">The original building data</param>
		/// <param name="buildingChanges">Our dictionary of building changes</param>
		private static void RandomizeAndAddBuilding(
			string buildingKey, 
			List<ItemAndMultiplier> buildMaterials,
            Dictionary<string, BuildingData> buildingData,
            Dictionary<string, BuildingData> buildingChanges)
		{
            BuildingData currentBuilding = buildingData[buildingKey];
            currentBuilding.BuildCost = ComputePrice(currentBuilding.BuildCost);
            currentBuilding.BuildMaterials = ComputeBuildMaterials(buildMaterials);

			// Cabins specifically have skins that need to be randomized as well
			if (buildingKey == "Cabin")
			{
				currentBuilding.Skins.ForEach(skin => 
					skin.BuildMaterials = ComputeBuildMaterials(GetRequiredItemsForCabin()));
			}

            buildingChanges.Add(buildingKey, currentBuilding);
        }

		/// <summary>
		/// Gets the required items for a cabin - applies to any cabin
		/// </summary>
		/// <returns />
		private static List<ItemAndMultiplier> GetRequiredItemsForCabin()
		{
			Item resource = ItemList.GetRandomResourceItem(new int[(int)ObjectIndexes.Hardwood]);
			Item easyItem = Globals.RNGGetRandomValueFromList(
				ItemList.GetItemsBelowDifficulty(ObtainingDifficulties.MediumTimeRequirements, new List<int> { resource.Id })
			);

			return Globals.RNGGetRandomValueFromList(new List<List<ItemAndMultiplier>> {
				new() { new ItemAndMultiplier(resource, 2) },
				new() {
					new ItemAndMultiplier(resource),
					new ItemAndMultiplier(easyItem)
				}
			});
		}

        /// <summary>
        /// Computes the price based on the base money
        /// This is any value between the base money, plus or minus the money variable percentage
        /// </summary>
        /// <param name="baseMoneyRequired">The amount the building normally costs</param>
        /// <returns>The new price</returns>
        private static int ComputePrice(int baseMoneyRequired)
        {
            const double MoneyVariablePercentage = 0.25;
            int variableAmount = (int)(baseMoneyRequired * MoneyVariablePercentage);
            return Range.GetRandomValue(baseMoneyRequired - variableAmount, baseMoneyRequired + variableAmount);
        }

        /// <summary>
        /// Returns the build materials based on the items and their multipliers
        /// Sums up the amounts of items if they have the same id to prevent errors
        /// </summary>
        /// <param name="itemsRequired">The items this building requires</param>
		/// <returns>The list of BuildingMaterials to be used by the building</returns>
        private static List<BuildingMaterial> ComputeBuildMaterials(List<ItemAndMultiplier> itemsRequired)
        {
            Dictionary<int, RequiredItem> requiredItemsDict = new();
            foreach (ItemAndMultiplier itemAndMultiplier in itemsRequired)
            {
                RequiredItem requiredItem = new(itemAndMultiplier.Item, itemAndMultiplier.Amount);
                int reqiredItemId = requiredItem.Item.Id;
                if (requiredItemsDict.ContainsKey(reqiredItemId))
                {
                    requiredItemsDict[reqiredItemId].NumberOfItems += requiredItem.NumberOfItems;
                }
                else
                {
                    requiredItemsDict.Add(reqiredItemId, requiredItem);
                }
            }

			List<BuildingMaterial> buildingMaterials = new();
			foreach(RequiredItem buildingItem in requiredItemsDict.Values)
			{
				buildingMaterials.Add(
					new BuildingMaterial
					{
						ItemId = buildingItem.Item.QualifiedId,
						Amount = buildingItem.NumberOfItems
					}
				);
			}

			return buildingMaterials;
        }

        /// <summary>
        /// Writes the buildings to the spoiler log
        /// </summary>
        /// <param name="buildingChanges">Info about the changed buildings</param>
        private static void WriteToSpoilerLog(Dictionary<string, BuildingData> buildingChanges)
		{
			if (!Globals.Config.RandomizeBuildingCosts) { return; }

			Globals.SpoilerWrite("==== BUILDINGS ====");
			foreach (var buildingData in buildingChanges)
			{
				string buildingName = buildingData.Key;
				BuildingData building = buildingData.Value;

				Globals.SpoilerWrite($"{buildingName} - {building.BuildCost}G");
				Globals.SpoilerWrite(GetRequiredItemsSpoilerString(building.BuildMaterials));
				Globals.SpoilerWrite("===");
			}
			Globals.SpoilerWrite("");
		}

        /// <summary>
        /// Gets the required items string for use in the spoiler log
        /// </summary>
        /// <param name="building">The building</param>
        /// <returns />
        private static string GetRequiredItemsSpoilerString(List<BuildingMaterial> buildingMaterials)
        {
			return string.Join(" - ", 
				buildingMaterials.Select(material =>
					$"{ItemList.GetItemFromStringId(material.Id).Name}: {material.Amount}"));
        }
    }

    /// <summary>
    /// An item and how much to multiply the amount required by
    /// </summary>
    public class ItemAndMultiplier
    {
        public Item Item { get; set; }
        public int Multiplier { get; set; }

        public ItemAndMultiplier(Item item, int multiplier = 1)
        {
            Item = item;
            Multiplier = multiplier;
        }

        /// <summary>
        /// The number of items - note that this will NOT return the same value each time it's called!
        /// </summary>
        public int Amount
        {
            get
            {
                return Item.GetAmountRequiredForCrafting() * Multiplier;
            }
        }
    }
}
