using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleSiteE.Manager;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BattleSiteE.GameScreens
{
    class ConfirmationScreen : GameScreen
    {
        String btnOKtext = "OK";
        String btnCANCELtext = "CANCEL";
        int selectedindex = 0;
        String message = "?";
        ContentManager contentMan;
        SpriteFont menuFont;
        Texture2D dialogtexture;
        Texture2D plainBlack;
        Rectangle maindialog = new Rectangle(0, 0, 604, 200);
        Rectangle btn_selected = new Rectangle  (0, 201, 302, 29);
        Rectangle btn_normal = new Rectangle    (302, 201, 302, 29);
        Rectangle pixel = new Rectangle(0, 0, 1, 1);

        public ConfirmationScreen(String message)
        {
            this.message = message;
            TransitionOnTime = TimeSpan.FromSeconds(0.1);
            TransitionOffTime = TimeSpan.FromSeconds(0.1);
        }

        public ConfirmationScreen(String message, String oktext, String canceltext) : this(message)
        {
            this.btnOKtext = oktext;
            this.btnCANCELtext = canceltext;
        }

        public override void LoadContent()
        {
            if (contentMan == null)
                contentMan = new ContentManager(ScreenManager.Game.Services, "Content");

            dialogtexture = contentMan.Load<Texture2D>("dialogparts");
            menuFont = contentMan.Load<SpriteFont>("menufont");
            plainBlack = new Texture2D(ScreenManager.GraphicsDevice, 1, 1);
            plainBlack.SetData<Color>(new Color[] {Color.Black});
            
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch sb = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            //calculate transision effects
            Color whitetextcolor = Color.White * TransitionAlpha;
            Color graytextcolor = Color.Gray * TransitionAlpha;
            Color blacktexcolor = Color.White * (TransitionAlpha);
            
            sb.Begin();

            //background
            sb.Draw(plainBlack, new Rectangle(0, 0, 1280, 720), pixel, Color.White * TransitionAlpha * 0.6f);

            //dialog
            sb.Draw(dialogtexture, new Rectangle(338, 260, 604, 200), maindialog, blacktexcolor);

            //dialog text
            sb.DrawString(menuFont, message, new Vector2(640-(menuFont.MeasureString(message).X/2), 360), whitetextcolor);

            //buttons
            if (selectedindex == 0)
            {
                sb.Draw(dialogtexture, new Rectangle(337, 462, 302, 29), btn_selected, blacktexcolor);
                sb.Draw(dialogtexture, new Rectangle(641, 462, 302, 29), btn_normal, blacktexcolor);
                sb.DrawString(menuFont, btnOKtext, new Vector2(353, 465), whitetextcolor);
                sb.DrawString(menuFont, btnCANCELtext, new Vector2(658, 465), graytextcolor);
            }
            else
            {
                sb.Draw(dialogtexture, new Rectangle(337, 462, 302, 29), btn_normal, blacktexcolor);
                sb.Draw(dialogtexture, new Rectangle(641, 462, 302, 29), btn_selected, blacktexcolor);
                sb.DrawString(menuFont, btnOKtext, new Vector2(353, 465), graytextcolor);
                sb.DrawString(menuFont, btnCANCELtext, new Vector2(658, 465), whitetextcolor);
            }

            

            sb.End();
        }

        public override void HandleInput()
        {
            if (ScreenManager.InputController.isMenuRight())
            {
                selectedindex += 1;
                selectedindex = (int)MathHelper.Clamp(selectedindex, 0, 1);
            }
            if (ScreenManager.InputController.isMenuLeft())
            {
                selectedindex -= 1;
                selectedindex = (int)MathHelper.Clamp(selectedindex, 0, 1);
            }
            if (ScreenManager.InputController.isMenuSelect())
            {
                if (selectedindex == 0) confirm();
                else if (selectedindex == 1) back();
            }
        }

        public virtual void confirm()
        {
            this.ExitScreen();
        }

        public virtual void back()
        {
            this.ExitScreen();
        }

    }
}
