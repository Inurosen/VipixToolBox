using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.ObjectData;//TileObjectData
using Terraria.DataStructures;//PlacementHook
using VipixToolBox.Projectiles;
using VipixToolBox.Tiles;
using static Terraria.ModLoader.ModContent;

namespace VipixToolBox.Items
{
	public class LevitationWand : ModItem
	{
		public int baseRange = 14;
		public int toolRange;
		public bool operationAllowed;
		public int manaDrain;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unstable Staff");
			Tooltip.SetDefault("It defies reason, and furniture\nCan be right-clicked");
		}
		public override void SetDefaults()
		{
			item.damage = 34;
			item.useStyle = 1;
			item.useAnimation = 12;
			item.useTime = 12;
			item.knockBack = 2f;
			item.width = 32;
			item.height = 32;
			item.scale = 1f;
			item.rare = 2;
			//item.UseSound = SoundID.Item1;
			item.value = Item.buyPrice(0, 60, 0, 0);
			item.autoReuse = true;

			manaDrain = 6;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override void HoldItem(Player player)
		{
			VipixToolBoxPlayer myPlayer = player.GetModPlayer<VipixToolBoxPlayer>();
			toolRange = Math.Max(baseRange, myPlayer.fargoRange);//blocks

			if (Vector2.Distance(player.position,myPlayer.pointerCoord) < toolRange*16 &&
			!myPlayer.pointedTile.active() || !Main.tileSolid[myPlayer.pointedTile.type] &&
			!myPlayer.treeList.Contains(myPlayer.pointedTile.type) &&
			VipixToolBoxWorld.toolEnabled["LevitationWand"])
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
			FlyingBlockTE myTE;
			VipixToolBoxPlayer myPlayer = player.GetModPlayer<VipixToolBoxPlayer>();
			toolRange = Math.Max(baseRange, myPlayer.fargoRange);//blocks

			Tile tile = Main.tile[myPlayer.pointedTileX,myPlayer.pointedTileY];

			Item fargotest = player.inventory[0];

			//Main.tile[myPlayer.pointedTileX,myPlayer.pointedTileY].active(true);
			//Main.tile[myPlayer.pointedTileX,myPlayer.pointedTileY].type = TileID.Dirt;//this doesnt respect tile properties
			//Can't be used because we need a hook on the tile to link with the tile entity
			//this hook is only called when the tile is placed normally
			//also, we can't place tileEntity manually. At least we shouldn't try (cf documentation)

			//next condition: checking reach, checking pointed block: should be either air or non solid block (like grass) but not trees (trees are non-solid too)
			if (operationAllowed)
			{
				if (player.altFunctionUse != 2 && player.statMana >= manaDrain)
				{
					WorldGen.KillTile(myPlayer.pointedTileX,myPlayer.pointedTileY, false, false, false);//otherwise, grass blocks the PlaceTile
					WorldGen.PlaceTile(myPlayer.pointedTileX,myPlayer.pointedTileY, (ushort)mod.TileType("FlyingBlockTile"));//respects tile properties
					//WorldGen.SquareTileFrame(myPlayer.pointedTileX, myPlayer.pointedTileY, true);
					if (Main.netMode == 1) NetMessage.SendTileSquare(-1, myPlayer.pointedTileX, myPlayer.pointedTileY, 1);
					int id = ModContent.GetInstance<FlyingBlockTE>().Find(myPlayer.pointedTileX,myPlayer.pointedTileY);
					if (id != -1)
					{
						//it should never be -1
						myTE = (FlyingBlockTE)TileEntity.ByID[id];
						myTE.timer = 140;
						myTE.tileX = myPlayer.pointedTileX;
						myTE.tileY = myPlayer.pointedTileY;
					}
					player.statMana = Math.Max(0,player.statMana - manaDrain);
					return true;
				}
				else if (player.altFunctionUse == 2 && player.statMana >= manaDrain*4)
				{
					WorldGen.KillTile(myPlayer.pointedTileX,myPlayer.pointedTileY, false, false, false);
					WorldGen.PlaceTile(myPlayer.pointedTileX,myPlayer.pointedTileY, (ushort)mod.TileType("FlyingHardBlockTile"));
					if (Main.netMode == 1) NetMessage.SendTileSquare(-1, myPlayer.pointedTileX, myPlayer.pointedTileY, 1);
					int id = ModContent.GetInstance<FlyingBlockTE>().Find(myPlayer.pointedTileX,myPlayer.pointedTileY);
					if (id != -1)
					{
						myTE = (FlyingBlockTE)TileEntity.ByID[id];
						myTE.timer = 400;
						myTE.tileX = myPlayer.pointedTileX;
						myTE.tileY = myPlayer.pointedTileY;
					}
					player.statMana = Math.Max(0,player.statMana - manaDrain*5);
					return true;
				}
				else return false;
			}
			else return false;
		}

		public override void AddRecipes()
		{/*
			ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ItemID.HardenedSand, 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}
}
