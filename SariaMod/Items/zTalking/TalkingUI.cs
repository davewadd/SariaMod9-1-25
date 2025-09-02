using Terraria.Localization;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using System.Collections.Generic;
using SariaMod.Items.Strange;
namespace SariaMod.Items.zTalking
{
    public class TalkingUI : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("Saria");
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
        }
        public int ConversationPoint;
        public int timer;
        public int timerState;
        public bool IsAnimated;
        public int FrameSpeed = 4;
        public int pausetimer;
        public int soundtimer;
        public int AnimationTimerEyes;
        public int Eyetimer;
        public int EyeFrames;
        public int HowManyLines;
        public int FrameToPause;
        public int NumberOfFrames;
        public int ClickTimer;
        public int Countdown = 600;
        public bool LastFrame;
        public override void SetDefaults()
        {
            base.Projectile.width = 30;
            base.Projectile.height = 30;
            base.Projectile.alpha = 300;
            base.Projectile.friendly = true;
            base.Projectile.tileCollide = false;
            base.Projectile.netImportant = true;
            base.Projectile.penetrate = -1;
            base.Projectile.timeLeft = 100;
            base.Projectile.ignoreWater = true;
            base.Projectile.usesLocalNPCImmunity = true;
            base.Projectile.localNPCHitCooldown = 4;
        }
        public override bool? CanCutTiles()
        {
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override bool MinionContactDamage()
        {
            return false;
        }
        public override void AI()
        {
            Player player = Main.player[base.Projectile.owner];
            Projectile mother = Main.projectile[(int)base.Projectile.ai[1]];
            FairyPlayer modPlayer = player.Fairy();
            int owner = player.whoAmI;
            Projectile.Center = player.Center;
            if (Countdown <= 0)
            {
                Projectile.Kill();
            }
            Main.NewText(ConversationPoint);
            if (timerState < HowManyLines && pausetimer <= 1 && !LastFrame)
            {
                timer++;
            }
            if ((NumberOfFrames) > 0)
            {
                if ((timer / FrameSpeed % NumberOfFrames) != FrameToPause)
                {
                    pausetimer = 0;
                }
            }
            if ((timerState >= HowManyLines) || pausetimer >= 2 || LastFrame)
            {
                IsAnimated = false;
            }
            else
            {
                IsAnimated = true;
            }
            if ((NumberOfFrames) > 0)
            {
                if ((timer / FrameSpeed % NumberOfFrames) >= (NumberOfFrames - 2))
                {
                    timerState += 1;
                    timer = 0;
                }
            }
            if (soundtimer < 7 && IsAnimated)
            {
                soundtimer++;
            }
            if (soundtimer >= 7 && IsAnimated)
            {
                soundtimer = 0;
                if (Main.myPlayer == Projectile.owner) SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/SariaTalking"), player.Center);
            }
            if (pausetimer > 1)
            {
                pausetimer--;
            }
            if (AnimationTimerEyes > 0)
            {
                AnimationTimerEyes--;
            }
            if (AnimationTimerEyes <= 0)
            {
                Eyetimer++;
            }
            if (AnimationTimerEyes > 0)
            {
                Eyetimer = 0;
            }
            if (EyeFrames == 3)
            {
                if (Main.rand.Next(4) == 0)
                {
                    AnimationTimerEyes += 80;
                }
                else if (Main.rand.Next(4) == 1)
                {
                    AnimationTimerEyes += 20;
                }
                else if (Main.rand.Next(4) == 1)
                {
                    AnimationTimerEyes += 150;
                }
                else
                {
                    AnimationTimerEyes += 100;
                }
            }
            EyeFrames = (Eyetimer / 4 % 4);// if you need her to not blink youll just enter the number of the frame after the equals instead
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Saria>()] <= 0f)
            {
                Projectile.Kill();
            }
            for (int i = 0; i < 100; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].ModProjectile is Saria modProjectile && i != base.Projectile.whoAmI && ((Main.projectile[i].owner == owner)))
                {
                    if (ConversationPoint != -1 && ConversationPoint != -2)
                    {
                        if (!modProjectile.SariaTalking)
                        {
                            if (Main.rand.Next(4) == 0)
                            {
                                ConversationPoint = -1;
                            }
                            else
                            {
                                ConversationPoint = -2;
                            }
                            timerState = 0;
                            timer = 0;
                        }
                        if (modProjectile.SariaTalking)
                        {
                            Projectile.timeLeft = 200;
                        }
                        if (modProjectile.ChangeForm >= 1)
                        {
                            modProjectile.ChangeForm = 0;
                        }
                    }
                }
            }
            float speed = 2;
            Vector2 idlePosition = player.Center;
            Vector2 direction = idlePosition - Projectile.Center;
            Projectile.velocity = ((Projectile.velocity * (13 - speed) + direction) / 20);
        }
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            FairyPlayer modPlayer = player.Fairy();
            {
                Projectile.DialogueUIdraw(lightColor, +17, +176);
                bool Rightclick = (player.HeldItem.type == ModContent.ItemType<HealBall>() && Main.mouseLeft);
                if (!Rightclick)
                {
                    ClickTimer = 0;
                }
                Projectile.Conversation(lightColor, (int)ConversationPoint, (int)FrameSpeed, (int)timer, (int)timerState, (int)EyeFrames, (bool)IsAnimated);
            }
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
            overWiresUI.Add(index);
        }
    }
}