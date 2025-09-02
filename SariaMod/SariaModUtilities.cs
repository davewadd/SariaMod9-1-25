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
namespace SariaMod
{
    public static class SariaModUtilities
    {
        public static float alpha1;
        public static bool alpha1Counter;
        public static float alpha2;
        public static bool alpha2Counter;
        public static float alpha3;
        public static bool alpha3Counter;
        public static void StartSandstorm()
        {
            typeof(Sandstorm).GetMethod("StartSandstorm", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
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
        public static void StopSandstorm()
        {
            Sandstorm.Happening = false;
        }
        public static FairyPlayer Fairy(this Player player)
        {
            return player.GetModPlayer<FairyPlayer>();
        }
        public static FairyProjectile Fairy(this Projectile proj)
        {
            return proj.GetGlobalProjectile<FairyProjectile>();
        }
        public static int CountProjectiles(int Type)
        {
            return Main.projectile.Count((Projectile proj) => proj.type == Type && proj.active);
        }
        public static bool InSpace(this Player player)
        {
            float x = (float)Main.maxTilesX / 4200f;
            x *= x;
            return (float)((double)(player.position.Y / 16f - (60f + 10f * x)) / (Main.worldSurface / 6.0)) < 1f;
        }
        public static void HealingProjectile(Projectile projectile, int healing, int playerToHeal, int timeCheck = 120)
        {
            Player player = Main.LocalPlayer;
            Vector2 playerVector = player.Center - projectile.Center;
            float playerDist = playerVector.Length();
            if (player.Hitbox.Intersects(projectile.Hitbox))
            {
                {
                    player.HealEffect(healing, broadcast: false);
                    player.statLife += healing;
                    if (player.statLife > player.statLifeMax2)
                    {
                        player.statLife = player.statLifeMax2;
                    }
                    NetMessage.SendData(66, -1, -1);
                }
            }
        }
        public static void HealingProjectile2(Projectile projectile, int healing, int playerToHeal, float homingVelocity, float N, bool autoHomes = true, int timeCheck = 120)
        {
            Player player = Main.player[playerToHeal];
            float homingSpeed = homingVelocity;
            player.HealEffect(healing, broadcast: false);
            player.statLife += healing;
            if (player.statLife > player.statLifeMax2)
            {
                player.statLife = player.statLifeMax2;
            }
            NetMessage.SendData(66, -1, -1, null, playerToHeal, healing);
        }
        public static string ColorMessage(string msg, Color color)
        {
            StringBuilder stringBuilder = new StringBuilder(msg.Length + 12);
            stringBuilder.Append("[c/").Append(color.Hex4()).Append(':')
                .Append(msg)
                .Append(']');
            return stringBuilder.ToString();
        }
        public static void LightHitWire(int type, int i, int j, int tileX, int tileY)
        {
            int x = i - Main.tile[i, j].TileFrameX / 18 % tileX;
            int y = j - Main.tile[i, j].TileFrameY / 18 % tileY;
            int tileXX18 = 18 * tileX;
            for (int l = x; l < x + tileX; l++)
            {
                for (int m = y; m < y + tileY; m++)
                {
                    if (Main.tile[l, m].HasTile && Main.tile[l, m].TileType == type)
                    {
                        if (Main.tile[l, m].TileFrameX < tileXX18)
                            Main.tile[l, m].TileFrameX += (short)(tileXX18);
                        else
                            Main.tile[l, m].TileFrameX -= (short)(tileXX18);
                    }
                }
            }
            if (Wiring.running)
            {
                for (int k = 0; k < tileX; k++)
                {
                    for (int l = 0; l < tileY; l++)
                        Wiring.SkipWire(x + k, y + l);
                }
            }
        }
        public static void SummonRupeeShard(this Projectile projectile, int ProjectileType, int CrystalState)
        {
            Player player = Main.player[projectile.owner];
            Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 100, projectile.position.Y - 60, 0, 0, ProjectileType, (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
        }
        public static void SariaStatRaise(this Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            if (!player.HasBuff(ModContent.BuffType<StatRaise>()))
            {
                SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/StatRaise"), projectile.Center);
                for (int j = 0; j < 1; j++) //set to 2
                {
                    if (Main.myPlayer == projectile.owner) Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 0, projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<PowerUp>(), (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
                }
                player.AddBuff(ModContent.BuffType<StatRaise>(), 20);
            }
            if (player.HasBuff(ModContent.BuffType<StatRaise>()))
            {
                player.AddBuff(ModContent.BuffType<StatRaise>(), 20);
            }
        }
        public static void SariaStatLower(this Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            if (!player.HasBuff(ModContent.BuffType<StatLower>()))
            {
                SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/StatLower"), projectile.Center);
                for (int j = 0; j < 1; j++) //set to 2
                {
                    if (Main.myPlayer == projectile.owner) Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 0, projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<PowerDown>(), (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
                }
                player.AddBuff(ModContent.BuffType<StatLower>(), 20);
            }
            if (player.HasBuff(ModContent.BuffType<StatLower>()))
            {
                player.AddBuff(ModContent.BuffType<StatLower>(), 20);
            }
        }
        public static void AttackCircleDust(this Projectile projectile, int dusttype, int Severity, int Speed, float Width, float lenght, float Scale)
        {
            for (int i = 0; i < Severity; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(Width, lenght);
                Dust d = Dust.NewDustPerfect(projectile.Center, dusttype, speed * 15, Scale: Scale);
                d.noGravity = true;
            }
        }
        public static void AttackDust(this Projectile projectile, int dusttype, int Severity, int Range)
        {
            if (Main.rand.NextBool(Severity))
            {
                float radius = (float)Math.Sqrt(Main.rand.Next(Range * Range));
                double angle = Main.rand.NextDouble() * 5.0 * Math.PI;
                Dust.NewDust(new Vector2(projectile.Center.X + radius * (float)Math.Cos(angle), projectile.Center.Y + radius * (float)Math.Sin(angle)), 0, 0, dusttype, 0f, 0f, 0, default(Color), 1.5f);
            }
        }
        public static void AttackDust2(this Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            if ((player.HasBuff(ModContent.BuffType<Overcharged>())) && !player.ZoneSnow)
            {
                projectile.AttackDust(ModContent.DustType<StaticDustNormalPurple>(), 1, 34);
                Lighting.AddLight(projectile.Center, Color.Purple.ToVector3());
            }
            else if (player.ZoneSnow)
            {
                projectile.AttackDust(ModContent.DustType<StaticDustNormalPink>(), 1, 34);
                Lighting.AddLight(projectile.Center, Color.Pink.ToVector3());
            }
            else if ((player.HasBuff(ModContent.BuffType<StatRaise>())))
            {
                projectile.AttackDust(ModContent.DustType<StaticDustNormalBlue>(), 1, 34);
                Lighting.AddLight(projectile.Center, Color.Blue.ToVector3());
            }
            else if (player.HasBuff(ModContent.BuffType<StatLower>()))
            {
                projectile.AttackDust(ModContent.DustType<StaticDustNormalRed>(), 1, 34);
                Lighting.AddLight(projectile.Center, Color.Red.ToVector3());
            }
            else
            {
                projectile.AttackDust(ModContent.DustType<StaticDustNormal>(), 1, 34);
                Lighting.AddLight(projectile.Center, Color.Yellow.ToVector3());
            }
        }
        public static void RockDust(this Projectile projectile, int dusttype, int Severity, int Range1, int Range2, int dustspotY, int sneezespotGreater, int sneezespotLesser)
        {
            float sneezespot = 5;
            if (Main.rand.NextBool(Severity))
            {
                float radius = (float)Math.Sqrt(Main.rand.Next(Range1 * Range2));
                double angle = Main.rand.NextDouble() * 5.0 * Math.PI;
                if (projectile.spriteDirection > 0)
                {
                    sneezespot = sneezespotGreater;
                }
                if (projectile.spriteDirection < 0)
                {
                    sneezespot = sneezespotLesser;
                }
                Dust.NewDust(new Vector2((projectile.Center.X + sneezespot) + radius * (float)Math.Cos(angle), (projectile.Center.Y + dustspotY) + radius * (float)Math.Sin(angle)), 0, 0, dusttype, 0f, 0f, 0, default(Color), 1.5f);
            }
        }
        public static void SneezeDust(this Projectile projectile, int dusttype, int Severity, int Range, int dustspotY, int sneezespotGreater, int sneezespotLesser)
        {
            float sneezespot = 5;
            if (Main.rand.NextBool(Severity))
            {
                float radius = (float)Math.Sqrt(Main.rand.Next(Range * Range));
                double angle = Main.rand.NextDouble() * 5.0 * Math.PI;
                if (projectile.spriteDirection > 0)
                {
                    sneezespot = sneezespotGreater;
                }
                if (projectile.spriteDirection < 0)
                {
                    sneezespot = sneezespotLesser;
                }
                Dust.NewDust(new Vector2((projectile.Center.X + sneezespot) + radius * (float)Math.Cos(angle), (projectile.Center.Y + dustspotY) + radius * (float)Math.Sin(angle)), 0, 0, dusttype, 0f, 0f, 0, default(Color), 1.5f);
            }
        }
        public static void FrameChargeElectricitydraw(this Projectile projectile, Texture2D texture, Color lightColor, bool nottoscreen, int startPosX = 0, int startPosY = 0)
        {
            Vector2 startPos = new Vector2(Main.screenWidth + 0, Main.screenHeight - 0) / 2f + new Vector2(0f, 0f);
            if (nottoscreen)
            {
                startPos = projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
            }
            int frameHeight = texture.Height / Main.projFrames[ModContent.ProjectileType<PinkCharge>()];
            int frameY = frameHeight * Main.projFrames[ModContent.ProjectileType<PinkCharge>()];
            Rectangle rectangle = texture.Frame(verticalFrames: 14, frameY: (int)Main.GameUpdateCount / 3 % 14);
            Color drawColor = Color.Lerp(lightColor, Color.WhiteSmoke, 100f);
            drawColor = Color.Lerp(drawColor, Color.LightPink, 0);
            Vector2 origin = rectangle.Size() / 2f;
            float rotation = projectile.rotation;
            float scale = projectile.scale;
            SpriteEffects spriteEffects = SpriteEffects.None;
            startPos.X += startPosX;
            startPos.Y += startPosY;
            Main.spriteBatch.Draw(texture, startPos, rectangle, projectile.GetAlpha(drawColor), rotation, origin, scale, spriteEffects, 0f);
        }
        public static void FrameChargedraw(this Projectile projectile, Texture2D texture, Color lightColor, bool nottoscreen, bool Eightframes, int startPosX = 0, int startPosY = 0)
        {
            Vector2 startPos = new Vector2(Main.screenWidth + 0, Main.screenHeight - 0) / 2f + new Vector2(0f, 0f);
            if (nottoscreen)
            {
                startPos = projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
            }
            int frameHeight = texture.Height / Main.projFrames[ModContent.ProjectileType<PinkCharge>()];
            int frameY = frameHeight * Main.projFrames[ModContent.ProjectileType<PinkCharge>()];
            Rectangle rectangle = texture.Frame(verticalFrames: 4, frameY: (int)Main.GameUpdateCount / 6 % 4);
            if (Eightframes)
            {
                frameHeight = texture.Height / Main.projFrames[ModContent.ProjectileType<BlueCharge>()];
                frameY = frameHeight * Main.projFrames[ModContent.ProjectileType<BlueCharge>()];
                rectangle = texture.Frame(verticalFrames: 8, frameY: (int)Main.GameUpdateCount / 8 % 8);
            }
            Color drawColor = Color.Lerp(lightColor, Color.WhiteSmoke, 100f);
            drawColor = Color.Lerp(drawColor, Color.DarkViolet, 0);
            Vector2 origin = rectangle.Size() / 2f;
            float rotation = projectile.rotation;
            float scale = projectile.scale;
            SpriteEffects spriteEffects = SpriteEffects.None;
            startPos.X += startPosX;
            startPos.Y += startPosY;
            Main.spriteBatch.Draw(texture, startPos, rectangle, projectile.GetAlpha(drawColor), rotation, origin, scale, spriteEffects, 0f);
        }
       
        public static void SariaBubbleFaces(this Projectile projectile, Texture2D texture, bool shoulditflip, int FrameSpeed, int NumFrames, int startPosY, Color lightColor)
        {
            Vector2 startPos = projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
            int frameHeight = texture.Height / NumFrames;
            int frameY = frameHeight * NumFrames;
            Color drawColor = Color.Lerp(lightColor, Color.WhiteSmoke, 20f);
            drawColor = Color.Lerp(drawColor, Color.DarkViolet, 0);
            Rectangle rectangle = texture.Frame(verticalFrames: NumFrames, frameY: (int)Main.GameUpdateCount / FrameSpeed % NumFrames);
            Vector2 origin = rectangle.Size() / 2;
            float rotation = projectile.rotation;
            float scale = projectile.scale;
            SpriteEffects spriteEffects = SpriteEffects.None;
            startPos.Y += startPosY;
            if (projectile.spriteDirection == -1)
            {
                startPos.X += 0;
                if (shoulditflip)
                {
                    spriteEffects = SpriteEffects.FlipHorizontally;
                }
            }
            if (projectile.spriteDirection == 1)
            {
                startPos.X += 0;
            }
            Main.spriteBatch.Draw(texture, startPos, rectangle, projectile.GetAlpha(drawColor), rotation, origin, scale, spriteEffects, 0f);
        }
        public static void SariaMaindraw(this Projectile projectile, Texture2D texture, bool Glowinthedark, bool ShoulditFlip, bool DoesitTrail, int startPosY, int HowlongisTrail, Color lightColor)
        {
            Vector2 startPos = projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
            int frameHeight = texture.Height / Main.projFrames[projectile.type];
            int frameY = frameHeight * projectile.frame;
            Rectangle rectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = rectangle.Size() / 2f;
            float rotation = projectile.rotation;
            float scale = projectile.scale;
            SpriteEffects spriteEffects = SpriteEffects.None;
            Color drawColor = lightColor;
            if (Glowinthedark)
            {
                drawColor = Color.Lerp(lightColor, Color.GhostWhite, 20f);
            }
            startPos.Y += startPosY;
            startPos.X += 0;
            if (ShoulditFlip)
            {
                if (projectile.spriteDirection == -1)
                {
                    spriteEffects = SpriteEffects.FlipHorizontally;
                }
            }
            if (!DoesitTrail)
            {
                Main.spriteBatch.Draw(texture, startPos, rectangle, projectile.GetAlpha(drawColor), rotation, origin, scale, spriteEffects, 0f);
            }
            if (DoesitTrail)
            {
                for (int i = 1; i < HowlongisTrail; i++)
                {
                    startPos = projectile.oldPos[i] + projectile.Size * 0.5f - Main.screenPosition;
                    startPos.Y += startPosY;
                    startPos.X += 0;
                    float completionRatio = (float)i / (float)projectile.oldPos.Length;
                    drawColor = Color.Lerp(drawColor, Color.DeepPink, completionRatio);
                    drawColor = Color.Lerp(drawColor, Color.Transparent, completionRatio);
                    Main.spriteBatch.Draw(texture, startPos, rectangle, projectile.GetAlpha(drawColor), rotation, origin, scale, spriteEffects, 0f);
                }
            }
        }
        public static void SariaSparksDraw(this Projectile projectile, Texture2D texture, Color lightColor)
        {
            Vector2 startPos = projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
            int frameHeight = texture.Height / Main.projFrames[ModContent.ProjectileType<SariaSparks>()];
            int frameY = frameHeight * Main.projFrames[ModContent.ProjectileType<SariaSparks>()];
            Rectangle rectangle = texture.Frame(verticalFrames: 14, frameY: (int)Main.GameUpdateCount / 3 % 14);
            Vector2 origin = rectangle.Size() / 2f;
            float rotation = projectile.rotation;
            float scale = projectile.scale;
            SpriteEffects spriteEffects = SpriteEffects.None;
            Color drawColor = Color.Lerp(lightColor, Color.LightBlue, 2f);
            drawColor = Color.Lerp(drawColor, Color.DarkViolet, 0);
            startPos.Y += 1;
            startPos.X += 0;
            Lighting.AddLight(projectile.Center, Color.LightBlue.ToVector3() * .9f);
            if (projectile.spriteDirection == -1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Main.spriteBatch.Draw(texture, startPos, rectangle, projectile.GetAlpha(drawColor), rotation, origin, scale, spriteEffects, 0f);
        }
        public static void FlatImageDraw(this Projectile projectile, Texture2D texture, Color lightColor, int startPosX = 0, int startPosY = 0)
        {
            Vector2 startPos = new Vector2(Main.screenWidth + 0, Main.screenHeight - 0) / 2f + new Vector2(0f, 0f);
            int frameHeight = texture.Height / 1;
            int frameY = frameHeight * 1;
            Color drawColor = Color.Lerp(lightColor, Color.LightBlue, 90f);
            drawColor = Color.Lerp(drawColor, Color.GhostWhite, 90);
            drawColor = Color.Lerp(drawColor, Color.Transparent, .75f);
            Rectangle rectangle = texture.Frame(verticalFrames: 1, frameY: (int)Main.GameUpdateCount / 6 % 1);
            Vector2 origin = rectangle.Size() / 2f;
            float rotation = projectile.rotation;
            float scale = projectile.scale * 1.25f;
            startPos.X += (startPosX + 1.5f);
            startPos.Y += startPosY;
            SpriteEffects spriteEffects = SpriteEffects.None;
            Main.spriteBatch.Draw(texture, startPos, rectangle, projectile.GetAlpha(drawColor), rotation, origin, scale, spriteEffects, 0f);
        }
        public static void VisualSetUpDraw(this Projectile projectile, Texture2D texture, Color lightColor, int startPosX = 0, int startPosY = 0)
        {
            Vector2 startPos = new Vector2(Main.screenWidth + 0, Main.screenHeight - 0) / 2f + new Vector2(0f, 0f);
            int frameHeight = texture.Height / 1;
            int frameY = frameHeight * 1;
            Color drawColor = Color.Lerp(lightColor, Color.Yellow, 80f);
            drawColor = Color.Lerp(drawColor, Color.DarkViolet, 0);
            Rectangle rectangle = texture.Frame(verticalFrames: 1, frameY: (int)Main.GameUpdateCount / 6 % 1);
            Vector2 origin = rectangle.Size() / 2f;
            float rotation = projectile.rotation;
            float scale = projectile.scale;
            startPos.X += startPosX;
            startPos.Y += startPosY;
            SpriteEffects spriteEffects = SpriteEffects.None;
            Main.spriteBatch.Draw(texture, startPos, rectangle, projectile.GetAlpha(drawColor), rotation, origin, scale, spriteEffects, 0f);
        }
        public static void SariaEyesGlowandFadedraw(this Projectile projectile, Texture2D texture, Color lightColor, Color WhatColor)
        {
            if (alpha3Counter)
            {
                alpha3 -= 0.04f;
            }
            if (alpha3 <= 0)
            {
                alpha3Counter = false;
            }
            if (!alpha3Counter)
            {
                alpha3 += 0.004f;
            }
            if (alpha3 >= 1)
            {
                alpha3Counter = true;
            }
            Vector2 startPos = projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
            int frameHeight = texture.Height / Main.projFrames[projectile.type];
            int frameY = frameHeight * projectile.frame;
            Rectangle rectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = rectangle.Size() / 2f;
            float rotation = projectile.rotation;
            float scale = projectile.scale;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (projectile.spriteDirection == -1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Lighting.AddLight(projectile.Center, Color.DeepPink.ToVector3() * (1f - alpha3));
            Color drawColor = Color.Lerp(lightColor, WhatColor, 300f);
            drawColor = Color.Lerp(drawColor, Color.Transparent, alpha3);
            float light = 80.15f * alpha1;
            startPos.Y += 1;
            startPos.X += 0;
            Main.spriteBatch.Draw(texture, startPos, rectangle, (drawColor), rotation, origin, scale, spriteEffects, 0f);
        }
        public static void DialogueUEyeMaskdraw(this Projectile projectile, Texture2D texture, Color lightColor, Vector2 startPos2, int NumFrames, int WhichFrame)
        {
            Player player = Main.player[projectile.owner];
            int owner = player.whoAmI;
            Vector2 startPos = startPos2;
            int frameHeight = texture.Height / NumFrames;
            int frameY = frameHeight * NumFrames;
            Rectangle rectangle = texture.Frame(verticalFrames: NumFrames, frameY: WhichFrame);
            Vector2 origin = rectangle.Size() / NumFrames;
            float rotation = projectile.rotation;
            float scale = projectile.scale;
            SpriteEffects spriteEffects = SpriteEffects.None;
            Color drawColor = lightColor;
            drawColor = Color.Lerp(drawColor, Color.AntiqueWhite, 20f);
            drawColor = Color.Lerp(drawColor, Color.Transparent, alpha3);
            Main.spriteBatch.Draw(texture, startPos, rectangle, (drawColor), rotation, origin, scale, spriteEffects, 0f);
        }
        public static void DialogueUIMask3draw(this Projectile projectile, Color lightColor, int startPosX = 0, int startPosY = 0)
        {
            Player player = Main.player[projectile.owner];
            int owner = player.whoAmI;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Greetings2EmeraldMask3");
            Vector2 startPos = new Vector2(Main.screenWidth + 0, Main.screenHeight + 0) / 2f + new Vector2(0f, 0f);
            int frameHeight = texture.Height / Main.projFrames[projectile.type];
            int frameY = frameHeight * projectile.frame;
            Rectangle rectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = rectangle.Size() / 2f;
            float rotation = projectile.rotation;
            float scale = projectile.scale;
            SpriteEffects spriteEffects = SpriteEffects.None;
            Color drawColor = lightColor;
            drawColor = Color.Lerp(drawColor, Color.FloralWhite, 30f);
            drawColor = Color.Lerp(drawColor, Color.Transparent, alpha3);
            startPos.X += startPosX;
            startPos.Y += startPosY;
            Main.spriteBatch.Draw(texture, startPos, rectangle, (drawColor), rotation, origin, scale, spriteEffects, 0f);
        }
        public static void DialogueUIMask2draw(this Projectile projectile, Color lightColor, int startPosX = 0, int startPosY = 0)
        {
            Player player = Main.player[projectile.owner];
            int owner = player.whoAmI;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Greetings2EmeraldMask2");
            Vector2 startPos = new Vector2(Main.screenWidth + 0, Main.screenHeight + 0) / 2f + new Vector2(0f, 0f);
            int frameHeight = texture.Height / Main.projFrames[projectile.type];
            int frameY = frameHeight * projectile.frame;
            Rectangle rectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = rectangle.Size() / 2f;
            float rotation = projectile.rotation;
            float scale = projectile.scale;
            SpriteEffects spriteEffects = SpriteEffects.None;
            Color drawColor = lightColor;
            drawColor = Color.Lerp(drawColor, Color.FloralWhite, 30f);
            drawColor = Color.Lerp(drawColor, Color.Transparent, alpha1);
            startPos.X += startPosX;
            startPos.Y += startPosY;
            Main.spriteBatch.Draw(texture, startPos, rectangle, (drawColor), rotation, origin, scale, spriteEffects, 0f);
        }
        public static void DialogueUIMaskdraw(this Projectile projectile, Color lightColor, int startPosX = 0, int startPosY = 0)
        {
            Player player = Main.player[projectile.owner];
            int owner = player.whoAmI;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Greetings2EmeraldMask");
            Vector2 startPos = new Vector2(Main.screenWidth + 0, Main.screenHeight + 0) / 2f + new Vector2(0f, 0f);
            int frameHeight = texture.Height / Main.projFrames[projectile.type];
            int frameY = frameHeight * projectile.frame;
            Rectangle rectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = rectangle.Size() / 2f;
            float rotation = projectile.rotation;
            float scale = projectile.scale;
            SpriteEffects spriteEffects = SpriteEffects.None;
            Color drawColor = lightColor;
            drawColor = Color.Lerp(drawColor, Color.FloralWhite, 30f);
            drawColor = Color.Lerp(drawColor, Color.Transparent, alpha2);
            startPos.X += startPosX;
            startPos.Y += startPosY;
            Main.spriteBatch.Draw(texture, startPos, rectangle, (drawColor), rotation, origin, scale, spriteEffects, 0f);
        }
        public static void Saria5GlowMaskdraw(this Projectile projectile, Texture2D texture, Color lightColor, bool counter1, bool counter2)
        {
            if (alpha1Counter)
            {
                alpha1 -= 0.001f;
            }
            if (alpha1 <= 0)
            {
                alpha1Counter = false;
            }
            if (!alpha1Counter)
            {
                alpha1 += 0.001f;
            }
            if (alpha1 >= 1)
            {
                alpha1Counter = true;
            }
            if (alpha2Counter)
            {
                alpha2 -= 0.002f;
            }
            if (alpha2 <= 0)
            {
                alpha2Counter = false;
            }
            if (!alpha2Counter)
            {
                alpha2 += 0.002f;
            }
            if (alpha2 >= 1)
            {
                alpha2Counter = true;
            }
            Vector2 startPos = projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
            int frameHeight = texture.Height / Main.projFrames[projectile.type];
            int frameY = frameHeight * projectile.frame;
            Rectangle rectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = rectangle.Size() / 2f;
            float rotation = projectile.rotation;
            float scale = projectile.scale;
            SpriteEffects spriteEffects = SpriteEffects.None;
            Lighting.AddLight(projectile.Center, Color.DeepPink.ToVector3() * (1f - alpha1));
            Lighting.AddLight(projectile.Center, Color.Green.ToVector3() * (1f - alpha2));
            Color drawColor = Color.Lerp(lightColor, Color.FloralWhite, 30f);
            if (counter1)
            {
                drawColor = Color.Lerp(drawColor, Color.Transparent, alpha1);
                float light = 80.15f * alpha1;
                projectile.RockDust(ModContent.DustType<RockSparkle>(), (20), 50, 20, -10, 15, 10);
            }
            if (counter2)
            {
                drawColor = Color.Lerp(drawColor, Color.Transparent, alpha2);
                float light = 80.15f * alpha2;
            }
            startPos.Y += 1;
            startPos.X += 0;
            Main.spriteBatch.Draw(texture, startPos, rectangle, (drawColor), rotation, origin, scale, spriteEffects, 0f);
        }
        public static void Saria3GlowMaskdraw(this Projectile projectile, Texture2D texture, int i, int j, Color lightColor)
        {
            Vector2 startPos = projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
            int frameHeight = texture.Height / Main.projFrames[projectile.type];
            int frameY = frameHeight * projectile.frame;
            Rectangle rectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = rectangle.Size() / 2f;
            float rotation = projectile.rotation;
            float scale = projectile.scale;
            SpriteEffects spriteEffects = SpriteEffects.None;
            Color drawColor = Color.Lerp(lightColor, Color.LightYellow, 30f);
            Lighting.AddLight(projectile.Center, Color.Yellow.ToVector3() * .2f);
            ulong randShakeEffect = (Main.GameUpdateCount / 8) ^ (ulong)((long)j << 20 | (long)(uint)i);
            float drawPositionX = i * 1 - (int)Main.screenPosition.X - (projectile.width - 16f) / 2f;
            float drawPositionY = j * 1 - (int)Main.screenPosition.Y;
            float shakeX = Utils.RandomInt(ref randShakeEffect, -4, -3) * 0.07f;
            float shakeY = Utils.RandomInt(ref randShakeEffect, -4, 3) * 0.07f;
            startPos.Y += (1 + shakeX);
            startPos.X += (+0 + shakeY);
            Main.spriteBatch.Draw(texture, startPos, rectangle, projectile.GetAlpha(drawColor), rotation, origin, scale, spriteEffects, 0f);
        }
        public static void DrawFlameEffect(Texture2D flameTexture, int i, int j, int offsetX = 0, int offsetY = 0)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
            int width = 16;
            int height = 16;
            int yOffset = TileObjectData.GetTileData(tile).DrawYOffset;
            ulong randShakeEffect = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)(uint)i);
            float drawPositionX = i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f;
            float drawPositionY = j * 16 - (int)Main.screenPosition.Y;
            for (int c = 0; c < 7; c++)
            {
                float shakeX = Utils.RandomInt(ref randShakeEffect, -10, 11) * 0.15f;
                float shakeY = Utils.RandomInt(ref randShakeEffect, -10, 1) * 0.35f;
                Main.spriteBatch.Draw(flameTexture, new Vector2(drawPositionX + shakeX, drawPositionY + shakeY + yOffset) + zero, new Rectangle(tile.TileFrameX + offsetX, tile.TileFrameY + offsetY, width, height), new Color(100, 100, 100, 0), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
            }
        }
        public static void DrawFlameSparks(int dustType, int rarity, int i, int j)
        {
            if (!Main.gamePaused && Main.instance.IsActive && (!Lighting.UpdateEveryFrame || Main.rand.NextBool(4)))
            {
                if (Main.rand.NextBool(rarity))
                {
                    int dust = Dust.NewDust(new Vector2(i * 16 + 4, j * 16 + 2), 4, 4, dustType, 0f, 0f, 100, default, 1f);
                    if (Main.rand.Next(3) != 0)
                        Main.dust[dust].noGravity = true;
                    // Prevent lag.
                    Main.dust[dust].noLightEmittence = true;
                    Main.dust[dust].velocity *= 0.3f;
                    Main.dust[dust].velocity.Y = Main.dust[dust].velocity.Y - 1.5f;
                }
            }
        }
        public static void BlueRingofdust(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            for (int j = 0; j < 72; j++)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, 113);
                dust.velocity = ((float)Math.PI * 2f * Vector2.Dot(((float)j / 72f * ((float)Math.PI * 2f)).ToRotationVector2(), player.velocity.SafeNormalize(Vector2.UnitY).RotatedBy((float)j / 72f * ((float)Math.PI * -2f)))).ToRotationVector2();
                dust.velocity = dust.velocity.RotatedBy((float)j / 36f * ((float)Math.PI * 2f)) * 8f;
                dust.noGravity = true;
                dust.scale *= 3.9f;
            }
        }
        public static void SariaBaseDamage(this Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            FairyPlayer modPlayer = player.Fairy();
            if (modPlayer.Sarialevel == 6)
            {
                projectile.damage = 900 + (modPlayer.SariaXp / 20);
                projectile.netUpdate = true;
            }
            else if (modPlayer.Sarialevel == 5)
            {
                projectile.damage = 200 + (modPlayer.SariaXp / 342);
                projectile.netUpdate = true;
            }
            else if (modPlayer.Sarialevel == 4)
            {
                projectile.damage = 75 + (modPlayer.SariaXp / 640);
                projectile.netUpdate = true;
            }
            else if (modPlayer.Sarialevel == 3)
            {
                projectile.damage = 50 + (modPlayer.SariaXp / 1600);
                projectile.netUpdate = true;
            }
            else if (modPlayer.Sarialevel == 2)
            {
                projectile.damage = 26 + (modPlayer.SariaXp / 833);
                projectile.netUpdate = true;
            }
            else if (modPlayer.Sarialevel == 1)
            {
                projectile.damage = 15 + (modPlayer.SariaXp / 818);
                projectile.netUpdate = true;
            }
            else
            {
                projectile.damage = 10 + (modPlayer.SariaXp / 600);
                projectile.netUpdate = true;
            }
            if (player.HasBuff(ModContent.BuffType<StatRaise>()))
            {
                projectile.damage += (projectile.damage) / 4;
            }
            else if (player.HasBuff(ModContent.BuffType<StatLower>()))
            {
                projectile.damage /= 2;
            }
        }
        public static NPC MinionHoming(this Vector2 origin, float maxDistanceToCheck, Player owner, bool ignoreTiles = true)
        {
            if (owner == null || owner.whoAmI < 0 || owner.whoAmI > 255 || owner.MinionAttackTargetNPC < 0 || owner.MinionAttackTargetNPC > 200)
            {
                return origin.ClosestNPCAt(maxDistanceToCheck, ignoreTiles);
            }
            NPC npc = Main.npc[owner.MinionAttackTargetNPC];
            bool canHit = true;
            if (!ignoreTiles)
            {
                origin = owner.Center;
                canHit = Collision.CanHit(origin, 1, 1, npc.Center, 1, 1);
            }
            if (owner.HasMinionAttackTargetNPC && canHit)
            {
                return npc;
            }
            return origin.ClosestNPCAt(maxDistanceToCheck, ignoreTiles);
        }
        public static NPC ClosestNPCAt(this Vector2 origin, float maxDistanceToCheck, bool ignoreTiles = true, bool bossPriority = false)
        {
            NPC closestTarget = null;
            float distance = maxDistanceToCheck;
            if (bossPriority)
            {
                bool bossFound = false;
                for (int index2 = 0; index2 < Main.npc.Length; index2++)
                {
                    if ((bossFound && !Main.npc[index2].boss && Main.npc[index2].type != NPCID.WallofFleshEye) || !Main.npc[index2].CanBeChasedBy())
                    {
                        continue;
                    }
                    float extraDistance2 = Main.npc[index2].width / 2 + Main.npc[index2].height / 2;
                    bool canHit2 = true;
                    if (extraDistance2 < distance && !ignoreTiles)
                    {
                        canHit2 = Collision.CanHit(origin, 1, 1, Main.npc[index2].Center, 1, 1);
                    }
                    if (Vector2.Distance(origin, Main.npc[index2].Center) < distance + extraDistance2 && canHit2)
                    {
                        if (Main.npc[index2].boss || Main.npc[index2].type == NPCID.WallofFleshEye)
                        {
                            bossFound = true;
                        }
                        distance = Vector2.Distance(origin, Main.npc[index2].Center);
                        closestTarget = Main.npc[index2];
                    }
                }
            }
            else
            {
                for (int index = 0; index < Main.npc.Length; index++)
                {
                    if (Main.npc[index].CanBeChasedBy())
                    {
                        float extraDistance = Main.npc[index].width / 2 + Main.npc[index].height / 2;
                        bool canHit = true;
                        if (extraDistance < distance && !ignoreTiles)
                        {
                            canHit = Collision.CanHit(origin, 1, 1, Main.npc[index].Center, 1, 1);
                        }
                        if (Vector2.Distance(origin, Main.npc[index].Center) < distance + extraDistance && canHit)
                        {
                            distance = Vector2.Distance(origin, Main.npc[index].Center);
                            closestTarget = Main.npc[index];
                        }
                    }
                }
            }
            return closestTarget;
        }
    }
}