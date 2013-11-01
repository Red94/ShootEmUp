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
    class PowerUP
    {
        public Rectangle boundingBox;
        public Texture2D texture;
        public Vector2 position;
        public int speed, power;
        //public float timer;
        public bool isVisible;

        public PowerUP(Texture2D newTexture, Vector2 newPosition, int newPower)
        {
            texture = newTexture;
            position = newPosition;
            speed = 2;
            isVisible = true;
            power = newPower;
        }

        public void Update(GameTime gameTime)
        {
            //the bounding box
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            //the movement of the powerups
            position.Y += speed;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
