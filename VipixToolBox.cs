using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using VipixToolBox.UI;

namespace VipixToolBox
{
	class VipixToolBox : Mod
	{
		private UserInterface colorUserInterface;
		private UserInterface hammerUserInterface;
		private UserInterface blockUserInterface;
		private UserInterface mossUserInterface;
		internal ColorUI colorUI;
		internal HammerUI hammerUI;
		internal BlockUI blockUI;
		internal MossUI mossUI;
		public static Dictionary<string, bool> toolEnabled;

		public VipixToolBox()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
			toolEnabled = new Dictionary<string, bool>();
			toolEnabled.Add("all",false);
			toolEnabled.Add("AutoHammer",true);
			toolEnabled.Add("BlockWand",true);
			toolEnabled.Add("ColorPalette",true);
			toolEnabled.Add("LevitationWand",true);
			toolEnabled.Add("RattlesnakeWand",true);
			toolEnabled.Add("StaffofRegrowthEdit",true);
			toolEnabled.Add("WallHammer",true);
		}
		public override void Load()
		{
			//Activation of the UIs
			colorUI = new ColorUI();
			hammerUI = new HammerUI();
			blockUI = new BlockUI();
			mossUI = new MossUI();
			colorUI.Activate();
			hammerUI.Activate();
			blockUI.Activate();
			mossUI.Activate();
			colorUserInterface = new UserInterface();
			hammerUserInterface = new UserInterface();
			blockUserInterface = new UserInterface();
			mossUserInterface = new UserInterface();
			blockUserInterface.SetState(blockUI);
			colorUserInterface.SetState(colorUI);
			hammerUserInterface.SetState(hammerUI);
			mossUserInterface.SetState(mossUI);
		}
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (MouseTextIndex != -1)
			{
				layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
				"Vipix Toolbox",
				delegate
				{
					if (ColorUI.visible)
					{
						colorUserInterface.Update(Main._drawInterfaceGameTime);	//I don't understand
						colorUI.Draw(Main.spriteBatch);
					}
					if (HammerUI.visible)
					{
						hammerUserInterface.Update(Main._drawInterfaceGameTime);	//I don't understand
						hammerUI.Draw(Main.spriteBatch);
					}
					if (BlockUI.visible)
					{
						blockUserInterface.Update(Main._drawInterfaceGameTime);	//I don't understand
						blockUI.Draw(Main.spriteBatch);
					}
					if (MossUI.visible)
					{
						mossUserInterface.Update(Main._drawInterfaceGameTime);
						mossUI.Draw(Main.spriteBatch);
					}
					return true;
					},
					InterfaceScaleType.UI)
					);
				}
			}
		}
	}
