using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using VipixToolBox;
using VipixToolBox.Items;

namespace VipixToolBox.Items
{
    public class SwapPickaxe : ModItem
    {
        public int baseRange = 12;
        public int toolRange;
        public bool operationAllowed;


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swap Pickaxe");
            Tooltip.SetDefault("Nothing is lost, nothing is created\n100% equivalent pickaxe power");
        }

        public override void SetDefaults()
        {
            Item moltenPickaxe = new Item(); //defaults on copper pickaxe
            moltenPickaxe.CloneDefaults(ItemID.MoltenPickaxe);
            item.damage = moltenPickaxe.damage;
            item.knockBack = moltenPickaxe.knockBack;
            item.useStyle = 1;
            item.useAnimation = 16;
            item.useTime = 16;
            item.width = 32;
            item.height = 32;
            item.rare = moltenPickaxe.rare + +1;
            item.UseSound = SoundID.Item1;
            item.value = Item.buyPrice(0, 5, 0, 0);
            item.autoReuse = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return false;
        }

        public override void HoldItem(Player player)
        {
            //this method determines if the pointed block is buildable and in range of the player
            //it shows the item icon if true
            //and it allows the actions in CanUseItem
            VipixToolBoxPlayer myPlayer = player.GetModPlayer<VipixToolBoxPlayer>();
            toolRange = Math.Max(baseRange, myPlayer.fargoRange);//blocks
            //Main.NewText(validBlocks.Contains(myPlayer.pointedTile.type).ToString());
            if (Vector2.Distance(player.position, myPlayer.pointerCoord) < toolRange * 16 &&
            myPlayer.pointedTile.active() &&
            Main.tileSolid[myPlayer.pointedTile.type])
            {
                operationAllowed = true;
                player.showItemIcon = true;
            }
            else
            {
                operationAllowed = false;
                player.showItemIcon = false;
            }
        }

        public override bool CanUseItem(Player player)
        {
            VipixToolBoxPlayer myPlayer = player.GetModPlayer<VipixToolBoxPlayer>();

            return true;
        }

        public override bool UseItem(Player player)
        {
            VipixToolBoxPlayer myPlayer = player.GetModPlayer<VipixToolBoxPlayer>();

            if (operationAllowed)
            {
                int tileToCreate = -1;
                int itemIndex = -1;

                for (int i = 0; i < player.inventory.Length; i++)
                {
                    tileToCreate = player.inventory[i].createTile;

                    if (tileToCreate > -1 && Main.tileSolid[player.inventory[i].createTile]) {
                        itemIndex = i;
                        break;
                    }
                }

                
                //need to check for resource first
                if (tileToCreate > -1 && itemIndex > -1 && tileToCreate != myPlayer.pointedTile.type)
                {
                    bool halfBrick = myPlayer.pointedTile.halfBrick();
                    byte slope = myPlayer.pointedTile.slope();
                    bool inActive = myPlayer.pointedTile.inActive();
                    byte color = myPlayer.pointedTile.color();

                    // A very hacky block swap to keep furniture in place
                    int worldHeight = Main.maxTilesY - 1;
                    Tile oldTile = Main.tile[0, worldHeight];
                    WorldGen.PlaceTile(0, worldHeight, myPlayer.pointedTile.type, false, true);
                    Tile newTile = Main.tile[0, worldHeight];
                    WorldGen.KillTile(0, worldHeight);

                    Vector2 playerPos = new Vector2((int)player.position.X, (int)player.position.Y);
                    for (int i = 0; i < Main.maxItems; i++)
                    {
                        if (Main.item[i].position.X < 32 && Main.item[i].position.Y > Main.bottomWorld - 32)
                        {
                            Main.item[i].position = playerPos;
                            break;
                        }
                    }
                    WorldGen.PlaceTile(0, worldHeight, oldTile.type, false, true);

                    WorldGen.PlaceTile(myPlayer.pointedTileX, myPlayer.pointedTileY, tileToCreate, false, true);
                    player.inventory[itemIndex].stack--;
                    myPlayer.pointedTile.halfBrick(halfBrick);
                    myPlayer.pointedTile.slope(slope);
                    myPlayer.pointedTile.inActive(inActive);
                    myPlayer.pointedTile.color(color);

                    if (Main.netMode == 1) NetMessage.SendTileSquare(-1, myPlayer.pointedTileX, myPlayer.pointedTileY, 1);
                    Main.PlaySound(SoundID.Dig);
                }
            }
            return true;
        }
    }
}