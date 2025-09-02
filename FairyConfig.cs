using Terraria.ModLoader.Config;
namespace SariaMod
{
    public class FairyConfig : ModConfig
    {
        public static FairyConfig Instance;
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public bool Afterimages { get; }
    }
}
