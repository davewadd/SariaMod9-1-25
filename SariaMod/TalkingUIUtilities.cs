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
    public static class TalkingUIUtilities
    {
        public static void Conversation(this Projectile projectile, Color lightColor, int ConversationPoint, int FrameSpeed, int timer, int timerState, int EyeFrames, bool IsAnimated)
        {
            Player player = Main.player[projectile.owner];
            FairyPlayer modPlayer = player.Fairy();
            int owner = player.whoAmI;
            if (ConversationPoint == -2)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Exit2").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 1, 8, 8, 0, 34, 0, 34, 0, 500, -82, +138);
                projectile.AnimatedUIMouths(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SariaMouthNormal").Value), lightColor, 6, 0, (bool)IsAnimated, 7, -129, +174);
                projectile.AnimatedUEyes(lightColor, 5, (int)EyeFrames);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 300, true, false, false, false, false, 0, false, 0, 35, +150, +217, 166, 227); //third button
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 2, 2, true, false, false, false, false, 0, false, 0, 35, +48, +217, 64, 227); //middle button
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 1, 3, true, false, false, false, false, 0, false, 0, 35, -54, +217, -38, 227); //firstbutton
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BackChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 0, true, true, false, false, false, 0, false, 0, 24, -128, +213, -118, 223);// backbutton
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/ExitChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 0, true, true, true, false, false, 0, false, 0, 15, -170, +213, -164, 218);// exitbutton
            }
            if (ConversationPoint == -1)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Exit1").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 1, 8, 8, 0, 34, 0, 34, 0, 500, -82, +138);
                projectile.AnimatedUIMouths(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SariaMouthNormal").Value), lightColor, 6, 0, (bool)IsAnimated, 7, -129, +174);
                projectile.AnimatedUEyes(lightColor, 5, (int)EyeFrames);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 300, true, false, false, false, false, 0, false, 0, 35, +150, +217, 166, 227); //third button
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 2, 2, true, false, false, false, false, 0, false, 0, 35, +48, +217, 64, 227); //middle button
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 1, 3, true, false, false, false, false, 0, false, 0, 35, -54, +217, -38, 227); //firstbutton
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BackChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 0, true, true, false, false, false, 0, false, 0, 24, -128, +213, -118, 223);// backbutton
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/ExitChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 0, true, true, true, false, false, 0, false, 0, 15, -170, +213, -164, 218);// exitbutton
            }
            if (ConversationPoint == 0)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Talk0").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 1, 32, 4, 0, 34, 0, 34, 0, 30, -82, +138);
                projectile.AnimatedUIMouths(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SariaMouthNormal").Value), lightColor, 6, 0, (bool)IsAnimated, 7, -129, +174);
                projectile.AnimatedUEyes(lightColor, 5, (int)EyeFrames);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices1").Value), ("0"), lightColor, 3, 0, 300, false, false, false, false, false, 0, false, 0, 35, +150, +217, 166, 227); //third button
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices1").Value), ("0"), lightColor, 3, 2, 2, true, false, false, false, false, 0, false, 0, 35, +48, +217, 64, 227); //middle button
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices1").Value), ("0"), lightColor, 3, 1, 3, false, false, false, false, false, 0, false, 0, 35, -54, +217, -38, 227); //firstbutton
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BackChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 0, true, true, false, false, false, 0, false, 0, 24, -128, +213, -118, 223);// backbutton
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/ExitChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 0, false, true, true, false, false, 0, false, 0, 15, -170, +213, -164, 218);// exitbutton
            }
            if (ConversationPoint == 1)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Talk1").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 1, 32, 34, 0, 34, 0, 34, 0, 30, -82, +138);
                projectile.AnimatedUIMouths(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SariaMouthNormal").Value), lightColor, 6, 0, (bool)IsAnimated, 7, -129, +174);
                projectile.AnimatedUEyes(lightColor, 5, (int)EyeFrames);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices1").Value), ("0"), lightColor, 3, 0, 300, false, false, false, false, false, 0, false, 0, 35, +150, +217, 166, 227); //third button
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices1").Value), ("0"), lightColor, 3, 2, 2, true, false, false, false, false, 0, false, 0, 35, +48, +217, 64, 227); //middle button
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices1").Value), ("0"), lightColor, 3, 1, 3, false, false, false, false, false, 0, false, 0, 35, -54, +217, -38, 227); //firstbutton
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BackChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 0, true, true, false, false, false, 0, false, 0, 24, -128, +213, -118, 223);// backbutton
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/ExitChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 0, false, true, true, false, false, 0, false, 0, 15, -170, +213, -164, 218);// exitbutton
            }
            if (ConversationPoint == 300)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Talk300").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 3, 8, 26, 0, 34, 0, 34, 0, 20, -82, +138);
                projectile.AnimatedUIMouths(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SariaMouthNormal").Value), lightColor, 6, 0, (bool)IsAnimated, 7, -129, +174); ;
                projectile.AnimatedUEyes(lightColor, 5, (int)EyeFrames);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices2").Value), ("0"), lightColor, 3, 0, 320, false, false, false, false, false, 0, false, 0, 35, +150, +217, 166, 227); //third button
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices2").Value), ("0"), lightColor, 3, 2, 330, false, false, false, false, false, 0, false, 0, 35, +48, +217, 64, 227); //middle button
                if (modPlayer.TMPoints >= 1)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices2").Value), ("0"), lightColor, 3, 1, 310, false, false, false, false, false, 0, false, 0, 35, -54, +217, -38, 227); //firstbutton
                }
                if (modPlayer.TMPoints < 1)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices2").Value), ("0"), lightColor, 3, 1, 390, false, false, false, false, false, 0, false, 0, 35, -54, +217, -38, 227); //firstbutton
                }
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BackChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 1, false, true, false, false, false, 0, false, 0, 24, -128, +213, -118, 223);// backbutton
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/ExitChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 0, false, true, true, false, false, 0, false, 0, 15, -170, +213, -164, 218);// exitbutton
            }
            if (ConversationPoint == 301)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Talk301").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 1, 16, 34, 0, 34, 0, 34, 0, 30, -82, +138);
                projectile.AnimatedUIMouths(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SariaMouthNormal").Value), lightColor, 6, 0, (bool)IsAnimated, 7, -129, +174);
                projectile.AnimatedUEyes(lightColor, 5, (int)EyeFrames);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices2").Value), ("0"), lightColor, 3, 0, 320, false, false, false, false, false, 0, false, 0, 35, +150, +217, 166, 227); //third button
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices2").Value), ("0"), lightColor, 3, 2, 330, false, false, false, false, false, 0, false, 0, 35, +48, +217, 64, 227); //middle button
                if (modPlayer.TMPoints >= 1)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices2").Value), ("0"), lightColor, 3, 1, 310, false, false, false, false, false, 0, false, 0, 35, -54, +217, -38, 227); //firstbutton
                }
                if (modPlayer.TMPoints < 1)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices2").Value), ("0"), lightColor, 3, 1, 390, false, false, false, false, false, 0, false, 0, 35, -54, +217, -38, 227); //firstbutton
                }
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BackChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 1, false, true, false, false, false, 0, false, 0, 24, -128, +213, -118, 223);// backbutton
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/ExitChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 0, false, true, true, false, false, 0, false, 0, 15, -170, +213, -164, 218);// exitbutton
            }
            if (ConversationPoint == 310)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Talk310").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 2, 8, 5, 0, 34, 0, 34, 0, 30, -82, +138);
                projectile.AnimatedUIMouths(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SariaMouthNormal").Value), lightColor, 6, 0, (bool)IsAnimated, 7, -129, +174);
                projectile.AnimatedUEyes(lightColor, 5, (int)EyeFrames);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 1, true, false, false, false, false, 0, false, 0, 35, +150, +217, 166, 227); //third button
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 2, 2, true, false, false, false, false, 0, false, 0, 35, +48, +217, 64, 227); //middle button
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 1, 3, true, false, false, false, false, 0, false, 0, 35, -54, +217, -38, 227); //firstbutton
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BackChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 301, false, true, false, false, false, 0, false, 0, 24, -128, +213, -118, 223);// backbutton
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/ExitChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 0, false, true, true, false, false, 0, false, 0, 15, -170, +213, -164, 218);// exitbutton
                if (!modPlayer.SariaUnlockPsychic2)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/PinkCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("Psyshock"), lightColor, 4, 1, 311, false, false, false, true, false, 0, false, 0, 15, -90, 90, -90, 90);
                }
                if (modPlayer.SariaUnlockPsychic2)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/PinkCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("Psyshock"), lightColor, 4, 1, 0, true, false, false, true, false, 0, false, 0, 15, -90, 90, -90, 90);
                }
                if (!modPlayer.SariaUnlockWater)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/BlueCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("AquaRing"), lightColor, 8, 1, 312, false, false, false, true, false, 0, false, 0, 15, -55, 90, -55, 90);
                }
                if (modPlayer.SariaUnlockWater && !modPlayer.SariaUnlockWater2)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/BlueCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("AquaRing"), lightColor, 8, 1, 313, false, false, false, true, false, 0, false, 0, 15, -55, 90, -55, 90);
                }
                if (modPlayer.SariaUnlockWater && modPlayer.SariaUnlockWater2)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/BlueCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("AquaRing"), lightColor, 8, 1, 0, true, false, false, true, false, 0, false, 0, 15, -55, 90, -55, 90);
                }
                if (!modPlayer.SariaUnlockFire)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/RedCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("Eruption"), lightColor, 4, 1, 314, false, false, false, true, false, 0, false, 0, 15, -20, 90, -20, 90);
                }
                if (modPlayer.SariaUnlockFire && !modPlayer.SariaUnlockFire2)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/RedCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("Eruption"), lightColor, 4, 1, 315, false, false, false, true, false, 0, false, 0, 15, -20, 90, -20, 90);
                }
                if (modPlayer.SariaUnlockFire && modPlayer.SariaUnlockFire2)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/RedCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("Eruption"), lightColor, 4, 1, 0, true, false, false, true, false, 0, false, 0, 15, -20, 90, -20, 90);
                }
                if (!modPlayer.SariaUnlockElectric)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/YellowCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("ThunderBolt"), lightColor, 4, 1, 316, false, false, false, true, false, 0, false, 0, 15, 15, 90, 15, 90);
                }
                if (modPlayer.SariaUnlockElectric && !modPlayer.SariaUnlockElectric2)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/YellowCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("ThunderBolt"), lightColor, 4, 1, 317, false, false, false, true, false, 0, false, 0, 15, 15, 90, 15, 90);
                }
                if (modPlayer.SariaUnlockElectric && modPlayer.SariaUnlockElectric2)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/YellowCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("ThunderBolt"), lightColor, 4, 1, 0, true, false, false, true, false, 0, false, 0, 15, 15, 90, 15, 90);
                }
                if (!modPlayer.SariaUnlockRock)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/GreenCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("PowerGem"), lightColor, 4, 1, 318, false, false, false, true, false, 0, false, 0, 15, 50, 90, 50, 90);
                }
                if (modPlayer.SariaUnlockRock && !modPlayer.SariaUnlockRock2)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/GreenCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("PowerGem"), lightColor, 4, 1, 319, false, false, false, true, false, 0, false, 0, 15, 50, 90, 50, 90);
                }
                if (modPlayer.SariaUnlockRock && modPlayer.SariaUnlockRock2)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/GreenCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("PowerGem"), lightColor, 4, 1, 0, true, false, false, true, false, 0, false, 0, 15, 50, 90, 50, 90);
                }
                if (!modPlayer.SariaUnlockBug)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/OrangeCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("AttackOrder"), lightColor, 8, 1, 320, false, false, false, true, false, 0, false, 0, 15, 85, 90, 85, 90);
                }
                if (modPlayer.SariaUnlockBug && !modPlayer.SariaUnlockBug2)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/OrangeCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("AttackOrder"), lightColor, 8, 1, 321, false, false, false, true, false, 0, false, 0, 15, 85, 90, 85, 90);
                }
                if (modPlayer.SariaUnlockBug && modPlayer.SariaUnlockBug2)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/OrangeCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("AttackOrder"), lightColor, 8, 1, 0, true, false, false, true, false, 0, false, 0, 15, 85, 90, 85, 90);
                }
                if (!modPlayer.SariaUnlockBug)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/OrangeCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("AttackOrder"), lightColor, 8, 1, 320, false, false, false, true, false, 0, false, 0, 15, 85, 90, 85, 90);
                }
                if (modPlayer.SariaUnlockBug && !modPlayer.SariaUnlockBug2)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/OrangeCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("AttackOrder"), lightColor, 8, 1, 321, false, false, false, true, false, 0, false, 0, 15, 85, 90, 85, 90);
                }
                if (modPlayer.SariaUnlockBug && modPlayer.SariaUnlockBug2)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/OrangeCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("AttackOrder"), lightColor, 8, 1, 0, true, false, false, true, false, 0, false, 0, 15, 85, 90, 85, 90);
                }
                if (!modPlayer.SariaUnlockGhost)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/PurpleCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("ShadowBall"), lightColor, 8, 1, 322, false, false, false, true, false, 0, false, 0, 15, 120, 90, 120, 90);
                }
                if (modPlayer.SariaUnlockGhost && !modPlayer.SariaUnlockGhost2)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/PurpleCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("ShadowBall"), lightColor, 8, 1, 323, false, false, false, true, false, 0, false, 0, 15, 120, 90, 120, 90);
                }
                if (modPlayer.SariaUnlockGhost && modPlayer.SariaUnlockGhost2)
                {
                    projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/PurpleCharge").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("ShadowBall"), lightColor, 8, 1, 0, true, false, false, true, false, 0, false, 0, 15, 120, 90, 120, 90);
                }
            }
            if (ConversationPoint >= 311 && ConversationPoint <= 323)
            {
                projectile.AnimatedUIMouths(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SariaMouthNormal").Value), lightColor, 6, 0, (bool)IsAnimated, 7, -129, +174);
                projectile.AnimatedUEyes(lightColor, 5, (int)EyeFrames);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 320, true, false, false, false, false, 0, false, 0, 35, +150, +217, 166, 227); //third button
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices").Value), ("0"), lightColor, 3, 2, 301, false, false, false, false, false, 0, false, 0, 35, +48, +217, 64, 227); //middle button
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BackChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 301, false, true, false, false, false, 0, false, 0, 24, -128, +213, -118, 223);// backbutton
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/ExitChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 0, false, true, true, false, false, 0, false, 0, 15, -170, +213, -164, 218);// exitbutton
            }
            if (ConversationPoint == 311)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Talk311").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 3, 24, 29, 0, 8, 2, 34, 0, 30, -82, +138);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices").Value), ("0"), lightColor, 3, 1, 324, false, false, false, false, true, 1, false, 0, 35, -54, +217, -38, 227); //firstbutton
            }
            if (ConversationPoint == 312)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Talk312").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 3, 24, 9, 0, 7, 2, 34, 0, 30, -82, +138);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices").Value), ("0"), lightColor, 3, 1, 324, false, false, false, false, true, 2, false, 0, 35, -54, +217, -38, 227); //firstbutton
            }
            if (ConversationPoint == 313)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Talk311").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 3, 24, 29, 0, 8, 2, 34, 0, 30, -82, +138);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices").Value), ("0"), lightColor, 3, 1, 324, false, false, false, false, true, 3, false, 0, 35, -54, +217, -38, 227); //firstbutton
            }
            if (ConversationPoint == 314)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Talk312").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 3, 24, 9, 0, 7, 2, 34, 0, 30, -82, +138);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices").Value), ("0"), lightColor, 3, 1, 324, false, false, false, false, true, 4, false, 0, 35, -54, +217, -38, 227); //firstbutton
            }
            if (ConversationPoint == 315)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Talk311").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 3, 24, 29, 0, 8, 2, 34, 0, 30, -82, +138);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices").Value), ("0"), lightColor, 3, 1, 324, false, false, false, false, true, 5, false, 0, 35, -54, +217, -38, 227); //firstbutton
            }
            if (ConversationPoint == 316)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Talk312").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 3, 24, 9, 0, 7, 2, 34, 0, 30, -82, +138);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices").Value), ("0"), lightColor, 3, 1, 324, false, false, false, false, true, 6, false, 0, 35, -54, +217, -38, 227); //firstbutton
            }
            if (ConversationPoint == 317)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Talk311").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 3, 24, 29, 0, 8, 2, 34, 0, 30, -82, +138);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices").Value), ("0"), lightColor, 3, 1, 324, false, false, false, false, true, 7, false, 0, 35, -54, +217, -38, 227); //firstbutton
            }
            if (ConversationPoint == 318)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Talk312").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 3, 24, 9, 0, 7, 2, 34, 0, 30, -82, +138);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices").Value), ("0"), lightColor, 3, 1, 324, false, false, false, false, true, 8, false, 0, 35, -54, +217, -38, 227); //firstbutton
            }
            if (ConversationPoint == 319)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Talk311").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 3, 24, 29, 0, 8, 2, 34, 0, 30, -82, +138);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices").Value), ("0"), lightColor, 3, 1, 324, false, false, false, false, true, 9, false, 0, 35, -54, +217, -38, 227); //firstbutton
            }
            if (ConversationPoint == 320)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Talk312").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 3, 24, 9, 0, 7, 2, 34, 0, 30, -82, +138);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices").Value), ("0"), lightColor, 3, 1, 324, false, false, false, false, true, 10, false, 0, 35, -54, +217, -38, 227); //firstbutton
            }
            if (ConversationPoint == 321)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Talk311").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 3, 24, 29, 0, 8, 2, 34, 0, 30, -82, +138);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices").Value), ("0"), lightColor, 3, 1, 324, false, false, false, false, true, 11, false, 0, 35, -54, +217, -38, 227); //firstbutton
            }
            if (ConversationPoint == 322)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Talk312").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 3, 24, 9, 0, 7, 2, 34, 0, 30, -82, +138);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices").Value), ("0"), lightColor, 3, 1, 324, false, false, false, false, true, 12, false, 0, 35, -54, +217, -38, 227); //firstbutton
            }
            if (ConversationPoint == 323)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Talk311").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 3, 24, 29, 0, 8, 2, 34, 0, 30, -82, +138);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BTXChoices").Value), ("0"), lightColor, 3, 1, 324, false, false, false, false, true, 13, false, 0, 35, -54, +217, -38, 227); //firstbutton
            }
            if (ConversationPoint == 324)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Talk324").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 1, 19, 34, 0, 34, 0, 34, 0, 500, -82, +138);
                projectile.AnimatedUIMouths(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SariaMouthNormal").Value), lightColor, 6, 0, (bool)IsAnimated, 7, -129, +174);
                projectile.AnimatedUEyes(lightColor, 5, (int)EyeFrames);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 300, true, false, false, false, false, 0, false, 0, 35, +150, +217, 166, 227); //third button
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 2, 2, true, false, false, false, false, 0, false, 0, 35, +48, +217, 64, 227); //middle button
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 1, 3, true, false, false, false, false, 0, false, 0, 35, -54, +217, -38, 227); //firstbutton
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BackChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 0, true, true, false, false, false, 0, false, 0, 24, -128, +213, -118, 223);// backbutton
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/ExitChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 0, true, true, true, false, false, 0, false, 0, 15, -170, +213, -164, 218);// exitbutton
                for (int U = 0; U < 1000; U++)
                {
                    if (Main.projectile[U].active && Main.projectile[U].ModProjectile is TalkingUI modProjectile && U == projectile.whoAmI && ((Main.projectile[U].owner == owner)))
                    {
                        modProjectile.Countdown--;
                    }
                }
            }
            if (ConversationPoint == 390)
            {
                projectile.UIText(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Talk390").Value), lightColor, 1, 210, +200);
                projectile.AnimatedUITextCover(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/TalkingUI").Value), lightColor, 32, 32, 32, 32, (int)FrameSpeed, (int)timer, (int)timerState, 3, 9, 7, 0, 9, 1, 34, 0, 40, -82, +138);
                projectile.AnimatedUIMouths(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SariaMouthShocked").Value), lightColor, 6, 0, (bool)IsAnimated, 7, -129, +176);
                projectile.AnimatedUEyesShocked(lightColor, 5, (int)EyeFrames);
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 1, true, false, false, false, false, 0, false, 0, 35, +150, +217, 166, 227); //third button
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 2, 2, true, false, false, false, false, 0, false, 0, 35, +48, +217, 64, 227); //middle button
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/SmallChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 1, 301, true, false, false, false, false, 0, false, 0, 35, -54, +217, -38, 227); //firstbutton
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/BackChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 301, false, true, false, false, false, 0, false, 0, 24, -128, +213, -118, 223);// backbutton
                projectile.ClickableUIButton1(((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/ExitChoiceUI").Value), ((Texture2D)ModContent.Request<Texture2D>("SariaMod/Items/zTalking/Blank").Value), ("0"), lightColor, 3, 0, 0, false, true, true, false, false, 0, false, 0, 15, -170, +213, -164, 218);// exitbutton
            }
        }
    }
}