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
        City[] cities;
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
            gameState = GameState.StartScreen;
            cities = new City[6];
            cities[0] = new City(new Rectangle(80, 415, 45, 15), false);
            cities[1] = new City(new Rectangle(170, 415, 45, 15), false);
            cities[2] = new City(new Rectangle(260, 415, 45, 15), false);
            cities[3] = new City(new Rectangle(495, 415, 45, 15), false);
            cities[4] = new City(new Rectangle(585, 415, 45, 15), false);
            cities[5] = new City(new Rectangle(675, 415, 45, 15), false);
            score = 0;
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

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            if (gameState == GameState.StartScreen && kb.IsKeyDown(Keys.Space))
            {
                gameState = GameState.GameplayScreen;
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
            if (gameState == GameState.StartScreen)
            {
                spriteBatch.DrawString(font1, "Welcome to", new Vector2(200, 60), Color.Red);
                spriteBatch.DrawString(font1, "Missile Command!", new Vector2(90, 140), Color.Red);
                spriteBatch.DrawString(font2, "Instructions: Use the mouse to move the crosshair", new Vector2(110, 250), Color.Red);
                spriteBatch.DrawString(font2, "  and press the 1, 2, and 3 number keys to fire  ", new Vector2(110, 275), Color.Red);
                spriteBatch.DrawString(font2, " missiles from the respective missile batteries. ", new Vector2(110, 300), Color.Red);
                spriteBatch.DrawString(font2, "          Press the space key to start.          ", new Vector2(110, 360), Color.Red);
            }
            else if (gameState == GameState.GameplayScreen)
            {
                spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);
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
                spriteBatch.DrawString(font2, "Score: " + score, new Vector2(600, 0), Color.Red);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}