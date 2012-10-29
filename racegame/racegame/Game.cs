//TEST! :D
//nog 1tje!!!
//3RD
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

namespace racegame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        #region Variables

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState keyboardState;
 
        Track currentTrack;
        Car car;

        #endregion


        #region Loading

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            currentTrack = new Track(Content.Load<Texture2D>("Tracks/1"), Content);
            //new MovableObject(new Vector2(0.0f,0.5f), Content.Load<Texture2D>("Crosshair"), new Vector2(5.0f,5.0f)),
            //new MovableObject(new Vector2(5.0f,0.5f), Content.Load<Texture2D>("Crosshair"), new Vector2(15.0f,15.0f)),
            car = new Car(new Vector2(10.0f, 10.0f), Content.Load<Texture2D>("Car"), 100, 100, 0, 1000.0f, 0.0f, currentTrack) { Velocity = new Vector2(0.0f, 0.0f) };
            currentTrack.AddObject(car);
        }

        #endregion


        #region Update & Draw

        protected override void Update(GameTime gameTime)
        {
            GetInput();
            currentTrack.Update(gameTime);

            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) this.Exit();
            base.Update(gameTime);
        }

        private void GetInput()
        {
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.A))
            {
                
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepPink);

            spriteBatch.Begin();
            currentTrack.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}
