using Terraria;
using SariaMod.Items.Strange;
using Terraria.ID;
using Terraria.ModLoader;
namespace SariaMod.Items
{
    public class SmileTime : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("Saria");
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            base.Projectile.width = 30;
            base.Projectile.height = 30;
            base.Projectile.alpha = 300;
            base.Projectile.friendly = true;
            base.Projectile.tileCollide = false;
            base.Projectile.netImportant = true;
            base.Projectile.penetrate = 1;
            base.Projectile.timeLeft = 100;
            base.Projectile.ignoreWater = true;
            base.Projectile.usesLocalNPCImmunity = true;
            base.Projectile.localNPCHitCooldown = 4;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void AI()
        {
            Player player = Main.player[base.Projectile.owner];
            Projectile mother = Main.projectile[(int)base.Projectile.ai[1]];
            Projectile.rotation += 0.095f;
            int owner = player.whoAmI;
            for (int U = 0; U < 1000; U++)
            {
                if (Main.projectile[U].active && Main.projectile[U].ModProjectile is Saria modProjectile && U != Projectile.whoAmI && ((Main.projectile[U].owner == owner)))
                {
                    if (modProjectile.CanMove >= 1)
                    {
                        Projectile.Kill();
                    }
                    if (modProjectile.CanMove <= 0)
                    {
                        Projectile.timeLeft = 100;
                    }
                }
            }
        }
    }
}
