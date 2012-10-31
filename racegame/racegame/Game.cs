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
    public enum GameState
    {
        Intro,
        Menu,
        GameSP,
        GameMP,
        HowToPlay,
        Exit
    }

    public class Game : Microsoft.Xna.Framework.Game
    {
        #region Variables

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState keyboardState;
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        Intro intro;
        Texture2D HowToPlay;
        MainMenu mainMenu;

        Track currentTrack;
        Car car;
        Car car2;

        public GameState currentGameState = GameState.Intro;

        #endregion


        #region Loading

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;

            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the intro, menu etc.
            intro = new Intro(Content);
            HowToPlay = Content.Load<Texture2D>("HowToPlay");
            mainMenu = new MainMenu(Content, graphics.GraphicsDevice);
            
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
            currentKeyboardState = Keyboard.GetState();

            // Here is the order of the GameStates declared + what happens when pressing which button or clicking the mouse.
            switch (currentGameState)
            {
                case GameState.Intro:
                    intro.Update(gameTime);
                    if (currentKeyboardState.IsKeyDown(Keys.Escape)) currentGameState = GameState.Menu;
                    if (gameTime.TotalGameTime.Seconds >= 5) currentGameState = GameState.Menu;
                    break;

                case GameState.Menu:
                    mainMenu.Update(gameTime, currentKeyboardState, previousKeyboardState, this);
                    break;

                case GameState.GameSP:
                    if (currentKeyboardState.IsKeyDown(Keys.Back)) currentGameState = GameState.Menu;
                    currentTrack.Update(gameTime);
                    break;

                case GameState.GameMP:
                    if (currentKeyboardState.IsKeyDown(Keys.Back)) currentGameState = GameState.Menu;
                    currentTrack.Update(gameTime);
                    break;

                case GameState.HowToPlay:
                    if (currentKeyboardState.IsKeyDown(Keys.Back)) currentGameState = GameState.Menu;
                    break;

                case GameState.Exit:
                    break;
            }

            // Store the current keyboardstate
            previousKeyboardState = currentKeyboardState;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepPink);

            spriteBatch.Begin();
           
            switch (currentGameState)
            {
                case GameState.Intro:
                    intro.Draw(gameTime, spriteBatch);
                    break;

                case GameState.Menu:
                    mainMenu.Draw(spriteBatch);
                    break;

                case GameState.GameSP:
                    currentTrack.Draw(spriteBatch);
                    if (currentKeyboardState.IsKeyDown(Keys.Tab)) spriteBatch.Draw(HowToPlay, new Vector2(180.0f, 187.5f), Color.White);
                    break;

                case GameState.GameMP:      
                    currentTrack.Draw(spriteBatch);
                    if (currentKeyboardState.IsKeyDown(Keys.Tab)) spriteBatch.Draw(HowToPlay, new Vector2(180.0f, 187.5f), Color.White);
                    break;

                case GameState.HowToPlay:
                    spriteBatch.Draw(HowToPlay, new Vector2(180.0f, 187.5f), Color.White);
                    break;

                case GameState.Exit:
                    break;
            }

            spriteBatch.End();
        }

        #endregion
    }
}
