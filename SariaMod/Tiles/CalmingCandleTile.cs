using SariaMod.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using SariaMod.Items.Bands;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using SariaMod;
namespace SariaMod.Tiles
{
	public class CalmingCandleTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			SariaModUtilities.SetUpCandle(this);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Tranquility Candle");
			AddMapEntry(new Color(238, 145, 105), name);
		}
		 public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<Items.Bands.CalmingCandle>();
        }
		public override void NearbyEffects(int i, int j, bool closer)
		{
			Player player = Main.LocalPlayer;
			if (player != null && !player.dead && player.active)
			{
				player.AddBuff(ModContent.BuffType<CalmMindBuff>(), 20);
			}
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (Main.tile[i, j].TileFrameX < 18)
			{
				r = 1f;
				g = 0.55f;
				b = 1f;
			}
			else
			{
				r = 0f;
				g = 0f;
				b = 0f;
			}
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<Items.Bands.CalmingCandle>());
		}
		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{
			if (Main.tile[i, j].TileFrameX < 18)
				SariaModUtilities.DrawFlameSparks(62, 5, i, j);
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			SariaModUtilities.DrawFlameEffect(ModContent.Request<Texture2D>("SariaMod/Tiles/CalmingCandleTileFlame").Value, i, j);
		}
		public override bool RightClick(int i, int j)
		{
			SariaModUtilities.LightHitWire(Type, i, j, 1, 1);
			return true;
		}
	}
}
