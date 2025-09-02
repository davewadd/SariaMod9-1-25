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
using SariaMod.Items.Strange;
namespace SariaMod
{
    public class SariaMod : Mod
    {
        public static SariaMod Instance { get; private set; }
        public override void Load()
        {
            // Set the static instance when the mod is loaded.
            Instance = this;
        }
        public override void Unload()
        {
            // Set the static instance back to null when the mod is unloaded.
            // This is good practice to prevent memory leaks and issues on reload.
            Instance = null;
        }
        public enum SoundMessageType : byte
        {
            PlaySound,
            RemoveBuff,
            PlayFrozenHitEffect,
            SyncRainSoundState,
            StartRain,
            SyncBuff,
            SyncProjectileState,
            SyncSariaLevel,
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            SoundMessageType type = (SoundMessageType)reader.ReadByte();
            if (type == SoundMessageType.PlaySound)
            {
                int npcWhoAmI = reader.ReadInt32();
                int soundIndex = reader.ReadInt32();
                NPC npc = Main.npc[npcWhoAmI];
                PlaySound(npc.Center, soundIndex);
            }
            else if (type == SoundMessageType.RemoveBuff) // Handle the new buff removal message
            {
                int npcWhoAmI = reader.ReadInt32();
                if (Main.netMode == NetmodeID.Server)
                {
                    // On the server, apply the removal.
                    NPC npc = Main.npc[npcWhoAmI];
                    int buffIndex = npc.FindBuffIndex(ModContent.BuffType<EnemyFrozen>());
                    if (buffIndex != -1)
                    {
                        npc.DelBuff(buffIndex);
                        npc.netUpdate = true; // Sync the buff removal to all clients.
                    }
                }
            }
            else if (type == SoundMessageType.PlayFrozenHitEffect)
            {
                int npcWhoAmI = reader.ReadInt32();
                NPC npc = Main.npc[npcWhoAmI];
                if (npc.active)
                {
                    // Play sound and create gore on the client
                    int backGoreType = ModContent.GoreType<IceGore2>();
                    for (int G = 0; G < 3; G++)
                    {
                        Gore B = Gore.NewGorePerfect(npc.GetSource_FromThis(), npc.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), backGoreType, 2f);
                        B.light = .5f;
                        SoundEngine.PlaySound(SoundID.Item27, npc.Center);
                    }
                }
            }
            else if (type == SoundMessageType.SyncRainSoundState)
            {
                bool newRainState = reader.ReadBoolean();
                // This ensures the packet is handled on the correct side.
                // In this case, the packet is sent from the server/host to clients.
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    // Find the local player's ModPlayer instance.
                    FairyPlayerMiscEffects modPlayer = Main.player[Main.myPlayer].GetModPlayer<FairyPlayerMiscEffects>();
                    // Call the method in your ModPlayer to update the rain state.
                    modPlayer.ReceiveRainSoundState(newRainState);
                }
            }
            else if (type == SoundMessageType.StartRain)
            {
                // The server receives the request and then broadcasts the rain state change.
                if (Main.raining)
                {
                    Main.StopRain();
                    Main.NewText("The storm passes for now.", 50, 100, 150);
                }
                else
                {
                    Main.StartRain();
                    Main.NewText("Another Storm! You played the Ocarina again didn't you?", 50, 100, 150);
                }
            }
            else if (type == SoundMessageType.SyncBuff)
            {
                int playerIndex = reader.ReadInt32();
                int buffType = reader.ReadInt32();
                int buffTime = reader.ReadInt32();
                // Ensure the buff is only added on the client that received the packet
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    // Apply the buff to the correct player
                    if (Main.player[playerIndex].active)
                    {
                        // The 'quiet' flag prevents the buff from trying to resend a network packet
                        // which would cause an infinite loop.
                        Main.player[playerIndex].AddBuff(buffType, buffTime, quiet: true);
                    }
                }
            }
            else if (type == SariaMod.SoundMessageType.SyncProjectileState)
            {
                int projWhoAmI = reader.ReadInt32();
                int frame = reader.ReadInt32();
                int direction = reader.ReadInt32();
                int frameCounter = reader.ReadInt32(); // ADD THIS LINE
                if (Main.netMode == NetmodeID.MultiplayerClient && projWhoAmI >= 0 && projWhoAmI < Main.maxProjectiles)
                {
                    Projectile projectile = Main.projectile[projWhoAmI];
                    if (projectile.active && projectile.type == ModContent.ProjectileType<Saria>())
                    {
                        projectile.frame = frame;
                        projectile.spriteDirection = direction;
                        projectile.frameCounter = frameCounter; // ADD THIS LINE
                    }
                }
            }
            else if (type == SoundMessageType.SyncSariaLevel)
            {
                int playerIndex = reader.ReadInt32();
                int sariaLevel = reader.ReadInt32();
                int sariaXp = reader.ReadInt32();
                if (playerIndex >= 0 && playerIndex < Main.player.Length)
                {
                    FairyPlayer modPlayer = Main.player[playerIndex].GetModPlayer<FairyPlayer>();
                    if (modPlayer != null)
                    {
                        modPlayer.Sarialevel = sariaLevel;
                        modPlayer.SariaXp = sariaXp;
                    }
                    // If we are the server, re-broadcast the update to all other players
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ModPacket packet = Instance.GetPacket();
                        packet.Write((byte)SoundMessageType.SyncSariaLevel);
                        packet.Write(playerIndex);
                        packet.Write(sariaLevel);
                        packet.Write(sariaXp);
                        packet.Send(-1, whoAmI);
                    }
                }
            }
        }
        public static SariaMod GetInstance()
        {
            return ModContent.GetInstance<SariaMod>();
        }
        // Generic helper method to play any sound based on its index.
        public static void PlaySound(Vector2 position, int soundIndex)
        {
            string soundPath;
            if (soundIndex == 6)
            {
                soundPath = "SariaMod/Sounds/Death6";
            }
            else if (soundIndex == 7)
            {
                soundPath = "SariaMod/Sounds/BeatMe";
            }
            else 
            {
                soundPath = "SariaMod/Sounds/Die" + soundIndex;
            }
            SoundEngine.PlaySound(new SoundStyle(soundPath), position);
        }
        public static void PlayFrozenHitEffect(int npcWhoAmI)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket packet = Instance.GetPacket();
                packet.Write((byte)SoundMessageType.PlayFrozenHitEffect);
                packet.Write(npcWhoAmI);
                packet.Send(-1, -1); // Send to all clients
            }
        }
    }
}