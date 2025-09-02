using Terraria;
using Terraria.ModLoader;
namespace SariaMod.MusicChanges
{
    public class PeaceTheme : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player) => (Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].Fairy().CalmMind == true);
        public override SceneEffectPriority Priority => SceneEffectPriority.Event;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/A_Lonely_Figure");
    }
}