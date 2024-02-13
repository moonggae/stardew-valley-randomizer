using StardewValley.GameData.Shops;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (Globals.Config.Shops.AddTapperCraftItemsToRobinsShop)
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
        /// <param name="menu">The shop menu</param>
        private void AddRandomTapperCraftingIngredient()
        {
            var exitingStockIds = CurrentShopData.Items
                .Select(item => item.ItemId)
                .ToList();

            Random shopRNG = Globals.GetDailyRNG(nameof(RandomizedCarpenterShop));
            var tapperItemIdsAndStock = ((CraftableItem)ItemList.BigCraftableItems[BigCraftableIndexes.Tapper]).LastRecipeGenerated;
            var tapperItems = tapperItemIdsAndStock.Keys
                .Select(id => ItemList.Items[id])
                .Where(item => !exitingStockIds.Contains(item.QualifiedId))
                .ToList();

            if (tapperItems.Any())
            {
                var tapperItemToSell = Globals.RNGGetRandomValueFromList(tapperItems, shopRNG);
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
            Random shopRNG = Globals.GetDailyRNG(nameof(RandomizedCarpenterShop));
            const int BaseClayPrice = 50;
            var clayStock = Range.GetRandomValue(20, 40, shopRNG);
            var clayPrice = Globals.RNGGetIntWithinPercentage(BaseClayPrice, 50, shopRNG);

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
