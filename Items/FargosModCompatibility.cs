using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using VipixToolBox.UI;

namespace VipixToolBox.Items
{
  public class FargosModCompatibility : GlobalItem
  {
    public override void UpdateAccessory (Item item, Player player, bool hideVisual)
    {
      VipixToolBoxPlayer myPlayer = player.GetModPlayer<VipixToolBoxPlayer>();
      if (item.Name == "World Shaper Soul")//feel free to create an item with this display name
      {
        myPlayer.fargoRange = 54;//range of normal tools with fargo mod
      }
    }
  }
}
