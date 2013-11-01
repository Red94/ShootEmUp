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
    public class Explosion
    {
        public Texture2D texture;
        public Vector2 position, origin;
        public float timer, interval;
        public int currentFrame, spriteWidth, spriteHeight;
        public Rectangle srcRect;
        public bool isVisible;

        //constructer
        public Explosion(Texture2D newTexture,Vector2 newPosition)
        {
            position = newPosition;
            texture = newTexture;
            timer = 0f;
            interval = 20f;
            currentFrame = 1;
            spriteWidth = 128;
            spriteHeight = 128;
            isVisible = true;
        }

        //load content
        public void LoadContent(ContentManager Content)
        {
            
        }

        //update
        public void Update(GameTime gameTime)
        {
            //increase the timer by the number of milliseconds since update was last called
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //check the timer is more than the chosen interval
            if (timer > interval)
            {
                //show the next frame
                currentFrame++;

                //reset the timer
                timer = 0f;
            }

            //if were on the last frame, make the explosion invisible and reset currentframe to the first image
            if (currentFrame == 17)
            {
                isVisible = false;
                currentFrame = 0;
            }

            srcRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            origin = new Vector2(srcRect.Width / 2, srcRect.Height / 2);
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            //if visible then draw
            if (isVisible == true)
                spriteBatch.Draw(texture, position, srcRect, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
        }
    }
}
