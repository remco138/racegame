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

namespace RetroRacer
{
    public enum GameState
    {
        Intro,
        FirstMenu,
        MainMenu,
        GameSP,
        GameMP,
        HowToPlay,
        WinnerScreen,
        Exit
    }

    public class Game : Microsoft.Xna.Framework.Game
    {
        #region Variables

        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        Intro intro;
        Texture2D HowToPlay;
        FirstMenu firstMenu;
        MainMenu mainMenu;


        Texture2D menuBackground;

        Track currentTrack;
        Hud hud;

        public GameState currentGameState = GameState.Intro;

        #endregion


        #region Loading

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Load the intro, menu etc.
            intro = new Intro(Content);
            hud = new Hud(Content);
            HowToPlay = Content.Load<Texture2D>("HowToPlay");
            firstMenu = new FirstMenu(Content, graphics.GraphicsDevice);
            mainMenu = new MainMenu(Content, graphics.GraphicsDevice);

            menuBackground = Content.Load<Texture2D>("Menu/Menu_BG");
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
                    if (currentKeyboardState.IsKeyDown(Keys.Escape)) currentGameState = GameState.FirstMenu;
                    if (gameTime.TotalGameTime.Seconds >= 5) currentGameState = GameState.FirstMenu;
                    break;

                case GameState.FirstMenu:
                    firstMenu.Update(gameTime, currentKeyboardState, previousKeyboardState, this);
                    break;

                case GameState.MainMenu:
                    mainMenu.Update(gameTime, currentKeyboardState, previousKeyboardState, this);
                    break;

                case GameState.GameSP:
                    if (currentKeyboardState.IsKeyDown(Keys.Back)) currentGameState = GameState.MainMenu;
                    currentTrack.Update(gameTime);
                    break;

                case GameState.GameMP:
                    if (currentKeyboardState.IsKeyDown(Keys.Back)) currentGameState = GameState.MainMenu;
                    currentTrack.Update(gameTime);
                    break;

                case GameState.HowToPlay:
                    if (currentKeyboardState.IsKeyDown(Keys.Back)) currentGameState = GameState.MainMenu;
                    break;

                case GameState.Exit:
                    break;
            }
            // Store the current keyboardstate
            previousKeyboardState = currentKeyboardState;
        }

        public void startGame(int trackNumber, int amountOfPlayers)
        {
            currentTrack = new Track(Content.Load<Texture2D>("Tracks/" + trackNumber), Content, amountOfPlayers, this);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
           
            switch (currentGameState)
            {
                case GameState.Intro:
                    intro.Draw(gameTime, spriteBatch);
                    break;

                case GameState.FirstMenu:
                    firstMenu.Draw(spriteBatch);
                    break;

                case GameState.MainMenu:
                    mainMenu.Draw(spriteBatch);
                    break;

                case GameState.GameSP:
                    currentTrack.Draw(spriteBatch);
                    hud.draw(spriteBatch, currentTrack);
                    if (currentKeyboardState.IsKeyDown(Keys.Tab)) spriteBatch.Draw(HowToPlay, new Vector2(410.0f, 187.0f), Color.White);
                    break;

                case GameState.GameMP:
                    currentTrack.Draw(spriteBatch);
                    hud.draw(spriteBatch, currentTrack);
                    if (currentKeyboardState.IsKeyDown(Keys.Tab)) spriteBatch.Draw(HowToPlay, new Vector2(410.0f, 187.0f), Color.White);
                    break;

                case GameState.HowToPlay:
                    spriteBatch.Draw(menuBackground, new Vector2(0.0f, 0.0f), Color.White);
                    spriteBatch.Draw(HowToPlay, new Vector2(410.0f, 187.0f), Color.White);
                    break;

                case GameState.Exit:
                    break;
            }

            spriteBatch.End();
        }

        #endregion
    }
}
