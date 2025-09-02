using Microsoft.Xna.Framework;
using SariaMod.Buffs;
using SariaMod.Dusts;
using SariaMod.Items.Emerald;
using SariaMod.Items.Strange;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using SariaMod.Items.Sapphire;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Content;
namespace SariaMod
{
    public class FairyPlayerMiscEffects : ModPlayer
    {
        private const int sphereRadius3 = 1;
        private static int LavaSoundTimer;
        private static int Soundtimer;
        // Structured data for level-up conditions.
        // Sound styles for rain and thunder. Now pointing to the OGG file without the extension.
        public static readonly SoundStyle OutdoorRain = new SoundStyle("SariaMod/Sounds/Rain")
        {
            IsLooped = true,
            MaxInstances = 1
        };
        public static readonly SoundStyle RainIndoors = new SoundStyle("SariaMod/Sounds/RainIndoors")
        {
            IsLooped = true,
            MaxInstances = 1
        };
        public static readonly SoundStyle Thunder1 = new SoundStyle("SariaMod/Sounds/Thunder1");
        public static readonly SoundStyle Thunder2 = new SoundStyle("SariaMod/Sounds/Thunder2");
        public static readonly SoundStyle Thunder3 = new SoundStyle("SariaMod/Sounds/Thunder3");
        public static readonly SoundStyle Thunder4 = new SoundStyle("SariaMod/Sounds/Thunder4");
        public static readonly SoundStyle ThunderThighs = new SoundStyle("SariaMod/Sounds/ThunderThighs");
        // Non-static fields to hold the active SoundEffectInstances for this player
        private SoundEffectInstance outdoorRainInstance;
        private SoundEffectInstance indoorRainInstance;
        private bool playRainSound;
        private bool wasPlayingRain;
        private static readonly int[][] sarialevelXpThresholds =
        {
             // Sarialevel 0 thresholds
             new int[] { 375, 750, 1125, 1500, 1875, 2250, 2625, 3000 },
             // Sarialevel 1 thresholds
             new int[] { 1125, 2250, 3375, 4500, 5625, 6750, 7875, 9000 },
             // Sarialevel 2 thresholds
             new int[] { 2500, 5000, 7500, 10000, 12500, 15000, 17500, 20000 },
             // Sarialevel 3 thresholds
             new int[] { 5000, 10000, 15000, 20000, 25000, 30000, 35000, 40000 },
             // Sarialevel 4 thresholds
             new int[] { 10000, 20000, 30000, 40000, 50000, 60000, 70000, 80000 },
             // Sarialevel 5 thresholds
             new int[] { 30000, 60000, 90000, 120000, 150000, 180000, 210000, 240000 }
        };
        public static void FairyPostUpdateMiscEffects(Player player, Mod mod)
        {
            FairyPlayer modPlayer = player.Fairy();
            MiscEffects(player, modPlayer, mod);
        }
        public void StopAllLoopedSounds()
        {
            if (outdoorRainInstance != null)
            {
                outdoorRainInstance.Stop(false);
                outdoorRainInstance = null;
            }
            if (indoorRainInstance != null)
            {
                indoorRainInstance.Stop(false);
                indoorRainInstance = null;
            }
        }
        public override void Initialize()
        {
            playRainSound = false;
            wasPlayingRain = false;
        }
        public override void OnEnterWorld(Player player)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)SariaMod.SoundMessageType.SyncRainSoundState);
                packet.Write(playRainSound);
                packet.Send(-1, player.whoAmI);
            }
        }
        public void ReceiveRainSoundState(bool newState)
        {
            if (playRainSound != newState)
            {
                playRainSound = newState;
                if (!playRainSound)
                {
                    StopAllLoopedSounds();
                }
            }
        }
        private bool CheckForMostlyFullWallCoverage(int playerTileX, int playerTileY, int rectangleSize, float percentageRequired)
        {
            int coveredTiles = 0;
            int totalTiles = 0;
            for (int x = playerTileX - rectangleSize; x <= playerTileX + rectangleSize; x++)
            {
                for (int y = playerTileY - rectangleSize; y <= playerTileY + rectangleSize; y++)
                {
                    if (x == playerTileX && (y == playerTileY || y == playerTileY + 1))
                    {
                        continue;
                    }
                    totalTiles++;
                    Tile tile = Framing.GetTileSafely(x, y);
                    if ((tile.HasTile && Main.tileSolid[tile.TileType] && !tile.IsActuated) || tile.WallType > 0)
                    {
                        coveredTiles++;
                    }
                }
            }
            if (totalTiles == 0) return false;
            float coveragePercentage = (float)coveredTiles / totalTiles;
            return coveragePercentage >= percentageRequired;
        }
        private float CalculateCoverage(int x, int startY, int endY)
        {
            int coveredTiles = 0;
            int totalTiles = 0;
            for (int y = startY; y <= endY; y++)
            {
                totalTiles++;
                Tile tile = Framing.GetTileSafely(x, y);
                if ((tile.HasTile && Main.tileSolid[tile.TileType] && !tile.IsActuated) || tile.WallType > 0)
                {
                    coveredTiles++;
                }
            }
            if (totalTiles == 0) return 0f;
            return (float)coveredTiles / totalTiles;
        }
        private void HandleRainSoundState(bool isRaining, int closestCeilingTileY, int furthestCeilingTileY)
        {
            if (!isRaining)
            {
                StopAllLoopedSounds();
                return;
            }
            int playerTileX = (int)(Player.Center.X / 16f);
            int playerTileY = (int)(Player.Center.Y / 16f);
            const float requiredCoverage = 0.75f;
            const int wallSearchRadius = 2;
            bool isCovered = false;
            if (furthestCeilingTileY != -1)
            {
                float areaCoverageFurthest = CalculateCoverage(playerTileX, furthestCeilingTileY, playerTileY);
                bool isMostlySurrounded = CheckForMostlyFullWallCoverage(playerTileX, playerTileY, wallSearchRadius, requiredCoverage);
                if (areaCoverageFurthest >= requiredCoverage && isMostlySurrounded)
                {
                    isCovered = true;
                }
            }
            if (!isCovered && closestCeilingTileY != -1)
            {
                float areaCoverageClosest = CalculateCoverage(playerTileX, closestCeilingTileY, playerTileY);
                bool isMostlySurrounded = CheckForMostlyFullWallCoverage(playerTileX, playerTileY, wallSearchRadius, requiredCoverage);
                if (areaCoverageClosest >= requiredCoverage && isMostlySurrounded)
                {
                    isCovered = true;
                }
            }
            bool outsiderain = !isCovered;
            bool insiderain = isCovered;
            if (outsiderain)
            {
                if (outdoorRainInstance == null || outdoorRainInstance.State != SoundState.Playing)
                {
                    Asset<SoundEffect> asset = ModContent.Request<SoundEffect>("SariaMod/Sounds/Rain");
                    if (asset != null && asset.IsLoaded)
                    {
                        outdoorRainInstance = asset.Value.CreateInstance();
                        if (outdoorRainInstance != null)
                        {
                            outdoorRainInstance.Volume = 0.3f;
                            outdoorRainInstance.IsLooped = true;
                            outdoorRainInstance.Play();
                        }
                    }
                }
                if (indoorRainInstance != null && indoorRainInstance.State == SoundState.Playing)
                {
                    indoorRainInstance.Stop(true);
                    indoorRainInstance = null;
                }
            }
            else if (insiderain)
            {
                if (outdoorRainInstance != null && outdoorRainInstance.State == SoundState.Playing)
                {
                    outdoorRainInstance.Stop(true);
                    outdoorRainInstance = null;
                }
                if (indoorRainInstance == null || indoorRainInstance.State != SoundState.Playing)
                {
                    Asset<SoundEffect> asset = ModContent.Request<SoundEffect>("SariaMod/Sounds/RainIndoors");
                    if (asset != null && asset.IsLoaded)
                    {
                        indoorRainInstance = asset.Value.CreateInstance();
                        if (indoorRainInstance != null)
                        {
                            indoorRainInstance.IsLooped = true;
                            int distanceToCeiling = playerTileY - furthestCeilingTileY;
                            float minDistanceForMaxVolume = 5f;
                            float maxDistanceForMinVolume = 100f;
                            float volume;
                            if (distanceToCeiling <= minDistanceForMaxVolume)
                            {
                                volume = .3f;
                            }
                            else if (distanceToCeiling >= maxDistanceForMinVolume)
                            {
                                volume = 0.1f;
                            }
                            else
                            {
                                float scaleFactor = (distanceToCeiling - minDistanceForMaxVolume) / (maxDistanceForMinVolume - minDistanceForMaxVolume);
                                volume = MathHelper.Lerp(.3f, 0.1f, scaleFactor);
                            }
                            indoorRainInstance.Volume = volume;
                            indoorRainInstance.Play();
                        }
                    }
                }
                else if (indoorRainInstance.State == SoundState.Playing)
                {
                    int distanceToCeiling = playerTileY - furthestCeilingTileY;
                    float minDistanceForMaxVolume = 5f;
                    float maxDistanceForMinVolume = 100f;
                    float volume;
                    if (distanceToCeiling <= minDistanceForMaxVolume)
                    {
                        volume = .3f;
                    }
                    else if (distanceToCeiling >= maxDistanceForMinVolume)
                    {
                        volume = 0.1f;
                    }
                    else
                    {
                        float scaleFactor = (distanceToCeiling - minDistanceForMaxVolume) / (maxDistanceForMinVolume - minDistanceForMaxVolume);
                        volume = MathHelper.Lerp(.3f, 0.1f, scaleFactor);
                    }
                    indoorRainInstance.Volume = volume;
                }
            }
            else
            {
                StopAllLoopedSounds();
            }
            if (Main.rand.NextBool(600) && (insiderain || outsiderain))
            {
                int soundChoice = Main.rand.Next(5);
                string thunderPath = "";
                switch (soundChoice)
                {
                    case 0: thunderPath = "SariaMod/Sounds/Thunder1"; break;
                    case 1: thunderPath = "SariaMod/Sounds/Thunder2"; break;
                    case 2: thunderPath = "SariaMod/Sounds/Thunder3"; break;
                    case 3: thunderPath = "SariaMod/Sounds/Thunder4"; break;
                    case 4: thunderPath = "SariaMod/Sounds/ThunderThighs"; break;
                    default: return;
                }
                if (!string.IsNullOrEmpty(thunderPath))
                {
                    Asset<SoundEffect> thunderAsset = ModContent.Request<SoundEffect>(thunderPath);
                    if (thunderAsset != null && thunderAsset.IsLoaded)
                    {
                        SoundEffectInstance thunderInstance = thunderAsset.Value.CreateInstance();
                        if (thunderInstance != null)
                        {
                            thunderInstance.Play();
                        }
                    }
                }
            }
        }
        public override void PostUpdate()
        {
            if (Player.whoAmI != Main.myPlayer)
            {
                return;
            }
            bool isRaining = Player.ZoneRain && !Player.ZoneSnow;
            if (Main.netMode == NetmodeID.Server)
            {
                if (wasPlayingRain != isRaining)
                {
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)SariaMod.SoundMessageType.SyncRainSoundState);
                    packet.Write(isRaining);
                    packet.Send();
                    wasPlayingRain = isRaining;
                }
            }
            playRainSound = isRaining;
            int playerTileX = (int)(Player.Center.X / 16f);
            int playerTileY = (int)(Player.Center.Y / 16f);
            int maxSearchHeight = 120;
            int closestCeilingTileY = -1;
            int furthestCeilingTileY = -1;
            for (int y = playerTileY - 1; y > playerTileY - maxSearchHeight; y--)
            {
                Tile tile = Framing.GetTileSafely(playerTileX, y);
                if (tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType])
                {
                    closestCeilingTileY = y;
                    break;
                }
            }
            if (closestCeilingTileY != -1)
            {
                furthestCeilingTileY = closestCeilingTileY;
                int consecutiveBlanks = 0;
                for (int y = closestCeilingTileY - 1; y > playerTileY - maxSearchHeight; y--)
                {
                    Tile tile = Framing.GetTileSafely(playerTileX, y);
                    if (!tile.HasTile && tile.WallType == 0)
                    {
                        consecutiveBlanks++;
                        if (consecutiveBlanks >= 5)
                        {
                            furthestCeilingTileY = y + 5;
                            break;
                        }
                    }
                    else
                    {
                        consecutiveBlanks = 0;
                    }
                }
            }
            HandleRainSoundState(playRainSound, closestCeilingTileY, furthestCeilingTileY);
        }
    private static void MiscEffects(Player player, FairyPlayer modPlayer, Mod mod)
        {
            Player player2 = Main.LocalPlayer;
            if (player.statLife < (player.statLifeMax2 / 2))
            {
                player.QuickHeal();
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<BufferProj>()] > 0f)
            {
                if (player.velocity.Y > 1)
                {
                    player.velocity.Y = -10;
                }
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<BufferProj>()] > 0f || player.ownedProjectileCounts[ModContent.ProjectileType<Sweetspot4>()] > 0f)
            {
                player.controlDown = false;
                player.controlLeft = false;
                player.controlRight = false;
                player.controlMount = false;
                player.controlJump = false;
                player.controlHook = false;
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Emeraldspike>()] > 0f || player.ownedProjectileCounts[ModContent.ProjectileType<Emeraldspike2>()] > 0f || (player.ownedProjectileCounts[ModContent.ProjectileType<Emeraldspike3>()] > 0f && player.ownedProjectileCounts[ModContent.ProjectileType<BufferProj>()] <= 0f))
            {
                player.maxFallSpeed = 30f;
                if (player.velocity.Y < 1)
                {
                    player.velocity.Y = 10;
                }
                player.moveSpeed = 0;
                player.wingAccRunSpeed = 0;
            }
            switch (modPlayer.Sarialevel)
            {
                case 0:
                    modPlayer.TMPoints = 0; // Removed "- modPlayer.TMPointsUsed" as it's redundant when TMPoints is 0
                    break;
                case 1:
                    modPlayer.TMPoints = 1 - modPlayer.TMPointsUsed;
                    break;
                case 2:
                    modPlayer.TMPoints = 3 - modPlayer.TMPointsUsed;
                    break;
                case 3:
                    modPlayer.TMPoints = 6 - modPlayer.TMPointsUsed;
                    break;
                case 4:
                    modPlayer.TMPoints = 9 - modPlayer.TMPointsUsed;
                    break;
                case 5:
                    modPlayer.TMPoints = 10 - modPlayer.TMPointsUsed;
                    break;
                case 6:
                    modPlayer.TMPoints = 13 - modPlayer.TMPointsUsed;
                    break;
                default:
                    // Optional: Handle unexpected or out-of-bounds Sarialevels
                    // For example, you could reset the TMPoints to a default value,
                    // or throw an exception if an unexpected value is truly an error state.
                    modPlayer.TMPoints = -modPlayer.TMPointsUsed;
                    break;
            }
            switch (modPlayer.Sarialevel)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    // Get the correct XP thresholds for the current Sarialevel
                    int[] xpThresholds = sarialevelXpThresholds[modPlayer.Sarialevel];
                    // Loop through the thresholds to determine the XPBarLevel
                    for (int i = 0; i < xpThresholds.Length; i++)
                    {
                        if (modPlayer.SariaXp <= xpThresholds[i])
                        {
                            modPlayer.XPBarLevel = i;
                            return; // Exit the method once the level is set
                        }
                    }
                    // If SariaXp is higher than the max threshold, set to XPBarLevel 8
                    modPlayer.XPBarLevel = 8;
                    modPlayer.SariaXp = xpThresholds[xpThresholds.Length - 1] + 1;
                    break;
                case 6:
                    // Handle Sarialevel 6, which has a single XPBarLevel
                    modPlayer.XPBarLevel = 8;
                    // The original code has no XP cap for level 6, so we won't add one here.
                    break;
                default:
                    // Handle unexpected Sarialevels
                    modPlayer.XPBarLevel = 0;
                    modPlayer.SariaXp = 0;
                    break;
            }
            if (Soundtimer > 0)
            {
                Soundtimer--;
            }
            LavaSoundTimer++;
            if (player.ZoneUnderworldHeight)
            {
                if (Main.rand.NextBool(5000))
                {
                    SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/Cave10"));
                }
                if (LavaSoundTimer >= 650)
                {
                    SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/LavaSound"));
                    LavaSoundTimer = 0;
                }
                if (Main.rand.NextBool(1))
                {
                    float radius = (float)Math.Sqrt(Main.rand.Next(3000 * 3000));
                    double angle = Main.rand.NextDouble() * 2.0 * Math.PI;
                    Dust.NewDust(new Vector2(player.Center.X + radius * (float)Math.Cos(angle), player.Center.Y + radius * (float)Math.Sin(angle)), 0, 0, ModContent.DustType<SmokeDust>(), 0f, 0f, 0, default(Color), 1.5f);
                }
            }
            float sneezespot = 5;
            bool Warm = (player.behindBackWall && player.HasBuff(BuffID.Campfire));
            bool immunityToCold = player.HasBuff(BuffID.Warmth) || player.HasBuff(BuffID.OnFire) || player.arcticDivingGear || player.HasBuff(ModContent.BuffType<WillOWispBuff>());
            bool immunityToHeat = player.HasBuff(BuffID.ObsidianSkin) || player.lavaImmune || player.ZoneWaterCandle || player.HasBuff(ModContent.BuffType<Veil>());
            if (player.whoAmI == Main.myPlayer)
            {
                player.buffImmune[ModContent.BuffType<Frostburn2>()] = false;
                player.buffImmune[ModContent.BuffType<Frozen2>()] = false;
                player.buffImmune[ModContent.BuffType<Burning2>()] = false;
                if (player.ZoneSnow && Main.raining && (!immunityToCold || (player.HasBuff(ModContent.BuffType<StatLower>()) && !Warm)))
                {
                    modPlayer.FreezingTemp++;
                    if (!player.behindBackWall)
                    {
                        modPlayer.FreezingTemp++;
                        player.AddBuff(ModContent.BuffType<Frostburn2>(), 2);
                    }
                }
                if (player.ZoneSnow && player.wet && !immunityToCold)
                {
                    modPlayer.FreezingTemp += 3;
                    {
                        modPlayer.FreezingTemp++;
                        player.AddBuff(ModContent.BuffType<Frostburn2>(), 2);
                    }
                }
                if (modPlayer.FreezingTemp >= 3000)
                {
                    player.AddBuff(ModContent.BuffType<Frozen2>(), 398);
                    SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/HardIce"), player.Center);
                    modPlayer.FreezingTemp = 0;
                }
            }
            if (immunityToCold && modPlayer.FreezingTemp > 0)
            {
                modPlayer.FreezingTemp--;
            }
            {
                if (!player.behindBackWall && (!immunityToHeat || (player.HasBuff(ModContent.BuffType<StatLower>()) && !player.HasBuff(ModContent.BuffType<Veil>()))) && player.ZoneUnderworldHeight)
                {
                    player.AddBuff(ModContent.BuffType<Burning2>(), 2, quiet: false);
                }
            }
            if (!player.behindBackWall && (!immunityToCold || (player.HasBuff(ModContent.BuffType<StatLower>()) && (!Warm && !immunityToCold))) && player.InSpace())
            {
                player.AddBuff(ModContent.BuffType<Frostburn3>(), 2, quiet: false);
            }
            if (!player.behindBackWall && (!immunityToHeat || (player.HasBuff(ModContent.BuffType<StatLower>()) && !player.HasBuff(ModContent.BuffType<Veil>()))) && player.InSpace())
            {
                player.AddBuff(ModContent.BuffType<Burning2>(), 2, quiet: false);
            }
            if (((Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].ZoneSnow) && !(Main.player[Main.myPlayer].behindBackWall && player.HasBuff((BuffID.Campfire)))) || (Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].ZoneSkyHeight) && !(Main.player[Main.myPlayer].behindBackWall && player.HasBuff((BuffID.Campfire))) || (Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].ZoneDesert && !Main.dayTime) && !(Main.player[Main.myPlayer].behindBackWall && player.HasBuff((BuffID.Campfire))) || (Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].ZoneRain && !Main.player[Main.myPlayer].ZoneJungle && !(Main.player[Main.myPlayer].ZoneDesert && Main.dayTime)) && !(Main.player[Main.myPlayer].behindBackWall && player.HasBuff((BuffID.Campfire))))
            {
                if (player.velocity.X <= 1)
                {
                    if (Main.rand.NextBool(50))
                    {
                        float radius = (float)Math.Sqrt(Main.rand.Next(sphereRadius3 * sphereRadius3));
                        double angle = Main.rand.NextDouble() * 5.0 * Math.PI;
                        if (player.direction > 0)
                        {
                            sneezespot = 10;
                        }
                        if (player.direction < 0)
                        {
                            sneezespot = -8;
                        }
                        for (int j = 0; j < 2; j++)
                        {
                            Dust.NewDust(new Vector2((player.Center.X + sneezespot) + radius * (float)Math.Cos(angle), (player.Center.Y - 10) + radius * (float)Math.Sin(angle)), 0, 0, ModContent.DustType<Fog>(), 0f, 0f, 0, default(Color), 1.5f);
                        }
                    }
                }
                else if (player.velocity.X > 1)
                {
                    if (Main.rand.NextBool(10))
                    {
                        float radius = (float)Math.Sqrt(Main.rand.Next(sphereRadius3 * sphereRadius3));
                        double angle = Main.rand.NextDouble() * 5.0 * Math.PI;
                        if (player.direction > 0)
                        {
                            sneezespot = 10;
                        }
                        if (player.direction < 0)
                        {
                            sneezespot = -8;
                        }
                        for (int j = 0; j < 2; j++)
                        {
                            Dust.NewDust(new Vector2((player.Center.X + sneezespot) + radius * (float)Math.Cos(angle), (player.Center.Y - 10) + radius * (float)Math.Sin(angle)), 0, 0, ModContent.DustType<Fog>(), 0f, 0f, 0, default(Color), 1.5f);
                        }
                    }
                }
            }
        }
    }
}