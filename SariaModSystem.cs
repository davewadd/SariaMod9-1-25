using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using SariaMod;
using SariaMod.Buffs;
using Terraria.ModLoader.IO;
using SariaMod.Gores;
namespace SariaMod
{
    public class SariaModSystem : ModSystem
    {
        public static bool CustomRainSoundIsPlaying = false;
        public override void PostUpdateWorld()
        {
            // Only execute this on the client
            if (Main.dedServ)
                return;
            // If a custom rain sound is playing, mute the vanilla rain sound.
            if (CustomRainSoundIsPlaying)
            {
                Main.ambientVolume = 0f;
            }
            else
            {
                // Otherwise, restore the vanilla volume.
                // We'll fade it back in smoothly.
                Main.ambientVolume = Utils.Clamp(Main.ambientVolume + 0.01f, 0f, 1f);
            }
        }
        public override void OnWorldUnload()
        {
            // Get the ModPlayer instance for the local player.
            // This is necessary because ModSystem does not have a direct reference to a Player instance.
            if (Main.LocalPlayer.TryGetModPlayer(out FairyPlayerMiscEffects modPlayer))
            {
                // Call a public method on the ModPlayer to clean up its sounds.
                modPlayer.StopAllLoopedSounds();
            }
            // Also reset the ambient volume and flag when unloading the world
            Main.ambientVolume = 1f;
            CustomRainSoundIsPlaying = false;
        }
    }
}