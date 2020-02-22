using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace VipixToolBox.Items
{
	public class WallHammer : ModItem
	{
		public int baseRange = 10;
		public int toolRange;
		public bool operationAllowed;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wall Hammer");
			Tooltip.SetDefault("Better calm your nerves on walls rather than enemies");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.WoodenSword);//not inspired for weapon stats
			item.damage = 8;
			item.width = 40;
			item.height = 40;
			item.useTime = 7;
			item.useAnimation = 7;

			item.value = Item.buyPrice(0, 15, 0, 0);
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}
		public override void HoldItem (Player player)
		{
			VipixToolBoxPlayer myPlayer = player.GetModPlayer<VipixToolBoxPlayer>();
			toolRange = Math.Max(baseRange, myPlayer.fargoRange);//blocks
			if (Vector2.Distance(player.position, myPlayer.pointerCoord) < toolRange*16 && VipixToolBoxWorld.toolEnabled["WallHammer"])
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
		public override bool UseItem(Player player)
		{
			VipixToolBoxPlayer myPlayer = player.GetModPlayer<VipixToolBoxPlayer>();
			if (operationAllowed)
			{
				WorldGen.KillWall(myPlayer.pointedTileX, myPlayer.pointedTileY, false);
				if (Main.netMode == 1) NetMessage.SendTileSquare(-1, myPlayer.pointedTileX, myPlayer.pointedTileY, 1);
			}
			//no smartcursor
			//meh no control
			return true;
		}
	}
}
