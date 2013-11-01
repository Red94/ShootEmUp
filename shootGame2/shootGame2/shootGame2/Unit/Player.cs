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
    public class Player
    {
        public enum powerUP
        {
            one,
            two,
            three
        }

        public powerUP power = powerUP.one;

        public float timer, interval;

        public Texture2D texture,bulletTexture, healthTexture;
        public Vector2 position, healthBarPosition;
        public int speed, health;
        public float bulletDelay;
        public List<Bullet> bulletLists;
        public Rectangle healthRectangle;
        
        //collision var
        public bool isColliding;
        public Rectangle boundinBox;

        //constructer
        public Player()
        {
            bulletLists = new List<Bullet>();
            texture = null;
            timer = 0;
            interval = 5000;
            position = new Vector2(375, 800);
            speed = 10;
            bulletDelay = 1;
            isColliding = false;
            health = 200;
            healthBarPosition = new Vector2(50, 50);
        }

        //loadContent method
        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("ship");
            bulletTexture = Content.Load<Texture2D>("playerbullet");
            healthTexture = Content.Load<Texture2D>("healthbar");
        }

        //draw 
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.Draw(healthTexture, healthRectangle, Color.White);
            
            foreach (Bullet b in bulletLists)
                b.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            //getting keyboard state
            KeyboardState keyState = Keyboard.GetState();

            //power up timer
            if ((int)power > 0)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timer > interval)
                {
                    timer = 0;
                    power = powerUP.one; 
                }
            }

            //boundingbox for our player ship
            boundinBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            //set rectangle for health bar
            healthRectangle = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, health, 25);

            //Fire Bullets
            if( keyState.IsKeyDown(Keys.Space))
            {
                Shoot();
            }

            UpdateBullets();
            
            //controles of the player
            if (keyState.IsKeyDown(Keys.Up))
            {
                position.Y = position.Y - speed;
            }
            if (keyState.IsKeyDown(Keys.Left))
            {
                position.X = position.X - speed;
            }

            if (keyState.IsKeyDown(Keys.Down))
            {
                position.Y = position.Y + speed;
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                position.X = position.X + speed;
            }

            //ship stays in screen
            if (position.X <= 0) 
                position.X = 0; 

            if (position.X >= 750 - texture.Width) 
                position.X = 750 - texture.Width;

            if (position.Y <= 0) 
                position.Y = 0;

            if (position.Y >= 850 - texture.Height) 
                position.Y = 850 - texture.Height;
        }

        //function used to set the position of the bullets
        public void Shoot()
        {
            //shoot only if bulletDelay resets
            if (bulletDelay >= 0)
                bulletDelay--;

            //if the bulletDelay is at 0, create a new bullet at the player position and show it, then add the bullet to the list
            if (bulletDelay <= 0)
            {
                Bullet newBullet;

                //the switch for wich power up has what
                switch (power)
                {
                    case powerUP.one:
                            newBullet = new Bullet(bulletTexture);
                            newBullet.position = new Vector2(position.X + 32 - newBullet.texture.Width / 2, position.Y + 30);

                            //making the bullet visible
                            newBullet.isVisible = true;

                            if (bulletLists.Count() < 20)
                                bulletLists.Add(newBullet);
                        break;

                     case powerUP.two:
                        for (int i = 1; i <= 2; i++)
                        {
                            newBullet = new Bullet(bulletTexture);
                            newBullet.position = new Vector2(position.X + (25 * i) - newBullet.texture.Width / 2, position.Y + 30);
                            
                            //making the bullet visible
                            newBullet.isVisible = true;

                            if (bulletLists.Count() < 20)
                                bulletLists.Add(newBullet);

                        }
                        break;

                    case powerUP.three:
                        for (int i = 1; i <= 3; i++)
                        {
                            newBullet = new Bullet(bulletTexture);
                            newBullet.position = new Vector2(position.X + 32 - newBullet.texture.Width / 2, position.Y + 30);

                            if (i == 1)
                                newBullet.direction = 245;

                            if (i == 2)
                                newBullet.direction = 270;

                            if (i == 3)
                                newBullet.direction = 295;

                            //making the bullet visible
                            newBullet.isVisible = true;

                            if (bulletLists.Count() < 20)
                                bulletLists.Add(newBullet);
                        }
                        break;

                }
 
              }

            //reset bulletDelay
            if (bulletDelay == 0)
                bulletDelay = 10;
        }

        //updates the bullets
        public void UpdateBullets()
        {
            // foreach bullet in our bulletList, update the movement and if the bullet hits the top of the screen remove it from the list
            foreach (Bullet bullet in bulletLists)
            {
                //bouding box for every bullet in our bullet list
                bullet.boundingBox = new Rectangle((int)bullet.position.X, (int)bullet.position.Y, bullet.texture.Width, bullet.texture.Height);

                //set movement for bullet
                bullet.position.X += (float)Math.Round(Math.Cos(bullet.direction * (Math.PI / 180)) * bullet.speed);
                bullet.position.Y += (float)Math.Round(Math.Sin(bullet.direction * (Math.PI / 180)) * bullet.speed);

                //if bullet hits the top of the screen, then make visible is false
                if (bullet.position.Y <= 0)
                    bullet.isVisible = false;
            }

            //interate trough bullet list and see of any of the bullets are not visible, if they aren't remove the bullet from our bullet list
            for (int i = 0; i < bulletLists.Count; i++)
            {
                if (!bulletLists[i].isVisible)
                {
                    bulletLists.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}