using Microsoft.Xna.Framework;
using Randomizer.OverriddenFiles;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;
using System;
using xTile.Dimensions;

namespace Randomizer
{
    /// <summary>
    /// The submarine GameLocation - identical to the original Submarine.cs file, but with a
    /// different getFish function to include our randomized fish instead of their fish
    /// </summary>
    public class OverriddenCommunityCenter : CommunityCenter
    {
        public OverriddenCommunityCenter() : base("CommunityCenter") { }

        /// <summary>
        /// The old community center location
        /// </summary>
        private static CommunityCenter NormalCommunityCenterLocation { get; set; }

        /// <summary>
        /// Replaces the community center location with an overridden one so that the fish that
        /// appear there are correct
        /// </summary>
        public static void UseOverriddenCommunityCenter()
        {
            int communityCenterIndex;
            foreach (GameLocation location in Game1.locations)
            {
                if (location.Name == "CommunityCenter")
                {
                    if (location.GetType() != typeof(OverriddenCommunityCenter))
                    {
                        NormalCommunityCenterLocation = (CommunityCenter)location;
                    }

                    communityCenterIndex = Game1.locations.IndexOf(location);
                    Game1.locations[communityCenterIndex] = new OverriddenCommunityCenter();
                    break;
                }
            }
        }

        /// <summary>
        /// Restores the community center location - this should be done before saving the game
        /// to avoid a crash
        /// </summary>
        public static void RestoreCommunityCenter()
        {
            if (NormalCommunityCenterLocation == null) { return; }

            int communityCenterIndex;
            foreach (GameLocation location in Game1.locations)
            {
                if (location.Name == "CommunityCenter")
                {
                    communityCenterIndex = Game1.locations.IndexOf(location);
                    Game1.locations[communityCenterIndex] = NormalCommunityCenterLocation;
                    break;
                }
            }
        }

        /// <summary>
        /// Unchanged from the original - checks the player's action and shows the bundle UI when necessary
        /// </summary>
        /// <param name="tileLocation">The tile clicked</param>
        /// <param name="viewport">The viewpoint</param>
        /// <param name="who">The player</param>
        /// <returns></returns>
        public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
        {
            switch (this.map.GetLayer("Buildings").Tiles[tileLocation] != null ? this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex : -1)
            {
                case 1799:
                    if (this.numberOfCompleteBundles() > 2)
                    {
                        this.checkBundle(5);
                        break;
                    }
                    break;
                case 1824:
                case 1825:
                case 1826:
                case 1827:
                case 1828:
                case 1829:
                case 1830:
                case 1831:
                case 1832:
                case 1833:
                    this.checkBundle(this.getAreaNumberFromLocation(who.getTileLocation()));
                    return true;
            }
            return base.checkAction(tileLocation, viewport, who);
        }

        /// <summary>
        /// Gets the area representing the bundle to open
        /// </summary>
        /// <param name="tileLocation">The tile</param>
        /// <returns />
        private int getAreaNumberFromLocation(Vector2 tileLocation)
        {
            for (int area = 0; area < this.areasComplete.Count; ++area)
            {
                if (this.getAreaBounds(area).Contains((int)tileLocation.X, (int)tileLocation.Y))
                    return area;
            }
            return -1;
        }

        /// <summary>
        /// Gets the bounds that belong to the given area
        /// </summary>
        /// <param name="area">The area</param>
        /// <returns />
        private Microsoft.Xna.Framework.Rectangle getAreaBounds(int area)
        {
            switch (area)
            {
                case 0:
                    return new Microsoft.Xna.Framework.Rectangle(0, 0, 22, 11);
                case 1:
                    return new Microsoft.Xna.Framework.Rectangle(0, 12, 21, 17);
                case 2:
                    return new Microsoft.Xna.Framework.Rectangle(35, 4, 9, 9);
                case 3:
                    return new Microsoft.Xna.Framework.Rectangle(52, 9, 16, 12);
                case 4:
                    return new Microsoft.Xna.Framework.Rectangle(45, 0, 15, 9);
                case 5:
                    return new Microsoft.Xna.Framework.Rectangle(22, 13, 28, 9);
                case 7:
                    return new Microsoft.Xna.Framework.Rectangle(44, 10, 6, 3);
                case 8:
                    return new Microsoft.Xna.Framework.Rectangle(22, 4, 13, 9);
                default:
                    return Microsoft.Xna.Framework.Rectangle.Empty;
            }
        }

        /// <summary>
        /// Pulls up the UI bundle
        /// MODIFIED: Use the OverriddenJunimoNoteMenu so rings can be deposited
        /// </summary>
        /// <param name="area">The room to pull up the UI for</param>
        public void checkBundle(int area)
        {
            this.bundleMutexes[area].RequestLock((Action)(() =>
                Game1.activeClickableMenu = (IClickableMenu)new OverriddenJunimoNoteMenu(area, this.bundlesDict())));
        }
    }
}
