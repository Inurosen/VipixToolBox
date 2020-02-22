using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;//TileObjectData
using Terraria.DataStructures;//PlacementHook
using Terraria.Enums;//AnchorType
using static Terraria.ModLoader.ModContent;

namespace VipixToolBox.Tiles
{
	public class FlyingBlockTile : ModTile
	{
		private FlyingBlockTE myTE;

		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = false;
			Main.tileWaterDeath[Type] = false;
			Main.tileLavaDeath[Type] = false;

			TileObjectData.newTile.CoordinateHeights = new int[]{ 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleMultiplier = 27;
			TileObjectData.newTile.StyleWrapLimit = 27;
			TileObjectData.newTile.UsesCustomCanPlace = false;
			TileObjectData.newTile.LavaDeath = true;

			//TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			//dustType = ;
			//drop = mod.ItemType("FlyingBlock");
			//AddMapEntry(new Color(200, 200, 200));
			TileObjectData.newTile.HookCheck = new PlacementHook(mod.GetTileEntity("FlyingBlockTE").Hook_AfterPlacement, -1, 0, false);
			//TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<FlyingBlockTE>().Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);
		}
		public override void PlaceInWorld (int i, int j, Item item)
		{
			//in case the block is placed manually
			int id = ModContent.GetInstance<FlyingBlockTE>().Find(i, j);
			//looking for the ID of the tileEntity we just placed
			//because there is no tileEntity hook for after placement
			if (id != -1)
			{
				//it should never be -1
				myTE = (FlyingBlockTE)TileEntity.ByID[id];
				myTE.timer = 120;
				myTE.tileX = i;
				myTE.tileY = j;
			}
		}
		public override bool NewRightClick(int i, int j)
		{
            return false;
            /*
			int id = mod.GetTileEntity<FlyingBlockTE>().Find(i, j);
			if (id != -1)
			{
				myTE = (FlyingBlockTE)TileEntity.ByID[id];
				Main.NewText(myTE.timer.ToString(),255,255,255);
			}
			else Main.NewText("Nothing",255,255,255);*/
		}
	}
}
