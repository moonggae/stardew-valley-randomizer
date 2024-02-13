using Force.DeepCloner;
using StardewValley;
using StardewValley.GameData.Shops;
using System;
using static StardewValley.GameData.QuantityModifier;

namespace Randomizer
{
    public abstract class RandomizedShop
    {
        /// <summary>
        /// The key to the shop in Data/Shops
        /// </summary>
        public string ShopId { get; private set; }

        /// <summary>
        /// The default data in Data/Shops
        /// </summary>
        protected static ShopData DefaultShopData { get; set; }

        /// <summary>
        /// The current shop data to use as a replacement - deep cloned from the default
        /// </summary>
        protected ShopData CurrentShopData { get; set; }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="currentShopState">The state of ShopData</param>
        /// <param name="shopId">The shop id</param>
        public RandomizedShop(string shopId) 
        {
            ShopId = shopId;
            DefaultShopData ??= DataLoader.Shops(Game1.content)[shopId];
            CurrentShopData = DefaultShopData.DeepClone();
        }

        /// <summary>
        /// The method that will modify the shop stock in some way
        /// </summary>
        /// <returns>The modified shop data</returns>
        public abstract ShopData ModifyShop();

        /// <summary>
        /// Gets a new price for the item
        /// Takes the greater of the item's price and the fallback price and multiplies them by
        /// the multiplier and the difficulty level
        /// </summary>
        /// <param name="item">The item to get the price for</param>
        /// <param name="fallbackPrice">The price to use if the item costs too little</param>
        /// <param name="multiplier">The multiplier</param>
        /// <returns>The new item price</returns>
        protected static int GetAdjustedItemPrice(Item item, int fallbackPrice, int multiplier)
        {
            return GetAdjustedItemPrice(item.GetSaliableObject(), fallbackPrice, multiplier);
        }

        /// <summary>
        /// Gets a new price for the item
        /// Takes the greater of the item's price and the fallback price and multiplies them by
        /// the multiplier and the difficulty level
        /// </summary>
        /// <param name="item">The item to get the price for</param>
        /// <param name="fallbackPrice">The price to use if the item costs too little</param>
        /// <param name="multiplier">The multiplier</param>
        /// <returns>The new item price</returns>
        protected static int GetAdjustedItemPrice(ISalable item, int fallbackPrice, int multiplier)
        {
            return Math.Max(fallbackPrice, (int)(item.salePrice() * Game1.MasterPlayer.difficultyModifier)) * multiplier;
        }

        /// <summary>
        /// Gets a shop item to be added to the shop
        /// Uses mostly default values
        /// The stock values will use the limited mode that's player specific
        /// </summary>
        /// <param name="id">The quantified id string</param>
        /// <param name="price">The price of the item - use -1 for a default price</param>
        /// <param name="availableStock">The stock of the item - use -1 for infinite</param>
        /// <returns>The data to add to the shop</returns>
        protected static ShopItemData GetShopItem(string id, int price = -1, int availableStock = -1)
        {
            return new ShopItemData()
            {
                TradeItemId = null,
                TradeItemAmount = 1,
                Price = price,
                ApplyProfitMargins = null,
                AvailableStock = availableStock,
                AvailableStockLimit = LimitedStockMode.Player,
                AvoidRepeat = false,
                UseObjectDataPrice = false,
                IgnoreShopPriceModifiers = true,
                PriceModifiers = null,
                PriceModifierMode = QuantityModifierMode.Stack,
                AvailableStockModifiers = null,
                AvailableStockModifierMode = QuantityModifierMode.Stack,
                Condition = null,
                Id = $"{Globals.ModRef.ModManifest.UniqueID}-{id}", // This has to be a unique id in this list
                ItemId = id,
                RandomItemId = null,
                MaxItems = null,
                MinStack = -1,
                MaxStack = -1,
                Quality = -1,
                ObjectInternalName = null,
                ObjectDisplayName = null,
                ToolUpgradeLevel = -1,
                IsRecipe = false,
                StackModifiers = null,
                StackModifierMode = QuantityModifierMode.Stack,
                QualityModifiers = null,
                QualityModifierMode = QuantityModifierMode.Stack,
                PerItemCondition = null
            };
        }

        /// <summary>
        /// Adds an item into the shop at the end of the list
        /// </summary>
        /// <param name="itemToAdd">The item to add</param>
        protected void AddStock(ShopItemData itemToAdd)
        {
            CurrentShopData.Items.Add(itemToAdd);
        }

        /// <summary>
        /// Inserts an item into the shop data at the given index (the beginning, by default)
        /// </summary>
        /// <param name="itemToAdd">The item to add</param>
        /// <param name="index">The index to add to (defaults to 0)</param>
        protected void InsertStockAt(ShopItemData itemToAdd, int index = 0)
        {
            CurrentShopData.Items.Insert(index, itemToAdd);
        }
    }
}
