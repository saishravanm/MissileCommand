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


namespace Missile_Command
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
            enemylist = new List<EnemyMissle>(10);
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
            t = new Texture2D(GraphicsDevice, 1, 1);
            t.SetData(new Color[] { Color.White });
            plane = new Crosshair(r, t);
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
            switch (gameState) {
                case GameState.StartScreen:
                    if (kb.IsKeyDown(Keys.Space)) {
                        for (int i = 0; i < 10; i++)
                        {
                            enemylist.Add(new EnemyMissle(70*i+100, w-50, w));
                        }

                        //advance gamestate
                        gameState = GameState.GameplayScreen;
                    }

                    break;
                case GameState.GameplayScreen:
                    plane.update(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, newMouse.X, newMouse.Y);

                    //update enemymissiles
                    for (int i = 0; i < enemylist.Count; i++) {

                        Boolean removethis=enemylist[i].update();
                        //delete missle when explosion size==0, which is returned by update method
                        if (removethis)
                        {
                            enemylist.RemoveAt(i);
                        }
                        else {
                            Boolean[] deadcities = enemylist[i].intersects(cities);
                            //change city to be destroyed
                            for (int j = 0; j < cities.Length; j++) {
                                if (deadcities[j]) {
                                    cities[j].isDestroyed = true;
                                }
                            }
                        }
                    }


                    break;

            }

            base.Update(gameTime);
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
            switch (gameState) {
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
                        spriteBatch.Draw(enemylist[i].pic, enemylist[i].drect, EnemyMissle.srect, Color.White, enemylist[i].angle, EnemyMissle.origin, SpriteEffects.None, 0);
                    }

                    plane.Draw(spriteBatch, gameTime);
                    spriteBatch.DrawString(font2, "Score: " + score, new Vector2(600, 0), Color.Red);
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}