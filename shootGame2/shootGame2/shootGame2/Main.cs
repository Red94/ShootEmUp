using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using shootGame2.Unit;

namespace shootGame2
{
    public class Main : Microsoft.Xna.Framework.Game
    {
        //state enum
        public enum State
        {
            Menu,
            Game,
            GameOver
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Random random = new Random();

        public int enemyBulletDamage, enemyShipBulletDamage, playerBulletDamage;
        public int enemyHealth, enemyShipHealth, asteroidHealth;
        public int enemyCounter;

        public Texture2D menuImage;
        public Texture2D gameOverImage;
        public Texture2D bluePowerup, redPowerup;
        public Random randomPower;

        //our player and background objects
        Player player = new Player();
        Background background = new Background();
        HUD hud = new HUD();

        //Set first State
        State gameState = State.Menu;

        //lists
        List<Asteroid> asteroidList = new List<Asteroid>();
        List<EnemyShip> enShipList = new List<EnemyShip>();
        List<Enemy> enemyList = new List<Enemy>();
        List<Explosion> explosionList = new List<Explosion>();
        List<PowerUP> powerUpList = new List<PowerUP>();

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 750;
            graphics.PreferredBackBufferHeight = 850;
            this.Window.Title = "2D Shooter";
            Content.RootDirectory = "Content";

            enemyBulletDamage = 10;
            enemyHealth = 4;
            enemyShipHealth = 6;
            playerBulletDamage = 2;
            enemyShipBulletDamage = 5;
            menuImage = null;
            gameOverImage = null;
            enemyCounter = 0;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player.LoadContent(Content);
            background.LoadContent(Content);
            hud.LoadContent(Content);
            menuImage = Content.Load<Texture2D>("menu");
            gameOverImage = Content.Load<Texture2D>("gameOver");
            redPowerup = Content.Load<Texture2D>("PUred");
            bluePowerup = Content.Load<Texture2D>("PUblue");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // updating game state
            switch (gameState)
            {
                case State.Game:
                    {

                        // TODO: Add your update logic here
                        player.Update(gameTime);
                        background.Update(gameTime);
                        hud.Update(gameTime);

                        background.speed = 5;

                        //updateing enemy's and checking collision of enemyship to playership
                        foreach (Enemy e in enemyList)
                        {
                            //check if enemyship is collidiong with player
                            if (e.boundingBox.Intersects(player.boundinBox))
                            {
                                player.health -= 40;
                                e.isVisible = false;
                            }

                            //check enemy bullet collision with player ship
                            for (int i = 0; i < e.bulletList.Count; i++)
                            {
                                if (player.boundinBox.Intersects(e.bulletList[i].boundingBox))
                                {
                                    player.health -= enemyBulletDamage;
                                    e.bulletList[i].isVisible = false;
                                }
                            }

                            //check player bullet collision to enemy ship
                            for (int i = 0; i < player.bulletLists.Count; i++)
                            {
                                if (player.bulletLists[i].boundingBox.Intersects(e.boundingBox))
                                {
                                    explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion3"), new Vector2(e.position.X, e.position.Y)));
                                    hud.playerScore += 20;
                                    player.bulletLists[i].isVisible = false;
                                    e.isVisible = false;
                                    enemyCounter ++;

                                    randomPower = new Random();
                                    int p = randomPower.Next(0, 100);

                                    if (p < 10)
                                    {
                                        powerUpList.Add(new PowerUP(redPowerup, e.position, 1));
                                    }
                                    else if (p < 15)
                                    {
                                        powerUpList.Add(new PowerUP(bluePowerup, e.position, 2));
                                    }
                                }
                            }
                            e.Update(gameTime);
                        }
                        

                        //update explosions
                        foreach (Explosion ex in explosionList)
                        {
                            ex.Update(gameTime);
                        }

                        //for each asteroid in our asteroid list, update it and check for colission
                        foreach (Asteroid a in asteroidList)
                        {
                            //check to see if any of the asteroids are colliding with our player shi[, if they are set isVisible to false (remove them from the asteroid list)
                            if (a.boundingBox.Intersects(player.boundinBox))
                            {
                                player.health -= 10;
                                a.isVisible = false;
                            }

                            //interate trough our bulletList if any asteroids come in contacts with these bullets, destroy bullet and asteroid
                            for (int i = 0; i < player.bulletLists.Count(); i++)
                            {
                                if (a.boundingBox.Intersects(player.bulletLists[i].boundingBox))
                                {
                                    explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion3"), new Vector2(a.position.X, a.position.Y)));
                                    hud.playerScore += 5;
                                    a.isVisible = false;
                                    player.bulletLists.ElementAt(i).isVisible = false;
                                    enemyCounter++;
                                }
                            }

                            a.Update(gameTime);
                        }

                        foreach (EnemyShip es in enShipList)
                        {
                            //check to see if any of the asteroids are colliding with our player shi[, if they are set isVisible to false (remove them from the asteroid list)
                            if(es.boundingBox.Intersects(player.boundinBox))
                            {
                                player.health -=10;
                                es.isVisible = false;
                            }

                            //interate trough our bulletList if any asteroids come in contacts with these bullets, destroy bullet and asteroid
                            for (int i = 0; i < player.bulletLists.Count(); i++)
                            {
                                if (es.boundingBox.Intersects(player.bulletLists[i].boundingBox))
                                {
                                    explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion3"), new Vector2(es.position.X, es.position.Y)));
                                    hud.playerScore += 10;
                                    es.isVisible = false;
                                    player.bulletLists.ElementAt(i).isVisible = false;
                                    enemyCounter++;
                                    
                                    randomPower = new Random();
                                    int p = randomPower.Next(0, 100);

                                    if (p < 10)
                                    {
                                        powerUpList.Add(new PowerUP(redPowerup, es.position, 1));
                                    }
                                    else if (p < 15)
                                    {
                                        powerUpList.Add(new PowerUP(bluePowerup ,es.position,2));
                                    }
                                }
                            }

                            es.Update(gameTime);
                        }

                        //if the player health hits zero go to the gameover screen
                        if (player.health <= 0)
                            gameState = State.GameOver;

                        //the collision between powerUp red and the player
                        foreach (PowerUP powerUp in powerUpList)
                        {
                            powerUp.Update(gameTime);
                            if (powerUp.boundingBox.Intersects(player.boundinBox))
                            {
                                powerUp.isVisible = false;
                                
                                if (powerUp.power == 1)
                                    player.power = Player.powerUP.two;

                                if (powerUp.power == 2)
                                    player.power = Player.powerUP.three;
                            }

                        }

                        //remove the red powerUp from the screen
                        for (int i = 0; i < powerUpList.Count; i++)
                        {
                            if (!powerUpList[i].isVisible)
                            {
                                powerUpList.RemoveAt(i);
                            }
                        }

                        //functions that get called in the game screen
                        LoadAsteroids();
                        ManageExplosions();
                        LoadEnemys();
                        LoadEnemiesShip();
                        break;
                    }

                    //updating menu state
                case State.Menu:
                    {
                        //get keyboard state
                        KeyboardState keyState = Keyboard.GetState();

                        //on the menu screen press enter to start the game
                        if (keyState.IsKeyDown(Keys.Enter))
                            gameState = State.Game;

                        background.Update(gameTime);
                        background.speed = 1;
                        break;
                    }

                    //updating game over
                case State.GameOver:
                    {
                        //get keyboard state
                        KeyboardState keyState = Keyboard.GetState();

                        //if in the gameover screen and user hits "escape" key, return to the main menu
                        if (keyState.IsKeyDown(Keys.Escape))
                        {
                            //set the player back on its posision, resets the health and score and clears all the lists also gameState becomes menu
                            player.position = new Vector2(375, 850);
                            enemyList.Clear();
                            asteroidList.Clear();
                            enShipList.Clear();
                            player.health = 200;
                            hud.playerScore = 0;
                            gameState = State.Menu;
                        }

                        //letting the background update so the background keeps moving even in the GameOver screen but because of the speed it moves slower
                        background.Update(gameTime);
                        background.speed = 1;

                        break;
                    }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            switch (gameState)
            {
                    //drawing game state so it draws all the things we want to show when in the game state
                case State.Game:
                    {
                        background.Draw(spriteBatch);

                        player.Draw(spriteBatch);

                        foreach (Explosion ex in explosionList)
                        {
                            ex.Draw(spriteBatch);
                        }

                        foreach (Asteroid a in asteroidList)
                        {
                            a.Draw(spriteBatch);
                        }

                        foreach (Enemy e in enemyList)
                        {
                            e.Draw(spriteBatch);
                        }

                        foreach (EnemyShip es in enShipList)
                        {
                            es.Draw(spriteBatch);
                        }

                        foreach (PowerUP powerUp in powerUpList)
                        {
                            powerUp.Draw(spriteBatch);
                        }

                        hud.Draw(spriteBatch);

                        break;
                    }

                    //drawing menu state
                case State.Menu:
                    {
                        background.Draw(spriteBatch);
                        spriteBatch.Draw(menuImage, new Vector2(0, 0), Color.White);
                        break;
                    }

                    //drawing gameover
                case State.GameOver:
                    {
                        background.Draw(spriteBatch);
                        spriteBatch.Draw(gameOverImage, new Vector2(0, 0), Color.White);
                        spriteBatch.DrawString(hud.playerScoreFont, "Your Final score was " + hud.playerScore.ToString(), new Vector2(100, 400), Color.Red);
                        break;
                    }
            }
            
            spriteBatch.End();
            
            base.Draw(gameTime);
        }

        //load asteroids
        public void LoadAsteroids()
        {
            //creating random var for or x and y axis of our asteroids
            int randY = random.Next(-600, -50);
            int randX = random.Next(0, 700);

            //if there are less then 5 asteroids on the screen then create more until there are 5
            if (asteroidList.Count() < 5)
            {
                asteroidList.Add(new Asteroid(Content.Load<Texture2D>("asteroid"), new Vector2(randX, randY)));
            }

            //if any of the asteroids in the list are destroyed (or are invisible) then remove them from the list
            for (int i = 0; i < asteroidList.Count(); i++)
            {
                if (!asteroidList[i].isVisible)
                {
                    asteroidList.RemoveAt(i);
                    i--;
                }
            }
        }

        //load enemies
        public void LoadEnemys()
        {
            //creating random var for or x and y axis of our asteroids
            int randY = random.Next(-600, -50);
            int randX = random.Next(0, 700);

            //if there are less then 3 asteroids on the screen then create more until there are 3
            if (enemyList.Count() < 3 && enemyCounter >= 10)
            {
                enemyList.Add(new Enemy(Content.Load<Texture2D>("enemy"), new Vector2(randX, randY), Content.Load<Texture2D>("EnemyBullet")));
            }

            //if any of the enemies in the list are destroyed (or are invisible) then remove them from the list
            for (int i = 0; i < enemyList.Count(); i++)
            {
                if (!enemyList[i].isVisible)
                {
                    enemyList.RemoveAt(i);
                    i--;
                }
            }
        }

        //load asteroids
        public void LoadEnemiesShip()
        {
            //creating random var for or x and y axis of our asteroids
            int randY = random.Next(-600, -50);
            int randX = random.Next(0, 700);

            //if there are less then 5 asteroids on the screen then create more until there are 5
            if (enShipList.Count() < 5 && enemyCounter <= 10)
            {
                enShipList.Add(new EnemyShip(Content.Load<Texture2D>("medfrighter"), new Vector2(randX, randY)));
            }

            //if any of the asteroids in the list are destroyed (or are invisible) then remove them from the list
            for (int i = 0; i < enShipList.Count(); i++)
            {
                if (!enShipList[i].isVisible)
                {
                    enShipList.RemoveAt(i);
                    i--;
                }
            }
        }

        //manage explosions
        public void ManageExplosions()
        {
            for (int i = 0; i < explosionList.Count; i++)
            {
                if (!explosionList[i].isVisible)
                {
                    explosionList.RemoveAt(i);
                    i--;
                }
            }
        }

    }
}
