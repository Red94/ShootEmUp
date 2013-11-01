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
    public class Asteroid
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 origin;
        public Rectangle boundingBox;
        public float rotationAngle, direction;
        public int speed;

        public bool isVisible;
        Random random = new Random();
        public float ranX, ranY;

        //constructer
        public Asteroid(Texture2D newTexture,Vector2 newPosition)
        {
            position = newPosition;
            texture = newTexture;
            speed = 4;
            direction = random.Next(60, 120);
            isVisible = true;

            //the random position of the enemy
            ranX = random.Next(0, 700);
           // ranY = random.Next(-600, -50);
        }

        //loadcontent
        public void LoadContent(ContentManager Content)
        {
            //origin.X = texture.Width / 2;
            //origin.Y = texture.Height / 2;
        }

        //update also known as a loop
        public void Update(GameTime gameTime)
        {
            //set or boundingbox for collision
            boundingBox = new Rectangle((int)position.X, (int)position.Y, 45, 45);

            //update movement
            position.X += (float)Math.Round(Math.Cos(direction * (Math.PI / 180)) * speed);
            position.Y += (float)Math.Round(Math.Sin(direction * (Math.PI / 180)) * speed);
            if (position.Y >= 850)
            {
                position.Y = -50;
                direction = random.Next(60, 120);
            }

            //rotation asteroid
            //float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //rotationAngle += elapsed;
            //float circle = MathHelper.Pi * 2;
            //rotationAngle = rotationAngle % circle;
        }


        //draw
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
               // spriteBatch.Draw(texture, position, null, Color.White, rotationAngle, origin, 1.0f, SpriteEffects.None, 0f);
                spriteBatch.Draw(texture, position, Color.White);
            }
        }
    }
}
