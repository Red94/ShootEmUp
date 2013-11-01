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
    public class Enemy
    {
        public Rectangle boundingBox;
        public Texture2D texture, bulletTexture;
        public Vector2 position;
        public int health, speed,speed2, bulletDelay, currentDifficultyLevel;
        public bool isVisible;
        public List<Bullet> bulletList;

        //constructer
        public Enemy(Texture2D newTexture,Vector2 newPosition, Texture2D newBulletTexture )
        {
            bulletList = new List<Bullet>();
            texture = newTexture;
            bulletTexture = newBulletTexture;
            health = 5;
            position = newPosition;
            currentDifficultyLevel = 1;
            bulletDelay = 80;
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
            if (position.X >= 750 - texture.Width || position.X <= 0)
            {
                speed2 *= -1;
            }

            EnemyShoot();
            UpdateBullets();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //draw enemy ship
            spriteBatch.Draw(texture, position, Color.White);

            //draw enemy bullets
            foreach (Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }
        }

        //updates the bullets
        public void UpdateBullets()
        {
            // foreach bullet in our bulletList, update the movement and if the bullet hits the top of the screen remove it from the list
            foreach (Bullet bullet in bulletList)
            {
                //bouding box for every bullet in our bullet list
                bullet.boundingBox = new Rectangle((int)bullet.position.X, (int)bullet.position.Y, bullet.texture.Width, bullet.texture.Height);

                //set movement for bullet
                bullet.position.Y = bullet.position.Y + bullet.speed;

                //if bullet hits the top of the screen, then make visible is false
                if (bullet.position.Y >= 850)
                    bullet.isVisible = false;
            }

            //interate trough bullet list and see of any of the bullets are not visible, if they aren't remove the bullet from our bullet list
            for (int i = 0; i < bulletList.Count; i++)
            {
                if (!bulletList[i].isVisible)
                {
                    bulletList.RemoveAt(i);
                    i--;
                }
            }
        }

        //enemy shoot function
        public void EnemyShoot()
        {
            //shoot only if the bulletdelay resets
            if (bulletDelay >= 0)
                bulletDelay--;

            if (bulletDelay <= 0)
            {
                //create new bullet and position it front and center of enemy shop
                Bullet newBullet = new Bullet(bulletTexture);
                newBullet.position = new Vector2(position.X + texture.Width / 2 - newBullet.texture.Width / 2, position.Y + 30);

                newBullet.isVisible = true;

                if (bulletList.Count() < 20)
                    bulletList.Add(newBullet);
            }

            if (bulletDelay == 0)
                bulletDelay = 80;
        }
    }
}
