using Microsoft.Xna.Framework;
using System.Linq;
using System.Reflection;
using System.Text;
using Terraria;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using System;
using System.IO;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SariaMod.Items;
using SariaMod.Buffs;
using SariaMod.Items.Strange;
using SariaMod.Dusts;
using SariaMod.Items.Amber;
using SariaMod.Items.Amethyst;
using SariaMod.Items.Bands;
using SariaMod.Items.Emerald;
using SariaMod.Items.Ruby;
using SariaMod.Items.Sapphire;
using SariaMod.Items.Topaz;
using SariaMod.Items.zPearls;
using SariaMod.Items.zTalking;
using Terraria.Localization;
using Terraria.Audio;
using Terraria.UI;
using SariaMod;
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;
using Terraria.DataStructures;

namespace SariaMod.Items.Strange
{
    public static class SariaExtensions1
    {
        public static float alpha1;
        public static bool alpha1Counter;
        public static float alpha2;
        public static bool alpha2Counter;
        public static float alpha3;
        public static bool alpha3Counter;

        public enum InterfaceType
        {
            XPBar,
            NextBoss
        }

        public static void SendPacket(this Player player, ModPacket packet, bool server)
        {
            // Client: Send the packet only to the host.
            if (!server)
                packet.Send();
            // Server: Send the packet to every OTHER client.
            else
                packet.Send(-1, player.whoAmI);
        }
        internal static void SetUpCandle(ModTile mt, bool lavaImmune = false, int offset = -4)
        {
            Main.tileLighted[mt.Type] = true;
            Main.tileFrameImportant[mt.Type] = true;
            Main.tileLavaDeath[mt.Type] = !lavaImmune;
            Main.tileWaterDeath[mt.Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
            TileObjectData.newTile.CoordinateHeights = new int[1] { 20 };
            TileObjectData.newTile.LavaDeath = !lavaImmune;
            TileObjectData.newTile.DrawYOffset = offset;
            TileObjectData.addTile(mt.Type);
            mt.AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
        }
        private static Asset<Texture2D> GetXPBarTexture(FairyPlayer modPlayer)
        {
            return modPlayer.XPBarLevel switch
            {
                1 => ModContent.Request<Texture2D>("SariaMod/Items/Strange/SariaXpBar/SariaXPBar2"),
                2 => ModContent.Request<Texture2D>("SariaMod/Items/Strange/SariaXpBar/SariaXPBar3"),
                3 => ModContent.Request<Texture2D>("SariaMod/Items/Strange/SariaXpBar/SariaXPBar4"),
                4 => ModContent.Request<Texture2D>("SariaMod/Items/Strange/SariaXpBar/SariaXPBar5"),
                5 => ModContent.Request<Texture2D>("SariaMod/Items/Strange/SariaXpBar/SariaXPBar6"),
                6 => ModContent.Request<Texture2D>("SariaMod/Items/Strange/SariaXpBar/SariaXPBar7"),
                7 => ModContent.Request<Texture2D>("SariaMod/Items/Strange/SariaXpBar/SariaXPBar8"),
                8 => ModContent.Request<Texture2D>("SariaMod/Items/Strange/SariaXpBar/SariaXPBar9"),
                _ => ModContent.Request<Texture2D>("SariaMod/Items/Strange/SariaXpBar/SariaXPBar1")
            };
        }

        private static Asset<Texture2D> GetNextBossTexture(FairyPlayer modPlayer)
        {
            return modPlayer.Sarialevel switch
            {
                1 => ModContent.Request<Texture2D>("SariaMod/Items/Bands/QueenBee"),
                2 => ModContent.Request<Texture2D>("SariaMod/Items/Bands/WallOfFlesh"),
                3 => ModContent.Request<Texture2D>("SariaMod/Items/Bands/Retinazer"),
                4 => ModContent.Request<Texture2D>("SariaMod/Items/Bands/Plantera"),
                5 => ModContent.Request<Texture2D>("SariaMod/Items/Bands/TheDuke"),
                6 => ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank"),
                _ => ModContent.Request<Texture2D>("SariaMod/Items/Bands/KingSlime")
            };
        }
        public static void SariaDrawInterface(this Projectile projectile, Color lightColor, InterfaceType type)
        {
            Player player = Main.player[projectile.owner];
            FairyPlayer modPlayer = player.Fairy();

            if (Main.myPlayer != projectile.owner)
            {
                return;
            }

            // Fix: Access the .Value property to get the Texture2D from the Asset<Texture2D>
            Texture2D texture = type switch
            {
                InterfaceType.XPBar => GetXPBarTexture(modPlayer).Value,
                InterfaceType.NextBoss => GetNextBossTexture(modPlayer).Value,
                _ => ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value // Default/fallback
            };

            Vector2 startPos = projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
            Vector2 offset = type switch
            {
                InterfaceType.XPBar => new Vector2(0f, 60f),
                InterfaceType.NextBoss => new Vector2(43f, 60f),
                _ => Vector2.Zero
            };

            Color drawColor = Color.Lerp(lightColor, Color.LightPink, 20f);
            drawColor = Color.Lerp(drawColor, Color.DarkViolet, 0);

            Rectangle rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = rectangle.Size() / 2f;

            Main.spriteBatch.Draw(texture, startPos + offset, null, projectile.GetAlpha(drawColor), projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0f);
        }
        public static void SariaChargingAnimation(this Projectile projectile, int Transform, bool Sleep, int Eating, int isCharging, bool Cursed, int ChannelState, int Mood, Color lightColor)
        {
            Player player = Main.player[projectile.owner];
            bool isRight = (projectile.spriteDirection == 1);

            if (!Sleep && Eating <= 0 && ChannelState > 0 && (projectile.frame < 20 || isCharging >= 1))
            {
                // General file path logic
                string baseDir = $"SariaMod/Items/Strange/{Transform + 1}SariaAnimations/{Transform + 1}SariaCharging";
                string dirSuffix = isRight ? "Right" : "Left";
                string baseTexturePath = $"{baseDir}{dirSuffix}";
                string basicMaskPath = $"SariaMod/Items/Strange/1SariaAnimations/1SariaCharging{dirSuffix}Mask1";

                switch (Transform)
                {
                    case 0:
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>(baseTexturePath).Value, false, false, false, 1, 1, lightColor);
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>($"{baseTexturePath}Mask1").Value, true, false, false, 1, 1, lightColor);
                        projectile.SariaSmallChargeSetup(Transform, isRight, lightColor);
                        break;
                    case 1:
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>(baseTexturePath).Value, false, false, false, 1, 1, lightColor);
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>($"{baseTexturePath}Mask1").Value, true, false, false, 1, 1, lightColor);
                        projectile.SariaSmallChargeSetup(Transform, isRight, lightColor);
                        break;
                    case 2:
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>(baseTexturePath).Value, false, false, false, 1, 1, lightColor);
                        projectile.Saria3GlowMaskdraw(ModContent.Request<Texture2D>($"{baseTexturePath}Mask2").Value, 1, 1, lightColor);
                        projectile.SariaSmallChargeSetup(Transform, isRight, lightColor);
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>($"{baseTexturePath}Mask1").Value, true, false, false, 1, 1, lightColor);
                        break;
                    case 3:
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>(baseTexturePath).Value, false, false, false, 1, 1, lightColor);
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>($"{baseTexturePath}Mask3").Value, true, false, false, 1, 1, lightColor);
                        if (Main.rand.NextBool(40))
                        {
                            projectile.SariaMaindraw(ModContent.Request<Texture2D>($"{baseTexturePath}Mask2").Value, true, false, false, 1, 1, lightColor);
                        }
                        projectile.SariaSmallChargeSetup(Transform, isRight, lightColor);
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>($"{baseTexturePath}Mask1").Value, true, false, false, 1, 1, lightColor);
                        break;
                    case 4:
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>(baseTexturePath).Value, true, false, false, 1, 1, lightColor);
                        projectile.Saria5GlowMaskdraw(ModContent.Request<Texture2D>($"{baseTexturePath}Mask2").Value, lightColor, true, false);
                        projectile.Saria5GlowMaskdraw(ModContent.Request<Texture2D>($"{baseTexturePath}Mask3").Value, lightColor, false, true);
                        projectile.SariaSmallChargeSetup(Transform, isRight, lightColor);
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>($"{baseTexturePath}Mask1").Value, true, false, false, 1, 1, lightColor);
                        break;
                    case 5:
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>(baseTexturePath).Value, false, false, false, 1, 1, lightColor);
                        projectile.SariaSmallChargeSetup(Transform, isRight, lightColor);
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>($"{baseTexturePath}Mask1").Value, true, false, false, 1, 1, lightColor);
                        break;
                    case 6:
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>(baseTexturePath).Value, false, false, false, 1, 1, lightColor);
                        projectile.SariaSmallChargeSetup(Transform, isRight, lightColor);
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>($"{baseTexturePath}Mask1").Value, true, false, false, 1, 1, lightColor);
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/7SariaAnimations/7SariaChargingEyes").Value, true, true, false, 1, 1, lightColor);
                        break;
                }

                // Logic for Transform != 6, note path changes due to new folder structure
                if (Transform != 6)
                {
                    projectile.SariaMaindraw(ModContent.Request<Texture2D>("SariaMod/Items/Strange/GlobalSariaAnimations/SariaChargingEyes").Value, true, true, false, 1, 1, lightColor);
                    if (Transform == 4)
                    {
                        projectile.SariaEyesGlowandFadedraw(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/Strange/5SariaAnimations/5SariaChargingEyes").Value), lightColor, Color.White);
                    }
                }
            }
        }
        public static void SariaBodyDraw(this Projectile projectile, int Transform, int Eating, int isCharging, int ChannelState, int SpecialAnimate, Color lightColor)
        {
            Player player = Main.player[projectile.owner];
            // Assuming Fairy() is a valid extension or method on Player, you can keep this.
            // FairyPlayer modPlayer = player.Fairy();

            bool IsEating = (Eating == 3 || Eating == 4) && projectile.frame <= 60;
            bool ThistoRight = (projectile.spriteDirection == 1 && !IsEating && !(ChannelState > 0 && (projectile.frame < 20 || isCharging >= 1)));
            bool ThistoLeft = (projectile.spriteDirection == -1 && !IsEating && !(ChannelState > 0 && (projectile.frame < 20 || isCharging >= 1)));

            switch (Transform)
            {
                case 0:
                    if (IsEating)
                    {
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>("SariaMod/Items/Strange/1SariaAnimations/1SariaEat").Value, false, false, false, 1, 1, lightColor);
                    }
                    if (ThistoRight)
                    {
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>("SariaMod/Items/Strange/1SariaAnimations/1SariaRight").Value, false, false, false, 1, 1, lightColor);
                    }
                    if (ThistoLeft)
                    {
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>("SariaMod/Items/Strange/1SariaAnimations/1SariaLeft").Value, false, false, false, 1, 1, lightColor);
                    }
                    break;
                case 1:
                    if (IsEating)
                    {
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>("SariaMod/Items/Strange/2SariaAnimations/2SariaEat").Value, false, false, false, 1, 1, lightColor);
                    }
                    if (ThistoRight)
                    {
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>("SariaMod/Items/Strange/2SariaAnimations/2SariaRight").Value, false, false, false, 1, 1, lightColor);
                    }
                    if (ThistoLeft)
                    {
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>("SariaMod/Items/Strange/2SariaAnimations/2SariaLeft").Value, false, false, false, 1, 1, lightColor);
                    }
                    break;
                case 2:
                    string dirSuffix2 = ThistoRight ? "Right" : "Left";
                    if (IsEating)
                    {
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>("SariaMod/Items/Strange/3SariaAnimations/3SariaEat").Value, true, false, false, 1, 1, lightColor);
                        projectile.Saria3GlowMaskdraw(ModContent.Request<Texture2D>("SariaMod/Items/Strange/3SariaAnimations/3SariaEatMask1").Value, 1, 1, lightColor);
                    }
                    if (ThistoRight || ThistoLeft)
                    {
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/3SariaAnimations/3Saria{dirSuffix2}").Value, true, false, false, 1, 1, lightColor);
                        projectile.Saria3GlowMaskdraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/3SariaAnimations/3Saria{dirSuffix2}Mask1").Value, 1, 1, lightColor);
                    }
                    break;
                case 3:
                    string dirSuffix3 = ThistoRight ? "Right" : "Left";
                    if (IsEating)
                    {
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>("SariaMod/Items/Strange/4SariaAnimations/4SariaEat").Value, false, false, false, 1, 1, lightColor);
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>("SariaMod/Items/Strange/4SariaAnimations/4SariaEatMask1").Value, true, false, false, 1, 1, lightColor);
                        if (Main.rand.NextBool(40))
                        {
                            projectile.SariaMaindraw(ModContent.Request<Texture2D>("SariaMod/Items/Strange/4SariaAnimations/4SariaEatMask2").Value, true, false, false, 1, 1, lightColor);
                        }
                    }
                    if (ThistoRight || ThistoLeft)
                    {
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/4SariaAnimations/4Saria{dirSuffix3}").Value, false, false, false, 1, 1, lightColor);
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/4SariaAnimations/4Saria{dirSuffix3}Mask1").Value, true, false, false, 1, 1, lightColor);
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/4SariaAnimations/4Saria{dirSuffix3}Mask2").Value, true, false, false, 1, 1, lightColor);
                        if (Main.rand.NextBool(40))
                        {
                            projectile.SariaMaindraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/4SariaAnimations/4Saria{dirSuffix3}Mask3").Value, true, false, false, 1, 1, lightColor);
                        }
                    }
                    break;
                case 4:
                    string dirSuffix4 = ThistoRight ? "Right" : "Left";
                    if (IsEating)
                    {
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/5SariaAnimations/5SariaEat").Value, true, false, false, 1, 1, lightColor);
                        projectile.Saria5GlowMaskdraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/5SariaAnimations/5SariaEatMask1").Value, lightColor, true, false);
                        projectile.Saria5GlowMaskdraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/5SariaAnimations/5SariaEatMask2").Value, lightColor, false, true);
                    }
                    if (ThistoRight || ThistoLeft)
                    {
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/5SariaAnimations/5Saria{dirSuffix4}").Value, true, false, false, 1, 1, lightColor);
                        projectile.Saria5GlowMaskdraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/5SariaAnimations/5Saria{dirSuffix4}Mask1").Value, lightColor, true, false);
                        projectile.Saria5GlowMaskdraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/5SariaAnimations/5Saria{dirSuffix4}Mask2").Value, lightColor, false, true);
                    }
                    break;
                case 5:
                    string dirSuffix5 = ThistoRight ? "Right" : "Left";
                    if (IsEating)
                    {
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/6SariaAnimations/6SariaEat").Value, false, false, false, 1, 1, lightColor);
                    }
                    if (ThistoRight || ThistoLeft)
                    {
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/6SariaAnimations/6Saria{dirSuffix5}").Value, false, false, false, 1, 1, lightColor);
                    }
                    break;
                case 6:
                    string dirSuffix6 = ThistoRight ? "Right" : "Left";
                    if (IsEating)
                    {
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/7SariaAnimations/7SariaEat").Value, false, false, false, 1, 1, lightColor);
                    }
                    if (ThistoRight || ThistoLeft)
                    {
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/7SariaAnimations/7Saria{dirSuffix6}").Value, false, false, false, 1, 1, lightColor);
                    }
                    break;
            }

            if (Transform == 3 && SpecialAnimate > 0)
            {
                projectile.SariaSparksDraw(TextureAssets.Projectile[ModContent.ProjectileType<SariaSparks>()].Value, lightColor);
            }
        }
        public static void SariaEatDraw(this Projectile projectile, int Transform, int Eating, Color lightColor)
        {
            if (Eating == 3 || Eating == 4)
            {
                // Logic for eyes during eating animations
                if (Transform != 6)
                {
                    projectile.SariaMaindraw(ModContent.Request<Texture2D>("SariaMod/Items/Strange/GlobalSariaAnimations/SariaEatEyes").Value, true, false, false, 1, 1, lightColor);
                    if (Transform == 4)
                    {
                        projectile.SariaEyesGlowandFadedraw(ModContent.Request<Texture2D>("SariaMod/Items/Strange/5SariaAnimations/5SariaEatEyes").Value, lightColor, Color.White);
                    }
                }
                else // Transform == 6
                {
                    // Use string interpolation for the Transform 6 eating eyes
                    projectile.SariaMaindraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/{Transform + 1}SariaAnimations/{Transform + 1}SariaEatEyes").Value, true, false, false, 1, 1, lightColor);
                }
            }

            // Logic for the eating aura/effect
            if (Eating == 3)
            {
                // Use string interpolation for the Eating 3 texture
                projectile.SariaMaindraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/GlobalSariaAnimations/SariaEat3").Value, true, false, false, 1, 1, lightColor);
            }
            if (Eating == 4)
            {
                // Use string interpolation for the Eating 4 texture
                projectile.SariaMaindraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/GlobalSariaAnimations/SariaEat2").Value, true, false, false, 1, 1, lightColor);
            }
        }
        public static void SariaSleepDraw(this Projectile projectile, int Transform, bool Sleeping, Color lightColor)
        {
            if (Sleeping && Transform != 6 && Transform != 7)
            {
                projectile.SariaMaindraw(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/Strange/GlobalSariaAnimations/SariaSleep").Value), true, true, false, 1, 1, lightColor);
                if (Transform == 4)
                {
                    projectile.SariaEyesGlowandFadedraw(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/Strange/5SariaAnimations/5SariaSleep").Value), lightColor, Color.White);
                }
            }
            if (Sleeping && Transform == 6)
            {
                projectile.SariaMaindraw(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/Strange/7SariaAnimations/7SariaSleep").Value), true, true, false, 1, 1, lightColor);
            }
        }
        public static void SariaBubbleFaceLoader(this Projectile projectile, int changeform, int eating, Color lightColor)
        {
            Player player = Main.player[projectile.owner];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Notice>()] >= 1f)
            {
                projectile.SariaBubbleFaces(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/Notice").Value), true, 1, 1, -50, lightColor);
            }
            if (changeform <= 0 && (Main.myPlayer == projectile.owner))
            {
                if (player.ownedProjectileCounts[ModContent.ProjectileType<Competitive>()] >= 1f && eating == 4)
                {
                    projectile.SariaBubbleFaces(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/Competitive").Value), true, 60, 2, -50, lightColor);
                }
                if (player.ownedProjectileCounts[ModContent.ProjectileType<Smile>()] >= 1f || player.ownedProjectileCounts[ModContent.ProjectileType<Smile2>()] >= 1f || player.ownedProjectileCounts[ModContent.ProjectileType<Happiness>()] >= 1f)
                {
                    projectile.SariaBubbleFaces(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/Smile").Value), true, 60, 2, -50, lightColor);
                }
                if (player.ownedProjectileCounts[ModContent.ProjectileType<Anger>()] >= 1f)
                {
                    projectile.SariaBubbleFaces(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/Anger").Value), true, 60, 2, -50, lightColor);
                }
                if (player.ownedProjectileCounts[ModContent.ProjectileType<Sad>()] >= 1f || player.ownedProjectileCounts[ModContent.ProjectileType<Sad2>()] >= 1f)
                {
                    projectile.SariaBubbleFaces(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/Sad").Value), true, 60, 2, -50, lightColor);
                }
                if (player.ownedProjectileCounts[ModContent.ProjectileType<Flash>()] >= 1f)
                {
                    projectile.SariaBubbleFaces(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/Flash").Value), true, 5, 11, -50, lightColor);
                }
            }
        }
        public static void SariaFeetandArmDraw(this Projectile projectile, int Transform, int eating, Color lightColor)
        {
            bool isRight = (projectile.spriteDirection == 1);
            string dirSuffix = isRight ? "Right" : "Left";

            // Feet drawing logic
            string feetTexturePath = eating <= 2 ? "SariaMod/Items/Strange/GlobalSariaAnimations/SariaFeet" : "SariaMod/Items/Strange/GlobalSariaAnimations/SariaFeetEating";
            projectile.SariaMaindraw(ModContent.Request<Texture2D>(feetTexturePath).Value, true, true, true, 4, 25, lightColor);

            // Default arm drawing logic
            string armTexturePath = $"SariaMod/Items/Strange/GlobalSariaAnimations/SariaArm{dirSuffix}";
            projectile.SariaMaindraw(ModContent.Request<Texture2D>(armTexturePath).Value, true, false, true, 1, 30, lightColor);

            // Conditional attack arm drawing logic
            switch (Transform)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    string attackArmTexturePath = $"SariaMod/Items/Strange/{Transform + 1}SariaAnimations/{Transform + 1}SariaAttackArm{dirSuffix}";
                    projectile.SariaMaindraw(ModContent.Request<Texture2D>(attackArmTexturePath).Value, true, false, true, 1, 3, lightColor);
                    break;
            }
        }
        public static void SariaSmallFacesOrWhencursed(this Projectile projectile, int Transform, bool Sleep, int Eating, int isCharging, bool Cursed, int ChannelState, int Mood, Color lightColor)
        {
            Player player = Main.player[projectile.owner];
            bool isRight = (projectile.spriteDirection == 1);

            // Draw regular face and attack arms if not sleeping, not eating, not charging, and not cursed
            if (!Sleep && Eating <= 2 && !(ChannelState > 0 && (projectile.frame < 20 || isCharging >= 1)))
            {
                if (!Cursed)
                {
                    string faceTextureName;
                    string form5FaceTextureName = null;
                    string form7FaceTextureName = null;

                    // Logic to determine face based on Mood
                    if (Mood >= 3600)
                    {
                        faceTextureName = "SariaPumped";
                        form5FaceTextureName = "5SariaPumped";
                        form7FaceTextureName = "7SariaPumped";
                    }
                    else if (Mood >= 2400)
                    {
                        faceTextureName = "SariaHappy";
                        form5FaceTextureName = "5SariaHappy";
                        form7FaceTextureName = "7SariaHappy";
                    }
                    else if ((Mood <= -1200 && Mood > -2400) || Mood <= -3600 || player.HasBuff(ModContent.BuffType<Extinguished>()))
                    {
                        faceTextureName = "SariaSad";
                        form5FaceTextureName = "5SariaSad";
                        form7FaceTextureName = "7SariaSad";
                    }
                    else if (Mood <= -2400 && Mood > -3600)
                    {
                        faceTextureName = "SariaAngry";
                        form7FaceTextureName = "7SariaAngry";
                    }
                    else
                    {
                        faceTextureName = "SariaNormalFace";
                        form5FaceTextureName = "5SariaNormalFace";
                        form7FaceTextureName = "7SariaNormalFace";
                    }

                    // Drawing logic for faces
                    if (Transform != 6 && Transform != 7)
                    {
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/GlobalSariaAnimations/{faceTextureName}").Value, true, true, false, 1, 1, lightColor);
                        if (Transform == 4 && form5FaceTextureName != null)
                        {
                            projectile.SariaEyesGlowandFadedraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/5SariaAnimations/{form5FaceTextureName}").Value, lightColor, Color.White);
                        }
                    }
                    if (Transform == 6)
                    {
                        if (form7FaceTextureName != null)
                        {
                            projectile.SariaMaindraw(ModContent.Request<Texture2D>($"SariaMod/Items/Strange/7SariaAnimations/{form7FaceTextureName}").Value, true, true, false, 1, 1, lightColor);
                        }
                    }

                    // Drawing logic for attack arms
                    string dirSuffix = isRight ? "Right" : "Left";
                    if (Transform >= 0 && Transform <= 6) // All forms draw an attack arm
                    {
                        string attackArmTexturePath = $"SariaMod/Items/Strange/{Transform + 1}SariaAnimations/{Transform + 1}SariaAttackArm{dirSuffix}";
                        projectile.SariaMaindraw(ModContent.Request<Texture2D>(attackArmTexturePath).Value, true, false, true, 1, 3, lightColor);
                    }
                }
                if (Cursed)
                {
                    projectile.SariaMaindraw(ModContent.Request<Texture2D>("SariaMod/Items/Strange/GlobalSariaAnimations/SariaShader").Value, true, true, false, 1, 1, lightColor);
                    projectile.SariaMaindraw(ModContent.Request<Texture2D>("SariaMod/Items/Strange/GlobalSariaAnimations/SariaCursed").Value, true, true, false, 1, 1, lightColor);
                }
            }

        }


    }
}