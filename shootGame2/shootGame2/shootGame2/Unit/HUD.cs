using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace shootGame2.Unit
{
    public class HUD
    {
        public int playerScore, screenWidth, screenHeight;
        public SpriteFont playerScoreFont;
        public Vector2 playerScorePos;
        public bool showHud;

        //constructer
        public HUD()
        {
            playerScore = 0;
            showHud = true;
            screenHeight = 850;
            screenWidth = 750;
            playerScoreFont = null;
            playerScorePos = new Vector2(screenWidth / 2, 50);
        }

        //loadcontent
        public void LoadContent(ContentManager Content)
        {
            playerScoreFont = Content.Load<SpriteFont>("SpriteFont1");
        }

        //update
        public void Update(GameTime gameTime)
        {
            // get keyboad state
            KeyboardState keyState = Keyboard.GetState();
        }

        //draw
        public void Draw(SpriteBatch spriteBatch)
        {
            //if we are showing our HUD (if showHud == true) then show our HUD
            if (showHud)
                spriteBatch.DrawString(playerScoreFont, "Score:  " + playerScore, playerScorePos, Color.Red);
        }
    }
}
