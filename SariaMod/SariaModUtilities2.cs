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
    public static class SariaModUtilities2
    {
        public static float alpha4;
        public static bool alpha4Counter;
        public static float alpha5;
        public static bool alpha5Counter;
        public static float alpha6;
        public static bool alpha6Counter;
        public static void BlueRingofdust(this Projectile projectile, int howmany)
        {
            Player player = Main.player[projectile.owner];
            for (int j = 0; j < howmany; j++)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, 113);
                dust.velocity = ((float)Math.PI * 2f * Vector2.Dot(((float)j / 72f * ((float)Math.PI * 2f)).ToRotationVector2(), player.velocity.SafeNormalize(Vector2.UnitY).RotatedBy((float)j / 72f * ((float)Math.PI * -2f)))).ToRotationVector2();
                dust.velocity = dust.velocity.RotatedBy((float)j / 36f * ((float)Math.PI * 2f)) * 8f;
                dust.noGravity = true;
                dust.scale *= 3.9f;
            }
        }
        public static void SariaSmallChargeSetup(this Projectile projectile, int Transform, bool IsRight, Color lightColor)
        {
            int startpositionx = -50;
            int startpositiony = 10;
            if (IsRight)
            {
                startpositionx = 12;
                startpositiony = 10;
            }
                if (Transform == 0)
                {
                    projectile.FrameChargeElectricitydraw(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/Strange/1SariaAnimations/1ChargingSpark").Value), lightColor, true,startpositionx, startpositiony);
            }
                if (Transform == 1)
                {
                projectile.FrameChargeElectricitydraw(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/Strange/2SariaAnimations/2ChargingSpark").Value), lightColor, true, startpositionx, startpositiony);
            }
                if (Transform == 2)
                {
                projectile.FrameChargeElectricitydraw(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/Strange/3SariaAnimations/3ChargingSpark").Value), lightColor, true, startpositionx, startpositiony);
            }
                if (Transform == 3)
                {
                projectile.FrameChargeElectricitydraw(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/Strange/4SariaAnimations/4ChargingSpark").Value), lightColor, true, startpositionx, startpositiony);
            }
                if (Transform == 4)
                {
                projectile.FrameChargeElectricitydraw(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/Strange/5SariaAnimations/5ChargingSpark").Value), lightColor, true, startpositionx, startpositiony);
            }
                if (Transform == 5)
                {
                projectile.FrameChargeElectricitydraw(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/Strange/6SariaAnimations/6ChargingSpark").Value), lightColor, true, startpositionx, startpositiony);
            }
                if (Transform == 6)
                {
                projectile.FrameChargeElectricitydraw(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/Strange/7SariaAnimations/7ChargingSpark").Value), lightColor, true, startpositionx, startpositiony);
            }
                projectile.SariaRandomChargeCircle((int)Transform, (bool)IsRight);
        }
        public static void SariaRandomChargeCircle(this Projectile projectile, int transform, bool isright)
        {
            Vector2 ToSpot = projectile.Right;
            if (!isright)
            {
                ToSpot = projectile.Left;
            }
            if (transform == 0)
            {
                if (Main.rand.NextBool(30))
                {
                    for (int i = 0; i < 50; i++)
                    {
                        Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                        Dust d = Dust.NewDustPerfect(ToSpot, ModContent.DustType<AbsorbPsychic>(), speed * -5, Scale: 1.5f);
                        d.noGravity = true;
                    }
                    SoundEngine.PlaySound(SoundID.DD2_SkyDragonsFuryShot, projectile.Center);
                    Lighting.AddLight(projectile.Center, Color.HotPink.ToVector3() * 4f);
                }
            }
            if (transform == 1)
            {
                if (Main.rand.NextBool(30))
                {
                    for (int i = 0; i < 50; i++)
                    {
                        Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                        Dust d = Dust.NewDustPerfect(ToSpot, ModContent.DustType<BubbleDust2>(), speed * -6, Scale: 2.7f);
                        d.noGravity = true;
                    }
                    SoundEngine.PlaySound(SoundID.Drown, projectile.Center);
                    Lighting.AddLight(projectile.Center, Color.White.ToVector3() * 4f);
                }
            }
            if (transform == 2)
            {
                if (Main.rand.NextBool(30))
                {
                    for (int i = 0; i < 25; i++)
                    {
                        Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                        Dust d = Dust.NewDustPerfect(ToSpot, ModContent.DustType<ShadowFlameDustCharge>(), speed * 5, Scale: 4.5f);
                        d.noGravity = true;
                    }
                    for (int i = 0; i < 25; i++)
                    {
                        Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                        Dust d = Dust.NewDustPerfect(ToSpot, ModContent.DustType<SmokeDust6>(), speed * 6, Scale: 2.5f);
                        d.noGravity = true;
                    }
                    SoundEngine.PlaySound(SoundID.Item88, projectile.Center);
                    Lighting.AddLight(projectile.Center, Color.Red.ToVector3() * 4f);
                }
            }
            if (transform == 3)
            {
                if (Main.rand.NextBool(30))
                {
                    for (int i = 0; i < 50; i++)
                    {
                        Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                        Dust d = Dust.NewDustPerfect(ToSpot, ModContent.DustType<StaticDustRing>(), speed * -6, Scale: 2.7f);
                        d.noGravity = true;
                    }
                    SoundEngine.PlaySound(SoundID.NPCHit34, projectile.Center);
                    Lighting.AddLight(projectile.Center, Color.LightYellow.ToVector3() * 4f);
                }
            }
            if (transform == 4)
            {
                if (Main.rand.NextBool(30))
                {
                    for (int i = 0; i < 50; i++)
                    {
                        Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                        Dust d = Dust.NewDustPerfect(ToSpot, ModContent.DustType<RockDustRing>(), speed * -6, Scale: 2.7f);
                        d.noGravity = true;
                    }
                    SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact, projectile.Center);
                    Lighting.AddLight(projectile.Center, Color.Green.ToVector3() * 6f);
                }
            }
            if (transform == 5)
            {
                if (Main.rand.NextBool(30))
                {
                    SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact, projectile.Center);
                    Lighting.AddLight(projectile.Center, Color.Orange.ToVector3() * 6f);
                }
            }
            if (transform == 6)
            {
                if (Main.rand.NextBool(30))
                {
                    SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact, projectile.Center);
                    Lighting.AddLight(projectile.Center, Color.GhostWhite.ToVector3() * 6f);
                }
            }
        }
        public static bool NewIdlePosition(this Projectile projectile, int howclose)
        {
            Player player = Main.player[projectile.owner];
            Vector2 PointtoCheck = player.Center;
            PointtoCheck.Y -= 10;
            PointtoCheck.X += howclose * player.direction;
            bool NoDetectWall = Collision.CanHitLine(player.position, -5, 1, PointtoCheck, 1, 1);
            if (NoDetectWall)
            {
                return true;
            }
            else return false;
        }
        public static void EmeraldspikeGlowandFadedraw(this Projectile projectile, Texture2D texture, Color lightColor, Color WhatColor, float glowspeed, int numframes)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            int frameHeight = texture.Height / numframes;
            Rectangle rectangle = texture.Frame(verticalFrames: numframes, frameY: 0);
            Vector2 origin = rectangle.Size() / 2f;
            float rotation = projectile.rotation;
            float scale = projectile.scale;
            Vector2 startPos = projectile.Center - Main.screenPosition;
            startPos.X += 0;
            startPos.Y += 0;
            if (projectile.type == ModContent.ProjectileType<Emeraldspike>())
            {
                Lighting.AddLight(projectile.Center, Color.Green.ToVector3() * (2f - glowspeed));
            }
            if (projectile.type == ModContent.ProjectileType<Emeraldspike2>())
            {
                Lighting.AddLight(projectile.Center, Color.Purple.ToVector3() * (2f - glowspeed));
            }
            if (projectile.type == ModContent.ProjectileType<Emeraldspike3>())
            {
                Lighting.AddLight(projectile.Center, Color.Silver.ToVector3() * (2f - glowspeed));
            }
            if (projectile.type == ModContent.ProjectileType<Emeraldspike3_2>())
            {
                Lighting.AddLight(projectile.Center, Color.Silver.ToVector3() * (3f - glowspeed));
            }
            Color drawColor = Color.Lerp(lightColor, WhatColor, 300f);
            drawColor = Color.Lerp(drawColor, Color.Transparent, glowspeed);
            if (projectile.spriteDirection == -1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Main.spriteBatch.Draw(texture, startPos, rectangle, (drawColor), rotation, origin, scale, spriteEffects, 0f);
        }
        public static void Emeraldspikedraw(this Projectile projectile, Texture2D texture, Color lightColor, Color WhatColor, bool doesanimate, int startposX, int startposY, int NumFrames)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            int frameHeight = texture.Height / NumFrames;
            Rectangle rectangle = texture.Frame(verticalFrames: NumFrames, frameY: 0);
            if (doesanimate)
            {
                int frameY = frameHeight * (projectile.frame);
                rectangle = texture.Frame(verticalFrames: NumFrames, frameY: (projectile.frame));
            }
            Vector2 origin = rectangle.Size() / 2f;
            float rotation = projectile.rotation;
            float scale = projectile.scale;
            Vector2 startPos = projectile.Center - Main.screenPosition;
            startPos.X += startposX;
            startPos.Y -= startposY;
            Color drawColor = Color.Lerp(lightColor, WhatColor, 300f);
            drawColor = Color.Lerp(drawColor, Color.Transparent, .50f);
            if (projectile.spriteDirection == -1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Main.spriteBatch.Draw(texture, startPos, rectangle, (drawColor), rotation, origin, scale, spriteEffects, 0f);
        }
        public static void Rupeedraw(this Projectile projectile, Texture2D texture, Color lightColor, Color WhatColor, int NumFrames)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            int frameHeight = texture.Height / NumFrames;
            int frameY = frameHeight * (projectile.frame);
            Rectangle rectangle = texture.Frame(verticalFrames: NumFrames, frameY: (projectile.frame));
            Vector2 origin = rectangle.Size() / 2f;
            float rotation = projectile.rotation;
            float scale = projectile.scale;
            Vector2 startPos = projectile.Center - Main.screenPosition;
            startPos.X += -5;
            startPos.Y += 0;
            Color drawColor = Color.Lerp(lightColor, WhatColor, 300f);
            drawColor = Color.Lerp(drawColor, Color.Transparent, .30f);
            Main.spriteBatch.Draw(texture, startPos, rectangle, (drawColor), rotation, origin, scale, spriteEffects, 0f);
        }
        public static void RupeeGlowandFadedraw(this Projectile projectile, Texture2D texture, Color lightColor, Color WhatColor, int numframes)
        {
            if (alpha4Counter)
            {
                alpha4 -= 0.04f;
            }
            if (alpha4 <= 0)
            {
                alpha4Counter = false;
            }
            if (!alpha4Counter)
            {
                alpha4 += 0.004f;
            }
            if (alpha4 >= 1)
            {
                alpha4Counter = true;
            }
            SpriteEffects spriteEffects = SpriteEffects.None;
            int frameHeight = texture.Height / numframes;
            int frameY = frameHeight * (projectile.frame);
            Rectangle rectangle = texture.Frame(verticalFrames: numframes, frameY: (projectile.frame));
            Vector2 origin = rectangle.Size() / 2f;
            float rotation = projectile.rotation;
            float scale = projectile.scale;
            Vector2 startPos = projectile.Center - Main.screenPosition;
            startPos.X += -5;
            startPos.Y += 0;
            if (projectile.type == ModContent.ProjectileType<BouncingShard>())
            {
                Lighting.AddLight(projectile.Center, Color.Green.ToVector3() * (2f - alpha4));
            }
            if (projectile.type == ModContent.ProjectileType<BouncingShard2>())
            {
                Lighting.AddLight(projectile.Center, Color.Purple.ToVector3() * (2f - alpha4));
            }
            if (projectile.type == ModContent.ProjectileType<BouncingShard3>())
            {
                Lighting.AddLight(projectile.Center, Color.Silver.ToVector3() * (2f - alpha4));
            }
            Color drawColor = Color.Lerp(lightColor, WhatColor, 300f);
            drawColor = Color.Lerp(drawColor, Color.Transparent, alpha4);
            Main.spriteBatch.Draw(texture, startPos, rectangle, (drawColor), rotation, origin, scale, spriteEffects, 0f);
        }
        public static bool IsTouchingWaterBarrier(this Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            int WaterVeil = ModContent.ProjectileType<WaterBarrier3>();
            int WaterVeil2 = ModContent.ProjectileType<WaterBarrier>();
            int owner = player.whoAmI;
                for (int l = 0; l < 1000; l++)
                {
                    if (Main.projectile[l].active && l != projectile.whoAmI && ((Main.projectile[l].type == WaterVeil || Main.projectile[l].type == WaterVeil2)))
                    {
                        if (Main.projectile[l].Hitbox.Intersects(projectile.Hitbox))
                        {
                            return true;
                        }
                    }
                }
            return false;
        }
        public static bool IsUnderThunderCloud(this Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            int CloudStrife = ModContent.ProjectileType<LightningCloud>();
                for (int l = 0; l < 1000; l++)
                {
                    if (Main.projectile[l].active && l != projectile.whoAmI && ((Main.projectile[l].type == CloudStrife)))
                    {
                        {
                            Vector2 UpWardPosition = projectile.Center;
                            int sneezespot = 18;
                            if (projectile.spriteDirection > 0)
                            {
                                sneezespot = 18;
                            }
                            if (projectile.spriteDirection < 0)
                            {
                                sneezespot = 3;
                            }
                            UpWardPosition.X += sneezespot;
                            Vector2 CloudPosition = Main.projectile[l].Center;
                            bool NoCover = Collision.CanHitLine(UpWardPosition, projectile.width / 4, projectile.height - 50, CloudPosition, 0, 1);
                            if ((Math.Abs(UpWardPosition.X - CloudPosition.X) <= 100) && (UpWardPosition.Y >= CloudPosition.Y) && (Math.Abs(UpWardPosition.Y - CloudPosition.Y) <= 1000) && NoCover)
                            {
                                return true;
                            }
                        }
                    }
                }
            return false;
        }
        public static void SariaBubbleFaceSpawner(this Projectile projectile, bool sleep, int canmove, bool cursed, int mood)
        {
            Player player = Main.player[projectile.owner];
            int owner = player.whoAmI;
            Vector2 infrontofSaria = projectile.Center;
            infrontofSaria.X += (50 * projectile.spriteDirection);
            float between = Vector2.Distance(player.Center, infrontofSaria);
            bool cansee = false;
            if (between <= 40 && player.direction != projectile.spriteDirection && canmove <= 0f)
            {
                cansee = true;
            }
            for (int U = 0; U < 1000; U++)
            {
                if (Main.projectile[U].active && Main.projectile[U].ModProjectile is Saria modProjectile && U == projectile.whoAmI && ((Main.projectile[U].owner == owner)))
                {
                    if (!sleep && player.statLife == player.statLifeMax2 && (projectile.frame >= 0 && projectile.frame <= 72 && projectile.ai[0] == 0 && (player.ownedProjectileCounts[ModContent.ProjectileType<SmileTime>()] <= 0f) && (!player.HasBuff(ModContent.BuffType<Sickness>()) && (!player.HasBuff(ModContent.BuffType<BloodmoonBuff>()) && (!player.HasBuff(ModContent.BuffType<StatLower>()) && (player.ownedProjectileCounts[ModContent.ProjectileType<Happiness>()] <= 0f) && (player.ownedProjectileCounts[ModContent.ProjectileType<Anger>()] <= 0f) && (player.ownedProjectileCounts[ModContent.ProjectileType<Sad>()] <= 0f) && (player.ownedProjectileCounts[ModContent.ProjectileType<Smile>()] <= 0f) && player.velocity.X == 0)) && modProjectile.ChannelTime < 20 && cansee)) && Main.myPlayer == projectile.owner)
                    {
                        float radius = (float)Math.Sqrt(Main.rand.Next(1 * 1));
                        double angle = Main.rand.NextDouble() * 5.0 * Math.PI;
                        if (modProjectile.Mood <= 4800)
                        {
                            modProjectile.Mood += 600;
                            if (Main.myPlayer == projectile.owner) Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 0, projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<SmileTime>(), (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
                            if (Main.myPlayer == projectile.owner) Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 0, projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<Smile>(), (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
                            Dust.NewDust(new Vector2((projectile.Center.X + 40) + radius * (float)Math.Cos(angle), (projectile.Center.Y - 10) + radius * (float)Math.Sin(angle)), 0, 0, ModContent.DustType<HeartDust>(), 0f, 0f, 0, default(Color), 1.5f);
                        }
                    }
                    if ((player.ownedProjectileCounts[ModContent.ProjectileType<Smile>()] >= 1f) && (player.ownedProjectileCounts[ModContent.ProjectileType<Anger>()] <= 0f) && projectile.spriteDirection == player.direction && Main.myPlayer == projectile.owner)
                    {
                        if (modProjectile.Mood >= 0)
                        {
                            modProjectile.Mood = 0;
                        }
                        if (Main.myPlayer == projectile.owner) Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 0, projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<Anger>(), (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
                    }
                    if ((player.HasBuff(ModContent.BuffType<Sickness>())))
                    {
                        modProjectile.Mood = -4800;
                        if (player.ownedProjectileCounts[ModContent.ProjectileType<Sad>()] <= 0f && (player.ownedProjectileCounts[ModContent.ProjectileType<period>()] <= 0f) && Main.myPlayer == projectile.owner)
                        {
                            {
                                if (Main.myPlayer == projectile.owner) Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 0, projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<Sad>(), (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
                                if (Main.myPlayer == projectile.owner) Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 0, projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<period>(), (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
                            }
                        }
                    }
                    if (player.ownedProjectileCounts[ModContent.ProjectileType<Sad2>()] <= 0f && (player.ownedProjectileCounts[ModContent.ProjectileType<period>()] <= 0f) && player.HasBuff(ModContent.BuffType<Extinguished>()) && Main.myPlayer == projectile.owner)
                    {
                        {
                            if (Main.myPlayer == projectile.owner) Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 0, projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<Sad2>(), (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
                            if (Main.myPlayer == projectile.owner) Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 0, projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<period>(), (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
                        }
                    }
                    if (player.ownedProjectileCounts[ModContent.ProjectileType<Competitivetime>()] == 1f && player.ownedProjectileCounts[ModContent.ProjectileType<Competitive>()] <= 0f && Main.myPlayer == projectile.owner)
                    {
                        {
                            if (Main.myPlayer == projectile.owner) Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 0, projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<Competitive>(), (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
                        }
                    }
                    if ((player.active && Main.eclipse) && ((!player.HasBuff(ModContent.BuffType<Soothing>()))))
                    {
                        player.AddBuff(ModContent.BuffType<EclipseBuff>(), 20);
                        projectile.SneezeDust(ModContent.DustType<Blood>(), 30, 1, -10, 3, -12);
                        projectile.SneezeDust(ModContent.DustType<BlackSmoke>(), 20, 6, -10, 3, -12);
                        if (player.ownedProjectileCounts[ModContent.ProjectileType<period>()] <= 0f && !sleep && Main.myPlayer == projectile.owner)
                        {
                            modProjectile.Mood = -3600;
                            projectile.netUpdate = true;
                            if (Main.myPlayer == projectile.owner) Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 0, projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<Anger>(), (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
                            if (Main.myPlayer == projectile.owner) Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 0, projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<period>(), (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
                        }
                    }
                    if (player.HasBuff(ModContent.BuffType<BloodmoonBuff>()) && (player.ownedProjectileCounts[ModContent.ProjectileType<period>()] <= 0f) && !sleep && Main.myPlayer == projectile.owner)
                    {
                        modProjectile.Mood = -3600;
                        projectile.netUpdate = true;
                        if (Main.myPlayer == projectile.owner) Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 0, projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<Anger>(), (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
                        if (Main.myPlayer == projectile.owner) Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 0, projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<period>(), (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
                    }
                    if (player.HasBuff(ModContent.BuffType<StatLower>()) && (player.ownedProjectileCounts[ModContent.ProjectileType<period>()] <= 0f) && !sleep && !cursed && Main.myPlayer == projectile.owner)
                    {
                        if (modProjectile.Mood > -4800)
                        {
                            modProjectile.Mood -= 600;
                            if (Main.myPlayer == projectile.owner) Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 0, projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<Sad2>(), (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
                            if (Main.myPlayer == projectile.owner) Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 0, projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<period>(), (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
                        }
                    }
                    if (player.HasBuff(ModContent.BuffType<StatRaise>()) && (player.ownedProjectileCounts[ModContent.ProjectileType<period2>()] <= 0f) && !sleep && !cursed && Main.myPlayer == projectile.owner)
                    {
                        if (modProjectile.Mood < 4800)
                        {
                            modProjectile.Mood += 600;
                            if (Main.myPlayer == projectile.owner) Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 0, projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<Smile2>(), (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
                            if (Main.myPlayer == projectile.owner) Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 0, projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<period2>(), (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
                        }
                    }
                    if (!player.HasBuff(ModContent.BuffType<StatRaise>()) && !player.HasBuff(ModContent.BuffType<StatLower>()) && (player.ownedProjectileCounts[ModContent.ProjectileType<period3>()] <= 0f) && !cursed && mood < 0 && Main.myPlayer == projectile.owner)
                    {
                        modProjectile.Mood += 600;
                        projectile.netUpdate = true;
                        if (Main.myPlayer == projectile.owner) Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 0, projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<period3>(), (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
                    }
                    if (!player.HasBuff(ModContent.BuffType<StatRaise>()) && !player.HasBuff(ModContent.BuffType<StatLower>()) && (player.ownedProjectileCounts[ModContent.ProjectileType<period3>()] <= 0f) && !cursed && mood > 0 && Main.myPlayer == projectile.owner)
                    {
                        modProjectile.Mood -= 600;
                        projectile.netUpdate = true;
                        if (Main.myPlayer == projectile.owner) Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + 0, projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<period3>(), (int)(projectile.damage), 0f, projectile.owner, player.whoAmI, projectile.whoAmI);
                    }
                }
            }
        }
        public static void SariaBiomeEffectivness(this Projectile projectile, int biometime, int transform)
        {
            Player player = Main.player[projectile.owner];
            Player player2 = Main.LocalPlayer;
            int owner = player.whoAmI;
            Vector2 UpWardPosition = projectile.Center;
            UpWardPosition.Y -= 550f;
            Vector2 UpWardPositionCover = projectile.Center;
            float minionPositionOffCover = ((20 + projectile.minionPos / 80) * player.direction) - 15;
            UpWardPositionCover.Y -= 50f;
            UpWardPositionCover.X += minionPositionOffCover;
            bool NoCover = Collision.CanHitLine(UpWardPositionCover, projectile.width / 2, projectile.height + 0, UpWardPosition, 0, 1);
            if (biometime <= 0f)
            {
                if (transform == 0)
                {
                    if ((player.active && player.ZoneCrimson || player.ZoneCorrupt) && (Main.myPlayer == projectile.owner))
                    {
                        projectile.SariaStatLower();
                    }
                    if ((player.active && player.ZoneSkyHeight || player.ZoneGlowshroom || player.ZoneJungle && !player.ZoneCrimson && !player.ZoneCorrupt) && (!player.HasBuff(ModContent.BuffType<StatLower>())) && (Main.myPlayer == projectile.owner))
                    {
                        projectile.SariaStatRaise();
                    }
                }
                if (transform == 1)
                {
                    if (player.ZoneSnow && !player.HasBuff(BuffID.Campfire) && !player.HasBuff(BuffID.Warmth))
                    {
                        player.AddBuff(ModContent.BuffType<Frostburn2>(), 2);
                    }
                    if ((player.ZoneRain && !player.InSpace() && NoCover) || (player.wet && !player.honeyWet && !player.lavaWet) || (projectile.IsUnderThunderCloud()) || projectile.IsTouchingWaterBarrier())
                    {
                        player.AddBuff(ModContent.BuffType<PassiveHealing>(), 2);
                    }
                    if ((player.active && player.ZoneDesert || player.ZoneJungle) || player.ZoneGlowshroom || player.ZoneSnow && (Main.myPlayer == projectile.owner))
                    {
                        projectile.SariaStatLower();
                    }
                    if ((player.active && player.ZoneUnderworldHeight || player.ZoneRain || player.ZoneBeach || player.ZoneMeteor || player.ZoneWaterCandle) && (!player.HasBuff(ModContent.BuffType<StatLower>())) && (Main.myPlayer == projectile.owner))
                    {
                        projectile.SariaStatRaise();
                    }
                }
                if (transform == 2)
                {
                    float between = Vector2.Distance(player2.Center, projectile.Center);
                    if (between < 500f)
                    {
                        player2.resistCold = true;
                        player2.AddBuff(BuffID.Warmth, 20);
                    }
                    if ((player.wet && player.honeyWet != true && player.lavaWet != true) || (Collision.WetCollision(projectile.position, projectile.width / 2, projectile.height / 3)) && (Main.myPlayer == projectile.owner))
                    {
                        projectile.SneezeDust(ModContent.DustType<SmokeDust3>(), 20, 6, -10, 3, -12);
                        for (int U = 0; U < 1000; U++)
                        {
                            if (Main.projectile[U].active && Main.projectile[U].ModProjectile is Saria modProjectile && U != projectile.whoAmI && ((Main.projectile[U].owner == owner)))
                            {
                                if (modProjectile.SoundTimer2 <= 0)
                                {
                                    SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/mist"), player.Center);
                                    modProjectile.SoundTimer2 += 200;
                                }
                            }
                        }
                        player.AddBuff(ModContent.BuffType<Extinguished>(), 20);
                    }
                    if (((player.ZoneRain && !player.InSpace() && NoCover)) || projectile.IsUnderThunderCloud() || projectile.IsTouchingWaterBarrier())
                    {
                        projectile.SneezeDust(ModContent.DustType<SmokeDust3>(), 20, 6, -10, 3, -12);
                        for (int U = 0; U < 1000; U++)
                        {
                            if (Main.projectile[U].active && Main.projectile[U].ModProjectile is Saria modProjectile && U != projectile.whoAmI && ((Main.projectile[U].owner == owner)))
                            {
                                if (modProjectile.SoundTimer2 <= 0)
                                {
                                    SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/mist"), player.Center);
                                    modProjectile.SoundTimer2 += 200;
                                }
                            }
                        }
                        player.AddBuff(ModContent.BuffType<Extinguished>(), 20);
                    }
                    if (player.ZoneUnderworldHeight && !player.HasBuff(ModContent.BuffType<Veil>()))
                    {
                        player.AddBuff(ModContent.BuffType<Burning2>(), 20);
                    }
                    if ((player.active && player.ZoneBeach || (player.ZoneRain && !player.ZoneSnow) || player.ZoneSandstorm) && (Main.myPlayer == projectile.owner))
                    {
                        projectile.SariaStatLower();
                    }
                    if ((player.active && player.ZoneSnow || player.ZoneGlowshroom || player.ZoneUnderworldHeight || player.ZoneJungle || player.ZoneDungeon || player.ZoneHallow && (!player.HasBuff(ModContent.BuffType<StatLower>()))) && (Main.myPlayer == projectile.owner))
                    {
                        projectile.SariaStatRaise();
                    }
                }
                if (transform == 3)
                {
                    if ((player.wet && !player.honeyWet && !player.lavaWet))
                    {
                        player2.AddBuff(BuffID.Electrified, 2);
                    }
                    if ((player.active && (player.ZoneUndergroundDesert || player.ZoneUnderworldHeight || player.ZoneRockLayerHeight || player.ZoneDirtLayerHeight)) && (Main.myPlayer == projectile.owner))
                    {
                        projectile.SariaStatLower();
                    }
                    if ((player.active && player.ZoneBeach || player.ZoneRain) && (Main.myPlayer == projectile.owner))
                    {
                        projectile.SariaStatRaise();
                    }
                    projectile.netUpdate = true;
                }
                if (transform == 4)
                {
                    if ((player.active && player.ZoneSkyHeight || player.ZoneRain || player.ZoneBeach) && (Main.myPlayer == projectile.owner))
                    {
                        projectile.SariaStatLower();
                    }
                    if ((player.active && player.ZoneUndergroundDesert || player.ZoneUnderworldHeight || player.ZoneRockLayerHeight) && (Main.myPlayer == projectile.owner))
                    {
                        projectile.SariaStatRaise();
                    }
                    projectile.netUpdate = true;
                }
                if (transform == 5)
                {
                }
                if (transform == 6)
                {
                    if ((player.active && player.ZoneOverworldHeight && Main.dayTime && !player.ZoneCrimson && !player.ZoneCorrupt))
                    {
                        projectile.SariaStatLower();
                    }
                    if ((player.active && player.ZoneCorrupt || player.ZoneCrimson || player.ZoneDungeon || !Main.dayTime))
                    {
                        projectile.SariaStatRaise();
                    }
                }
            }
        }
    }
}