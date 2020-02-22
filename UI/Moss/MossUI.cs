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
	public class MossUI : UIState
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
			panelWidth = 230f;
			panelHeight = 230f;
			buttonDimension = 42f;
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
			for (int i = 0; i < 7; i++)
			{
				Texture2D myButtonTexture = ModLoader.GetMod("VipixToolBox").GetTexture("UI/Moss/"+i.ToString());
				buttonList.Add(new UIImageButton(myButtonTexture));
				var x = Math.Cos(i * 2 * Math.PI/6);
				var y = Math.Sin(i * 2 * Math.PI/6);
				buttonList[i].Left.Set((float)Math.Round(x * 45f, 0) + panelWidth/2 - buttonDimension/2,0f);
				buttonList[i].Top.Set((float)Math.Round(y * 45f, 0) + panelHeight/2 - buttonDimension/2,0f);
				buttonList[i].Width.Set(buttonDimension, 0f);
				buttonList[i].Height.Set(buttonDimension, 0f);
				int index = i;//the next line doesnt work with i directly, and doesn't work if index is declared only once outside the loop
				buttonList[i].OnClick += (evt, element) => ButtonClicked(index);//that's called a lambda expression. Neat tool
				//the lambda expression is necessary to avoid having to create 1 method for each button
				backgroundPanel.Append(buttonList[i]);
			}
			buttonList[0].Left.Set(panelWidth/2 - buttonDimension/2,0f);
			buttonList[0].Top.Set(panelHeight/2 - buttonDimension/2,0f);
			base.Append(backgroundPanel);
			Recalculate();
		}
		public override void Update(GameTime gametime)
		{
			//setting the UI at the mouse position
			Mod myMod = ModLoader.GetMod("VipixToolBox");
			Player player = Main.player[Main.myPlayer];
			VipixToolBoxPlayer myPlayer = player.GetModPlayer<VipixToolBoxPlayer>();
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
			myPlayer.mossTool = index;
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
