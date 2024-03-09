using StardewValley.GameData.Shops;
using System.Linq;

namespace Randomizer
{
    public class RandomizedCarpenterShop : RandomizedShop
    {
        public RandomizedCarpenterShop() : base("Carpenter") { }

        /// <summary>
        /// Adds clay and tapper craft items
        /// </summary>
        public override ShopData ModifyShop()
        {
            if (Globals.Config.Shops.AddTapperCraftItemsToRobinsShop &&
                Globals.Config.CraftingRecipes.Randomize)
            {
                AddRandomTapperCraftingIngredient();
            }

            if (Globals.Config.Shops.AddClayToRobinsShop)
            {
                AddClay();
            }

            return CurrentShopData;
        }

        /// <summary>
        /// Adds a random item to the shop that is required to make a tapper - changes daily
        /// </summary>
        private void AddRandomTapperCraftingIngredient()
        {
            var exitingStockIds = CurrentShopData.Items
                .Select(item => item.ItemId)
                .ToList();

            RNG shopRNG = RNG.GetDailyRNG($"{nameof(RandomizedCarpenterShop)}.{nameof(AddRandomTapperCraftingIngredient)}");
            var tapperItemIdsAndStock = ((CraftableItem)ItemList.BigCraftableItems[BigCraftableIndexes.Tapper]).LastRecipeGenerated;
            var tapperItems = tapperItemIdsAndStock.Keys
                .Select(id => ItemList.Items[id])
                .Where(item => !exitingStockIds.Contains(item.QualifiedId))
                .ToList();

            if (tapperItems.Any())
            {
                var tapperItemToSell = shopRNG.GetRandomValueFromList(tapperItems);
                var stock = tapperItemIdsAndStock[(ObjectIndexes)tapperItemToSell.Id];
                var price = GetAdjustedItemPrice(tapperItemToSell, 50, 5);
                InsertStockAt(tapperItemToSell.QualifiedId, "TapperItem", price, stock, index: 2);
            }
        }

        /// <summary>
        /// Adds Clay to Robin's shop since it's really grindy to get
        /// Add some randomness to the price each day (between 25-75 coins each)
        /// </summary>
        private void AddClay()
        {
            RNG shopRNG = RNG.GetDailyRNG($"{nameof(RandomizedCarpenterShop)}.{nameof(AddClay)}");
            const int BaseClayPrice = 50;
            var clayStock = shopRNG.NextIntWithinRange(20, 40);
            var clayPrice = shopRNG.NextIntWithinPercentage(BaseClayPrice, 50);

            InsertStockAt(
                ItemList.GetQualifiedId(ObjectIndexes.Clay), 
                "Clay", 
                clayPrice, 
                clayStock, 
                index: 2
            );
        }
    }
}
