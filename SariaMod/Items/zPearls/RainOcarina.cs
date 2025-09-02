using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
namespace SariaMod.Items.zPearls
{
    public class RainOcarina : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("RainOcarina");
            Tooltip.SetDefault("Causes Rain");
        }
        public override void SetDefaults()
        {
            base.Item.width = 26;
            base.Item.height = 22;
            base.Item.maxStack = 1;
            Item.useTime = 140;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            base.Item.value = 0;
        }
        public override bool? UseItem(Player player)
        {
            SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/SongCorrect"));
            // Use the static instance of your mod to get the packet,
            // as 'Mod.GetPacket()' is a non-static method.
            ModPacket packet = SariaMod.Instance.GetPacket();
            // Write the StartRain message type to the packet.
            packet.Write((byte)SariaMod.SoundMessageType.StartRain);
            // Send the packet.
            // On a client, this sends the packet to the server.
            // On a server, it is sent to all clients (not necessary here, as we only need the server's logic).
            packet.Send();
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .Register();
        }
    }
}