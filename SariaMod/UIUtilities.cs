using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SariaMod.Buffs;
using SariaMod.Dusts;
using SariaMod.Items;
using SariaMod.Items.Strange;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SariaMod.Items.zTalking;
namespace SariaMod
{
    public static class UIUtilities
    {
        public static int Tip1;
        public static int Tip2;
        public static int Tip3;
        public static void UIText(this Projectile projectile, Texture2D texture, Color lightColor, int NumFrames, int startPosX = 0, int startPosY = 0)
        {
            Player player = Main.player[projectile.owner];
            if (Main.myPlayer == projectile.owner)
            {
                Vector2 startPos = new Vector2(Main.screenWidth + 0, Main.screenHeight + 0) / 2f + new Vector2(0f, 0f);
                startPos.X += startPosX;
                startPos.Y += startPosY;
                Vector2 startPos2 = startPos;
                int frameHeight = texture.Height / NumFrames;
                int frameY = frameHeight * NumFrames;
                Rectangle rectangle = texture.Frame(verticalFrames: NumFrames, frameY: 0);
                Vector2 origin = rectangle.Size() / NumFrames;
                float rotation = projectile.rotation;
                float scale = projectile.scale;
                SpriteEffects spriteEffects = SpriteEffects.None;
                Color drawColor = lightColor;
                drawColor = Color.Lerp(drawColor, Color.AntiqueWhite, 20f);
                Main.spriteBatch.Draw(texture, startPos, rectangle, (drawColor), rotation, origin, scale, spriteEffects, 0f);
            }
        }
        public static void AnimatedUITextCover(this Projectile projectile, Texture2D texture, Color lightColor, int NumFrames, int WhichFrame, int WhichFrame2, int WhichFrame3, int FrameSpeed, int timer, int State, int howmanyLines, int LastFrame, int frametopause, int linetopause, int frametopause2, int linetopause2, int frametopause3, int linetopause3, int howlongtopause, int startPosX = 0, int startPosY = 0)
        {
            Player player = Main.player[projectile.owner];
            int owner = player.whoAmI;
            if (Main.myPlayer == projectile.owner)
            {
                Vector2 startPos = new Vector2(Main.screenWidth + 0, Main.screenHeight + 0) / 2f + new Vector2(0f, 0f);
                startPos.X += startPosX;
                startPos.Y += startPosY;
                Vector2 startPos2 = startPos;
                startPos2.Y += 17;
                Vector2 startPos3 = startPos2;
                startPos3.Y += 17;
                int frameHeight = texture.Height / NumFrames;
                int frameY = frameHeight * NumFrames;
                for (int U = 0; U < 1000; U++)
                {
                    if (Main.projectile[U].active && Main.projectile[U].ModProjectile is TalkingUI modProjectile && U == projectile.whoAmI && ((Main.projectile[U].owner == owner)))
                    {
                        if (modProjectile.timerState == (howmanyLines-1) && (timer / FrameSpeed % NumFrames) == LastFrame)
                        {
                            modProjectile.LastFrame = true;
                        }
                        else
                        {
                            modProjectile.LastFrame = false;
                        }
                        modProjectile.NumberOfFrames = NumFrames;
                        modProjectile.HowManyLines = howmanyLines;
                        if (((timer / FrameSpeed % NumFrames) == frametopause) && modProjectile.timerState == linetopause && modProjectile.pausetimer <= 0)
                        {
                            modProjectile.pausetimer = howlongtopause;//time to actually pause for
                            modProjectile.FrameToPause = frametopause;
                        }
                        else if (((timer / FrameSpeed % NumFrames) == frametopause2) && modProjectile.timerState == linetopause2 && modProjectile.pausetimer <= 0)
                        {
                            modProjectile.pausetimer = howlongtopause;//time to actually pause for
                            modProjectile.FrameToPause = frametopause2;
                        }
                        else if (((timer / FrameSpeed % NumFrames) == frametopause3) && modProjectile.timerState == linetopause3 && modProjectile.pausetimer <= 0)
                        {
                            modProjectile.pausetimer = howlongtopause;//time to actually pause for
                            modProjectile.FrameToPause = frametopause3;
                        }
                    }
                }
                Rectangle rectangle = texture.Frame(verticalFrames: NumFrames, frameY: 0);
                Rectangle rectangle2 = texture.Frame(verticalFrames: NumFrames, frameY: 0);
                Rectangle rectangle3 = texture.Frame(verticalFrames: NumFrames, frameY: 0);
                if (State == 0)
                {
                    rectangle = texture.Frame(verticalFrames: NumFrames, frameY: timer / FrameSpeed % NumFrames);
                }
                if (State >= 1)
                {
                    rectangle = texture.Frame(verticalFrames: NumFrames, frameY: WhichFrame);
                }
                if (State == 1)
                {
                    rectangle2 = texture.Frame(verticalFrames: NumFrames, frameY: timer / FrameSpeed % NumFrames);
                }
                if (State >= 2)
                {
                    rectangle2 = texture.Frame(verticalFrames: NumFrames, frameY: WhichFrame2);
                }
                if (State == 2)
                {
                    rectangle3 = texture.Frame(verticalFrames: NumFrames, frameY: timer / FrameSpeed % NumFrames);
                }
                if (State >= 3)
                {
                    rectangle3 = texture.Frame(verticalFrames: NumFrames, frameY: WhichFrame3);
                }
                Vector2 origin = rectangle.Size() / NumFrames;
                float rotation = projectile.rotation;
                float scale = projectile.scale;
                SpriteEffects spriteEffects = SpriteEffects.None;
                Color drawColor = lightColor;
                drawColor = Color.Lerp(drawColor, Color.AntiqueWhite, 20f);
                Main.spriteBatch.Draw(texture, startPos, rectangle, (drawColor), rotation, origin, scale, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, startPos2, rectangle2, (drawColor), rotation, origin, scale, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, startPos3, rectangle3, (drawColor), rotation, origin, scale, spriteEffects, 0f);
            }
        }
        public static void AnimatedUIMouths(this Projectile projectile, Texture2D texture, Color lightColor, int NumFrames, int WhichFrame, bool isanimated, int FrameSpeed, int startPosX = 0, int startPosY = 0)
        {
            Player player = Main.player[projectile.owner];
            if (Main.myPlayer == projectile.owner)
            {
                Vector2 startPos = new Vector2(Main.screenWidth + 0, Main.screenHeight + 0) / 2f + new Vector2(0f, 0f);
                startPos.X += startPosX;
                startPos.Y += startPosY;
                Vector2 startPos2 = startPos;
                int frameHeight = texture.Height / NumFrames;
                int frameY = frameHeight * NumFrames;
                Rectangle rectangle = texture.Frame(verticalFrames: NumFrames, frameY: WhichFrame);
                if (isanimated == true)
                {
                    rectangle = texture.Frame(verticalFrames: NumFrames, frameY: (int)Main.GameUpdateCount / FrameSpeed % 5);
                }
                Vector2 origin = rectangle.Size() / NumFrames;
                float rotation = projectile.rotation;
                float scale = projectile.scale;
                SpriteEffects spriteEffects = SpriteEffects.None;
                Color drawColor = lightColor;
                drawColor = Color.Lerp(drawColor, Color.AntiqueWhite, 20f);
                Main.spriteBatch.Draw(texture, startPos, rectangle, (drawColor), rotation, origin, scale, spriteEffects, 0f);
            }
        }
        public static void AnimatedUEyesShocked(this Projectile projectile, Color lightColor, int NumFrames, int WhichFrame)
        {
            Player player = Main.player[projectile.owner];
            int owner = player.whoAmI;
            if (Main.myPlayer == projectile.owner)
            {
                for (int U = 0; U < 1000; U++)
                {
                    if (Main.projectile[U].active && Main.projectile[U].ModProjectile is Saria modProjectile && U != projectile.whoAmI && ((Main.projectile[U].owner == owner)))
                    {
                        Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SariaEyesShocked");
                        if (modProjectile.Transform == 6)
                        {
                            texture = (Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SariaEyesShockedGhost");
                        }
                        Vector2 startPos = new Vector2(Main.screenWidth + 0, Main.screenHeight + 0) / 2f + new Vector2(0f, 0f);
                        startPos.X += -139;
                        startPos.Y += +154;
                        Vector2 startPos2 = startPos;
                        int frameHeight = texture.Height / NumFrames;
                        int frameY = frameHeight * NumFrames;
                        Rectangle rectangle = texture.Frame(verticalFrames: NumFrames, frameY: WhichFrame);
                        Vector2 origin = rectangle.Size() / NumFrames;
                        float rotation = projectile.rotation;
                        float scale = projectile.scale;
                        SpriteEffects spriteEffects = SpriteEffects.None;
                        Color drawColor = lightColor;
                        drawColor = Color.Lerp(drawColor, Color.AntiqueWhite, 20f);
                        Main.spriteBatch.Draw(texture, startPos, rectangle, (drawColor), rotation, origin, scale, spriteEffects, 0f);
                    }
                }
            }
        }
        public static void AnimatedUEyes(this Projectile projectile, Color lightColor, int NumFrames, int WhichFrame)
        {
            Player player = Main.player[projectile.owner];
            int owner = player.whoAmI;
            if (Main.myPlayer == projectile.owner)
            {
                for (int U = 0; U < 1000; U++)
                {
                    if (Main.projectile[U].active && Main.projectile[U].ModProjectile is Saria modProjectile && U != projectile.whoAmI && ((Main.projectile[U].owner == owner)))
                    {
                        Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SariaEyesNormal");
                        if (modProjectile.Transform == 6)
                        {
                            texture = (Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SariaEyesNormalGhost");
                        }
                            Vector2 startPos = new Vector2(Main.screenWidth + 0, Main.screenHeight + 0) / 2f + new Vector2(0f, 0f);
                        startPos.X += -140;
                        startPos.Y += +152;
                        Vector2 startPos2 = startPos;
                        int frameHeight = texture.Height / NumFrames;
                        int frameY = frameHeight * NumFrames;
                        Rectangle rectangle = texture.Frame(verticalFrames: NumFrames, frameY: WhichFrame);
                        Vector2 origin = rectangle.Size() / NumFrames;
                        float rotation = projectile.rotation;
                        float scale = projectile.scale;
                        SpriteEffects spriteEffects = SpriteEffects.None;
                        Color drawColor = lightColor;
                        drawColor = Color.Lerp(drawColor, Color.AntiqueWhite, 20f);
                        Main.spriteBatch.Draw(texture, startPos, rectangle, (drawColor), rotation, origin, scale, spriteEffects, 0f);
                        if (modProjectile.Transform == 4)
                        {
                            projectile.DialogueUEyeMaskdraw(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SariaEyesNormal5Mask").Value), lightColor, (Vector2)startPos, (int)NumFrames, (int)WhichFrame);
                        }
                    }
                }
            }
        }
        public static void ButtonUnlock(this Projectile projectile, int Unlock)
        {
            Player player = Main.player[projectile.owner];
            int owner = player.whoAmI;
            FairyPlayer modPlayer = player.Fairy();
            if (Unlock == 1)
            {
                modPlayer.SariaUnlockPsychic2 = true;
                modPlayer.TMPointsUsed += 1;
            }
            if (Unlock == 2)
            {
                modPlayer.SariaUnlockWater = true;
                modPlayer.TMPointsUsed += 1;
            }
            if (Unlock == 3)
            {
                modPlayer.SariaUnlockWater2 = true;
                modPlayer.TMPointsUsed += 1;
            }
            if (Unlock == 4)
            {
                modPlayer.SariaUnlockFire = true;
                modPlayer.TMPointsUsed += 1;
            }
            if (Unlock == 5)
            {
                modPlayer.SariaUnlockFire2 = true;
                modPlayer.TMPointsUsed += 1;
            }
            if (Unlock == 6)
            {
                modPlayer.SariaUnlockElectric = true;
                modPlayer.TMPointsUsed += 1;
            }
            if (Unlock == 7)
            {
                modPlayer.SariaUnlockElectric2 = true;
                modPlayer.TMPointsUsed += 1;
            }
            if (Unlock == 8)
            {
                modPlayer.SariaUnlockRock = true;
                modPlayer.TMPointsUsed += 1;
            }
            if (Unlock == 9)
            {
                modPlayer.SariaUnlockRock2 = true;
                modPlayer.TMPointsUsed += 1;
            }
            if (Unlock == 10)
            {
                modPlayer.SariaUnlockBug = true;
                modPlayer.TMPointsUsed += 1;
            }
            if (Unlock == 11)
            {
                modPlayer.SariaUnlockBug2 = true;
                modPlayer.TMPointsUsed += 1;
            }
            if (Unlock == 12)
            {
                modPlayer.SariaUnlockGhost = true;
                modPlayer.TMPointsUsed += 1;
            }
            if (Unlock == 13)
            {
                modPlayer.SariaUnlockGhost2 = true;
                modPlayer.TMPointsUsed += 1;
            }
        }
        public static void ClickableUIButton1(this Projectile projectile, Texture2D texture, Texture2D texture2, string Text, Color lightColor, int NumFrames, int WhichFrame, int NextConversation, bool isGray, bool sound2, bool isExit, bool ismoveButton, bool isitUnlock, int Unlock, bool isitUpgrade, int Upgrade, int yup, int startPosX = 0, int startPosY = 0, int ButtonHitPointX = 0, int ButtonHitPointY = 0)
        {
            Player player = Main.player[projectile.owner];
            int owner = player.whoAmI;
            FairyPlayer modPlayer = player.Fairy();
            bool Rightclick = (player.HeldItem.type == ModContent.ItemType<HealBall>() && Main.mouseLeft);
            if (Main.myPlayer == projectile.owner)
            {
                Vector2 mouse = Main.MouseWorld;
                Vector2 ButtonHitPosition = projectile.Center;
                ButtonHitPosition.X += ButtonHitPointX;
                ButtonHitPosition.Y += ButtonHitPointY;
                Vector2 startPos = new Vector2(Main.screenWidth + 0, Main.screenHeight + 0) / 2f + new Vector2(0f, 0f);
                float between = Vector2.Distance(ButtonHitPosition, mouse);
                startPos.X += startPosX;
                startPos.Y += startPosY;
                Vector2 startPos2 = startPos;
                int frameHeight = texture.Height / NumFrames;
                int frameY = frameHeight * NumFrames;
                Rectangle rectangle = texture.Frame(verticalFrames: NumFrames, frameY: 0);
                Rectangle rectangle2 = texture.Frame(verticalFrames: NumFrames, frameY: WhichFrame);
                Vector2 origin = rectangle.Size() / NumFrames;
                if (ismoveButton)
                {
                    origin = rectangle.Size() / 2;
                }
                float rotation = projectile.rotation;
                float scale = projectile.scale;
                SpriteEffects spriteEffects = SpriteEffects.None;
                Color drawColor = lightColor;
                drawColor = Color.Lerp(drawColor, Color.AntiqueWhite, 20f);
                if (between > yup && Tip2 <= 0)
                {
                    Tip1 = 0;
                }
                if (ismoveButton)
                {
                    rectangle = texture.Frame(verticalFrames: NumFrames, frameY: (int)Main.GameUpdateCount / 8 % NumFrames);
                }
                if (!isGray)
                {
                    if (Tip2 > 0)
                    {
                        Tip2--;
                    }
                    if (between <= yup)
                    {
                        Tip2 = 18;// make sure this number is more than the max amount of buttons as every button will put in its value. if you have more buttons than this the value will always come up at zero
                        if (isExit)
                        {
                            player.noThrow = 2;
                            player.cursorItemIconEnabled = true;
                            player.cursorItemIconID = ModContent.ItemType<Items.Bands.Blank>();
                            player.cursorItemIconText = (SariaModUtilities.ColorMessage("Exit", new Color(135, 206, 180)));
                            if (Rightclick)
                            {
                                for (int U = 0; U < 1000; U++)
                                {
                                    if (Main.projectile[U].active && Main.projectile[U].ModProjectile is TalkingUI modProjectile && U == projectile.whoAmI && ((Main.projectile[U].owner == owner)))
                                    {
                                        if (Main.rand.Next(4) == 0)
                                        {
                                            modProjectile.ConversationPoint = -1;
                                        }
                                        else
                                        {
                                            modProjectile.ConversationPoint = -2;
                                        }
                                    }
                                }
                            }
                        }
                        if (!ismoveButton)
                        {
                            rectangle = texture.Frame(verticalFrames: NumFrames, frameY: 1);
                            startPos2.X += 2;
                            startPos2.Y += 2;
                        }
                        if (ismoveButton)
                        {
                            player.noThrow = 2;
                            player.cursorItemIconEnabled = true;
                            player.cursorItemIconID = ModContent.ItemType<Items.Bands.Blank>();
                            player.cursorItemIconText = (SariaModUtilities.ColorMessage(Text, new Color(135, 206, 180)));
                            rectangle2 = texture.Frame(verticalFrames: 1, frameY: 0);
                            Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/FormHighlight"), startPos, rectangle2, (drawColor), rotation, origin, scale, spriteEffects, 0f);
                        }
                        if (!Rightclick)
                        {
                            Tip3 = 0;
                        }
                        if (Rightclick && Tip3 <= 0)
                        {
                            for (int U = 0; U < 1000; U++)
                            {
                                if (Main.projectile[U].active && Main.projectile[U].ModProjectile is TalkingUI modProjectile && U == projectile.whoAmI && ((Main.projectile[U].owner == owner)))
                                {
                                    if (isitUnlock)
                                    {
                                        projectile.ButtonUnlock((int)Unlock);
                                    }
                                    if (isitUpgrade)
                                    {
                                    }
                                    modProjectile.ConversationPoint = NextConversation;
                                    modProjectile.timerState = 0;
                                    modProjectile.timer = 0;
                                    if (ismoveButton)
                                    {
                                        SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/OptionSelect"), player.Center);
                                    }
                                    else
                                    {
                                        SoundEngine.PlaySound(SoundID.MenuOpen);
                                    }
                                    Tip3 = 1;
                                }
                            }
                        }
                        if (Tip1 <= 0)
                        {
                            if (sound2)
                            {
                                SoundEngine.PlaySound(SoundID.MenuTick);
                            }
                            if (!sound2)
                            {
                                SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/MenuCursor"), player.Center);
                            }
                            Tip1 = 1;
                        }
                    }
                }
                if (isGray && !ismoveButton)
                {
                    rectangle = texture.Frame(verticalFrames: NumFrames, frameY: 2);
                }
                else if (isGray && ismoveButton)
                {
                    if (Tip2 > 0)
                    {
                        Tip2--;
                    }
                    if (between <= yup)
                    {
                        Tip2 = 18;
                        player.noThrow = 2;
                        player.cursorItemIconEnabled = true;
                        player.cursorItemIconID = ModContent.ItemType<Items.Bands.Blank>();
                        player.cursorItemIconText = (SariaModUtilities.ColorMessage(Text, new Color(135, 206, 180)));
                        rectangle2 = texture.Frame(verticalFrames: 1, frameY: 0);
                        Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/FormHighlight"), startPos, rectangle2, (drawColor), rotation, origin, scale, spriteEffects, 0f);
                        player.noThrow = 2;
                        player.cursorItemIconEnabled = true;
                        player.cursorItemIconID = ModContent.ItemType<Items.Bands.Blank>();
                        player.cursorItemIconText = (SariaModUtilities.ColorMessage("???", new Color(135, 206, 180)));
                        if (Tip1 <= 0)
                        {
                            if (sound2)
                            {
                                SoundEngine.PlaySound(SoundID.MenuTick);
                            }
                            if (!sound2)
                            {
                                SoundEngine.PlaySound(new SoundStyle("SariaMod/Sounds/MenuCursor"), player.Center);
                            }
                            Tip1 = 1;
                        }
                    }
                    if (NumFrames == 8)
                    {
                        texture = (Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/GreyCharge2");
                    }
                    if (NumFrames == 4)
                    {
                        texture = (Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/GreyCharge");
                    }
                }
                Main.spriteBatch.Draw(texture, startPos, rectangle, (drawColor), rotation, origin, scale, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture2, startPos2, rectangle2, (drawColor), rotation, origin, scale, spriteEffects, 0f);
            }
        }
        public static void DialogueUIdraw(this Projectile projectile, Color lightColor, int startPosX = 0, int startPosY = 0)
        {
            Player player = Main.player[projectile.owner];
            int owner = player.whoAmI;
            for (int U = 0; U < 1000; U++)
            {
                if (Main.projectile[U].active && Main.projectile[U].ModProjectile is Saria modProjectile && U != projectile.whoAmI && (Main.myPlayer == projectile.owner) && ((Main.projectile[U].owner == owner)))
                {
                    Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Greetings2");
                    if (modProjectile.Transform == 1)
                    {
                        texture = (Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Greetings2Blue");
                    }
                    if (modProjectile.Transform == 2)
                    {
                        texture = (Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Greetings2Red");
                    }
                    if (modProjectile.Transform == 3)
                    {
                        texture = (Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Greetings2Gray");
                    }
                    if (modProjectile.Transform == 4)
                    {
                        texture = (Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Greetings2Emerald");
                    }
                    if (modProjectile.Transform == 5)
                    {
                        texture = (Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Greetings2Orange");
                    }
                    if (modProjectile.Transform == 6)
                    {
                        texture = (Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Greetings2Purple");
                    }
                    Vector2 startPos = new Vector2(Main.screenWidth + 0, Main.screenHeight + 0) / 2f + new Vector2(0f, 0f);
                    int frameHeight = texture.Height / Main.projFrames[projectile.type];
                    int frameY = frameHeight * projectile.frame;
                    Rectangle rectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
                    Vector2 origin = rectangle.Size() / 2f;
                    float rotation = projectile.rotation;
                    float scale = projectile.scale;
                    SpriteEffects spriteEffects = SpriteEffects.None;
                    Color drawColor = lightColor;
                    drawColor = Color.Lerp(drawColor, Color.AntiqueWhite, 20f);
                    startPos.X += startPosX;
                    startPos.Y += startPosY;
                    Main.spriteBatch.Draw(texture, startPos, rectangle, (drawColor), rotation, origin, scale, spriteEffects, 0f);
                    if (modProjectile.Transform == 4)
                    {
                        projectile.DialogueUIMaskdraw(lightColor, (int)startPosX, (int)startPosY);
                        projectile.DialogueUIMask2draw(lightColor, (int)startPosX, (int)startPosY);
                        projectile.DialogueUIMask3draw(lightColor, (int)startPosX, (int)startPosY);
                    }
                }
            }
        }
    }
}