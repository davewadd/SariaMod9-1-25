using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace SariaMod.Items.Strange
{
    public class Ztarget : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("Saria");
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
            Main.projFrames[base.Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            base.Projectile.width = 82;
            base.Projectile.height = 82;
            base.Projectile.netImportant = true;
            base.Projectile.alpha = 0;
            base.Projectile.friendly = true;
            base.Projectile.tileCollide = false;
            base.Projectile.penetrate = -1;
            base.Projectile.timeLeft = 500;
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
            Projectile.scale = (float)0.7;
            base.Projectile.rotation += (float)0.07;
            FairyProjectile.HomeInOnNPC(base.Projectile, ignoreTiles: true, 600f, 25f, 20f);
            {
                float distanceFromTarget = 10f;
                Vector2 targetCenter = Projectile.position;
                bool foundTarget = false;
                // This code is required if your minion weapon has the targeting feature
                if (player.HasMinionAttackTargetNPC)
                {
                    NPC npc = Main.npc[player.MinionAttackTargetNPC];
                    float between = Vector2.Distance(npc.Center, Projectile.Center);
                    // Reasonable distance away so it doesn't target across multiple screens
                    if (between < 2000f)
                    {
                        distanceFromTarget = between;
                        targetCenter = npc.Center;
                        targetCenter.Y -= 0f;
                        targetCenter.X += 0f;
                        foundTarget = true;
                    }
                }
                if (player.ownedProjectileCounts[ModContent.ProjectileType<Saria>()] > 0f && Projectile.timeLeft <= 10)
                {
                    Projectile.timeLeft = 20;
                }
                if (player.HasMinionAttackTargetNPC && Projectile.alpha >= 200)
                {
                    SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/ZtargetEnemy"), Projectile.Center);
                    Projectile.alpha = 0;
                    Projectile.scale = (float).7;
                }
                if (!player.HasMinionAttackTargetNPC && Projectile.alpha <= 100)
                {
                    if (Projectile.timeLeft <= 490)
                    {
                        SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/ZtargetCancel"), Projectile.Center);
                    }
                    Projectile.alpha = 300;
                }
                int owner = player.whoAmI;
                int VeilBubble = ModContent.ProjectileType<Ztarget>();
                for (int i = 0; i < 1000; i++)
                {
                    float between = Vector2.Distance(Main.projectile[i].Center, Projectile.Center);
                    {
                        if (Main.projectile[i].active && Main.projectile[i].whoAmI != base.Projectile.whoAmI && ((Main.projectile[i].type == VeilBubble && Main.projectile[i].owner == owner)))
                        {
                                Main.projectile[i].Kill();
                        }
                    }
                }
                Projectile.friendly = foundTarget;
                if (Projectile.alpha == 0)
                {
                    Lighting.AddLight(Projectile.Center, Color.LightGoldenrodYellow.ToVector3() * 1f);
                }
                // Default movement parameters (here for attacking)
                float inertia = 13f;
                Vector2 idlePosition = player.Center;
                float minionPositionOffsetX = ((60 + Projectile.minionPos / 80) * player.direction) - 15;
                idlePosition.Y -= 70f;
                idlePosition.X += minionPositionOffsetX;
                Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
                float distanceToIdlePosition = vectorToIdlePosition.Length();
                if (player.HasMinionAttackTargetNPC)
                {
                    // The immediate range around the target (so it doesn't latch onto it when close)
                    Projectile.Center = targetCenter;
                }
                if (!foundTarget)
                {
                    if ((distanceToIdlePosition >= 2000))
                    {
                        Projectile.position = mother.Center;
                    }
                    {
                        inertia = 10;
                        Vector2 direction2 = idlePosition - Projectile.Center;
                        Projectile.velocity = (Projectile.velocity * (inertia - 8) + direction2) / 20;
                    }
                }
                else if (Projectile.velocity == Vector2.Zero)
                {
                    // If there is a case where it's not moving at all, give it a little "poke"
                    Projectile.velocity.X = -0.15f;
                    Projectile.velocity.Y = -0.05f;
                }
            }
        }
    }
}
