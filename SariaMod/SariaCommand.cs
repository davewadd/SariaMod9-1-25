using Terraria;
using Terraria.ModLoader;
using Terraria.ID; 
using Microsoft.Xna.Framework;
namespace SariaMod
{
    public class SariaSyncCommand : ModCommand
    {
        public override string Command => "saria_sync";
        public override CommandType Type => CommandType.Server | CommandType.Chat;
        public override string Description => "Forces a sync of SariaLevel and SariaXp for all players.";
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                Main.NewText("Forcing SariaLevel and SariaXp sync for all players...", Color.LightGreen);
                // Call the SyncPlayer method for each player to send a fresh packet
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    if (player.active)
                    {
                        FairyPlayer modPlayer = player.GetModPlayer<FairyPlayer>();
                        if (modPlayer != null)
                        {
                            modPlayer.SyncPlayer(-1, i, false);
                        }
                    }
                }
            }
        }
    }
}
