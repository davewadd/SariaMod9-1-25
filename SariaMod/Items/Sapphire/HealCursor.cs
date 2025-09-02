using Microsoft.Xna.Framework;
using SariaMod.Buffs;
using SariaMod.Dusts;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using SariaMod.Items.Amber;
using SariaMod.Items.Amethyst;
using SariaMod.Items.Bands;
using SariaMod.Items.Emerald;
using SariaMod.Items.Ruby;
using SariaMod.Items.Sapphire;
using SariaMod.Items.Topaz;
using System;
using System.IO;
using SariaMod.Items.Strange;
namespace SariaMod.Items.Sapphire
{
    public class HealCursor : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("Saria");
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            base.Projectile.width = 250;
            base.Projectile.height = 250;
            base.Projectile.alpha = 300;
            base.Projectile.friendly = true;
            base.Projectile.tileCollide = false;
            base.Projectile.netImportant = true;
            base.Projectile.penetrate = 1;
            base.Projectile.timeLeft = 10;
            base.Projectile.ignoreWater = true;
            base.Projectile.usesLocalNPCImmunity = true;
            base.Projectile.localNPCHitCooldown = 4;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override bool? CanCutTiles()
        {
            return false;
        }
        public override void AI()
        {
            Player player = Main.player[base.Projectile.owner];
            Player player2 = Main.LocalPlayer;
            FairyPlayer modPlayer = player.Fairy();
            int Yesh = ((player2.statManaMax2) / 8);
            int Yesh2 = ((player2.statManaMax2) / 5);
            Projectile mother = Main.projectile[(int)base.Projectile.ai[1]];
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Saria>()] > 0f)
            {
                Projectile.timeLeft = 20;
            }
            Projectile.netUpdate = true;
            Vector2 mouse = Main.MouseWorld;
            mouse.X += 0f;
            mouse.Y -= 5f;
            Vector2 idlePosition = player.Center;
            idlePosition.Y -= 48f; // Go up 48 coordinates (three tiles from the center of the player)
            // If your minion doesn't aimlessly move around when it's idle, you need to "put" it into the line of other summoned minions
            // The index is projectile.minionPos
            float between = Vector2.Distance(mouse, Projectile.Center);
            if (Projectile.timeLeft <= 9900 && Main.myPlayer == Projectile.owner)
            {
                Vector2 direction2 = mouse - Projectile.Center;
                direction2.Normalize();
                Projectile.Center = mouse;
            }
            float between33 = Vector2.Distance(player.Center, Projectile.Center);
            for (int i = 0; i < 100; i++)
            {
                Player player3 = Main.player[i];
                float between35 = Vector2.Distance(Main.player[i].Center, Projectile.Center);
                if (((Main.player[i].statLife < (Main.player[i].statLifeMax2 - (Main.player[i].statLifeMax2 / 16))) && Main.player[i].active && Main.player[i] != player && player.ownedProjectileCounts[ModContent.ProjectileType<HealBarrier>()] <= 0f && (Main.player[i].team == player.team) && modPlayer.StoredHealth >= 25 && !Main.player[i].HasBuff(ModContent.BuffType<Healed>()) && (between35 <= 100)))
                {
                    if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Main.player[i].position.X + 0, Main.player[i].position.Y + 0, 0, 0, ModContent.ProjectileType<HealBarrier>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
                }
            }
            if ((player.statLife < (player.statLifeMax2 - (player.statLifeMax2 / 16))) && player.active && (between33 <= 100) && !player.HasBuff(ModContent.BuffType<Healed>()) && player.ownedProjectileCounts[ModContent.ProjectileType<HealBarrier>()] <= 0f && modPlayer.StoredHealth >= 25)
            {
                if (Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.position.X + 0, player.position.Y + 0, 0, 0, ModContent.ProjectileType<HealBarrier>(), (int)(Projectile.damage), 0f, Projectile.owner, player.whoAmI, Projectile.whoAmI);
            }
        }
    }
}
