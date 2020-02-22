using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using VipixToolBox.Projectiles;

namespace VipixToolBox.Items
{
	public class RattlesnakeWand : ModItem
	{
		public bool operationAllowed;
		public List<int> sandList;
		public List<int> hardenedSandList;
		public int baseRange = 10;
		public int toolRange;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rattlesnake Wand");
			Tooltip.SetDefault("Ideal for digging burrows\nCan be right-clicked");
		}
		public override void SetDefaults()
		{
			item.damage = 14;
			item.useStyle = 5;
			item.useAnimation = 20;
			item.useTime = 8;
			item.shootSpeed = 3.7f;
			item.knockBack = 6.5f;
			item.width = 32;
			item.height = 32;
			item.scale = 1f;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.shoot = ModContent.ProjectileType<RattleSnakeWandProjectile>();
			item.value = Item.buyPrice(0, 0, 40, 0);
			item.noMelee = true; // Important because the spear is acutally a projectile instead of an item. This prevents the melee hitbox of this item.
			item.noUseGraphic = true; // Important, it's kind of wired if people see two spears at one time. This prevents the melee animation of this item.
			item.melee = true;
			item.autoReuse = true; // Most spears dont autoReuse, but it's possible

			sandList = new List<int>();
			sandList.Add(TileID.Sand);//alphabetical order
			sandList.Add(TileID.Crimsand);
			sandList.Add(TileID.Ebonsand);
			sandList.Add(TileID.Pearlsand);
			hardenedSandList = new List<int>();//mirror order of sands
			hardenedSandList.Add(TileID.HardenedSand);
			hardenedSandList.Add(TileID.CrimsonHardenedSand);
			hardenedSandList.Add(TileID.CorruptHardenedSand);
			hardenedSandList.Add(TileID.HallowHardenedSand);
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override void HoldItem(Player player)
		{
			//this method determines if the pointed block is a type of sand in range of the player
			//it shows the item icon if true
			//and it allows the actions in CanUseItem
			VipixToolBoxPlayer myPlayer = player.GetModPlayer<VipixToolBoxPlayer>();
			toolRange = Math.Max(baseRange, myPlayer.fargoRange);//blocks

			if (Vector2.Distance(player.position,myPlayer.pointerCoord) < toolRange*16 &&
			(sandList.Contains(myPlayer.pointedTile.type) || hardenedSandList.Contains(myPlayer.pointedTile.type)) &&
			VipixToolBoxWorld.toolEnabled["RattlesnakeWand"])
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
			//Main.NewText(new Vector2(myPlayer.pointedTileX,myPlayer.pointedTileY).ToString(),255,255,255);
			if (operationAllowed)
			{
				if (player.altFunctionUse == 2)
				{
					int index = hardenedSandList.FindIndex(i => i == myPlayer.pointedTile.type);//exception at -1 if the wrong mouse button is used
					if (index != -1) myPlayer.pointedTile.type = (ushort)sandList[index];
				}
				else
				{
					int index = sandList.FindIndex(i => i == myPlayer.pointedTile.type);//exception at -1 if the wrong mouse button is used
					if (index != -1)
					{
						if (myPlayer.pointedTileAbove.type == TileID.Cactus) myPlayer.pointedTile.active(false);//cactus on top, only exception to handle ?
						WorldGen.SquareTileFrame(myPlayer.pointedTileX, myPlayer.pointedTileY, true);
						myPlayer.pointedTile.type = (ushort)hardenedSandList[index];
						myPlayer.pointedTile.active(true);
					}
				}
				WorldGen.SquareTileFrame(myPlayer.pointedTileX, myPlayer.pointedTileY, true);
				if (Main.netMode == 1) NetMessage.SendTileSquare(-1, myPlayer.pointedTileX, myPlayer.pointedTileY, 1);
				Main.PlaySound(SoundID.Dig);
				for (int i = 0; i < 5; i++)
				{
					int dust = Dust.NewDust(new Vector2((myPlayer.pointedTileX-1) * 16,(myPlayer.pointedTileY-1) * 16), 40, 40, mod.DustType("Sparkle"));
				}
			}
			//return base.CanUseItem(player);
			return player.ownedProjectileCounts[item.shoot] < 1; // This is to ensure the spear doesn't bug out when using autoReuse = true
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.AntlionMandible, 2);
			recipe.AddIngredient(ItemID.SandBlock, 5);
			recipe.AddIngredient(ItemID.HardenedSand, 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
