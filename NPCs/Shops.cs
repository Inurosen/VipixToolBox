using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VipixToolBox.NPCs
{
  public class VipixTBGlobalNPC : GlobalNPC
  {
    public override void SetupShop(int type, Chest shop, ref int nextSlot)
    {
      if (type == NPCID.Merchant)
      {
        //if (NPC.downedBoss3)
        //should be skeletron
        shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.AutoHammer>());
        nextSlot++;
        shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.WallHammer>());
        nextSlot++;
      }
      if (type == NPCID.Painter)
      {
        shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.ColorPalette>());
        nextSlot++;
      }
      if (type == NPCID.Dryad)
      {
        shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.RattlesnakeWand>());
        shop.item[nextSlot].shopCustomPrice = Item.buyPrice(1, 0, 0, 0);
        nextSlot++;
        shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.BlockWand>());
        nextSlot++;
      }
      if (type == NPCID.Wizard)
      {
        shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.LevitationWand>());
        nextSlot++;
      }
      if (type == NPCID.Mechanic)
      {
        shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.SwapPickaxe>());
        nextSlot++;
      }
    }
  }
}
