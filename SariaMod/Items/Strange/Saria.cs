using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using SariaMod.Buffs;
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
using SariaMod.Items.Strange;
using Terraria.Localization;
using System;
using Terraria.Map;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
namespace SariaMod.Items.Strange
{
    public class Saria : ModProjectile
    {
        public const float DistanceToCheck = 1100f;
        public int Transform; //used for when saria changes forms
        public int Mood; // Saria's mood, can effect textures for her faces
        private int MoveTimer; // used to help calculate if she can move or not
        private int SleepHeal;
        public bool Sleep; // is Saria asleep? effects visuals and movement as well as enemy detection
        private bool XpTimer; // the short buff that shows the xp bar
        private bool Cursed; //is saria currently curse? effects buffs, visuals
        public int ChannelTime; //time the player actually channels
        public int BiomeTime; //short downtime from biome weakness reset
        public int ChannelState; //used for when she is actually using charge animation
        public int IsCharging; //tells when saria is actually in the charging animation state
        public int ChannelAttack;
        public int Eating; // is Saria Eating?
        public int ChangeForm; // is the Projectile UI that is used for changing forms
        private bool Holding; // checks whether the player is holding sarias confect or frozen yogurt
        public int CanMove; // value to help set whether saria can move or not;
        private int SpecialAnimate;//for things like electric Saria's Electric mask animation
        public int SoundTimer;
        public int SoundTimer2;
        public bool SelectSound; // sound that plays when cursor is over saria when selecting move
        private bool IsPlayerAsleep; // checks if the player is asleep to let saria know if she should sleep too
        public int CantAttackTimer;//used to time how long she cant attack for
        public bool SariaTalking;//used to tell if Saria is Talking
        private bool CantAttack;// used for when she should not be able to attack between 0 and 1
        private const int ShortChannelThreshold = 20;
        public int frameToSync;
        public int directionToSync;
        public int syncedFrameCounter;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mother");
            Main.projFrames[Projectile.type] = 99;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = false;
            ProjectileID.Sets.MinionShot[Projectile.type] = false;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
        }
        public override bool? CanCutTiles()
        {
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return target.CanBeChasedBy(Projectile);
        }
        public override bool MinionContactDamage()
        {
            Player player = Main.player[Projectile.owner];
            FairyPlayer modPlayer = player.Fairy();
            NPC target = Projectile.Center.MinionHoming(500f, player);
            if (target != null && !Sleep)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Transform);
            writer.Write(Mood);
            writer.Write(MoveTimer);
            writer.Write(SleepHeal);
            writer.Write(Sleep);
            writer.Write(Cursed);
            writer.Write(ChannelTime);
            writer.Write(BiomeTime);
            writer.Write(ChannelState);
            writer.Write(IsCharging);
            writer.Write(ChannelAttack);
            writer.Write(Eating);
            writer.Write(ChangeForm);
            writer.Write(Holding);
            writer.Write(CanMove);
            writer.Write(SpecialAnimate);
            writer.Write(SoundTimer);
            writer.Write(SoundTimer2);
            writer.Write(SelectSound);
            writer.Write(IsPlayerAsleep);
            writer.Write(CantAttackTimer);
            writer.Write(SariaTalking);
            writer.Write(CantAttack);
            writer.Write(frameToSync);
            writer.Write(directionToSync);
            writer.Write(syncedFrameCounter);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Transform = reader.ReadInt32();
            Mood = reader.ReadInt32();
            MoveTimer = reader.ReadInt32();
            SleepHeal = reader.ReadInt32();
            Sleep = reader.ReadBoolean();
            Cursed = reader.ReadBoolean();
            ChannelTime = reader.ReadInt32();
            BiomeTime = reader.ReadInt32();
            ChannelState = reader.ReadInt32();
            IsCharging = reader.ReadInt32();
            ChannelAttack = reader.ReadInt32();
            Eating = reader.ReadInt32();
            ChangeForm = reader.ReadInt32();
            Holding = reader.ReadBoolean();
            CanMove = reader.ReadInt32();
            SpecialAnimate = reader.ReadInt32();
            SoundTimer = reader.ReadInt32();
            SoundTimer2 = reader.ReadInt32();
            SelectSound = reader.ReadBoolean();
            IsPlayerAsleep = reader.ReadBoolean();
            CantAttackTimer = reader.ReadInt32();
            SariaTalking = reader.ReadBoolean();
            CantAttack = reader.ReadBoolean();
            frameToSync = reader.ReadInt32();
            directionToSync = reader.ReadInt32();
            syncedFrameCounter = reader.ReadInt32();
            Projectile.frame = frameToSync;
            Projectile.frameCounter = syncedFrameCounter;
            Projectile.spriteDirection = directionToSync;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        { // Caching the player and modPlayer can make the code slightly cleaner.
            Player player = Main.player[Projectile.owner];
            FairyPlayer modPlayer = player.Fairy();
            // The logic to set hitDirection is separate from buffs and can be handled first.
            // Use a ternary operator for a more compact way to write this if/else.
            hitDirection = (player.position.X + (float)(player.width / 2) < Projectile.position.X + (float)(Projectile.width / 2)) ? 1 : -1;
            // Apply standard debuff immunities.
            target.buffImmune[BuffID.CursedInferno] = false;
            target.buffImmune[BuffID.Confused] = false;
            target.buffImmune[BuffID.Slow] = false;
            target.buffImmune[BuffID.ShadowFlame] = false;
            target.buffImmune[BuffID.Ichor] = false;
            target.buffImmune[BuffID.OnFire] = false;
            target.buffImmune[BuffID.Frostburn] = false;
            target.buffImmune[BuffID.Poisoned] = false;
            target.buffImmune[BuffID.Venom] = false;
            target.buffImmune[BuffID.Electrified] = false;
            modPlayer.SariaXp += 2;
            // Use a switch statement on the 'Transform' variable for clarity.
            switch (Transform)
            {
                case 0:
                    target.AddBuff(ModContent.BuffType<SariaCurse2>(), 200);
                    break;
                case 1:
                    target.buffImmune[ModContent.BuffType<Frostburn2>()] = false;
                    target.AddBuff(ModContent.BuffType<Frostburn2>(), 200);
                    break;
                case 2:
                    target.buffImmune[ModContent.BuffType<Burning2>()] = false;
                    target.AddBuff(ModContent.BuffType<Burning2>(), 200);
                    break;
                // Cases 3 and 4 are identical, so they can be combined.
                case 3:
                case 4:
                    target.AddBuff(BuffID.Electrified, 300);
                    break;
                case 5:
                    target.AddBuff(BuffID.Venom, 300);
                    target.AddBuff(BuffID.Poisoned, 300);
                    break;
                case 6:
                    target.buffImmune[ModContent.BuffType<SariaCurse>()] = false;
                    target.AddBuff(ModContent.BuffType<SariaCurse>(), 2000);
                    if (!player.HasBuff(ModContent.BuffType<StatLower>()))
                    {
                        if (Main.myPlayer == Projectile.owner)
                        {
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.position.X + 10, target.position.Y + 2, 0, 0, ModContent.ProjectileType<ShadowClaw>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                        }
                    }
                    break;
                // Default case is optional but good practice to handle unexpected values.
                default:
                    break;
            }
        }
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 78;
            Projectile.hide = false;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.netUpdate = true;
            Projectile.ignoreWater = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 50;
            Projectile.minionSlots = 0f;
            Projectile.timeLeft = 10;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.minion = false;
        }
        private const int sphereRadius3 = 1;
        private const int sphereRadius2 = 6;
        private const int sphereRadius4 = 32;
        private const int sphereRadius = 100;
        public override void AI()
        {
            //Main.NewText(ChangeForm);
            {
                Player player = Main.player[Projectile.owner];
                Player player2 = Main.LocalPlayer;
                FairyPlayer modPlayer = player.Fairy();
                FairyProjectile modprojectile = Projectile.Fairy();
                if (Projectile.frame != frameToSync || Projectile.spriteDirection != directionToSync)
                {
                    frameToSync = Projectile.frame;
                    directionToSync = Projectile.spriteDirection;
                    // This tells the game to call SendExtraAI and sync the variables.
                    Projectile.netUpdate = true;
                }
                Rectangle movehitbox = Projectile.Hitbox;
                int owner = player.whoAmI;
                ///recharge effect
                if (CantAttack && CantAttackTimer <= 0)
                {
                    Vector2 dustPosition = (Projectile.spriteDirection == 1) ? Projectile.Right : Projectile.Center;
                    for (int i = 0; i < 50; i++)
                    {
                        Vector2 dustspeed5 = Main.rand.NextVector2CircularEdge(1f, 1f) * -5;
                        Dust d = Dust.NewDustPerfect(dustPosition, ModContent.DustType<AbsorbPsychic>(), dustspeed5, Scale: 1.5f);
                        d.noGravity = true;
                    }
                    SoundEngine.PlaySound(SoundID.MaxMana, Projectile.Center);
                    CantAttack = false;
                }
                ///
                //////////////Transformation Timer
                ///
                Projectile.SariaBaseDamage();
                Projectile.SariaBiomeEffectivness((int)BiomeTime, (int)Transform);
                Projectile.SariaBubbleFaceSpawner((bool)Sleep, (int)CanMove, (bool)Cursed, (int)Mood);
                Projectile.damage /= 2;
                Projectile.knockBack = 10;
                if (player.HasBuff(ModContent.BuffType<XPBuff>()))
                {
                    XpTimer = true;
                    Projectile.netUpdate = true;
                }
                else
                {
                    XpTimer = false;
                    Projectile.netUpdate = true;
                }
                ///Channeling
                bool NotActive = Eating <= 0 && !IsPlayerAsleep && !Sleep;
                bool HoldingHealBall = player.HeldItem.type == ModContent.ItemType<HealBall>();
                bool HoldingHealBallInInventory = player.HasItem(ModContent.ItemType<HealBall>());
                bool CanChanneltoBeginWith = (ChannelTime > 20 && Eating <= 0 && !IsPlayerAsleep && !Sleep && !SariaTalking); /// if you only want her to attack after certain frames after charging edit this to match what frames you want to look for
                bool playerischanneling = (player.channel == true && HoldingHealBall && ChangeForm <= 0 && !SariaTalking && Main.myPlayer == Projectile.owner && !Main.mouseRight);
                bool notActive = Eating <= 0 && !IsPlayerAsleep && !Sleep && !SariaTalking;
                bool holdingHealBall = player.HeldItem.type == ModContent.ItemType<HealBall>();
                bool canChanneltoBeginWith = (ChannelTime > ShortChannelThreshold && notActive);
                bool playerIsChanneling = (player.channel && holdingHealBall && ChangeForm <= 0 && !SariaTalking && Main.myPlayer == Projectile.owner && !Main.mouseRight);
                // 1. Handle Channeling and Time Progression
                if (playerIsChanneling)
                {
                    UpdateChannelTime(player, modPlayer);
                }
                // 2. Spawn the Transform UI
                if (playerIsChanneling && player.ownedProjectileCounts[ModContent.ProjectileType<Transform>()] <= 0f && canChanneltoBeginWith)
                {
                    SpawnTransformUI(player);
                }
                // 3. Handle Channel Release and Actions
                if (player.ownedProjectileCounts[ModContent.ProjectileType<Transform>()] > 0f && !player.channel)
                {
                    HandleChannelRelease(player, NotActive);
                }
                /// ChangeForm stuff
                /// 
                if (ChangeForm >= 1 && player.ownedProjectileCounts[ModContent.ProjectileType<FormChangeOverlay>()] <= 0f)
                {
                    if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 0, Projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<FormChangeOverlay>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                }
                if (player.HeldItem.type != ModContent.ItemType<HealBall>() && CantAttackTimer < 100)
                {
                    CantAttackTimer = 100;
                }
                if (ChannelTime > 20 && NotActive && player.channel == true && HoldingHealBall && CantAttackTimer <= 0 && !player.HasBuff(ModContent.BuffType<HealpulseBuff>()) && !Main.mouseRight)
                {
                    ChannelState++;
                }
                {
                    if (ChannelState > 20 && NotActive)
                    {
                        IsCharging = 1;
                    }
                    else
                    {
                        IsCharging = 0;
                    }
                }
                if (CantAttackTimer > 0)
                {
                    CantAttackTimer--;
                }
                if (BiomeTime > 0)
                {
                    BiomeTime--;
                }
                if (Mood <= -2400)
                {
                    if (!player.HasBuff(ModContent.BuffType<Soothing>()) && !player.HasBuff(ModContent.BuffType<Sickness>()))
                    {
                        if (Main.myPlayer == Projectile.owner) player.AddBuff(ModContent.BuffType<Sickness>(), 30000);
                    }
                }
                if (Mood >= 3600)
                {
                    if (!player.HasBuff(ModContent.BuffType<Overcharged>()))
                    {
                        if (player.HasBuff(ModContent.BuffType<Drained>()))
                        {
                            player.ClearBuff(ModContent.BuffType<Drained>());
                        }
                        if (Main.myPlayer == Projectile.owner) player.AddBuff(ModContent.BuffType<Overcharged>(), 10800);
                        SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/StatRaise"), Projectile.Center);
                        if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 0, Projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<PowerUp>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                    }
                }
                if ((!(HoldingHealBall) && SariaTalking) || (player.ownedProjectileCounts[ModContent.ProjectileType<TalkingUI>()] <= 0f))
                {
                    SariaTalking = false;
                }
                if (SoundTimer2 > 0)
                {
                    SoundTimer2--;
                }
                if (ChangeForm <= 0)
                {
                    SelectSound = false;
                }
                if (player.HasBuff(ModContent.BuffType<BloodmoonBuff>()) || player.HasBuff(ModContent.BuffType<EclipseBuff>()))
                {
                    Cursed = true;
                    Projectile.netUpdate = true;
                }
                else
                {
                    Cursed = false;
                    Projectile.netUpdate = true;
                }
                if (player.HasBuff(ModContent.BuffType<Soothing>()) && player.HasBuff(ModContent.BuffType<Sickness>()))
                {
                    player.ClearBuff(ModContent.BuffType<Sickness>());
                }
                /////////////// End of Transformation Timer
                ///
                int dustspeed = 40;
                if (Projectile.frame >= 76)
                {
                    dustspeed = 5;
                }
                if (Transform == 2)
                {
                    Projectile.SneezeDust(ModContent.DustType<FlameDustSaria>(), 30, 100, -10, 3, -12);
                }
                if (Transform == 6)
                {
                    Projectile.SneezeDust(ModContent.DustType<ShadowFlameDust>(), 30, 100, -10, 3, -12);
                }
                if (Projectile.frame == 62 && (!Sleep) && (!player.HasBuff(ModContent.BuffType<StatLower>()) && !player.HasBuff(ModContent.BuffType<Sickness>()) && !player.HasBuff(ModContent.BuffType<BloodmoonBuff>()) && !player.HasBuff(ModContent.BuffType<EclipseBuff>())))
                {
                    Projectile.SneezeDust(ModContent.DustType<Sneeze>(), 1, 1, -10, 3, -12);
                }
                if (Projectile.frame == 62 && (!Sleep) && (player.HasBuff(ModContent.BuffType<StatLower>()) || player.HasBuff(ModContent.BuffType<Sickness>()) || player.HasBuff(ModContent.BuffType<BloodmoonBuff>()) || player.HasBuff(ModContent.BuffType<EclipseBuff>())))
                {
                    Projectile.SneezeDust(ModContent.DustType<Blood>(), 1, 1, -10, 3, -12);
                    Projectile.SneezeDust(ModContent.DustType<Blood>(), 30, 1, -10, 3, -12);
                }
                if ((player.active && Main.bloodMoon) && ((!player.HasBuff(ModContent.BuffType<Soothing>()))))
                {
                    player.AddBuff(ModContent.BuffType<BloodmoonBuff>(), 20);
                    Projectile.SneezeDust(ModContent.DustType<Blood>(), 30, 1, -10, 3, -12);
                    Projectile.SneezeDust(ModContent.DustType<BlackSmoke>(), 20, 6, -10, 3, -12);
                }
                Projectile.SneezeDust(ModContent.DustType<Psychic2>(), (int)dustspeed, 6, 34, 3, -12);
                if (((player.active && player.ZoneSnow) && !(player.behindBackWall && player.HasBuff((BuffID.Campfire)))) || (player.active && player.ZoneSkyHeight) && !(player.behindBackWall && player.HasBuff((BuffID.Campfire))) || (player.active && player.ZoneDesert && !Main.dayTime) && !(player.behindBackWall && player.HasBuff((BuffID.Campfire))) || (player.active && player.ZoneRain && !player.ZoneJungle && !(player.ZoneDesert && Main.dayTime)) && !(player.behindBackWall && player.HasBuff((BuffID.Campfire))))
                {
                    if (Projectile.velocity.X <= 1)
                    {
                        Projectile.SneezeDust(ModContent.DustType<Fog>(), 50, 1, -10, 10, -17);
                    }
                    else if (Projectile.velocity.X > 1)
                    {
                        Projectile.SneezeDust(ModContent.DustType<Fog>(), 5, 1, -10, 10, -17);
                    }
                }//end of dust stuff
                if (Projectile.localAI[0] == 0f && Main.myPlayer == Projectile.owner)
                {
                    Projectile.Fairy().spawnedPlayerMinionProjectileDamageValue = Projectile.damage;
                    for (int j = 0; j < 1; j++) //set to 2
                    {
                        if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 0, Projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<PowerUp>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                        if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 0, Projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<Ztarget>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                        if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 0, Projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<HealCursor>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                        CantAttackTimer = 120;
                    }
                    Projectile.localAI[0] = 1f;
                }
                ////Ztargets
                if (player.ownedProjectileCounts[ModContent.ProjectileType<HealCursor>()] <= 0f)
                {
                    if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 0, Projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<HealCursor>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                }
                Projectile.Ztargets((int)ChannelState, (int)Transform);
                ///
                if (player.dead)
                {
                    modPlayer.SariaXp /= 2;
                }
                if (player.HasBuff(ModContent.BuffType<SariaBuff>()))
                {
                    Projectile.timeLeft = 2;
                }
                if (!player.HasBuff(ModContent.BuffType<SariaBuff>()) && Projectile.timeLeft == 1)
                {
                    Projectile.Kill();
                }
                if ((!HoldingHealBallInInventory && !HoldingHealBall) && Projectile.timeLeft == 1)
                {
                    Projectile.Kill();
                }
                /// AiStuff
                Vector2 targetCenter = Projectile.position;
                bool foundTarget = false;
                bool CanSee = false;
                if (player.HasMinionAttackTargetNPC && player.HeldItem.type == ModContent.ItemType<HealBall>())
                {
                    NPC npc = Main.npc[player.MinionAttackTargetNPC];
                    bool CanSeeit = Collision.CanHitLine(Projectile.Center, 1, 1, npc.position, npc.width, npc.height);
                    float between = Vector2.Distance(npc.Center, Projectile.Center);
                    // Reasonable distance away so it doesn't target across multiple screens
                    if (between < 2000f)
                    {
                        targetCenter = npc.Center;
                        foundTarget = true;
                        if (CanSeeit)
                        {
                            CanSee = true;
                        }
                    }
                }
                if (Main.myPlayer == Projectile.owner)
                {
                    if (!foundTarget)
                    {
                        Vector2 bestFrozenTarget = Vector2.Zero;
                        float bestFrozenDistance = -1f;
                        bool bestFrozenCanSee = false;
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC npc = Main.npc[i];
                            if (!npc.CanBeChasedBy())
                            {
                                continue;
                            }
                            float between = Vector2.Distance(npc.Center, player.Center);
                            bool closeThroughWall = between < 800f;
                            bool canSeeit = Collision.CanHitLine(Projectile.Center, 1, 1, npc.position, npc.width, npc.height);
                            if (!closeThroughWall)
                            {
                                continue;
                            }
                            if (Transform == 1)
                            {
                                int frozenBuffId = ModContent.BuffType<EnemyFrozen>(); // Replace with your buff's path
                                bool isFrozen = npc.HasBuff(frozenBuffId);
                                if (!isFrozen)
                                {
                                    if (!foundTarget || Vector2.Distance(player.Center, targetCenter) > between)
                                    {
                                        targetCenter = npc.Center;
                                        foundTarget = true;
                                        CanSee = canSeeit;
                                    }
                                }
                                else
                                {
                                    if (bestFrozenDistance == -1f || bestFrozenDistance > between)
                                    {
                                        bestFrozenTarget = npc.Center;
                                        bestFrozenDistance = between;
                                        bestFrozenCanSee = canSeeit;
                                    }
                                }
                            }
                            else // Normal logic for Transform != 1
                            {
                                bool closest = Vector2.Distance(player.Center, targetCenter) > between;
                                if (closest || !foundTarget)
                                {
                                    targetCenter = npc.Center;
                                    foundTarget = true;
                                    CanSee = canSeeit;
                                }
                            }
                        }
                        if (Transform == 1 && !foundTarget && bestFrozenDistance != -1f)
                        {
                            targetCenter = bestFrozenTarget;
                            foundTarget = true;
                            CanSee = bestFrozenCanSee;
                        }
                    }
                }
                Projectile.SariaAI((int)Transform, (int)ChannelTime, (bool)NotActive, (bool)foundTarget, (bool)Sleep, (bool)HoldingHealBall, (int)CantAttackTimer, (int)ChannelState, (int)Eating, (bool)CanSee);
                if ((Main.rand.NextBool(550) || foundTarget) && SpecialAnimate <= 0)
                {
                    SpecialAnimate = 60;
                }
                if (SpecialAnimate > 0)
                {
                    SpecialAnimate--;
                }
                /////end
                //Flashupdate stuff
                for (int i = 0; i < 1000; i++)
                {
                    float between = Vector2.Distance(Main.projectile[i].Center, player.Center);
                    if (between <= 100)
                    {
                        if (Main.projectile[i].active && i != Projectile.whoAmI && ((!Main.projectile[i].friendly && Main.projectile[i].hostile) || (Main.projectile[i].trap)) && Main.myPlayer == Projectile.owner)
                        {
                            if ((!player.HasBuff(ModContent.BuffType<Sickness>()) && (!player.HasBuff(ModContent.BuffType<BloodmoonBuff>()) && (!player.HasBuff(ModContent.BuffType<EclipseBuff>()) && (player.ownedProjectileCounts[ModContent.ProjectileType<Sad>()] <= 0f) && (player.ownedProjectileCounts[ModContent.ProjectileType<Flash>()] <= 0f) && (player.ownedProjectileCounts[ModContent.ProjectileType<FlashCooldown>()] <= 0f)))) && Main.myPlayer == Projectile.owner)
                            {
                                if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 0, Projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<Flash>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                                SoundEngine.PlaySound(SoundID.Item76, Projectile.Center);
                                for (int o = 0; o < 50; o++)
                                {
                                    Vector2 speed2 = Main.rand.NextVector2CircularEdge(1.1f, 1.1f);
                                    Dust d = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<PsychicRingDust>(), speed2 * 15, Scale: 4f);
                                    d.noGravity = true;
                                }
                            }
                        }
                    }
                }
                if (player.ownedProjectileCounts[ModContent.ProjectileType<FlashCooldown>()] > 0f)
                {
                    Projectile.SneezeDust(ModContent.DustType<Psychic>(), 20, 6, -10, 3, -12);
                }
                if (CantAttackTimer > 0)
                {
                    CantAttack = true;
                }
                if (Projectile.frame >= 84 && Projectile.frame <= 95 && Transform == 1)
                {
                    Projectile.AttackDust(ModContent.DustType<BubbleDust>(), 8, 34);
                }
                if (Projectile.frame >= 84 && Projectile.frame <= 95 && Transform == 2)
                {
                    Projectile.AttackDust(ModContent.DustType<FlameDust>(), 8, 34);
                }
                if (Projectile.frame >= 84 && Projectile.frame <= 95 && Transform == 3)
                {
                    Projectile.AttackDust2();
                }
                if (Projectile.frame >= 84 && Projectile.frame <= 95 && Transform == 6)
                {
                    Projectile.AttackDust(ModContent.DustType<ShadowFlameDust>(), 1, 34);
                }
                Vector2 idlePosition = player.Center;
                float speed = 2;
                float Close = 60;
                if (Eating <= 0 && !Sleep)
                {
                    if (player.HeldItem.type == ModContent.ItemType<FrozenYogurt>() || player.HeldItem.type == ModContent.ItemType<SariasConfect>())
                    {
                        Close = 20;
                        if ((player.ownedProjectileCounts[ModContent.ProjectileType<Notice>()] <= 0f) && !Holding)
                        {
                            if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 0, Projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<Notice>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                            Holding = true;
                            MoveTimer = 0;
                            Projectile.netUpdate = true;
                        }
                    }
                    if (Eating <= 0 && (player.ownedProjectileCounts[ModContent.ProjectileType<FrozenYogurtSignal>()] > 0f))
                    {
                        Eating = 1;
                        Projectile.netUpdate = true;
                    }
                    if (Eating <= 0 && (player.ownedProjectileCounts[ModContent.ProjectileType<Competitivetime>()] > 0f))
                    {
                        Eating = 2;
                        Projectile.netUpdate = true;
                    }
                    if (HoldingHealBall)
                    {
                        Close = 60;
                        Holding = false;
                        Projectile.netUpdate = true;
                    }
                    if (player.HeldItem.type != ModContent.ItemType<FrozenYogurt>() && player.HeldItem.type != ModContent.ItemType<SariasConfect>() && player.HeldItem.type != ModContent.ItemType<HealBall>())
                    {
                        if (player.statLife >= (player.statLifeMax2 - player.statLifeMax2 / 12))
                        {
                            Close = 30;
                            Holding = false;
                            Projectile.netUpdate = true;
                        }
                        else
                        {
                            Close = 60;
                            Holding = false;
                            Projectile.netUpdate = true;
                        }
                    }
                    if (player.moveSpeed <= 5)
                    {
                        // Check distances in descending order.
                        int[] checkDistances = { 50, 40, 30, 20, 10 };
                        bool foundPosition = false;
                        foreach (int distance in checkDistances)
                        {
                            if (Projectile.NewIdlePosition(distance))
                            {
                                Close = distance;
                                foundPosition = true;
                                break; // Stop once a valid position is found.
                            }
                        }
                        // If no valid position from the loop, handle the default case.
                        if (!foundPosition)
                        {
                            if (Projectile.NewIdlePosition(0) || player.ownedProjectileCounts[ModContent.ProjectileType<Emeraldspike>()] > 0f || player.ownedProjectileCounts[ModContent.ProjectileType<Emeraldspike2>()] > 0f || player.ownedProjectileCounts[ModContent.ProjectileType<Emeraldspike3>()] > 0f)
                            {
                                Close = 0;
                            }
                        }
                    }
                }
                float minionPositionOffsetX = ((Close + Projectile.minionPos / 80) * player.direction);
                idlePosition.Y -= 15f;
                idlePosition.X += minionPositionOffsetX;
                Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
                float distanceToIdlePosition = vectorToIdlePosition.Length();
                if (Close <= 10 && distanceToIdlePosition <= 10)
                {
                    Projectile.spriteDirection = player.direction;
                }
                if (player.HasBuff(ModContent.BuffType<Veil>()) && Transform == 1)
                {
                    player.AddBuff(ModContent.BuffType<Veil>(), 8800);
                }
                Vector2 direction = idlePosition - Projectile.Center;
                if (foundTarget)
                {
                    {
                        speed = 2;
                        Projectile.velocity = (((Projectile.velocity * (13 - speed) + direction) / 20) * CanMove);
                        Projectile.netUpdate = true;
                    }
                }
                if (Sleep || Eating == 3 || Eating == 4 || (ChannelState > 0 && (IsCharging <= 0 || Projectile.frame <= 20)))// if you want Saria to not move when charging, copy---- || ChannelState > 0  ----- and put it behind Eating == 4
                {
                    CanMove = 0;
                    Projectile.netUpdate = true;
                }
                else if ((MoveTimer >= 275 && ((Projectile.frame >= 0) && (Projectile.frame <= 75)) && distanceToIdlePosition <= 180 && (Math.Abs(Projectile.velocity.X) <= .5) && (player.statLife >= player.statLifeMax2)))
                {
                    CanMove = 0;
                    Projectile.netUpdate = true;
                }
                else
                {
                    CanMove = 1;
                    Projectile.netUpdate = true;
                }
                if (Sleep && (distanceToIdlePosition > 280))
                {
                    MoveTimer = 0;
                    Projectile.netUpdate = true;
                }
                if (ChannelState > 0)
                {
                    MoveTimer = 0;
                    Projectile.netUpdate = true;
                }
                Projectile.velocity = ((Projectile.velocity * (13 - speed) + direction) / 20) * CanMove;
                if (Eating == 1 && distanceToIdlePosition <= 20 && Projectile.frame < 75)
                {
                    SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/Healpulse"), player.Center);
                    player.AddBuff(ModContent.BuffType<Soothing>(), 18000);
                    Mood = 600;
                    if ((player.ownedProjectileCounts[ModContent.ProjectileType<Happiness>()] <= 0f))
                    {
                        if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 0, Projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<Happiness>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                    }
                    Eating = 3;
                    Projectile.frame = 0;
                    Projectile.netUpdate = true;
                }
                if (Eating == 2 && distanceToIdlePosition <= 20 && Projectile.frame < 75)
                {
                    player.AddBuff(ModContent.BuffType<Overcharged>(), 45000);
                    SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/StatRaise"), Projectile.Center);
                    if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 0, Projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<PowerUp>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                    Mood = 3600;
                    Eating = 4;
                    Projectile.frame = 0;
                    Projectile.netUpdate = true;
                }
                if (Eating == 3 || Eating == 4)
                {
                    Projectile.spriteDirection = 1;
                    Projectile.netUpdate = true;
                }
                if (player.statLife < (player.statLifeMax2) / 4 && !player.HasBuff(ModContent.BuffType<HealpulseBuff>()) && !player.HasBuff(ModContent.BuffType<Sickness>()) && !player.HasBuff(ModContent.BuffType<BloodmoonBuff>()) && !player.HasBuff(ModContent.BuffType<EclipseBuff>()))
                {
                    Mood -= 600;
                    player.statLife += 500;
                    player.AddBuff(ModContent.BuffType<HealpulseBuff>(), 3000);
                    Projectile.netUpdate = true;
                    for (int j = 0; j < 1; j++) //set to 2
                    {
                        if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 0, Projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<Heal>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                        if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 0, Projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<Sad>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                    }
                }
                float positiveThreshold = Projectile.NewIdlePosition(51) ? 0.25f : 1.25f;
                float negativeThreshold = Projectile.NewIdlePosition(51) ? -0.25f : -1.25f;
                if (Projectile.velocity.X >= positiveThreshold)
                {
                    Projectile.spriteDirection = 1;
                }
                else if (Projectile.velocity.X <= negativeThreshold)
                {
                    Projectile.spriteDirection = -1;
                }
                if (Projectile.frame == 65 && Sleep && MoveTimer >= 550)
                {
                    Projectile.SneezeDust(ModContent.DustType<Z>(), 40, 1, -10, 3, -12);
                }
                ///Sleep Ai
                if ((Math.Abs(Projectile.velocity.X) >= 0.5f) || (Math.Abs(Projectile.velocity.Y) >= 0.5f))
                {
                    MoveTimer = 0;
                    Projectile.netUpdate = true;
                }
                if ((Math.Abs(Projectile.velocity.X) < 0.5f) && (Math.Abs(Projectile.velocity.Y) < 0.5f))
                {
                    if (MoveTimer < 10000)
                    {
                        MoveTimer += 1;
                        Projectile.netUpdate = true;
                    }
                }
                if (SariaTalking && !Sleep)
                {
                    if (MoveTimer >= 277)
                    {
                        MoveTimer = 276;
                        Projectile.netUpdate = true;
                    }
                }
                if (MoveTimer == 0)
                {
                    Sleep = false;
                    SleepHeal = 0;
                    Projectile.netUpdate = true;
                }
                if (Sleep && MoveTimer >= 8000 && SleepHeal <= 0 && (Main.myPlayer == Projectile.owner))
                {
                    SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/Healpulse"), player.Center);
                    player.AddBuff(ModContent.BuffType<Soothing>(), 44000);
                    Mood = 0;
                    SleepHeal = 1;
                    if (player.HasBuff(ModContent.BuffType<Drained>()))
                    {
                        player.ClearBuff(ModContent.BuffType<Drained>());
                    }
                    Projectile.netUpdate = true;
                }
                if (Sleep && MoveTimer >= 10000 && (Main.myPlayer == Projectile.owner))
                {
                    if (player.HasBuff(ModContent.BuffType<Drained>()))
                    {
                        player.ClearBuff(ModContent.BuffType<Drained>());
                    }
                    if (MoveTimer >= 10000)
                    {
                        player.AddBuff(ModContent.BuffType<Overcharged>(), 30000);
                        if (SoundTimer <= 0)
                        {
                            SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/StatRaise"), Projectile.Center);
                            for (int j = 0; j < 1; j++) //set to 2
                            {
                                if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 0, Projectile.position.Y + 0, 0, 0, ModContent.ProjectileType<PowerUp>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                            }
                            SoundTimer = 1;
                        }
                        Mood = 600;
                        MoveTimer = 0;
                        SoundTimer = 0;
                        Projectile.netUpdate = true;
                    }
                }
                if (player.sleeping.isSleeping && Eating <= 0)
                {
                    if (MoveTimer <= 6000)
                    {
                        MoveTimer = 6000;
                    }
                    if (IsPlayerAsleep && !Sleep)
                    {
                        if (Projectile.frame < 54)
                        {
                            Projectile.frame = 54;
                        }
                        Projectile.netUpdate = true;
                    }
                    else if (!IsPlayerAsleep && !Sleep && ChannelState <= 0)
                    {
                        if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 0, Projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<Notice>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                        IsPlayerAsleep = true;
                    }
                }
                if (!player.sleeping.isSleeping)
                {
                    IsPlayerAsleep = false;
                }
                if (MoveTimer >= (5000) && Projectile.frame == 59)
                {
                    {
                        Sleep = true;
                        Projectile.netUpdate = true;
                    }
                }
                ///Main.NewText(Sleep);
                ///end of sleep ai
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Projectile.frame != frameToSync || Projectile.spriteDirection != directionToSync || Projectile.frameCounter != syncedFrameCounter)
                    {
                        frameToSync = Projectile.frame;
                        syncedFrameCounter = Projectile.frameCounter;
                        directionToSync = Projectile.spriteDirection;
                        Projectile.netUpdate = true;
                    }
                    int frameSpeed = 30; //reduced by half due to framecounter speedup
                    Projectile.frameCounter += 2;
                    if (Projectile.frameCounter >= frameSpeed)
                    {
                        Projectile.frameCounter = 0;
                        if (Projectile.frame >= Main.projFrames[ModContent.ProjectileType<Saria>()]) //error here! you had the wrong projectile id, so the animation did not use the right frames
                        {
                            Projectile.frame = 0;
                        }
                        if (Projectile.frame == 25 && (Eating == 3 || Eating == 4))
                        {
                            Projectile.SneezeDust(ModContent.DustType<Fog>(), 1, 1, -10, 10, -17);
                        }
                        if (Projectile.frame == 34 && Eating <= 0)
                        {
                            SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/Step2"), Projectile.Center);
                        }
                        if (Projectile.frame == 37 && (Eating == 3 || Eating == 4))
                        {
                            Projectile.frame = 62;
                            Vector2 Throw = Projectile.Center;
                            Throw.Y += 0f;
                            Throw.X += 40f;
                            Vector2 ThrowToo = Projectile.DirectionTo(Main.MouseWorld).SafeNormalize(Vector2.Zero);
                            if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Throw, ThrowToo * 10, ModContent.ProjectileType<EmptyCup>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                        }
                        if (Projectile.frame == 74 && (Eating == 3 || Eating == 4))
                        {
                            Eating = 0;
                            Projectile.netUpdate = true;
                        }
                        if (Projectile.ai[0] == 0 || Projectile.ai[0] == 3 || Projectile.ai[0] == 4) //only run these animations if not attacking! no longer overrides
                        {
                            if ((Projectile.velocity.Y) > -1f && (Projectile.velocity.Y) < 1f && Math.Abs(Projectile.velocity.X) <= .25) //Idle animation, notice how I have (
                                                                                                                                         //
                                                                                                                                         //.Y greater than -3f and less than 4f. this DID conflict with the rising and Falling animations but this is how i fixed it.
                            { ////however you set up the attack animation, make sure that none of these other animations override it. 
                              //that's easy legit just
                                Projectile.frame++;
                                if (Projectile.frameCounter <= 76)
                                {
                                    Projectile.frameCounter = 0;
                                }
                                if (Sleep && MoveTimer > 250)
                                {
                                    if (Projectile.frame == 60)
                                    {
                                        Projectile.frame = 62;
                                        Projectile.netUpdate = true;
                                        SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/Hover"), Projectile.Center);
                                    }
                                    if (Projectile.frame >= 66 && MoveTimer >= 550)
                                    {
                                        Projectile.frame = 62;
                                        Projectile.netUpdate = true;
                                        SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/Hover"), Projectile.Center);
                                    }
                                }
                                ///Charging animation
                                if (NotActive && ChannelState > 0)
                                {
                                    if (Projectile.frame >= 76 || Projectile.frame < 16)
                                    {
                                        if (IsCharging <= 0)
                                        {
                                            Projectile.frame = 16;
                                        }
                                        else
                                        {
                                            Projectile.frame = 20;
                                        }
                                    }
                                    if (Projectile.frame >= 16 && Projectile.frame < 76 && IsCharging <= 0)
                                    {
                                        Projectile.frame = 16;
                                    }
                                    if (Projectile.frame >= 24 && Projectile.frame < 76)
                                    {
                                        Projectile.frame = 20;
                                    }
                                    if (Projectile.frame > 41 && Projectile.frame < 52)
                                    {
                                        if (Transform == 1)
                                        {
                                            Projectile.SneezeDust(ModContent.DustType<BubbleDust>(), 20, 1, -4, 24, -1);
                                        }
                                        if (Transform == 2)
                                        {
                                            Projectile.SneezeDust(ModContent.DustType<FlameDust>(), 20, 1, -4, 24, -1);
                                        }
                                        if (Transform == 4)
                                        {
                                            Projectile.SneezeDust(ModContent.DustType<RockDust>(), 2, 1, -4, 24, -1);
                                            Projectile.SneezeDust(ModContent.DustType<RockDust2>(), 2, 1, -4, 24, -1);
                                        }
                                        if (Transform == 6)
                                        {
                                            Projectile.SneezeDust(ModContent.DustType<ShadowFlameDust>(), 20, 1, -4, 24, -1);
                                        }
                                    }
                                }
                                ////end of charging animation
                                if (Projectile.frame == 66 && (player.ownedProjectileCounts[ModContent.ProjectileType<Notice>()] <= 0f) && Sleep)
                                {
                                    if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 0, Projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<Notice>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                                }
                                if (Projectile.frame == 63 && !Sleep)
                                {
                                    SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/Step2"), Projectile.Center);
                                }
                                if (Projectile.frame >= 76)
                                {
                                    Projectile.frame = 0;
                                    if (Sleep)
                                    {
                                        Sleep = false;
                                    }
                                    SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/Step1"), Projectile.Center);
                                }
                                if (Projectile.frame == 58 && player.statLife < ((player.statLifeMax2) - (player.statLifeMax2 / 4)) && !player.HasBuff(ModContent.BuffType<Healpulse2Buff>()))
                                {
                                    Mood -= 600;
                                    player.statLife += 500;
                                    if (!player.HasBuff(ModContent.BuffType<Healpulse2Buff>()))
                                    {
                                        for (int j = 0; j < 1; j++) //set to 2
                                        {
                                            if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 0, Projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<Heal>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                                            if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 0, Projectile.position.Y + -24, 0, 0, ModContent.ProjectileType<Sad>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                                        }
                                    }
                                    player.AddBuff(ModContent.BuffType<Healpulse2Buff>(), 3000);
                                    Projectile.netUpdate = true;
                                }
                            }
                            if ((Projectile.velocity.Y) < 4f && Math.Abs(Projectile.velocity.X) > 0.25f && Math.Abs(Projectile.velocity.X) < 4f) //walking animation and such
                            {
                                Projectile.frame++;
                                Projectile.frameCounter += 3;
                                if (Projectile.frame <= 80)
                                {
                                    Projectile.frameCounter = 0;
                                }
                                if (Projectile.frame >= 80)
                                {
                                    Projectile.frame = 76;
                                }
                                if (Projectile.frame < 76)
                                {
                                    Projectile.frame = 76;
                                }
                            }
                            if ((Projectile.velocity.Y) < 4f && Math.Abs(Projectile.velocity.X) >= 4f)//running or (floating) animation
                            {
                                Projectile.frame++;
                                if (Projectile.frameCounter < 83)
                                {
                                    Projectile.frameCounter = 0;
                                }
                                if (Projectile.frame >= 83)
                                {
                                    Projectile.frame = 80;
                                    SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/Hover"), Projectile.Center);
                                }
                                if (Projectile.frame < 80)
                                {
                                    Projectile.frame = 80;
                                }
                            }
                            if ((Projectile.velocity.Y) < -1f) //rising animation
                            {
                                Projectile.frame++;
                                if (Projectile.frameCounter < 83)
                                {
                                    Projectile.frameCounter = 0;
                                }
                                if (Projectile.frame >= 83)
                                {
                                    Projectile.frame = 80;
                                }
                                if (Projectile.frame < 80)
                                {
                                    Projectile.frame = 80;
                                    SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/Fly"), Projectile.Center);
                                }
                            }
                            if (Projectile.velocity.Y > 4f && Math.Abs(Projectile.velocity.X) > 0.25f) //falling animation
                            {
                                Projectile.frame++;
                                {
                                    if (Projectile.frameCounter < 99)
                                    {
                                        Projectile.frameCounter = 0;
                                    }
                                    if (Projectile.frame >= 99)
                                    {
                                        Projectile.frame = 97;
                                    }
                                    if (Projectile.frame < 97)
                                    {
                                        Projectile.frame = 97;
                                    }
                                }
                            }
                            if (Projectile.velocity.Y > 1f && Math.Abs(Projectile.velocity.X) < 0.25f) //falling animation
                            {
                                Projectile.frame++;
                                {
                                    if (Projectile.frameCounter < 99)
                                    {
                                        Projectile.frameCounter = 0;
                                    }
                                    if (Projectile.frame >= 99)
                                    {
                                        Projectile.frame = 97;
                                    }
                                    if (Projectile.frame < 97)
                                    {
                                        Projectile.frame = 97;
                                    }
                                }
                            }
                        }
                        Projectile.SariaAttacks((int)Transform, (int)CantAttackTimer, (int)ChannelAttack, (bool)foundTarget, (Vector2)targetCenter);
                    }
                }
            }
        }
        private void UpdateChannelTime(Player player, FairyPlayer modPlayer)
        {
            // Note: The logic `ChannelTime < 18` is less than the `ShortChannelThreshold` of 20,
            // so this condition will only be true for a small initial window. This behavior
            // is preserved from your original code.
            if (!modPlayer.PlayercanCharge && ChannelTime < 18)
            {
                ChannelTime++;
                Projectile.netUpdate = true;
            }
            else if (modPlayer.PlayercanCharge)
            {
                ChannelTime++;
                Projectile.netUpdate = true;
            }
        }
        private void SpawnTransformUI(Player player)
        {
            // Ensure only the owner of the projectile can spawn the UI
            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.position.X,
                    Projectile.position.Y - 24,
                    0, 0,
                    ModContent.ProjectileType<Transform>(),
                    (int)Projectile.damage,
                    0f,
                    Projectile.owner,
                    player.whoAmI,
                    Projectile.whoAmI
                );
            }
        }
        private void HandleChannelRelease(Player player, bool NotActive)
        {
            int veilBubbleType = ModContent.ProjectileType<Transform>();
            // This loop iterates through all projectiles to find the Transform UI.
            // This is kept identical to your original code to preserve functionality.
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                if (Main.projectile[i].active && i != Projectile.whoAmI && ChannelTime > 0 && (Main.projectile[i].type == veilBubbleType && Main.projectile[i].owner == Projectile.owner))
                {
                    if (ChannelTime <= ShortChannelThreshold)
                    {
                        // Handle early channel release (form change)
                        if (CantAttackTimer > 0 && Projectile.frame >= 84 && Projectile.frame < 96)
                        {
                            Projectile.frame = 96;
                        }
                        else if (ChangeForm <= 0)
                        {
                            SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/OptionsUp"), player.Center);
                            ChangeForm = 1;
                        }
                    }
                    else if (ChannelTime > ShortChannelThreshold && NotActive)
                    {
                        // Handle charged attack and cooldown
                        ChannelState = 0;
                        // Condition derived from original `if (CanChanneltoBeginWith)` check
                        if (ChannelTime > ShortChannelThreshold && NotActive && !SariaTalking)
                        {
                            if (Transform == 3 || Transform == 2 || Transform == 0)
                            {
                                CantAttackTimer = 200;
                            }
                            ChannelAttack = 1;
                            Projectile.ai[0] = 1;
                        }
                    }
                    else
                    {
                        // Handle other release scenarios (e.g., in a non-active state)
                        ChannelState = 0;
                    }
                    // Actions common to all release scenarios
                    Main.projectile[i].Kill();
                    Projectile.netUpdate = true;
                    ChannelTime = 0;
                }
            }
        }
        public override void PostDraw(Color lightColor)
        {
            {
                Player player = Main.player[Projectile.owner];
                FairyPlayer modPlayer = player.Fairy();
                float sneezespot = 5;
                {
                    Vector2 drawPosition;
                    Vector2 mouse = Main.MouseWorld;
                    mouse.X += 10f;
                    mouse.Y -= 5f;
                    float radius = (float)Math.Sqrt(Main.rand.Next(sphereRadius3 * sphereRadius3));
                    double angle = Main.rand.NextDouble() * 5.0 * Math.PI;
                    Vector2 startPos2 = Projectile.Center;
                    float radius2 = ((Projectile.Center.Y - 10) + radius * (float)Math.Sin(angle));
                    startPos2.Y = radius2;
                    if (Projectile.spriteDirection > 0)
                    {
                        sneezespot = 18;
                    }
                    if (Projectile.spriteDirection < 0)
                    {
                        sneezespot = 3;
                    }
                    startPos2.X += sneezespot;
                    float between = Vector2.Distance(mouse, startPos2);
                    bool Rightclick = (player.HeldItem.type == ModContent.ItemType<HealBall>() && Main.mouseLeft && (Main.myPlayer == Projectile.owner));
                    if (between > 30)
                    {
                        SelectSound = false;
                    }
                    if (ChangeForm > 0 && (Main.myPlayer == Projectile.owner))
                    {
                        if (between <= 30 && (Main.myPlayer == Projectile.owner))
                        {
                            player.noThrow = 2;
                            player.cursorItemIconEnabled = true;
                            player.cursorItemIconID = ModContent.ItemType<Items.Bands.Blank>();
                            player.cursorItemIconText = (SariaModUtilities.ColorMessage("Saria", new Color(135, 206, 180)));
                            {
                                Projectile.SariaBubbleFaces(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/SariaTalk").Value), false, 60, 3, -50, lightColor);
                                if (!SelectSound)
                                {
                                    SoundEngine.PlaySound(SoundID.MenuTick);
                                    SelectSound = true;
                                }
                            }
                        }
                        if (between <= 30 && Rightclick && Eating <= 0)
                        {
                            if (player.ownedProjectileCounts[ModContent.ProjectileType<TalkingUI>()] <= 0f) ;
                            {
                                SariaTalking = true;
                                SoundEngine.PlaySound(SoundID.MenuOpen);
                                if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 0, Projectile.position.Y + 0, 0, 0, ModContent.ProjectileType<TalkingUI>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                            }
                        }
                    }
                    Projectile.SariaBubbleFaceLoader((int)ChangeForm, (int)Eating, lightColor);
                    Projectile.SariaFeetandArmDraw((int)Transform, (int)Eating, lightColor);
                    Projectile.SariaBodyDraw((int)Transform, (int)Eating, (int)IsCharging, (int)ChannelState, (int)SpecialAnimate, lightColor);
                    Projectile.SariaSmallFacesOrWhencursed((int)Transform, (bool)Sleep, (int)Eating, (int)IsCharging, (bool)Cursed, (int)ChannelState, (int)Mood, lightColor);
                    Projectile.SariaChargingAnimation((int)Transform, (bool)Sleep, (int)Eating, (int)IsCharging, (bool)Cursed, (int)ChannelState, (int)Mood, lightColor);
                    Projectile.SariaEatDraw((int)Transform, (int)Eating, lightColor);
                    Projectile.SariaSleepDraw((int)Transform, (bool)Sleep, lightColor);
                    if (XpTimer && Main.myPlayer == Projectile.owner)
                    {
                        Projectile.SariaDrawInterface(lightColor, SariaExtensions1.InterfaceType.XPBar);
                        Projectile.SariaDrawInterface(lightColor, SariaExtensions1.InterfaceType.NextBoss);
                    }
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            FairyPlayer modPlayer = player.Fairy();
            Projectile.BlueRingofdust(72);
            if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 15, Projectile.position.Y + 30, 0, 0, ModContent.ProjectileType<HealBallProjectile2>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
        }
    }
}