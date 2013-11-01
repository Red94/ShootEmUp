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
    //constructer
    public class EnemyShip
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 origin;
        public Rectangle boundingBox;
        public int health, speed, speed2, currentDifficultyLevel;

        public bool isVisible;
        Random random = new Random();

        public EnemyShip(Texture2D newTexture, Vector2 newPosition)
        {
            texture = newTexture;
            health = 5;
            position = newPosition;
            currentDifficultyLevel = 1;
            speed = 3;
            speed2 = 2;
            isVisible = true;
        }

        //Update
        public void Update(GameTime gameTime)
        {
            // update collision rectangle
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            //update enemy movement
            position.Y += speed;
            position.X += speed2;

            //move enemy back to the top of the screen if he fly's off the bottom
            if (position.Y >= 850)
                position.Y = -75;

            //move enemy back to the left if he fly's out off the screen on the right
            if (position.X >= 750)
                position.X = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //draw enemy ship
            spriteBatch.Draw(texture, position, Color.White);

        }


    }
}
