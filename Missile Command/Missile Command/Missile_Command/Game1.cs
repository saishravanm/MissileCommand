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


namespace MissileCommand
{
    public enum GameState
    {
        StartScreen, GameplayScreen, GameOverScreen
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font1, font2;
        SoundEffect missileFiringClip, explosionClip, rewardClip;
        GameState gameState;
        Texture2D background, destroyedCity, undestroyedCity;
        int w, h; //screen width and height
        Texture2D t;
        Rectangle r;
        Crosshair plane;
        KeyboardState oldKb = Keyboard.GetState();
        MouseState oldMouse = Mouse.GetState();
        City[] cities;
        List<EnemyMissle> enemylist;
        int score;
        int missileCount;
        int timer;
        List<MissileSite> sites = new List<MissileSite>();
        //Texture2D rocket, explos;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            w = GraphicsDevice.Viewport.Width;
            h = GraphicsDevice.Viewport.Height;
            gameState = GameState.StartScreen;
            cities = new City[6];
            cities[0] = new City(new Rectangle(80, 415, 45, 15), false);
            cities[1] = new City(new Rectangle(170, 415, 45, 15), false);
            cities[2] = new City(new Rectangle(260, 415, 45, 15), false);
            cities[3] = new City(new Rectangle(495, 415, 45, 15), false);
            cities[4] = new City(new Rectangle(585, 415, 45, 15), false);
            cities[5] = new City(new Rectangle(675, 415, 45, 15), false);
            r = new Rectangle(150, 100, 40, 10);
            score = 0;
            enemylist = new List<EnemyMissle>(15);
            timer = 0;
            missileCount = 30;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            font1 = Content.Load<SpriteFont>("SpriteFont1");
            font2 = Content.Load<SpriteFont>("SpriteFont2");
            background = Content.Load<Texture2D>("Background");
            destroyedCity = Content.Load<Texture2D>("Destroyed City");
            undestroyedCity = Content.Load<Texture2D>("Undestroyed City");
            EnemyMissle.rocketpic = Content.Load<Texture2D>("misslepic");
            EnemyMissle.explosionpic = Content.Load<Texture2D>("redexplosion");
            EnemyMissle.trailpic = Content.Load<Texture2D>("trail");
            PlayerMissile.rocketpic = Content.Load<Texture2D>("PlayerMissile");
            PlayerMissile.explosionpic = Content.Load<Texture2D>("yellowexplosion");
            missileFiringClip = Content.Load<SoundEffect>("Missile firing sound");
            explosionClip = Content.Load<SoundEffect>("Explosion sound");
            rewardClip = Content.Load<SoundEffect>("Reward sound");
            //rocket = Content.Load<Texture2D>("misslepic");
            //explos = Content.Load<Texture2D>("redexplosion");
            t = new Texture2D(GraphicsDevice, 1, 1);
            t.SetData(new Color[] { Color.White });
            plane = new Crosshair(r, t);
            int num = 10; 
            int numY = 402;
            for (int i = 0; i < 3; i++)
            {
                if (i == 1)
                {
                    numY = 405;
                }
                else
                {
                    numY = 402;
                }
                List<PlayerMissile> list = new List<PlayerMissile>();
                sites.Add(new MissileSite(new Rectangle(num, numY, 10, 10), t));
                //for (int j = 0; j < 10; j++)
                //{
                //    sites[i].addMissile(new PlayerMissile(0,0,GraphicsDevice.Viewport.Width, new Rectangle(sites[i].rect.X + sites[i].rect.Width / 2 - 20, sites[i].rect.Y - 20, 20, 20), rocket, explos));
                //}
                num += 385;
            }
            EnemyMissle.trailorigin = new Vector2(EnemyMissle.trailpic.Width, EnemyMissle.trailpic.Height / 2);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
            MouseState newMouse = Mouse.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // changed to case switch statement to make eeasier to read
            switch (gameState)
            {
                case GameState.StartScreen:
                    if (kb.IsKeyDown(Keys.Space))
                    {

                        //spawn enemy missles headed towards cities
                        for (int i = 0; i < cities.Length; i++)
                        {
                            enemylist.Add(new EnemyMissle(cities[i].rectangle.X + cities[i].rectangle.Width / 2, cities[i].rectangle.Y, w));
                        }

                        //spawn enemy missles headed towards missile sites
                        for (int i = 0; i < sites.Count; i++)
                        {
                            enemylist.Add(new EnemyMissle(sites[i].rect.X + sites[i].rect.Width / 2, sites[i].rect.Y, w));
                        }

                        //advance gamestate
                        gameState = GameState.GameplayScreen;
                    }

                    break;
                case GameState.GameplayScreen:
                    bool allDestroyed = true;
                    for (int i = 0; i < 6; i++)
                    {
                        if (!cities[i].isDestroyed)
                        {
                            allDestroyed = false;
                            break;
                        }
                    }
                    if (allDestroyed)
                    {
                        timer++;
                        if (timer == 1)
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                if (!cities[i].isDestroyed)
                                {
                                    score += 1000;
                                }
                            }
                            score += missileCount * 50;
                        }
                        if (timer == 120)
                        {                            
                            gameState = GameState.GameOverScreen;
                        }
                    }
                    if (enemylist.Count == 0)
                    {
                        timer++;
                        if (timer == 1)
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                if (!cities[i].isDestroyed)
                                {
                                    score += 1000;
                                }
                            }
                            score += missileCount * 50;
                        }
                        if (timer == 120)
                        {
                            gameState = GameState.GameOverScreen;
                        }
                    }
                    //plane.update(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, newMouse.X, newMouse.Y);
                    MissileSite s = closestSite(newMouse.X, newMouse.Y);
                    plane.update(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, newMouse.X, newMouse.Y);
                    if (newMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released && s != null && s.missileIndex < 10)
                    {
                        if (s.missileIndex < 10)
                        {
                            missileFiringClip.Play();
                            s.addMissile(new PlayerMissile(plane.rect.X + plane.rect.Width / 2, plane.rect.Y + plane.rect.Height / 2, GraphicsDevice.Viewport.Width, new Rectangle(s.rect.X + s.rect.Width / 2, s.rect.Y - 10, 20, 20)));
                            s.missileIndex++;
                            s.activated = true;
                        }
                        if(s.missileIndex == 10)
                        {
                            s.drained = true;
                        }
                    }
                    //if (newMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released && closestSite(newMouse.X, newMouse.Y).missileIndex < 10)
                    //{
                    //    s.missileIndex++;
                    //    s.activated = true;
                    //    if (s.missileIndex < 10)
                    //    {
                    //        s.addMissile(new PlayerMissile(plane.rect.X + plane.rect.Width / 2, plane.rect.Y + plane.rect.Height / 2, GraphicsDevice.Viewport.Width, new Rectangle(s.rect.X + s.rect.Width / 2 - 20, s.rect.Y - 20, 20, 20)));
                    //    }
                    //}
                    for (int i = 0; i < sites.Count; i++)
                    {
                        if (sites[i].activated)
                        {
                            //sites[i].getMissile(sites[i].missileIndex - 1).xT = plane.rect.X;
                            //sites[i].getMissile(sites[i].missileIndex - 1).yT = plane.rect.Y;
                            missileCount--;
                            sites[i].activated = false;
                        }

                        //player missile update method is here named firemissile for some reason
                        sites[i].fireMissile();
                        //sites[i].fireMissile();
                    }
                    oldMouse = newMouse;
                    //update enemymissiles
                    //check for exploded player missiles, kinda convulted because missile list is within a seperate class
                    for(int i = 0; i < sites.Count; i++)
                    {
                        for (int j = 0; j < sites[i].missiles.Count; j++)
                        {
                            if (sites[i].missiles[j].exploded)
                            {
                                //check with enemy missiles
                                for (int k = 0; k < enemylist.Count; k++)
                                {
                                    if (sites[i].missiles[j].circleintersects(enemylist[k]))
                                    {
                                        enemylist[k].startexploding();
                                        explosionClip.Play();
                                    }
                                }

                                //check with airplanes

                            }
                        }
                    }
                    for(int i = 0; i < sites.Count; i++)
                    {
                        for(int j = 0; j < enemylist.Count; j++)
                        {
                            if (enemylist[j].intersects(sites[i].rect))
                            {
                                missileCount -= (10 - sites[i].missileIndex);
                                sites.RemoveAt(i);
                                
                                i--;
                                
                                break;
                            }
                        }
                    }
                    oldMouse = newMouse;

                    //update enemymissiles
                    for (int i = 0; i < enemylist.Count; i++)
                    {
                        //delete missle when explosion size==0, which is returned by update method
                        if (enemylist[i].update())
                        {
                            enemylist.RemoveAt(i);
                        }
                        else
                        {
                            //check collision with cities
                            Boolean[] deadcities = enemylist[i].intersects(cities);
                            //change city to be destroyed
                            for (int j = 0; j < cities.Length; j++)
                            {
                                if (deadcities[j])
                                {
                                    cities[j].isDestroyed = true;
                                }
                            }

                            //check collision with exploded missiles
                            if (enemylist[i].exploded)
                            {
                                for (int j = 0; j < enemylist.Count; j++)
                                {
                                    if (enemylist[i].circleintersects(enemylist[j].drect))
                                    {
                                        enemylist[j].startexploding();
                                    }
                                }
                            }
                        }
                    }
                    base.Update(gameTime);
                    break;
                case GameState.GameOverScreen:
                    if (kb.IsKeyDown(Keys.R))
                    {
                        Initialize();
                        for (int i = 0; i < cities.Length; i++)
                        {
                            enemylist.Add(new EnemyMissle(cities[i].rectangle.X + cities[i].rectangle.Width / 2, cities[i].rectangle.Y, w));
                        }
                        for (int i = 0; i < sites.Count; i++)
                        {
                            enemylist.Add(new EnemyMissle(sites[i].rect.X + sites[i].rect.Width / 2, sites[i].rect.Y, w));
                        }
                        gameState = GameState.GameplayScreen;
                    }
                    break;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            switch (gameState)
            {
                case GameState.StartScreen:
                    spriteBatch.DrawString(font1, "Welcome to", new Vector2(200, 60), Color.Red);
                    spriteBatch.DrawString(font1, "Missile Command!", new Vector2(90, 140), Color.Red);
                    spriteBatch.DrawString(font2, "Instructions: Use the mouse to move the crosshair", new Vector2(110, 250), Color.Red);
                    spriteBatch.DrawString(font2, "  and press the 1, 2, and 3 number keys to fire  ", new Vector2(110, 275), Color.Red);
                    spriteBatch.DrawString(font2, " missiles from the respective missile batteries. ", new Vector2(110, 300), Color.Red);
                    spriteBatch.DrawString(font2, "          Press the space key to start.          ", new Vector2(110, 360), Color.Red);
                    break;
                case GameState.GameplayScreen:
                    spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);
                    spriteBatch.DrawString(font2, "Missile Count: " + missileCount + "", new Vector2(0, 0), Color.Red);
                    //missile sites and player missiles
                    for (int i = 0; i < sites.Count; i++)
                    {
                        sites[i].Draw(spriteBatch, gameTime);
                        for (int j = 0; j < sites[i].missiles.Count; j++)
                        {
                            sites[i].getMissile(j).Draw(spriteBatch, gameTime);
                        }
                    }
                    //city stuff
                    for (int i = 0; i < 6; i++)
                    {
                        if (cities[i].isDestroyed)
                        {
                            spriteBatch.Draw(destroyedCity, cities[i].rectangle, Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(undestroyedCity, cities[i].rectangle, Color.White);
                        }
                    }

                    //draw enemy missles
                    for (int i = 0; i < enemylist.Count; i++)
                    {
                        if (enemylist[i].traildrect.Width > 0)
                        {
                            spriteBatch.Draw(EnemyMissle.trailpic, enemylist[i].traildrect, null, Color.White, enemylist[i].angle, EnemyMissle.trailorigin, SpriteEffects.None, 0);
                        }
                        spriteBatch.Draw(enemylist[i].pic, enemylist[i].drect, EnemyMissle.srect, Color.White, enemylist[i].angle, EnemyMissle.origin, SpriteEffects.None, 0);
                    }

                    plane.Draw(spriteBatch, gameTime);
                    spriteBatch.DrawString(font2, "Score: " + score, new Vector2(600, 0), Color.Red);
                    break;
                case GameState.GameOverScreen:
                    spriteBatch.DrawString(font2, "End of Round", new Vector2(600, 200), Color.Red);
                    spriteBatch.DrawString(font2, "Score: " + score, new Vector2(600, 0), Color.Red);
                    spriteBatch.DrawString(font1, "Press R to restart", new Vector2(0, 100), Color.Red);
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }




        MissileSite closestSite(int x, int y)
        {
            int num = 0;
            for (int i = 0; i < sites.Count; i++)
            {
                if (!sites[i].drained)
                {
                    num++;
                }
            }
            for (int i = 0; i < sites.Count; i++)
            {
                if (!sites[i].drained)
                {
                    sites[i].distanceFromM = Math.Sqrt(Math.Pow(x - (sites[i].rect.X), 2) + Math.Pow(y - (sites[i].rect.Y), 2));
                }
            }
            int indexOfL = -1;
            for (int i = 0; i < sites.Count; i++)
            {
                if (!sites[i].drained)
                {
                    indexOfL = i;
                    break;
                }
            }
            if (indexOfL != -1)
            {
                for (int i = indexOfL; i < sites.Count; i++)
                {
                    if (!sites[i].drained && sites[i].distanceFromM < sites[indexOfL].distanceFromM)
                    {
                        indexOfL = i;
                    }
                }
                return sites[indexOfL];
            }
            return null;
        }
    }
}