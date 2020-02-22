using VipixToolBox;
using Terraria;
using Terraria.ModLoader;

namespace VipixToolBox.Commands
{
	public class CenterUI : ModCommand
	{
		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Command
		{
			get { return "centerUI"; }
		}

		public override string Description
		{
			get { return "Control interfaces position"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			Mod myMod = ModLoader.GetMod("VipixToolBox");
			Player player = Main.player[Main.myPlayer];
			VipixToolBoxPlayer myPlayer = player.GetModPlayer<VipixToolBoxPlayer>();
			/*
			bool choice;
			if (!bool.TryParse(args[0], out choice))
				throw new UsageException(args[0] + " is not a valid value\ncenterUI accepts <true|false>");
			else	myPlayer.centerUI = choice;*/
			if (args[0] == "fixed") myPlayer.centerUI = 0;
			else if (args[0] == "mouse") myPlayer.centerUI = 1;
			else if (args[0] == "free") myPlayer.centerUI = 2;
			else throw new UsageException(args[0] + " is not a valid value\ncenterUI accepts <fixed|mouse|free>");
		}
	}
}
