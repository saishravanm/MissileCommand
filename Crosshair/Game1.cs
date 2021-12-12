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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D t;
        Rectangle r;
        Crosshair plane;
        KeyboardState oldKb = Keyboard.GetState();
        MouseState oldMouse = Mouse.GetState();
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
            r = new Rectangle(150, 100, 40, 10);
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
            t = new Texture2D(GraphicsDevice, 1, 1);
            t.SetData(new Color[] { Color.White });
            plane = new Crosshair(r, t);
            // TODO: use this.Content to load your game content here
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
            KeyboardState newKb = Keyboard.GetState();
            MouseState newMouse = Mouse.GetState();
            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();
            //if(newKb.IsKeyDown(Keys.Up) && !oldKb.IsKeyDown(Keys.Up))
            //{
            //    plane.update(false,false, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            //}
            //if (newKb.IsKeyDown(Keys.Down) && !oldKb.IsKeyDown(Keys.Down))
            //{
            //    plane.update(false, true, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            //}
            //if (newKb.IsKeyDown(Keys.Left) && !oldKb.IsKeyDown(Keys.Left))
            //{
            //    plane.update(true, false, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            //}
            //if (newKb.IsKeyDown(Keys.Right) && !oldKb.IsKeyDown(Keys.Right))
            //{
            //    plane.update(true, true, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            //}
            //oldKb = newKb;
            // TODO: Add your update logic here
            plane.update(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, newMouse.X, newMouse.Y);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            plane.Draw(spriteBatch, gameTime);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
