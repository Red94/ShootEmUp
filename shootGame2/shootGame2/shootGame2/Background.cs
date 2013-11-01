using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace shootGame2
{
    public class Background
    {
        public Texture2D texture;
        public Vector2 bgPos1, bgPos2;
        public int speed;

        //constructer
        public Background()
        {
            texture = null;
            bgPos1 = new Vector2(0, 0);
            bgPos2 = new Vector2(0, -850);
            speed = 5;
        }

        //loadcontent
        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("space");
        }

        //draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bgPos1, Color.White);
            spriteBatch.Draw(texture, bgPos2, Color.White);
        }

        //update also known as a loop
        public void Update(GameTime gameTime)
        {
            bgPos1.Y = bgPos1.Y + speed;
            bgPos2.Y = bgPos2.Y + speed;

            // scrolling background
            if (bgPos1.Y >= 850)
            {
                bgPos1.Y = 0;
                bgPos2.Y = -850;
            }
        }
    }
}
