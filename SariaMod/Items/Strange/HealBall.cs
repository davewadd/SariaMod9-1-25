using Microsoft.Xna.Framework;
using SariaMod.Items.zPearls;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using SariaMod;
using SariaMod.Items.zTalking;
namespace SariaMod.Items.Strange
{
    public class HealBall : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("HealBall");
            Tooltip.SetDefault(SariaModUtilities.ColorMessage("Calls on Saria, the Champion of Foresight!", new Color(135, 206, 180)) + "\n" + SariaModUtilities.ColorMessage("Requires 3 minion slots to summon but doensn't occupy the slots", new Color(50, 200, 250)) + "\n" + SariaModUtilities.ColorMessage("Saria will level up as you battle with her!", new Color(0, 200, 250, 200)) + "\n " + SariaModUtilities.ColorMessage("As she levels up, she learns new attacks and gives added buffs depending on what biome she is in", new Color(0, 200, 250, 200)) + "\n" + SariaModUtilities.ColorMessage("Left Click to change available forms, Hold left to start a charged attack!", new Color(0, 20, 250, 200)) + "\n " + SariaModUtilities.ColorMessage("You must hold the Pokeball for Saria to target enemies!", new Color(0, 20, 250, 200)));
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.knockBack = 13f;
            Item.width = 32;
            Item.height = 32;
            base.Item.useTime = (base.Item.useAnimation = 10);
            Item.useStyle = 1;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.Master;
            Item.shootSpeed = 8;
            Item.noUseGraphic = true;
            Item.channel = true;
            // These below are needed for a minion weapon
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            // No buffTime because otherwise the item tooltip would say something like "1 minute duration"
            Item.shoot = ModContent.ProjectileType<Transform>();
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Lighting.AddLight(Item.Center, Color.LightPink.ToVector3() * 2f);
        }
        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<SariaBuff>()) && (player.ownedProjectileCounts[ModContent.ProjectileType<TalkingUI>()] <= 0f))
            {
                return true;
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<HealBallProjectile>()] > 0f || player.ownedProjectileCounts[ModContent.ProjectileType<HealBallProjectile2>()] > 0f || player.ownedProjectileCounts[ModContent.ProjectileType<ReturnBall>()] > 0f)
            {
                return false;
            }
            else if (!(player.HasBuff(ModContent.BuffType<SariaBuff>())))
            {
                return true;
            }
            return false;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
            int owner = player.whoAmI;
            if (player.altFunctionUse != 2 && (player.ownedProjectileCounts[ModContent.ProjectileType<Saria>()] <= 0f))
            {
                if (player.direction == -1)
                {
                    Projectile.NewProjectile(Item.GetSource_FromThis(), position.X + 0, position.Y + 0, velocity.X, velocity.Y, ModContent.ProjectileType<HealBallProjectile>(), damage, 0f, player.whoAmI);
                }
                else if (player.direction == 1)
                {
                    Projectile.NewProjectile(Item.GetSource_FromThis(), position.X + 0, position.Y + 0, velocity.X, velocity.Y, ModContent.ProjectileType<HealBallProjectile>(), damage, 0f, player.whoAmI);
                }
            }
            else if (player.altFunctionUse != 2 && (player.ownedProjectileCounts[ModContent.ProjectileType<Saria>()] > 0f))
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].ModProjectile is Saria modProjectile && ((Main.projectile[i].owner == owner)))
                    {
                        if (modProjectile.ChangeForm <= 0)
                        {
                            Projectile.NewProjectile(Item.GetSource_FromThis(), position.X + 0, position.Y + 0, 0, 0, ModContent.ProjectileType<Transform>(), damage, 0f, player.whoAmI);
                        }
                    }
                }
            }
            return false;
        }
        public override void AddRecipes()
        {
            {
                Recipe recipe = CreateRecipe();
                recipe.AddIngredient(ItemID.Glass, 3);
                recipe.AddRecipeGroup("IronBar", 3);
                recipe.AddIngredient(ItemID.ManaCrystal, 3);
                recipe.AddIngredient(ModContent.ItemType<XpPearl>(), 3);
                recipe.AddTile(ModContent.TileType<Tiles.StrangeBookcase>());
                recipe.Register();
            }
        }
    }
}