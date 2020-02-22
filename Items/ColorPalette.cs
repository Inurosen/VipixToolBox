using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using VipixToolBox.UI;

namespace VipixToolBox.Items
{
	public class ColorPalette : ModItem
	{
		public int baseRange = 17;
		public int toolRange;
		public bool operationAllowed = false;
		public List<int> paints;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Color Palette");
			Tooltip.SetDefault("For happy little accidents\n80% chance not to consume paint\nCan be right-clicked");
		}
		public override void SetDefaults()
		{
			paints = new List<int>();
			for (int i = 0; i < 27; i++) paints.Add(1073+i);//adding the item IDs from red paint to gray paint
			for (int i = 0; i < 3; i++) paints.Add(1966+i);//the last three, brown, shadow and negative
			item.width = 40;
			item.height = 40;
			item.useTime = 1;
			item.useAnimation = 10;
			item.useStyle = 1;
			item.value = Item.buyPrice(0, 50, 0, 0);
			item.rare = 5;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.noMelee = true;
			item.noUseGraphic = true;
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override void HoldItem (Player player)
		{
			//for showing the icon when an action is allowed
			VipixToolBoxPlayer myPlayer = player.GetModPlayer<VipixToolBoxPlayer>();
			toolRange = Math.Max(baseRange, myPlayer.fargoRange);//blocks
			if (Vector2.Distance(player.position , myPlayer.pointerCoord) < toolRange*16 && myPlayer.spotlight && VipixToolBoxWorld.toolEnabled["ColorPalette"])
			{
				Lighting.AddLight(myPlayer.pointerCoord, 2f, 2f, 2f);
			}
			if (Vector2.Distance(player.position , myPlayer.pointerCoord) < toolRange*16 &&
			(myPlayer.pointedTile.active() || myPlayer.pointedTile.wall != 0)&&
			VipixToolBoxWorld.toolEnabled["ColorPalette"])
			{
				player.showItemIcon = true;
				operationAllowed = true;
				switch (myPlayer.paintStatus)
				{
					case 0:
					player.showItemIcon2 = mod.ItemType("tilePaintIcon");
					break;
					case 1:
					player.showItemIcon2 = mod.ItemType("wallPaintIcon");
					break;
					case 2:
					player.showItemIcon2 = mod.ItemType("tileEraseIcon");
					break;
					case 3:
					player.showItemIcon2 = mod.ItemType("wallEraseIcon");
					break;
				}
			}
			else
			{
				player.showItemIcon = false;
				operationAllowed = false;
			}
		}
		public override bool CanUseItem(Player player)
		{
			VipixToolBoxPlayer myPlayer = player.GetModPlayer<VipixToolBoxPlayer>();
			if (player.altFunctionUse == 2)
			{
				//sending mouse coordinates (at the moment of the click) to VipixToolBoxPlayer for UI position
				myPlayer.tbMouseX = Main.mouseX;
				myPlayer.tbMouseY = Main.mouseY;
				ColorUI.visible = true;
			}
			else
			{
				if (operationAllowed)
				{
					int i = 0;//need to check for resource first
					bool found = false;
					bool operationDone = false;
					while (i < player.inventory.Length)
					{
						if (paints.Contains(player.inventory[i].type)) break;
						i++;
					}
					if (i < player.inventory.Length) found = true;
					switch (myPlayer.paintStatus)
					{
						case 0:
						if (found && myPlayer.pointedTile.color() != myPlayer.colorByte)
						{
							//no painting of already painted tiles (would consume paint)
							myPlayer.pointedTile.color(myPlayer.colorByte);
							if (Main.rand.NextDouble() <= 0.2) player.inventory[i].stack--;//80% not to consume paint
							operationDone = true;
						}
						break;
						case 1:
						if (found && myPlayer.pointedTile.wallColor() != myPlayer.colorByte)
						{
							myPlayer.pointedTile.wallColor(myPlayer.colorByte);
							if (Main.rand.NextDouble() <= 0.2) player.inventory[i].stack--;//80% not to consume paint
							operationDone = true;
						}
						break;
						case 2:
						if (myPlayer.pointedTile.color() != 0)
						{
							myPlayer.pointedTile.color(0);
							operationDone = true;
						}
						break;
						case 3:
						if (myPlayer.pointedTile.wallColor() != 0)
						{
							myPlayer.pointedTile.wallColor(0);
							operationDone = true;
						}
						break;
					}
					if (operationDone)
					{
						if (Main.netMode == 1) NetMessage.SendTileSquare(-1, myPlayer.pointedTileX, myPlayer.pointedTileY, 1);
						Main.PlaySound(SoundID.Dig);
						Vector2 dustSpawn;
						for (int j = 0;j < 10;j++)
						{
							dustSpawn = new Vector2(myPlayer.pointerCoord.X - 4,myPlayer.pointerCoord.Y - 4);//dunno why the offset looks better
							int dust = Dust.NewDust(dustSpawn, 15, 15, 33);//,0f, 0f, 0, new Color(255, 255, 255),1f);//80 is the type
						}
					}
				}
				//tile.color((byte)Paints.Red);
			}
			return false;
		}
		public enum Paints : byte
		{
			None,             //0    //sophisticated way to be able to call the colors by name (unused)
			Red,        //1073//1
			Orange,     //1074//2
			Yellow,     //1075//3
			Lime,       //1076//4
			Green,      //1077//5
			Teal,       //1078//6
			Cyan,       //1079//7
			SkyBlue,    //1080//8
			Blue,       //1081//9
			Purple,     //1082//10
			Violet,     //1083//11
			Pink,       //1084//12
			DeepRed,    //1085//13
			DeepOrange, //1086//14
			DeepYellow, //1087//15
			DeepLime,   //1088//16
			DeepGreen,  //1089//17
			DeepTeal,   //1090//18
			DeepCyan,   //1091//19
			DeepSkyBlue,//1092//20
			DeepBlue,   //1093//21
			DeepPurple, //1094//22
			DeepViolet, //1095//23
			DeepPink,   //1096//24
			Black,      //1097//25
			White,      //1098//26
			Gray,       //1099//27
			Brown,      //1966//28
			Shadow,     //1967//29
			Negative,   //1966//30
		}
	}
}
