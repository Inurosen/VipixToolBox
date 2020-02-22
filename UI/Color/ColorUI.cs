using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
//using Terraria.ObjectData;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;

namespace VipixToolBox.UI
{
	public class ColorUI : UIState
	{
		public static UIPanel backgroundPanel;
		public static bool visible = false;
		public float panelWidth;
		public float panelHeight;
		public float buttonDimension;
		public float padding;
		public float scaleX;
		public float scaleY;
		List<UIImageButton> colorList;
		List<UIImageButton> disabledList;
		List<UIImageButton> toolList;
		List<bool> activated;

		public override void OnInitialize()
		{
			panelWidth = 340f;
			panelHeight = 340f;
			buttonDimension = 32f;
			padding = 30f;
			activated = new List<bool>();
			for (int i = 0; i < 31; i++) activated.Add(true);

			backgroundPanel = new UIPanel();
			backgroundPanel.SetPadding(0);
			backgroundPanel.Left.Set((float)Main.screenWidth / 2 - panelWidth / 2, 0f);
			backgroundPanel.Top.Set((float)Main.screenHeight / 2 - panelHeight / 2, 0f);
			backgroundPanel.Width.Set(panelWidth, 0f);
			backgroundPanel.Height.Set(panelHeight, 0f);
			backgroundPanel.BackgroundColor = new Color(0, 0, 0, 0);
			backgroundPanel.BorderColor = new Color(0, 0, 0, 0);
			backgroundPanel.OnMouseDown += new UIElement.MouseEvent(DragStart);
			backgroundPanel.OnMouseUp += new UIElement.MouseEvent(DragEnd);
			//preparing
			colorList = new List<UIImageButton>();
			disabledList = new List<UIImageButton>();
			toolList = new List<UIImageButton>();
			Texture2D buttonTexture;
			//color buttons
			buttonTexture = ModLoader.GetMod("VipixToolBox").GetTexture("UI/Color/null");
			colorList.Add(new UIImageButton(buttonTexture));
			disabledList.Add(new UIImageButton(ModLoader.GetMod("VipixToolBox").GetTexture("UI/Color/no")));
			colorList[0].Left.Set(0f,0f);
			colorList[0].Top.Set(0f,0f);

			for (int i = 1; i < 31; i++)
			{
				buttonTexture = ModLoader.GetMod("VipixToolBox").GetTexture("UI/Color/" + i.ToString());
				colorList.Add(new UIImageButton(buttonTexture));
				disabledList.Add(new UIImageButton(ModLoader.GetMod("VipixToolBox").GetTexture("UI/Color/no")));
			}
			//
			//positions generated mathematically
			for (int i = 1; i < 13; i++)
			{
				double x = Math.Cos(Math.PI - (i-1) * Math.PI/11);
				double y = Math.Sin((i-1) * Math.PI/11);
				x = (float)Math.Round(x * 110,0) + panelWidth/2 - buttonDimension/2;
				y = panelHeight/2 - (float)Math.Round(y * 110f , 0) - buttonDimension/2;
				colorList[i].Left.Set((float)x,0f);
				colorList[i].Top.Set((float)y,0f);
				disabledList[i].Left.Set((float)x,0f);
				disabledList[i].Top.Set((float)y,0f);

			}
			for (int i = 13; i < 25; i++)
			{
				double x = Math.Cos(Math.PI - (i-13) * Math.PI/11);
				double y = Math.Sin((i-13) * Math.PI/11);
				x = (float)Math.Round(x * 140,0) + panelWidth/2 - buttonDimension/2;
				y = panelHeight/2 - (float)Math.Round(y * 140,0) - buttonDimension/2;
				colorList[i].Left.Set((float)x,0f);
				colorList[i].Top.Set((float)y,0f);
				disabledList[i].Left.Set((float)x,0f);
				disabledList[i].Top.Set((float)y,0f);
			}
			for (int i = 25; i < 31; i++)
			{
				double x = Math.Cos(Math.PI - (i-25) * Math.PI/5);
				double y = Math.Sin((i-25) * Math.PI/5);
				x = (float)Math.Round(x * 65,0) + panelWidth/2 - buttonDimension/2;
				y = panelHeight/2 - (float)Math.Round(y * 65,0) - buttonDimension/2;
				colorList[i].Left.Set((float)x,0f);
				colorList[i].Top.Set((float)y,0f);
				disabledList[i].Left.Set((float)x,0f);
				disabledList[i].Top.Set((float)y,0f);
			}
			//tool buttons done 1 by 1 because of positions
			int j = 0;
			buttonTexture = ModLoader.GetMod("VipixToolBox").GetTexture("UI/Color/tilePaint");
			toolList.Add(new UIImageButton(buttonTexture));
			toolList[(int)j].Left.Set(panelWidth/5 - 24f, 0f);
			toolList[(int)j].Top.Set(panelHeight/2 + 30f, 0f);

			j++;
			buttonTexture = ModLoader.GetMod("VipixToolBox").GetTexture("UI/Color/wallPaint");
			toolList.Add(new UIImageButton(buttonTexture));
			toolList[(int)j].Left.Set(panelWidth/5*2 - 24f, 0f);
			toolList[(int)j].Top.Set(panelHeight/2 + 30f, 0f);

			j++;
			buttonTexture = ModLoader.GetMod("VipixToolBox").GetTexture("UI/Color/tileErase");
			toolList.Add(new UIImageButton(buttonTexture));
			toolList[j].Left.Set(panelWidth/5*3 - 24f, 0f);
			toolList[j].Top.Set(panelHeight/2 + 30f, 0f);

			j++;
			buttonTexture = ModLoader.GetMod("VipixToolBox").GetTexture("UI/Color/wallErase");
			toolList.Add(new UIImageButton(buttonTexture));
			toolList[j].Left.Set(panelWidth/5*4 - 24f, 0f);
			toolList[j].Top.Set(panelHeight/2 + 30f, 0f);

			j++;
			buttonTexture = ModLoader.GetMod("VipixToolBox").GetTexture("UI/Color/spotlight");
			toolList.Add(new UIImageButton(buttonTexture));
			toolList[j].Left.Set(panelWidth/2 - 24f, 0f);
			toolList[j].Top.Set(panelHeight/2 + 90f, 0f);
			//adding all the buttonclicked methods and appending
			for (int i = 0; i < colorList.Count; i++)
			{
				int index = i;
				backgroundPanel.Append(colorList[i]);
				colorList[i].OnClick += (evt, element) => ColorClicked(index);
				colorList[i].OnRightClick += (evt, element) => DisableColor(index);
				disabledList[i].OnRightClick += (evt, element) => EnableColor(index);
			}
			backgroundPanel.RemoveChild(colorList[0]);//erasing is made with eraser, not "null paint"
			for (int i = 0; i < toolList.Count; i++)
			{
				int index = i;
				toolList[i].OnClick += (evt, element) => ToolClicked(index);
				backgroundPanel.Append(toolList[i]);
			}
			base.Append(backgroundPanel);
			Recalculate();
		}
		public override void Update(GameTime gametime)
		{
			//setting the palette at the mouse position
			Mod myMod = ModLoader.GetMod("VipixToolBox");
			Player player = Main.player[Main.myPlayer];
			VipixToolBoxPlayer myPlayer = player.GetModPlayer<VipixToolBoxPlayer>();
			if (myPlayer.centerUI == 1)
			{
				backgroundPanel.Left.Set(myPlayer.tbMouseX - panelWidth/2 ,0f);//exceeding the coordinates of the screen seems already handled
				backgroundPanel.Top.Set(myPlayer.tbMouseY - panelHeight/2,0f);
				//Main.NewText(colorList.Count.ToString(), 100, 110, 75, false);
				Recalculate(); //without it nothing happens
			}
			else if (myPlayer.centerUI == 0)
			{
				backgroundPanel.Left.Set((float)Main.screenWidth / 2 - panelWidth / 2, 0f);
				backgroundPanel.Top.Set((float)Main.screenHeight / 2 - panelHeight / 2, 0f);
				Recalculate();
			}
		}

		public void ColorClicked(int index)
		{
			//Main.NewText(colorList[index].Left.Get().ToString(),255,255,255);
			//colorList[index].UIImageButtonTexture.Set();
			Mod myMod = ModLoader.GetMod("VipixToolBox");
			Player player = Main.player[Main.myPlayer];
			VipixToolBoxPlayer myPlayer = player.GetModPlayer<VipixToolBoxPlayer>();
			//color button
			player.GetModPlayer<VipixToolBoxPlayer>().colorByte = (byte)index;
			visible = false;
		}
		public void DisableColor(int index)
		{
			backgroundPanel.RemoveChild(colorList[index]);
			backgroundPanel.Append(disabledList[index]);
		}
		public void EnableColor(int index)
		{
			backgroundPanel.RemoveChild(disabledList[index]);
			backgroundPanel.Append(colorList[index]);
		}
		public void ToolClicked(int index)
		{
			Mod myMod = ModLoader.GetMod("VipixToolBox");
			Player player = Main.player[Main.myPlayer];
			VipixToolBoxPlayer myPlayer = player.GetModPlayer<VipixToolBoxPlayer>();
			if (index >=0 && index < 4)
			{
				//tool button
				player.GetModPlayer<VipixToolBoxPlayer>().paintStatus = index;
			}
			else if (index == 4)
			{
				myPlayer.spotlight = !myPlayer.spotlight;
			}
			visible = false;
		}
		Vector2 offset;
		public bool dragging = false;
		private void DragStart(UIMouseEvent evt, UIElement listeningElement)
		{
			Mod myMod = ModLoader.GetMod("VipixToolBox");
			Player player = Main.player[Main.myPlayer];
			VipixToolBoxPlayer myPlayer = player.GetModPlayer<VipixToolBoxPlayer>();
			if (myPlayer.centerUI == 2)
			{
				offset = new Vector2(evt.MousePosition.X - backgroundPanel.Left.Pixels, evt.MousePosition.Y - backgroundPanel.Top.Pixels);
				dragging = true;
			}
		}

		private void DragEnd(UIMouseEvent evt, UIElement listeningElement)
		{
			Vector2 end = evt.MousePosition;
			dragging = false;

			backgroundPanel.Left.Set(end.X - offset.X, 0f);
			backgroundPanel.Top.Set(end.Y - offset.Y, 0f);

			Recalculate();
		}
		////////////////////////////////////no edit after that/////////////////////////////////////////////////////////

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Vector2 MousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
			if (backgroundPanel.ContainsPoint(MousePosition))
			{
				Main.LocalPlayer.mouseInterface = true;
			}
			if (dragging)
			{
				backgroundPanel.Left.Set(MousePosition.X - offset.X, 0f);
				backgroundPanel.Top.Set(MousePosition.Y - offset.Y, 0f);
				Recalculate();
			}
		}
	}
}
