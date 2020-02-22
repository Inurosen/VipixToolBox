using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;

namespace VipixToolBox.UI
{
	public class HammerUI : UIState
	{
		public static UIPanel backgroundPanel;
		public static bool visible = false;
		public float panelWidth;
		public float panelHeight;
		public float buttonDimension;
		public float padding;
		public float scaleX;
		public float scaleY;
		List<UIImageButton> buttonList;

		public override void OnInitialize()
		{
			panelWidth = 130f;
			panelHeight = 130f;
			buttonDimension = 38f;
			padding = 36f;

			backgroundPanel = new UIPanel();
			backgroundPanel.SetPadding(0);
			backgroundPanel.Left.Set((float)Main.screenWidth / 2 - panelWidth / 2, 0f);
			backgroundPanel.Top.Set((float)Main.screenHeight / 2 - panelHeight / 2, 0f);
			backgroundPanel.Width.Set(panelWidth, 0f);
			backgroundPanel.Height.Set(panelHeight, 0f);
			backgroundPanel.BackgroundColor = new Color(0, 0, 0, 0);
			backgroundPanel.BorderColor = new Color(0, 0, 0, 0);

			buttonList = new List<UIImageButton>();
			Texture2D buttonTexture;
			for (int i = 0; i < 6; i++)
			{
				buttonTexture = ModLoader.GetMod("VipixToolBox").GetTexture("UI/Hammer/"+i.ToString());
				buttonList.Add(new UIImageButton(buttonTexture));
				backgroundPanel.Append(buttonList[i]);
			}
			buttonList[0].Left.Set(panelWidth / 2 + padding - buttonDimension / 2,0f);//bottom left slope
			buttonList[0].Top.Set(panelHeight / 2 + padding - buttonDimension / 2,0f);

			buttonList[1].Left.Set(panelWidth / 2 + padding - buttonDimension / 2,0f);
			buttonList[1].Top.Set(panelHeight / 2 - padding - buttonDimension / 2,0f);

			buttonList[2].Left.Set(panelWidth / 2 - padding - buttonDimension / 2,0f);
			buttonList[2].Top.Set(panelHeight / 2 + padding- buttonDimension / 2,0f);

			buttonList[3].Left.Set(panelWidth / 2 - padding - buttonDimension / 2,0f);//top right slope
			buttonList[3].Top.Set(panelHeight / 2 - padding - buttonDimension / 2,0f);

			buttonList[4].Left.Set(panelWidth / 2 + padding - buttonDimension / 2,0f);//halfblock
			buttonList[4].Top.Set(panelHeight / 2 - buttonDimension / 2,0f);

			buttonList[5].Left.Set(panelWidth / 2 - buttonDimension / 2,0f);//reset
			buttonList[5].Top.Set(panelHeight / 2 - padding - buttonDimension / 2,0f);

			//adding all the buttonclicked methods
			for (int i = 0; i < buttonList.Count; i++)
			{
				int index = i;
				buttonList[i].OnClick += (evt, element) => ButtonClicked(index);
			}
			base.Append(backgroundPanel);
			Recalculate();
		}
		public override void Update(GameTime gametime)
		{
			//setting the UI at the mouse position
			Mod myMod = ModLoader.GetMod("VipixToolBox");
			Player player = Main.player[Main.myPlayer];
			VipixToolBoxPlayer myPlayer = player.GetModPlayer<VipixToolBoxPlayer>();//well that looks complicated
			if (myPlayer.centerUI == 1)
			{
				backgroundPanel.Left.Set(myPlayer.tbMouseX - panelWidth/2 ,0f);//exceeding the coordinates of the screen seems already handled
				backgroundPanel.Top.Set(myPlayer.tbMouseY - panelHeight/2,0f);
				//Main.NewText(buttonList.Count.ToString(), 100, 110, 75, false);
				Recalculate(); //without it nothing happens
			}
			else
			{
				backgroundPanel.Left.Set((float)Main.screenWidth / 2 - panelWidth / 2, 0f);
				backgroundPanel.Top.Set((float)Main.screenHeight / 2 - panelHeight / 2, 0f);
				Recalculate();
			}
		}
		public void ButtonClicked(int index)
		{
			Mod myMod = ModLoader.GetMod("VipixToolBox");
			Player player = Main.player[Main.myPlayer];
			VipixToolBoxPlayer myPlayer = player.GetModPlayer<VipixToolBoxPlayer>();
			Tile tile = Main.tile[myPlayer.tileX,myPlayer.tileY];
			//the tile change is done here instead of useItem otherwise you wouldnt have the time to click on the button
			switch (index)
			{
				case 0:
				tile.halfBrick(false);
				tile.slope(1);//slope bottom left
				break;
				case 1:
				tile.halfBrick(false);
				tile.slope(2);//slope bottom right
				break;
				case 2:
				tile.halfBrick(false);
				tile.slope(3);//slope top left
				break;
				case 3:
				tile.halfBrick(false);
				tile.slope(4);//slope top right
				break;
				case 4:
				tile.slope(0);
				tile.halfBrick(true);
				break;
				case 5:
				tile.halfBrick(false);
				tile.slope(0);
				break;
			}
			WorldGen.SquareTileFrame(myPlayer.tileX,myPlayer.tileY, true);
			if (Main.netMode == 1) NetMessage.SendTileSquare(-1, myPlayer.pointedTileX, myPlayer.pointedTileY, 1);
			Main.PlaySound(SoundID.Dig);//hammer sound too
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(new Vector2((myPlayer.tileX-1) * 16,(myPlayer.tileY-1) * 16), 40, 40, myMod.DustType("Sparkle"));
				//I don't know how to change the color according to the block (white dust for snow) SIMPLY
			}
			visible = false;
		}

		////////////////////////////////////no edit after that/////////////////////////////////////////////////////////

		protected override void DrawSelf(SpriteBatch spriteBatch) //I think this steals the priority of the click when inside the UI box (doesnt call the object function when clicking on button)
		{
			Vector2 MousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
			if (backgroundPanel.ContainsPoint(MousePosition))
			{
				Main.LocalPlayer.mouseInterface = true;
			}
		}
	}
}
