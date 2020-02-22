using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using VipixToolBox;
using VipixToolBox.UI;

namespace VipixToolBox.Items
{
	public class BlockWand : ModItem
	{
		public int baseRange = 12;
		public int toolRange;
		public bool operationAllowed;
		public List<int> toolList;
		string[] toolNames = {"a","b","c","d","e","f"};
		List<int> woods = new List<int>{ItemID.Wood,
			ItemID.Ebonwood,
			ItemID.RichMahogany,
			ItemID.Pearlwood,
			ItemID.Shadewood,
			ItemID.SpookyWood,
			ItemID.BorealWood,
			ItemID.PalmWood};

			public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Greater Wand");
				Tooltip.SetDefault("You should know how to use 6 chopsticks\n50% chance not to consume material\nCan be right-clicked");
			}
			public override void SetDefaults()
			{
				item.useStyle = 1;
				item.useAnimation = 12;
				item.useTime = 12;
				item.width = 32;
				item.height = 32;
				item.rare = 6;
				item.UseSound = SoundID.Item1;
				item.value = Item.buyPrice(0, 50, 0, 0);
				item.autoReuse = true;
				//item.useAmmo = ItemID.Wood;//only for display, works only if resources are set as ammo
				/*
				0 : leaf Wand
				1 : living wood Wand
				2 : rich mahogany leaf Wand
				3 : living mahogany wand
				4 : hive Wand
				5 : bone wand
				*/
				toolList = new List<int>();
				toolList.Add(TileID.LeafBlock);
				toolList.Add(TileID.LivingWood);
				toolList.Add(TileID.LivingMahoganyLeaves);
				toolList.Add(TileID.LivingMahogany);
				toolList.Add(TileID.Hive);
				toolList.Add(TileID.BoneBlock);
			}

			public override bool AltFunctionUse(Player player)
			{
				return true;
			}
			public override void HoldItem(Player player)
			{
				//this method determines if the pointed block is buildable and in range of the player
				//it shows the item icon if true
				//and it allows the actions in CanUseItem
				VipixToolBoxPlayer myPlayer = player.GetModPlayer<VipixToolBoxPlayer>();
				toolRange = Math.Max(baseRange, myPlayer.fargoRange);//blocks
				//should have a connected solid tile (that is not empty nor a decorative element) or wall
				//should also be empty or a non-solid tile
				Tile tileTop = Main.tile[myPlayer.pointedTileX,myPlayer.pointedTileY - 1];
				Tile tileBot = Main.tile[myPlayer.pointedTileX,myPlayer.pointedTileY + 1];
				Tile tileLeft = Main.tile[myPlayer.pointedTileX - 1,myPlayer.pointedTileY];
				Tile tileRight = Main.tile[myPlayer.pointedTileX + 1,myPlayer.pointedTileY];
				bool okTop = tileTop.active() && Main.tileSolid[tileTop.type];
				bool okBot = tileBot.active() && Main.tileSolid[tileBot.type];
				bool okLeft = tileLeft.active() && Main.tileSolid[tileLeft.type];
				bool okRight = tileRight.active() && Main.tileSolid[tileRight.type];

				if (Vector2.Distance(player.position,myPlayer.pointerCoord) < toolRange*16 &&
				(!myPlayer.pointedTile.active() || !Main.tileSolid[myPlayer.pointedTile.type] && !myPlayer.treeList.Contains(myPlayer.pointedTile.type))&&
				(okTop || okBot || okLeft || okRight || myPlayer.pointedTile.wall != 0)&&
				VipixToolBoxWorld.toolEnabled["BlockWand"])
				{
					operationAllowed = true;
					player.showItemIcon = true;
					player.showItemIcon2 = mod.ItemType(toolNames[myPlayer.blockTool]+"Icon");
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

				if (player.altFunctionUse == 2)
				{
					myPlayer.tbMouseX = Main.mouseX;
					myPlayer.tbMouseY = Main.mouseY;
					BlockUI.visible = true;
				}
				else if (operationAllowed)
				{
					//need to check for resource first
					int woodIndex = -1;
					int hiveIndex = -1;
					int boneIndex = -1;
					bool doOperation = false;
					for (int i = 0; i < player.inventory.Length; i++)
					{
						if (woods.Contains(player.inventory[i].type)) woodIndex = i;
						if (player.inventory[i].type == ItemID.Hive) hiveIndex = i;
						if (player.inventory[i].type == ItemID.Bone) boneIndex = i;
					}
					if ((myPlayer.blockTool == 1 || myPlayer.blockTool == 3) && woodIndex != -1)
					{
						player.inventory[woodIndex].stack--;//100% chance to consume wood if placing wood, otherwise duplication is possible
						doOperation = true;
					}
					else if ((myPlayer.blockTool == 0 || myPlayer.blockTool == 2) && woodIndex != -1)
					{
						if (Main.rand.NextDouble() <= 0.5) player.inventory[woodIndex].stack--;//50% chance not to consume ammo
						doOperation = true;
					}
					else if (myPlayer.blockTool == 4 && hiveIndex != -1)
					{
						if (Main.rand.NextDouble() <= 0.5) player.inventory[hiveIndex].stack--;//50% chance not to consume ammo
						doOperation = true;
					}
					else if (myPlayer.blockTool == 5 && boneIndex != -1)
					{
						if (Main.rand.NextDouble() <= 0.5) player.inventory[boneIndex].stack--;
						doOperation = true;
					}
					if (doOperation)
					{
						myPlayer.pointedTile.type = (ushort)toolList[myPlayer.blockTool];//actually the operation is done here
						myPlayer.pointedTile.active(true);
						WorldGen.SquareTileFrame(myPlayer.pointedTileX, myPlayer.pointedTileY, true);
						if (Main.netMode == 1) NetMessage.SendTileSquare(-1, myPlayer.pointedTileX, myPlayer.pointedTileY, 1);//player, X, Y, square "diameter". -1 as the player equals all ?
						Main.PlaySound(SoundID.Dig);
						for (int j = 0; j < 5; j++)
						{
							int dust = Dust.NewDust(new Vector2((myPlayer.pointedTileX-1) * 16,(myPlayer.pointedTileY-1) * 16), 40, 40, mod.DustType("Sparkle"));
						}
					}
				}
				return true;
			}
			public override void AddRecipes()
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddIngredient(ItemID.LeafWand);
				recipe.AddIngredient(ItemID.LivingWoodWand);
				recipe.AddIngredient(ItemID.LivingMahoganyLeafWand);
				recipe.AddIngredient(ItemID.LivingMahoganyWand);
				recipe.AddIngredient(ItemID.HiveWand);
				recipe.AddIngredient(ItemID.BoneWand);
				recipe.AddTile(TileID.TinkerersWorkbench);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
