using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using SariaMod.Items.Emerald;
using Terraria.DataStructures;
using SariaMod.Items.Bands;
using SariaMod.Items.zPearls;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using SariaMod.Buffs;
using SariaMod.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;
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
using Terraria.GameContent;
using SariaMod.Items.Strange;
using SariaMod.Items.zDinner;
using SariaMod.Gores;
namespace SariaMod
{
    public class SariaLevelUpTier
    {
        public int RequiredXP { get; set; }
        public Func<bool> Condition { get; set; }
    }
    public class FairyPlayer : ModPlayer
    {
        private static Dictionary<int, SariaLevelUpTier> levelUpTiers;
        public bool BloodmoonBuff;
        public bool SariaCurseD;
        public bool Statlowered;
        public bool Statrisen;
        public bool Sickness;
        public bool externalColdImmunity;
        public bool Burning2;
        public bool GhostBurning;
        public bool PassiveHealing;
        public bool Frostburn2;
        public bool Frostburn3;
        public bool EclipseBuff;
        public int Sarialevel;
        public int StoredHealth;
        public int Serving;
        public bool PlayerGreenGem;
        public bool PlayerPurpleGem;
        public bool PlayerSilverGem;
        public bool PlayerisPsychic;
        public bool PlayerisWater;
        public bool PlayerisFire;
        public bool PlayerisElectric;
        public bool PlayerisRock;
        public bool PlayerisBug;
        public bool PlayerisGhost;
        public bool PlayerisFairy;
        public bool PlayercanCharge;
        public bool holdingleft;
        public bool holdingright;
        public bool holdingdown;
        public bool SariaUnlockWater;
        public bool SariaUnlockFire;
        public bool SariaUnlockElectric;
        public bool SariaUnlockRock;
        public bool SariaUnlockBug;
        public bool SariaUnlockGhost;
        public bool SariaUnlockFairy;
        public bool SariaUnlockPsychic2;
        public bool SariaUnlockWater2;
        public bool SariaUnlockFire2;
        public bool SariaUnlockElectric2;
        public bool SariaUnlockRock2;
        public bool SariaUnlockBug2;
        public bool SariaUnlockGhost2;
        public bool SariaUnlockFairy2;
        public bool SariaUpgrade1;
        public bool SariaUpgrade2;
        public bool SariaUpgrade3;
        public bool SariaUpgrade4;
        public bool SariaUpgrade5;
        public bool SariaUpgrade6;
        public bool SariaUpgrade7;
        public bool SariaUpgrade8;
        public bool SariaUpgrade9;
        public bool SariaUpgrade10;
        public bool SariaUpgrade11;
        public bool SariaUpgrade12;
        public bool SariaUpgrade13;
        public bool SariaUpgrade14;
        public bool SariaUpgrade15;
        public bool SariaUpgrade16;
        public bool SariaUpgrade17;
        public bool SariaUpgrade18;
        public bool SariaUpgrade19;
        public bool SariaUpgrade20;
        public bool SariaUpgrade21;
        public bool CalmMind;
        public bool CorruptMind;
        public int FreezingTemp;
        public int SariaXp;
        public int Timer;
        public int XPBarLevel;
        public int TMPoints;
        public int TMPointsUsed;
        public int EVs;
        public int EVsUsed;
        public int DinnerHoldTimer;
        public int lastHeldItemType;
        public int KingsDinnerCooldownTimer = 0;
        public const int KingsDinnerResetTime = 240;
        public override void ResetEffects()
        {
            SariaCurseD = false;
            Statrisen = false;
            Statlowered = false;
            Sickness = false;
            CalmMind = false;
            CorruptMind = false;
            externalColdImmunity = false;
            BloodmoonBuff = false;
            PassiveHealing = false;
            Burning2 = false;
            GhostBurning = false;
            Frostburn2 = false;
            Frostburn3 = false;
            EclipseBuff = false;
            PlayerisPsychic = false;
            PlayerisWater = false;
            PlayerisFire = false;
            PlayerisElectric = false;
            PlayerisRock = false;
            PlayerisBug = false;
            PlayerisGhost = false;
            PlayerisFairy = false;
        }
        public override void UpdateDead()
        {
            Statrisen = false;
            Statlowered = false;
            SariaCurseD = false;
            Sickness = false;
            Burning2 = false;
            GhostBurning = false;
            CalmMind = false;
            CorruptMind = false;
            PassiveHealing = false;
            externalColdImmunity = false;
            BloodmoonBuff = false;
            Frostburn2 = false;
            Frostburn3 = false;
            EclipseBuff = false;
            PlayerisPsychic = false;
            PlayerisWater = false;
            PlayerisFire = false;
            PlayerisElectric = false;
            PlayerisRock = false;
            PlayerisBug = false;
            PlayerisGhost = false;
            PlayerisFairy = false;
        }
        public override void UpdateBadLifeRegen()
        {
            if (Timer < 40)
            {
                Timer++;
            }
            if (Frostburn3)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegenTime = 0;
                Player.lifeRegen -= 30;
            }
            if (Frostburn2)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegenTime = 0;
                Player.lifeRegen -= 30;
            }
            if (Sickness)
            {
                {
                    Player.statDefense = 1;
                    if (Player.statLife > ((Player.statLifeMax2) / 3))
                    {
                        if (Player.lifeRegen > 0)
                        {
                            Player.lifeRegen = 0;
                        }
                        Player.lifeRegenTime = 0;
                        Player.lifeRegen -= 32;
                    }
                }
            }
            if (PassiveHealing)
            {
                if (Timer >= 40 && (Player.statLife < Player.statLifeMax2))
                {
                    Player.Heal((3));
                    Timer = 0;
                }
            }
            if (Burning2)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegenTime = 0;
                Player.lifeRegen -= 32;
            }
            if (GhostBurning)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegenTime = 0;
                Player.lifeRegen -= 32;
            }
            if (SariaCurseD)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegenTime = 0;
                Player.lifeRegen -= 16;
            }
            if (Player.HasBuff(ModContent.BuffType<TriforceofCourage>()))
            {
                Player.statDefense += (Player.statDefense / 4);
            }
            if (Player.HasBuff(ModContent.BuffType<TriforceofPower>()))
            {
                Player.statDefense -= (Player.statDefense / 4);
            }
            if (Statlowered)
            {
                Player.statDefense -= (Player.statDefense / 4) * 3;
                Player.statLifeMax2 -= 50;
            }
            if (Statrisen)
            {
                Player.statDefense += Player.statDefense / 4;
            }
            if (BloodmoonBuff)
            {
                if (Player.statLife > ((Player.statLifeMax2) / 3))
                {
                    if (Player.lifeRegen > 0)
                    {
                        Player.lifeRegen = 0;
                    }
                    Player.lifeRegenTime = 0;
                    Player.lifeRegen -= 16;
                }
            }
            if (EclipseBuff)
            {
                if (Player.statLife > ((Player.statLifeMax2) / 3))
                {
                    if (Player.lifeRegen > 0)
                    {
                        Player.lifeRegen = 0;
                    }
                    Player.lifeRegenTime = 0;
                    Player.lifeRegen -= 16;
                }
            }
        }
        public override void Kill(double damage, int hitDirection, bool pvp, Terraria.DataStructures.PlayerDeathReason damageSource)
        {
            // Check if the player is holding the KingsDinner item when they die.
            // Player.HeldItem is the item in the currently selected hotbar slot.
            if (Player.HeldItem.type == ModContent.ItemType<Items.zDinner.KingsDinner>())
            {
                Projectile.NewProjectile(Player.GetSource_Death(), Player.Center, Vector2.Zero, ModContent.ProjectileType<Dinner>(), 0, 0, Player.whoAmI);
                // Ensure the projectile spawning only happens on the server (or owning client) in multiplayer.
                // This prevents duplicate projectiles from being spawned.
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // Roll a random number to select one of the two projectiles.
                    int rand = Main.rand.Next(2); // 0 or 1
                    int projectileType;
                    if (rand == 0)
                    {
                        // Spawn the first projectile type
                        projectileType = ModContent.ProjectileType<DinnerDeathSound1>();
                    }
                    else
                    {
                        // Spawn the second projectile type
                        projectileType = ModContent.ProjectileType<DinnerDeathSound2>();
                    }
                    // Spawn the projectile at the player's last position.
                    Projectile.NewProjectile(Player.GetSource_Death(), Player.Center, Vector2.Zero, projectileType, 0, 0, Player.whoAmI);
                    for (int i = 0; i < 20; i++)
                    {
                        // Roll a random number to select one of the three gore types.
                        int randGore = Main.rand.Next(3); // 0, 1, or 2
                        int goreType;
                        if (randGore == 0)
                        {
                            goreType = ModContent.GoreType<MessyDinner1>();
                        }
                        else if (randGore == 1)
                        {
                            goreType = ModContent.GoreType<MessyDinner2>();
                        }
                        else // randGore == 2
                        {
                            goreType = ModContent.GoreType<MessyDinner3>();
                        }
                        // Spawn the gore at the player's last position with a random spread velocity.
                        Gore.NewGore(
                            Player.GetSource_Death(),
                            Player.Center,
                            new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f)),
                            goreType
                        );
                    }
                }
            }
        }
        public override void SaveData(TagCompound tag)
        {
            tag["Sarialevel"] = Sarialevel;
            tag["Serving"] = Serving;
            tag["StoredHealth"] = StoredHealth;
            tag["SariaXp"] = SariaXp;
            tag["FreezingTemp"] = FreezingTemp;
            tag["XPBarLevel"] = XPBarLevel;
            tag["TMPoints"] = TMPoints;
            tag["TMPointsUsed"] = TMPointsUsed;
            tag["SariaUnlockWater"] = SariaUnlockWater;
            tag["SariaUnlockFire"] = SariaUnlockFire;
            tag["SariaUnlockElectric"] = SariaUnlockElectric;
            tag["SariaUnlockRock"] = SariaUnlockRock;
            tag["SariaUnlockBug"] = SariaUnlockBug;
            tag["SariaUnlockGhost"] = SariaUnlockGhost;
            tag["SariaUnlockFairy"] = SariaUnlockFairy;
            tag["SariaUnlockPsychic2"] = SariaUnlockPsychic2;
            tag["SariaUnlockWater2"] = SariaUnlockWater2;
            tag["SariaUnlockFire2"] = SariaUnlockFire2;
            tag["SariaUnlockElectric2"] = SariaUnlockElectric2;
            tag["SariaUnlockRock2"] = SariaUnlockRock2;
            tag["SariaUnlockBug2"] = SariaUnlockBug2;
            tag["SariaUnlockGhost2"] = SariaUnlockGhost2;
            tag["SariaUnlockFairy2"] = SariaUnlockFairy2;
            tag["SariaUpgrade1"] = SariaUpgrade1;
            tag["SariaUpgrade2"] = SariaUpgrade2;
            tag["SariaUpgrade3"] = SariaUpgrade3;
            tag["SariaUpgrade4"] = SariaUpgrade4;
            tag["SariaUpgrade5"] = SariaUpgrade5;
            tag["SariaUpgrade6"] = SariaUpgrade6;
            tag["SariaUpgrade7"] = SariaUpgrade7;
            tag["SariaUpgrade8"] = SariaUpgrade8;
            tag["SariaUpgrade9"] = SariaUpgrade9;
            tag["SariaUpgrade10"] = SariaUpgrade10;
            tag["SariaUpgrade11"] = SariaUpgrade11;
            tag["SariaUpgrade12"] = SariaUpgrade12;
            tag["SariaUpgrade13"] = SariaUpgrade13;
            tag["SariaUpgrade14"] = SariaUpgrade14;
            tag["SariaUpgrade15"] = SariaUpgrade15;
            tag["SariaUpgrade16"] = SariaUpgrade16;
            tag["SariaUpgrade17"] = SariaUpgrade17;
            tag["SariaUpgrade18"] = SariaUpgrade18;
            tag["SariaUpgrade19"] = SariaUpgrade19;
            tag["SariaUpgrade20"] = SariaUpgrade20;
            tag["SariaUpgrade21"] = SariaUpgrade21;
        }
        public override void LoadData(TagCompound tag)
        {
            Sarialevel = tag.GetInt("Sarialevel");
            Serving = tag.GetInt("Serving");
            StoredHealth = tag.GetInt("StoredHealth");
            SariaXp = tag.GetInt("SariaXp");
            FreezingTemp = tag.GetInt("FreezingTemp");
            XPBarLevel = tag.GetInt("XPBarLevel");
            TMPoints = tag.GetInt("TMPoints");
            TMPointsUsed = tag.GetInt("TMPointsUsed");
            SariaUnlockWater = tag.GetBool("SariaUnlockWater");
            SariaUnlockFire = tag.GetBool("SariaUnlockFire");
            SariaUnlockElectric = tag.GetBool("SariaUnlockElectric");
            SariaUnlockRock = tag.GetBool("SariaUnlockRock");
            SariaUnlockBug = tag.GetBool("SariaUnlockBug");
            SariaUnlockGhost = tag.GetBool("SariaUnlockGhost");
            SariaUnlockFairy = tag.GetBool("SariaUnlockFairy");
            SariaUnlockPsychic2 = tag.GetBool("SariaUnlockPsychic2");
            SariaUnlockWater2 = tag.GetBool("SariaUnlockWater2");
            SariaUnlockFire2 = tag.GetBool("SariaUnlockFire2");
            SariaUnlockElectric2 = tag.GetBool("SariaUnlockElectric2");
            SariaUnlockRock2 = tag.GetBool("SariaUnlockRock2");
            SariaUnlockBug2 = tag.GetBool("SariaUnlockBug2");
            SariaUnlockGhost2 = tag.GetBool("SariaUnlockGhost2");
            SariaUnlockFairy2 = tag.GetBool("SariaUnlockFairy2");
            SariaUpgrade1 = tag.GetBool("SariaUpgrade1");
            SariaUpgrade2 = tag.GetBool("SariaUpgrade2");
            SariaUpgrade3 = tag.GetBool("SariaUpgrade3");
            SariaUpgrade4 = tag.GetBool("SariaUpgrade4");
            SariaUpgrade5 = tag.GetBool("SariaUpgrade5");
            SariaUpgrade6 = tag.GetBool("SariaUpgrade6");
            SariaUpgrade7 = tag.GetBool("SariaUpgrade7");
            SariaUpgrade8 = tag.GetBool("SariaUpgrade8");
            SariaUpgrade9 = tag.GetBool("SariaUpgrade9");
            SariaUpgrade10 = tag.GetBool("SariaUpgrade10");
            SariaUpgrade11 = tag.GetBool("SariaUpgrade11");
            SariaUpgrade12 = tag.GetBool("SariaUpgrade12");
            SariaUpgrade13 = tag.GetBool("SariaUpgrade13");
            SariaUpgrade14 = tag.GetBool("SariaUpgrade14");
            SariaUpgrade15 = tag.GetBool("SariaUpgrade15");
            SariaUpgrade16 = tag.GetBool("SariaUpgrade16");
            SariaUpgrade17 = tag.GetBool("SariaUpgrade17");
            SariaUpgrade18 = tag.GetBool("SariaUpgrade18");
            SariaUpgrade19 = tag.GetBool("SariaUpgrade19");
            SariaUpgrade20 = tag.GetBool("SariaUpgrade20");
            SariaUpgrade21 = tag.GetBool("SariaUpgrade21");
        }
        public override void Load()
        {
            levelUpTiers = new Dictionary<int, SariaLevelUpTier>
            {
                { 1, new SariaLevelUpTier { RequiredXP = 3000, Condition = () => NPC.downedSlimeKing } },
                { 2, new SariaLevelUpTier { RequiredXP = 9000, Condition = () => NPC.downedQueenBee } },
                { 3, new SariaLevelUpTier { RequiredXP = 20000, Condition = () => Main.hardMode && NPC.downedMechBossAny } },
                { 4, new SariaLevelUpTier { RequiredXP = 40000, Condition = () => NPC.downedMechBossAny } },
                { 5, new SariaLevelUpTier { RequiredXP = 80000, Condition = () => NPC.downedPlantBoss } },
                { 6, new SariaLevelUpTier { RequiredXP = 240000, Condition = () => NPC.downedFishron } }
            };
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)SariaMod.SoundMessageType.SyncSariaLevel);
            packet.Write(Player.whoAmI);
            packet.Write(Sarialevel);
            packet.Write(SariaXp);
            packet.Send(toWho, fromWho);
        }
        // Method to add SariaXp and check for level ups
        public void AddSariaXp(int amount)
        {
            // Only add XP if the player is not at max level
            if (Sarialevel >= 6) // Assumes 6 is the max level
            {
                return;
            }
            // Accumulate XP locally
            SariaXp += amount;
            bool leveledUp = false;
            // Check for level ups and process them
            while (levelUpTiers.TryGetValue(Sarialevel + 1, out SariaLevelUpTier nextTier) && SariaXp >= nextTier.RequiredXP && nextTier.Condition())
            {
                leveledUp = true;
                Sarialevel++;
                // Check if the new XP should be reset (all except final level)
                if (Sarialevel < 6)
                {
                    SariaXp = 0;
                }
            }
            // Only send a network packet if a level up occurred.
            if (leveledUp)
            {
                PlayLevelUpEffects();
                // Send a custom packet to all clients
                if (Main.netMode == NetmodeID.Server)
                {
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)SariaMod.SoundMessageType.SyncSariaLevel);
                    packet.Write(Player.whoAmI);
                    packet.Write(Sarialevel);
                    packet.Write(SariaXp);
                    packet.Send(-1, Player.whoAmI);
                }
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)SariaMod.SoundMessageType.SyncSariaLevel);
                    packet.Write(Player.whoAmI);
                    packet.Write(Sarialevel);
                    packet.Write(SariaXp);
                    packet.Send();
                }
            }
        }
        // Separate method for the reusable level-up effects
        private void PlayLevelUpEffects()
        {
            for (int j = 0; j < 72; j++)
            {
                Dust dust = Dust.NewDustPerfect(Player.Center, 113);
                dust.velocity = ((float)Math.PI * 2f * Vector2.Dot(((float)j / 72f * ((float)Math.PI * 2f)).ToRotationVector2(), Player.velocity.SafeNormalize(Vector2.UnitY).RotatedBy((float)j / 72f * ((float)Math.PI * -2f)))).ToRotationVector2();
                dust.velocity = dust.velocity.RotatedBy((float)j / 36f * ((float)Math.PI * 2f)) * 8f;
                dust.noGravity = true;
                dust.scale = 1.9f;
            }
            SoundEngine.PlaySound(SoundID.Item110, Player.Center);
            SoundEngine.PlaySound(SoundID.Item14, Player.Center);
        }
        // Method to update SariaLevel and sync it across the network
        public void SetSariaLevel(int newLevel)
        {
            // Only send a packet if the value has actually changed
            if (Sarialevel != newLevel)
            {
                Sarialevel = newLevel;
                // Send a custom packet to all clients (including the server)
                if (Main.netMode == NetmodeID.Server)
                {
                    // The server needs to send the change to all other players
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)SariaMod.SoundMessageType.SyncSariaLevel);
                    packet.Write(Player.whoAmI);
                    packet.Write(Sarialevel);
                    packet.Write(SariaXp);
                    packet.Send(-1, Player.whoAmI); // Send to all except the player who triggered it
                }
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    // A client needs to tell the server about the change
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)SariaMod.SoundMessageType.SyncSariaLevel);
                    packet.Write(Player.whoAmI);
                    packet.Write(Sarialevel);
                    packet.Write(SariaXp);
                    packet.Send(); // Send to server (toWho: -1, fromWho: myPlayer)
                }
            }
        }
        public override void OnEnterWorld(Player player)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                // Iterate through all active NPCs in the world.
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    NPC npc = Main.npc[i];
                    // Check if the NPC is active and has your specific buff.
                    if (npc.active && npc.HasBuff(ModContent.BuffType<EnemyFrozen>()))
                    {
                        // Send a sync packet for this specific NPC to the newly joined player.
                        NetMessage.SendData(MessageID.SyncNPC, player.whoAmI, -1, null, i);
                    }
                }
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player otherPlayer = Main.player[i];
                    // Check if the other player is active and has the buff
                    if (otherPlayer.active && otherPlayer.HasBuff<SariaBuff>()) // Replace 'YourSummonBuff' with your actual summon buff class
                    {
                        // Create and send a buff sync packet to the joining player
                        ModPacket packet = Mod.GetPacket();
                        packet.Write((byte)SariaMod.SoundMessageType.SyncBuff);
                        packet.Write(otherPlayer.whoAmI); // The player who has the buff
                        packet.Write(ModContent.BuffType<SariaBuff>()); // The buff type
                        packet.Write(otherPlayer.buffTime[otherPlayer.FindBuffIndex(ModContent.BuffType<SariaBuff>())]); // The remaining buff time
                        packet.Send(player.whoAmI); // Send the packet to the newly joined player
                    }
                }
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile projectile = Main.projectile[i];
                    // Check if the projectile is an active Saria summon.
                    if (projectile.active && projectile.type == ModContent.ProjectileType<Saria>())
                    {
                        // Send the current frame and direction to the joining player.
                        // The `whoAmI` property of the joining `player` is used as the target client.
                        // The -1 for `ignoreClient` means no one is ignored.
                        SyncSariaProjectile(projectile, toClient: player.whoAmI, ignoreClient: -1);
                    }
                }
            }
        }
        public void ResetKingsDinnerTimer()
        {
            KingsDinnerCooldownTimer = 0;
        }
        public override void PostUpdate()
        {
            // Check if the player is holding the KingsDinner item
            FairyPlayer modPlayer = Player.Fairy();
            if (KingsDinnerCooldownTimer < KingsDinnerResetTime)
            {
                KingsDinnerCooldownTimer++;
            }
            if (modPlayer.Serving >= 101)
            {
                modPlayer.Serving = 100;
            }
            if (Player.HeldItem.type == ModContent.ItemType<Items.zDinner.KingsDinner>())
            {
                DinnerHoldTimer++; // Increment the timer each frame
            }
            else
            {
                DinnerHoldTimer = 0; // Reset the timer if the player is not holding the item
            }
            // Check if the timer has exceeded a certain amount of time (e.g., 180 ticks = 3 seconds)
            if (DinnerHoldTimer >= 18000)
            {
                // Ensure the projectile spawning happens on the server (or owning client) in multiplayer.
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // Roll a random number to select one of the two projectiles.
                    int randProjectile = Main.rand.Next(2); // 0 or 1
                    int projectileType;
                    if (randProjectile == 0)
                    {
                        projectileType = ModContent.ProjectileType<DinnerSoundHit1>();
                    }
                    else
                    {
                        projectileType = ModContent.ProjectileType<DinnerSoundHit2>();
                    }
                    // Spawn the projectile from the player's position
                    // You may want to spawn it towards the mouse or other location
                    Vector2 mousePosition = Main.MouseWorld;
                    Vector2 direction = mousePosition - Player.Center;
                    direction.Normalize();
                    direction *= 10f; // Set the projectile speed
                    Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), Player.Center, direction, projectileType, Player.HeldItem.damage, Player.HeldItem.knockBack, Player.whoAmI);
                }
                DinnerHoldTimer = 0; // Reset the timer after spawning the projectile
            }
            if (lastHeldItemType != Player.HeldItem.type && Player.HeldItem.type == ModContent.ItemType<Items.zDinner.KingsDinner>())
            {
                // Play the sound effect.
                // Replace "SariaMod/Sounds/ItemSelect" with the path to your sound file.
                SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/ohboy"), Player.Center);
            }
            // Update the last held item type for the next frame's check
            lastHeldItemType = Player.HeldItem.type;
        }
        public static void SyncSariaProjectile(Projectile projectile, int toClient = -1, int ignoreClient = -1)
        {
            if (Main.netMode == NetmodeID.Server && projectile.active && projectile.type == ModContent.ProjectileType<Saria>())
            {
                ModPacket packet = ModContent.GetInstance<SariaMod>().GetPacket();
                packet.Write((byte)SariaMod.SoundMessageType.SyncProjectileState);
                packet.Write(projectile.whoAmI);
                packet.Write(projectile.frame);
                packet.Write(projectile.spriteDirection);
                packet.Write(projectile.frameCounter);
                packet.Send(toClient, ignoreClient);
            }
        }
        public override void SetControls()
        {
            int owner = Player.whoAmI;
            if ((Player.ownedProjectileCounts[ModContent.ProjectileType<Emeraldspike>()] <= 0f) && (Player.ownedProjectileCounts[ModContent.ProjectileType<Emeraldspike2>()] <= 0f) && (Player.ownedProjectileCounts[ModContent.ProjectileType<Emeraldspike3>()] <= 0f))
            {
                holdingleft = false;
                holdingright = false;
                holdingdown = false;
            }
            if ((Player.ownedProjectileCounts[ModContent.ProjectileType<Emeraldspike>()] > 0f))
            {
                for (int U = 0; U < 1000; U++)
                {
                    if (Main.projectile[U].active && Main.projectile[U].ModProjectile is Emeraldspike modRupee && ((Main.projectile[U].owner == owner)))
                    {
                        if (Main.projectile[U].frame == 0)
                        {
                            if (Player.controlLeft)
                            {
                                holdingleft = false;
                            }
                            if (Player.controlRight)
                            {
                                holdingright = false;
                            }
                            if (Player.controlDown)
                            {
                                holdingdown = false;
                            }
                        }
                        if (Main.projectile[U].frame >= 1)
                        {
                            if (!Player.controlLeft)
                            {
                                holdingleft = true;
                            }
                            if (!Player.controlRight)
                            {
                                holdingright = true;
                            }
                            if (!Player.controlDown)
                            {
                                holdingdown = true;
                            }
                        }
                    }
                }
            }
            if ((Player.ownedProjectileCounts[ModContent.ProjectileType<Emeraldspike2>()] > 0f))
            {
                for (int U = 0; U < 1000; U++)
                {
                    if (Main.projectile[U].active && Main.projectile[U].ModProjectile is Emeraldspike2 modRupee && ((Main.projectile[U].owner == owner)))
                    {
                        if (Main.projectile[U].frame == 0)
                        {
                            if (Player.controlLeft)
                            {
                                holdingleft = false;
                            }
                            if (Player.controlRight)
                            {
                                holdingright = false;
                            }
                            if (Player.controlDown)
                            {
                                holdingdown = false;
                            }
                        }
                        if (Main.projectile[U].frame >= 1)
                        {
                            if (!Player.controlLeft)
                            {
                                holdingleft = true;
                            }
                            if (!Player.controlRight)
                            {
                                holdingright = true;
                            }
                            if (!Player.controlDown)
                            {
                                holdingdown = true;
                            }
                        }
                    }
                }
            }
            if ((Player.ownedProjectileCounts[ModContent.ProjectileType<Emeraldspike3>()] > 0f))
            {
                for (int U = 0; U < 1000; U++)
                {
                    if (Main.projectile[U].active && Main.projectile[U].ModProjectile is Emeraldspike3 modRupee && ((Main.projectile[U].owner == owner)))
                    {
                        if (Main.projectile[U].frame == 0)
                        {
                            if (Player.controlLeft)
                            {
                                holdingleft = false;
                            }
                            if (Player.controlRight)
                            {
                                holdingright = false;
                            }
                            if (Player.controlDown)
                            {
                                holdingdown = false;
                            }
                        }
                        if (Main.projectile[U].frame >= 1)
                        {
                            if (!Player.controlLeft)
                            {
                                holdingleft = true;
                            }
                            if (!Player.controlRight)
                            {
                                holdingright = true;
                            }
                            if (!Player.controlDown)
                            {
                                holdingdown = true;
                            }
                        }
                    }
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            FairyPlayerMiscEffects.FairyPostUpdateMiscEffects(Player, Mod);
        }
    }
}