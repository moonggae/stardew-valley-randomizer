using StardewValley;
using StardewValley.Menus;
using System;
using System.Linq;
using SVObject = StardewValley.Object;

namespace Randomizer
{
    public abstract class ShopMenuAdjustments
    {
        internal static readonly int _maxValue = int.MaxValue;

        /// <summary>
        /// Adds the given item to the given shop menu
        /// </summary>
        /// <param name="menu">The shop menu<param>
        /// <param name="item">The item to add</param>
        /// <param name="stock">The amount to sell - defaults to max</param>
        /// <param name="salePrice">The amount to sell at - defaults to the item's sale price</param>
        internal static void AddStock(
            ShopMenu menu,
            ISalable item, 
            int stock = int.MaxValue,
            int? salePrice = null)
        {
            AddToItemPriceAndStock(menu, item, stock, salePrice);
            menu.forSale.Add(item);
        }

        /// <summary>
        /// Inserts the given item at the given position - defaults to the first index (0)
        /// </summary>
        /// <param name="menu">The shop menu<param>
        /// <param name="item">The item to add</param>
        /// <param name="stock">The amount to sell - defaults to max</param>
        /// <param name="salePrice">The amount to sell at - defaults to the item's sale price</param>
        /// <param name="index">The index to insert the item to (where it will show up in the shop menu)</param>
        internal static void InsertStockAt(
            ShopMenu menu,
            ISalable item, 
            int stock = int.MaxValue,
            int? salePrice = null,
            int index = 0)
        {
            AddToItemPriceAndStock(menu, item, stock, salePrice);
            menu.forSale.Insert(index, item);
        }

        /// <summary>
        /// Adds the item to the menu's itemPriceAndStock dictionary
        /// </summary>
        /// <param name="menu">The shop menu<param>
        /// <param name="item">The item to add</param>
        /// <param name="stock">The amount to sell - defaults to max</param>
        /// <param name="salePrice">The amount to sell at - defaults to the item's sale price</param>
        internal static void AddToItemPriceAndStock(
            ShopMenu menu, 
            ISalable item, 
            int stock = int.MaxValue, 
            int? salePrice = null)
        {
            var price = salePrice ?? item.salePrice();
            menu.itemPriceAndStock.Add(item, new[] { price, stock });
        }

        /// <summary>
        /// Empty a given shop's stock
        /// </summary>
        /// <param name="menu">The menu of the shop</param>
        internal static void EmptyStock(ShopMenu menu)
        {
            while (menu.itemPriceAndStock.Any())
            {
                ISalable obj = menu.itemPriceAndStock.Keys.ElementAt(0);
                menu.itemPriceAndStock.Remove(obj);
                menu.forSale.Remove(obj);
            }
        }

        /// <summary>
        /// Removes an item fom the shop stock
        /// </summary>
        /// <param name="menu">The menu of the shop</param>
        /// <param name="itemId">The item to remove</param>
        /// <returns>Whether an item was removed</returns>
        internal static bool RemoveFromStock(ShopMenu menu, int itemId)
        {
            var itemToRemove = menu.itemPriceAndStock.Keys
                .Where(item => item is SVObject && (item as SVObject).ParentSheetIndex == itemId)
                .FirstOrDefault();

            if (itemToRemove != null)
            {
                menu.itemPriceAndStock.Remove(itemToRemove);
                menu.forSale.Remove(itemToRemove);
                return true;
            }

            return false;
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
        internal static int GetAdjustedItemPrice(ISalable item, int fallbackPrice, int multiplier)
        {
            return Math.Max(fallbackPrice, (int)(item.salePrice() * Game1.MasterPlayer.difficultyModifier)) * multiplier;
        }
    }
}
