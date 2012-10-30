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
        Car car2;

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

            car = new Car(new Vector2(950.0f, 600.0f), Content.Load<Texture2D>("Car"), 100, 100, 0, 1000.0f, currentTrack, 1);
            currentTrack.AddCar(car);
            car2 = new Car(new Vector2(950.0f, 650.0f), Content.Load<Texture2D>("Car2"), 100, 100, 0, 1000.0f, currentTrack, 2);
            currentTrack.AddCar(car2);
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
