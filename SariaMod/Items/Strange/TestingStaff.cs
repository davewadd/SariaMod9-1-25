using Microsoft.Xna.Framework;
using SariaMod.Buffs;
using SariaMod.Items.Topaz;
using SariaMod.Items.Ruby;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SariaMod.Items.Sapphire;
namespace SariaMod.Items.Strange
{
    public class TestingStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("XPStaff");
            Tooltip.SetDefault(SariaModUtilities.ColorMessage("Shows the Level of XP Saria has when used", new Color(0, 200, 250, 200)));
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.knockBack = 13f;
            Item.mana = 1;
            Item.width = 32;
            Item.height = 32;
            base.Item.useTime = (base.Item.useAnimation = 10);
            Item.useStyle = 1;
            Item.value = Item.sellPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.autoReuse = true;
            // These below are needed for a minion weapon
            Item.noMelee = true;
            Item.damage = 80;
            Item.DamageType = DamageClass.Summon;
            // No buffTime because otherwise the item tooltip would say something like "1 minute duration
            Item.shootSpeed = 4;
            Item.shoot = ModContent.ProjectileType<WaterBarrier>();
            Item.buffTime = 20;
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Lighting.AddLight(Item.Center, Color.SeaShell.ToVector3() * 2f);
        }
    }
}