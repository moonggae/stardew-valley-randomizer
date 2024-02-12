using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SVObject = StardewValley.Object;
using SVRing = StardewValley.Objects.Ring;

namespace Randomizer
{
	/// <summary>
	/// Represents an item in the game
	/// </summary>
	public class Item
    {
        public const string ObjectIdPrefix = "(O)";

        public int Id { get; }

        /// <summary>
		/// This is the QualifiedItemId in Stardew's code a prefix before the integer id
        /// BigCraftables (BC), 
		/// Boots (B), 
		/// Farmhouse Flooring (FL), 
		/// Furniture (F), 
		/// Hats (H), 
		/// Objects (O),
		/// Pants (P), 
		/// Shirts (S),
		/// Tools (T), 
		/// Wallpaper (WP),
		/// Weapons (W)
        /// </summary>
        public string QualifiedId { 
			get
			{
				string itemType = "O";
				if (IsRing)
				{
					itemType = "R";
                } 
				else if (IsBigCraftable)
				{
					itemType = "BC";
				}

				// We currently only define objects, rings, and big craftables here
				return $"({itemType}){Id}";
			} 
		}

		public string Name
		{
			get { return GetName(); }
		}
		public string DisplayName
		{
			get
			{
				if (!string.IsNullOrEmpty(OverrideName) || !string.IsNullOrEmpty(OverrideDisplayName))
				{
					bool isRandomizedCookedItem = Globals.Config.Crops.Randomize && IsCooked;
					bool isRandomizedCropOrSeedItem = Globals.Config.Crops.Randomize && (IsCrop || IsSeed);
					bool isRandomizedFishItem = Globals.Config.Fish.Randomize && IsFish;
					bool useOriginalName = isRandomizedCookedItem || isRandomizedCropOrSeedItem || isRandomizedFishItem;

					if (useOriginalName)
					{
						return Name;
					}
				}

				if (!string.IsNullOrEmpty(OverrideDisplayName))
				{
					return OverrideDisplayName;
				}

				return IsBigCraftable
					? Name
					: Game1.objectData[Id.ToString()].DisplayName;
            }
		}
		public string OverrideName { get; set; }
		public string OverrideDisplayName { get; set; } // Used in the xnb string if it is populated
		/// <summary>
		/// The Name field in the object data is the English name
		/// </summary>
		public string EnglishName
		{
			get
			{
				return IsBigCraftable
					? Name
					: Game1.objectData[Id.ToString()].Name;
            }
		}
		public bool ShouldBeForagable { get; set; }
		public bool IsForagable
		{
			get { return ShouldBeForagable; }
		}

		public bool IsTrash { get; set; }
		public bool IsCraftable { get; set; }
		public bool IsBigCraftable { get; set; }

		/// <summary>
		/// BigCraftables have no price, so we need to define our own
		/// Note that this is the price that we want to buy it at, so we need to split it in half when setting it as a value
		/// </summary>
		public int BigCraftablePrice { get; set; }

		public bool IsSmelted { get; set; }
		public bool IsAnimalProduct { get; set; }
		public bool IsMonsterItem { get; set; }
		public bool IsFish { get; set; }
		public bool IsArtifact { get; set; }
		public bool IsMayonaisse { get; set; }
		public bool IsGeodeMineral { get; set; }
		public bool IsCrabPotItem { get; set; }
		public bool IsCrop { get; set; }
		public virtual bool IsFlower { get; set; }
		public bool IsSeed { get; set; }
		public bool IsCooked { get; set; }
		public bool IsRing { get; set; }
		public bool IsFruit { get; set; }
		public bool IsTotem { get =>
            new List<int>() {
                (int)ObjectIndexes.WarpTotemFarm,
                (int)ObjectIndexes.WarpTotemBeach,
                (int)ObjectIndexes.WarpTotemMountains,
                (int)ObjectIndexes.WarpTotemDesert,
                (int)ObjectIndexes.RainTotem
            }.Contains(Id);
		}
		public bool RequiresOilMaker { get; set; }
		public bool RequiresBeehouse { get; set; }
		public bool RequiresKeg { get; set; }
		public bool CanStack { get; set; } = true;

		public bool IsResource { get; set; }
		public Range ItemsRequiredForRecipe { get; set; } = new Range(1, 1);
		public double RequiredItemMultiplier = 1;

		public string CoffeeIngredient { get; set; }

		/// <summary>
		/// The difficulty that this item is to obtain
		/// Will return values appropriate to foragable items - they are never impossible
		/// </summary>
		public ObtainingDifficulties DifficultyToObtain
		{
			get
			{
				if (_difficultyToObtain == ObtainingDifficulties.Impossible && IsForagable)
				{
					return ObtainingDifficulties.LargeTimeRequirements;
				}

				return _difficultyToObtain;
			}
			set
			{
				_difficultyToObtain = value;
			}
		}
		private ObtainingDifficulties _difficultyToObtain { get; set; } = ObtainingDifficulties.Impossible;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">The item ID</param>
		public Item(int id)
		{
			Id = id;
			CanStack = id >= -4 && !IsBigCraftable;
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">The item ID</param>
        /// <param name="id">Whether the item is a big craftable</param>
        /// <param name="difficultyToObtain">The difficulty to obtain this item</param>
        public Item(int id, ObtainingDifficulties difficultyToObtain, bool isBigCraftable = false)
		{
			Id = id;
			IsBigCraftable = isBigCraftable;
			DifficultyToObtain = difficultyToObtain;
			if (isBigCraftable) 
			{ 
				CanStack = false; 
			}
		}

        /// <summary>
        /// Returns whether the given qualified id is for a Stardew Valley object
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns>True if the given id is for a Stardew Valley object, false otherwise</returns>
        public static bool IsQualifiedIdForObject(string id)
        {
            return id.StartsWith(ObjectIdPrefix);
        }

        /// <summary>
        /// Returns the string version of this item to use in crafting recipes
        /// Will NOT return the same value each time this is called!
        /// </summary>
        /// <returns></returns>
        public string GetStringForCrafting()
		{
			return $"{Id} {GetAmountRequiredForCrafting()}";
		}

		/// <summary>
		/// Gets a randomly generated amount of this item required for a crafting recipe
		/// Will always return a value of at least 1
		/// </summary>
		/// <returns />
		public int GetAmountRequiredForCrafting()
		{
			int baseAmount = ItemsRequiredForRecipe.GetRandomValue();
			return Math.Max((int)(baseAmount * RequiredItemMultiplier), 1);
		}

		/// <summary>
		/// Gets the name of an item from
		/// </summary>
		/// <returns>
		/// Splits apart the name from the ObjectIndexes name - WildHorseradish -> Wild Horseradish
		/// Uses the override name if there is one and the item type in question actually has a new name
		/// </returns>
		private string GetName()
		{
            bool ignoreOverrideName =
				(!Globals.Config.Crops.Randomize && (IsCrop || IsSeed)) ||
				(!Globals.Config.Fish.Randomize && IsFish);

            if (!ignoreOverrideName && !string.IsNullOrEmpty(OverrideName))
            {
                return OverrideName;
            }

			string enumName = IsBigCraftable
				? ((BigCraftableIndexes)Id).ToString()
				: ((ObjectIndexes)Id).ToString();
			return Regex.Replace(enumName, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1").Trim();
		}

		/// <summary>
		/// Gets what a price for an item might be just based on its difficulty to obtain
		/// </summary>
		/// <param name="multiplier">The multiplier for the price - will add or subtract this as a percentage</param>
		/// <returns>The computed price</returns>
		public int GetPriceForObtainingDifficulty(double multiplier)
		{
			int basePrice;
			switch (DifficultyToObtain)
			{
				case ObtainingDifficulties.NoRequirements:
					basePrice = 1000;
					break;
				case ObtainingDifficulties.SmallTimeRequirements:
					basePrice = 5000;
					break;
				case ObtainingDifficulties.MediumTimeRequirements:
					basePrice = 7500;
					break;
				case ObtainingDifficulties.LargeTimeRequirements:
					basePrice = 10000;
					break;
				case ObtainingDifficulties.UncommonItem:
					basePrice = 2500;
					break;
				case ObtainingDifficulties.RareItem:
					basePrice = 20000;
					break;
				case ObtainingDifficulties.EndgameItem:
					basePrice = 20000;
					break;
				case ObtainingDifficulties.NonCraftingItem:
					basePrice = 5000;
					break;
				default:
					Globals.ConsoleError($"Tried to get a base price for an item with an unrecognized ObtainingDifficulty: {Name}");
					return 100;
			}

			int smallerBasePrice = basePrice / 10; // Guarantees that the price will be an even number
			Range range = new(
				(int)(smallerBasePrice - (smallerBasePrice * multiplier)),
				(int)(smallerBasePrice * (multiplier + 1))
			);
			return range.GetRandomValue() * 10;
		}

		public virtual ISalable GetSaliableObject(int initialStack = 1, bool isRecipe = false, int price = -1)
		{
			if (IsRing)
			{
                return new SVRing(Id.ToString())
                {
                    Stack = initialStack
                };
            }

            return IsBigCraftable 
				? new SVObject(Vector2.Zero, Id.ToString(), isRecipe)
					{
						Stack = initialStack,
						Price = price == -1 
							? (BigCraftablePrice / 2) // We want the sell price, not the buy price
							: price
					}
				: new SVObject(Id.ToString(), initialStack, isRecipe, price);
        }

		/// <summary>
		/// Not used, so log when it's called
		/// </summary>
		/// <returns />
		public override string ToString()
		{
			Globals.ConsoleError($"Called the ToString of unexpected item {Id}: {Name}");
			return "";
		}
	}
}
