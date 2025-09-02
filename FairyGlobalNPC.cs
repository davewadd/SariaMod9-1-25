using SariaMod.Items.Bands;
using SariaMod.Items.zPearls;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SariaMod.Items.Emerald;
using Microsoft.Xna.Framework;
using SariaMod.Buffs;
using SariaMod.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;
using Terraria.DataStructures;
using System;
using SariaMod.Items.Ruby;
using System.Collections.Generic;
using SariaMod.Items.Amber;
using SariaMod.Items.Amethyst;
using SariaMod.Items.Sapphire;
using SariaMod.Items.Topaz;
using SariaMod.Items.zTalking;
using Terraria.Localization;
using Terraria.Map;
using System.IO;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics;
using Terraria.ObjectData;
using Terraria.ModLoader.IO;
using SariaMod.Gores;
namespace SariaMod
{
    public class FairyGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        private readonly int buffToRemove = ModContent.BuffType<EnemyFrozen>();
        private static bool[] hasBuffSynced = new bool[Main.maxNPCs];
        public bool SariaCurseD;
        public bool Burning2;
        public bool GhostBurning;
        public bool Stronger;
        public bool Frostburn2;
        public override void ResetEffects(NPC npc)
        {
            SariaCurseD = false;
            Burning2 = false;
            GhostBurning = false;
            Frostburn2 = false;
            Stronger = false;
        }
        public int RandomSize { get; private set; }
        public override void SetDefaults(NPC npc)
        {
            RandomSize = Main.rand.Next(9, 12);
            // Add a check to ensure the index is within the array's bounds.
            if (npc.whoAmI >= 0 && npc.whoAmI < hasBuffSynced.Length)
            {
                hasBuffSynced[npc.whoAmI] = false;
            }
        }
        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            binaryWriter.Write(RandomSize);
            // We want to send all buffs, not just the custom one.
            for (int i = 0; i < NPC.maxBuffs; i++)
            {
                if (npc.buffType[i] > 0)
                {
                    bitWriter.WriteBit(true);
                    binaryWriter.Write(npc.buffType[i]);
                    binaryWriter.Write(npc.buffTime[i]);
                }
                else
                {
                    bitWriter.WriteBit(false);
                }
            }
        }
        // This hook is called on the client to read the packet.
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            RandomSize = binaryReader.ReadInt32();
            // Read buff sync logic for the joining player
            for (int i = 0; i < NPC.maxBuffs; i++)
            {
                if (bitReader.ReadBit())
                {
                    npc.buffType[i] = binaryReader.ReadInt32();
                    npc.buffTime[i] = binaryReader.ReadInt32();
                }
                else
                {
                    npc.buffType[i] = 0;
                }
            }
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (Frostburn2)
            {
                // These lines zero out any positive lifeRegen. This is expected for all bad life regeneration effects.
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 16;
                if (damage < (npc.lifeMax * .005f))
                {
                    damage = (int)((npc.lifeMax * .005f) + 1);
                }
            }
            if (Burning2)
            {
                // These lines zero out any positive lifeRegen. This is expected for all bad life regeneration effects.
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 16;
                if (damage < (npc.lifeMax * .005f))
                {
                    damage = (int)((npc.lifeMax * .005f) + 1);
                }
                if (SariaCurseD)
                {
                    if (npc.lifeRegen > 0)
                    {
                        npc.lifeRegen = 0;
                    }
                    npc.lifeRegen -= 16;
                    if (damage < (npc.lifeMax * .01f))
                    {
                        damage = (int)((npc.lifeMax * .01f) + 1);
                    }
                    if (!npc.boss)
                    {
                        npc.noTileCollide = false;
                        npc.noGravity = false;
                    }
                }
            }
            if (GhostBurning)
            {
                // These lines zero out any positive lifeRegen. This is expected for all bad life regeneration effects.
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 16;
                if (damage < (npc.lifeMax * .005f))
                {
                    damage = (int)((npc.lifeMax * .005f) + 1);
                }
            }
        }
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            // Define special event flags that should not be calmed down completely.
            bool bloodMoonIsActive = Main.bloodMoon;
            bool eclipseIsActive = Main.eclipse;
            bool majorEventIsActive = Main.invasionType > 0 || player.ZoneDungeon; // Add other events here as needed
            // Check high-priority override buffs first.
            if (SariaModUtilities.Fairy(player).CalmMind)
            {
                ApplyCalmMind(player, ref spawnRate, ref maxSpawns, majorEventIsActive, bloodMoonIsActive, eclipseIsActive);
            }
            else if (SariaModUtilities.Fairy(player).CorruptMind)
            {
                ApplyCorruptMind(ref spawnRate, ref maxSpawns);
            }
            else // Apply normal stacking logic.
            {
                ApplyNormalSpawnRates(player, ref spawnRate, ref maxSpawns, bloodMoonIsActive, eclipseIsActive);
            }
        }
        // Handles the logic for the CalmMind buff.
        private void ApplyCalmMind(Player player, ref int spawnRate, ref int maxSpawns, bool majorEventIsActive, bool bloodMoonIsActive, bool eclipseIsActive)
        {
            if (bloodMoonIsActive || eclipseIsActive)
            {
                // Set to normal rates during Blood Moon or Solar Eclipse.
                // Adjust these numbers to what you consider "normal."
                spawnRate = 600;
                maxSpawns = 6;
            }
            else if (!majorEventIsActive)
            {
                // Stop spawns completely otherwise.
                spawnRate = 0;
                maxSpawns = 0;
            }
        }
        // Handles the logic for the CorruptMind buff.
        private void ApplyCorruptMind(ref int spawnRate, ref int maxSpawns)
        {
            spawnRate = (int)((double)spawnRate * .00000000001);
            maxSpawns = (int)((float)maxSpawns * 30f);
        }
        // Handles the logic for normal, stacking spawn rate modifications.
        private void ApplyNormalSpawnRates(Player player, ref int spawnRate, ref int maxSpawns, bool bloodMoonIsActive, bool eclipseIsActive)
        {
            float spawnRateMultiplier = 1f;
            int fixedMaxSpawns = maxSpawns;
            // Apply stacking effects based on conditions.
            if (player.ZoneCorrupt || player.ZoneCrimson)
            {
                spawnRateMultiplier *= 0.4f;
            }
            if (bloodMoonIsActive)
            {
                spawnRateMultiplier *= 0.000001f;
                fixedMaxSpawns = 30;
            }
            if (eclipseIsActive)
            {
                spawnRateMultiplier *= 0.1f;
                fixedMaxSpawns = 20;
            }
            if (Main.moonPhase == 0 && !Main.dayTime)
            {
                spawnRateMultiplier *= 0.2f;
                fixedMaxSpawns = 50;
            }
            spawnRate = (int)((double)spawnRate * spawnRateMultiplier);
            maxSpawns = fixedMaxSpawns;
        }
        public override bool CheckActive(NPC npc)
        {
            if (npc.HasBuff(ModContent.BuffType<EnemyFrozen>()))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            if (npc.HasBuff(ModContent.BuffType<EnemyFrozen>()))
            {
                // Handle the buff removal, ensuring it's synced in multiplayer.
                if (Main.netMode == NetmodeID.Server)
                {
                    // The server removes the buff directly and syncs.
                    int buffIndex = npc.FindBuffIndex(ModContent.BuffType<EnemyFrozen>());
                    if (buffIndex != -1)
                    {
                        npc.DelBuff(buffIndex);
                        npc.netUpdate = true;
                    }
                }
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    // A client sends a packet to the server to remove the buff.
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)SariaMod.SoundMessageType.RemoveBuff);
                    packet.Write(npc.whoAmI);
                    packet.Send();
                }
                else if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    // In single-player, remove the buff directly.
                    int buffIndex = npc.FindBuffIndex(ModContent.BuffType<EnemyFrozen>());
                    if (buffIndex != -1)
                    {
                        npc.DelBuff(buffIndex);
                    }
                }
            }
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            if (npc.HasBuff(ModContent.BuffType<EnemyFrozen>()) && projectile.type != ModContent.ProjectileType<ColdWaveHitBox>() && projectile.type != ModContent.ProjectileType<HealBubble>() && projectile.type != ModContent.ProjectileType<ColdWaveCenter>())
            {
                // Handle the buff removal, ensuring it's synced in multiplayer.
                if (Main.netMode == NetmodeID.Server)
                {
                    // The server removes the buff directly and syncs.
                    int buffIndex = npc.FindBuffIndex(ModContent.BuffType<EnemyFrozen>());
                    if (buffIndex != -1)
                    {
                        npc.DelBuff(buffIndex);
                        npc.netUpdate = true;
                    }
                }
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    // A client sends a packet to the server to remove the buff.
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)SariaMod.SoundMessageType.RemoveBuff);
                    packet.Write(npc.whoAmI);
                    packet.Send();
                }
                else if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    // In single-player, remove the buff directly.
                    int buffIndex = npc.FindBuffIndex(ModContent.BuffType<EnemyFrozen>());
                    if (buffIndex != -1)
                    {
                        npc.DelBuff(buffIndex);
                    }
                }
            }
            if (projectile.type == ModContent.ProjectileType<LaunchHitBox>())
            {
                npc.position.Y = (player.position.Y - 50);
            }
        }
        public override void AI(NPC npc)
        {
            if (npc.HasBuff(ModContent.BuffType<MeteorSpikeDebuff>()) && !npc.HasBuff(ModContent.BuffType<MeteorLaunchDebuff>()))
            {
                if (npc.velocity.Y < 10)
                {
                    npc.velocity.Y = 10;
                    npc.netUpdate = true;
                }
            }
            if (npc.HasBuff(ModContent.BuffType<EnemyFrozen>()))
            {
                npc.frameCounter = 0;
                npc.velocity.X = 0;
                npc.netUpdate = true;
                Vector2 UpWardPosition = npc.Center;
                UpWardPosition.Y -= 30f;
                Vector2 UpWardPosition2 = npc.Center;
                UpWardPosition2.Y -= 50f;
                Vector2 UnderPosition = npc.Center;
                UnderPosition.Y += 30f;
                Vector2 UnderPosition2 = npc.Center;
                UnderPosition2.Y += ((npc.height/2) + .1f);
                bool over = Collision.WetCollision(UpWardPosition, npc.width, npc.height);
                bool over2 = Collision.WetCollision(UpWardPosition2, npc.width, npc.height);
                bool under = Collision.WetCollision(UnderPosition, npc.width, npc.height);
                bool under2 = Collision.WetCollision(UnderPosition2, npc.width, 1);
                bool iswet = Collision.WetCollision(npc.Center, npc.width, npc.height);
                float speed = 70;
                float inertia = 280f;
                npc.netUpdate = true;
                Vector2 direction = UpWardPosition - npc.Center;
                direction.Normalize();
                direction *= speed;
                float amplitude = .29f;
                float frequency = (RandomSize * .01f);
                if (!under && !over2 && !under2)
                {
                    npc.velocity.Y = 10;
                    npc.netUpdate = true;
                }
                else if (iswet && over && !npc.lavaWet)
                {
                    npc.velocity = (npc.velocity * (inertia - 17) + direction) / inertia;
                    npc.netUpdate = true;
                }
                else if (iswet && under && !over && !npc.lavaWet)
                {
                    npc.velocity.Y = 0;
                    npc.position.Y += amplitude * (float)Math.Sin(Main.time * frequency);
                    npc.netUpdate = true;
                }
                else
                {
                    npc.velocity.Y = 10;
                    npc.netUpdate = true;
                }
                npc.direction = -1;
                npc.rotation = 0;
                if (Main.rand.NextBool(20))
                {
                    float radius = (float)Math.Sqrt(Main.rand.Next(150 * 150));
                    double angle = Main.rand.NextDouble() * 5.0 * Math.PI;
                    Dust.NewDust(new Vector2((npc.Center.X) + radius * (float)Math.Cos(angle), (npc.Center.Y) + radius * (float)Math.Sin(angle)), 0, 0, ModContent.DustType<Snow2>(), 0f, 0f, 0, default(Color), 1.5f);
                }
            }
            if (npc.HasBuff(ModContent.BuffType<MeteorSpikeDebuff>()))
            {
                float radius = (float)Math.Sqrt(Main.rand.Next(1 * 1));
                double angle = Main.rand.NextDouble() * 2.0 * Math.PI;
                Dust.NewDust(new Vector2(npc.Center.X + radius * (float)Math.Cos(angle), npc.Center.Y + radius * (float)Math.Sin(angle)), 0, 0, ModContent.DustType<GreySmoke>(), 0f, 0f, 0, default(Color), 4.5f);
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile projectile = Main.projectile[i];
                    Vector2 DotMatch = projectile.position;
                    DotMatch.Y -= 150;
                    bool CanSee = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, DotMatch, 1, 1);
                    Player player = Main.player[projectile.owner];
                    int GiantMoth = ModContent.ProjectileType<Sweetspot>();
                    int owner = player.whoAmI;
                    if (Main.projectile[i].active && Main.projectile[i].Hitbox.Intersects(projectile.Hitbox) && ((Main.projectile[i].type == GiantMoth && Main.projectile[i].owner == owner)))
                    {
                        if (CanSee)
                        {
                            if (npc.velocity.Y < player.velocity.Y)
                            {
                                player.immuneTime = 30;
                                player.immune = true;
                                player.immuneNoBlink = true;
                                npc.position.Y = (player.position.Y + 50);
                                npc.netUpdate = true;
                            }
                        }
                    }
                }
            }
            if (npc.HasBuff(ModContent.BuffType<MeteorLaunchDebuff>()))
            {
                float radius = (float)Math.Sqrt(Main.rand.Next(1 * 1));
                double angle = Main.rand.NextDouble() * 2.0 * Math.PI;
                Dust.NewDust(new Vector2(npc.Center.X + radius * (float)Math.Cos(angle), npc.Center.Y + radius * (float)Math.Sin(angle)), 0, 0, ModContent.DustType<GreySmoke>(), 0f, 0f, 0, default(Color), 4.5f);
                if (npc.velocity.Y > -10)
                {
                    npc.velocity.Y = -10;
                    npc.netUpdate = true;
                }
            }
            if (Main.netMode == NetmodeID.Server && !npc.boss && !npc.townNPC && npc.lifeMax > 10 && npc.active && npc.friendly == false)
            {
                // Extremely rare chance to play a sound every tick.
                // A chance of 1 in 3600 is once every minute on average (60 ticks/sec * 60 sec).
                if (Main.rand.Next(160000) == 0)
                {
                    int soundIndex = 7;
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)SariaMod.SoundMessageType.PlaySound);
                    packet.Write(npc.whoAmI);
                    packet.Write(soundIndex);
                    packet.Send();
                }
            }
            // For single-player, run the same logic locally.
            else if (Main.netMode == NetmodeID.SinglePlayer && !npc.boss && !npc.townNPC && npc.lifeMax > 10 && npc.active && npc.friendly == false)
            {
                if (Main.rand.Next(160000) == 0)
                {
                    int soundIndex = 7;
                    SariaMod.PlaySound(npc.Center, soundIndex);
                }
            }
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            // Draw for EnemyFrozen buff
            if (npc.HasBuff(ModContent.BuffType<EnemyFrozen>()))
            {
                DrawBuffEffect( npc,spriteBatch, screenPos, "SariaMod/Items/Sapphire/IceDome", Color.Lerp(drawColor, Color.LightBlue, 0.80f),new Vector2(0f, -40f),npc.velocity.X * -0.05f, SpriteEffects.None,(RandomSize * 0.08f) * npc.scale, 1, 0.20f);
            }
            // Draw effects for MeteorSpikeDebuff
            if (npc.HasBuff(ModContent.BuffType<MeteorSpikeDebuff>()))
            {
                // First Meteor Flow effect
                DrawBuffEffect(npc,spriteBatch,screenPos, "SariaMod/Items/Emerald/MeteorFlow",Color.Lerp(drawColor, Color.Red, 0.3f),new Vector2(-40f, -40f),npc.velocity.X * -0.05f,SpriteEffects.None,npc.scale * 5.7f, 4,0.70f );
                // Second Meteor Flow effect
                DrawBuffEffect( npc, spriteBatch, screenPos, "SariaMod/Items/Emerald/MeteorFlow", Color.Lerp(drawColor, Color.WhiteSmoke, 2f),new Vector2(-10f, 0f),npc.velocity.X * -0.05f,SpriteEffects.None, npc.scale * 2.7f, 4, 0.50f );
            }
            // Draw effects for MeteorLaunchDebuff
            if (npc.HasBuff(ModContent.BuffType<MeteorLaunchDebuff>()))
            {
                // First Meteor Flow effect
                DrawBuffEffect( npc, spriteBatch, screenPos,"SariaMod/Items/Emerald/MeteorFlow",Color.Lerp(drawColor, Color.Red, 0.3f),new Vector2(-40f, -40f), npc.velocity.X * 0.05f, SpriteEffects.FlipVertically,npc.scale * 5.7f, 4,0.70f );
                // Second Meteor Flow effect
                DrawBuffEffect(npc, spriteBatch, screenPos,"SariaMod/Items/Emerald/MeteorFlow",Color.Lerp(drawColor, Color.WhiteSmoke, 2f),new Vector2(-10f, 0f), npc.velocity.X * 0.05f,SpriteEffects.FlipVertically, npc.scale * 2.7f, 4, 0.50f);
            }
        }
        // A helper method that now uses screenPos for accurate drawing
        private void DrawBuffEffect(
            NPC npc,
            SpriteBatch spriteBatch,
            Vector2 screenPos,
            string texturePath,
            Color buffColor,
            Vector2 offset,
            float rotation,
            SpriteEffects spriteEffects,
            float scale,
            int verticalFrames,
            float transparentAlpha)
        {
            Texture2D texture = ModContent.Request<Texture2D>(texturePath).Value;
            Vector2 startPos = npc.Center - screenPos + new Vector2(0f, npc.gfxOffY); // Use screenPos here
            int frameHeight = texture.Height / verticalFrames;
            int frameY = (int)Main.GameUpdateCount / 6 % verticalFrames;
            Rectangle rectangle = texture.Frame(verticalFrames: verticalFrames, frameY: frameY);
            Vector2 origin = rectangle.Size() / 2f;
            startPos += offset;
            Color finalDrawColor = Color.Lerp(buffColor, Color.Transparent, transparentAlpha);
            spriteBatch.Draw(texture, startPos, rectangle, finalDrawColor, rotation, origin, scale, spriteEffects, 0f);
        }
        public override void HitEffect(NPC npc, int hitDirection, double damage)
        {
            if (npc.HasBuff(ModContent.BuffType<EnemyFrozen>()))
            {
                if (Main.netMode == NetmodeID.Server)
                {
                    // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                    return;
                }
                if (npc.life <= 0)
                {
                    int backGoreType = ModContent.GoreType<IceGore1>();
                    int frontGoreType = ModContent.GoreType<IceGore2>();
                    var entitySource = npc.GetSource_Death();
                    npc.DeathSound = SoundID.Item27;
                    for (int i = 0; i < 2; i++)
                    {
                        Gore.NewGore(entitySource, npc.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), backGoreType);
                        Gore.NewGore(entitySource, npc.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), frontGoreType);
                    }
                }
            }
            npc.buffImmune[ModContent.BuffType<EnemyFrozen>()] = false;
            if (npc.HasBuff(ModContent.BuffType<EnemyFrozen>()))
            {
                // This check is a failsafe, gore is only on clients anyway.
                if (Main.netMode != NetmodeID.Server)
                {
                    int backGoreType = ModContent.GoreType<IceGore2>();
                    for (int G = 0; G < 3; G++)
                    {
                        Gore B = Gore.NewGorePerfect(npc.GetSource_FromThis(), npc.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), backGoreType, 2f);
                        B.light = .5f;
                        SoundEngine.PlaySound(SoundID.Item27, npc.Center);
                    }
                }
            }
        }
        public static void PlayRandomDeathSound(Vector2 position, int soundIndex)
        {
            switch (soundIndex)
            {
                case 0:
                    SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/Die0"), position);
                    break;
                case 1:
                    SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/Die1"), position);
                    break;
                case 2:
                    SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/Die2"), position);
                    break;
                case 3:
                    SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/Die3"), position);
                    break;
                case 4:
                    SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/Die4"), position);
                    break;
            }
        }
        private int GetRandomDeathSoundIndex(NPC npc)
        {
            // Normal death sound logic.
            return Main.rand.Next(5);
        }
        public override void OnKill(NPC npc)
        {
            if (npc.lifeMax <= 25)
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    int soundIndex = GetRandomDeathSoundIndex(npc);
                    SariaMod.PlaySound(npc.Center, soundIndex);
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    int soundIndex = GetRandomDeathSoundIndex(npc);
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)SariaMod.SoundMessageType.PlaySound);
                    packet.Write(npc.whoAmI);
                    packet.Write(soundIndex);
                    packet.Send();
                }
            }
            if (Main.netMode == NetmodeID.Server && !npc.boss && !npc.townNPC && npc.lifeMax > 1 && npc.active && npc.friendly == false)
            {
                // Extremely rare chance to play a sound every tick.
                // A chance of 1 in 3600 is once every minute on average (60 ticks/sec * 60 sec).
                if (Main.rand.Next(800) == 0)
                {
                    int soundIndex = 6;
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)SariaMod.SoundMessageType.PlaySound);
                    packet.Write(npc.whoAmI);
                    packet.Write(soundIndex);
                    packet.Send();
                }
            }
            // For single-player, run the same logic locally.
            else if (Main.netMode == NetmodeID.SinglePlayer && !npc.boss && !npc.townNPC && npc.lifeMax > 1 && npc.active && npc.friendly == false)
            {
                if (Main.rand.Next(800) == 0)
                {
                    int soundIndex = 6;
                    SariaMod.PlaySound(npc.Center, soundIndex);
                }
            }
            if (!npc.SpawnedFromStatue)
            {
                if (Main.rand.Next(50) == 0)
                {
                    Item.NewItem(npc.GetSource_FromThis(), (int)(npc.position.X + 0), (int)(npc.position.Y + 0), 0, 0, ModContent.ItemType<XpPearl>());
                }
                if (Main.rand.Next(70) == 0)
                {
                    Item.NewItem(npc.GetSource_FromThis(), (int)(npc.position.X + 0), (int)(npc.position.Y + 0), 0, 0, ModContent.ItemType<FrozenYogurt>());
                }
                if (Main.rand.Next(150) == 0)
                {
                    Item.NewItem(npc.GetSource_FromThis(), (int)(npc.position.X + 0), (int)(npc.position.Y + 0), 0, 0, ModContent.ItemType<MediumXpPearl>());
                }
                if (Main.rand.Next(300) == 0)
                {
                    Item.NewItem(npc.GetSource_FromThis(), (int)(npc.position.X + 0), (int)(npc.position.Y + 0), 0, 0, ModContent.ItemType<SariasConfect>());
                }
                if (Main.rand.Next(600) == 0)
                {
                    Item.NewItem(npc.GetSource_FromThis(), (int)(npc.position.X + 0), (int)(npc.position.Y + 0), 0, 0, ModContent.ItemType<LargeXpPearl>());
                }
                if (Main.rand.Next(1000) == 0)
                {
                    Item.NewItem(npc.GetSource_FromThis(), (int)(npc.position.X + 0), (int)(npc.position.Y + 0), 0, 0, ModContent.ItemType<SoaringConcoction>());
                }
                if (Main.rand.Next(25000) == 0)
                {
                    Item.NewItem(npc.GetSource_FromThis(), (int)(npc.position.X + 0), (int)(npc.position.Y + 0), 0, 0, ModContent.ItemType<RareXpPearl>());
                }
            }
        }
    }
}
