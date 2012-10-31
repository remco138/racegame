using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace racegame
{
    public class MainMenu
    {
        Texture2D menuBackground;

        Button buttonSP;
        Button buttonMP;
        Button buttonHTP;
        Button buttonE;

        Texture2D CarCursor1;
        Texture2D CarCursor2;

        float carCursorP1;// Variable for the 'Cursor' position.
        float carCursorP2;// Variable for the 'Cursor' position.
        int counter = 1;  // Variable for the Menu Control

        /// <summary>
        /// The Constructor of MainMenu, this code gets executed at the start of making a new object
        /// </summary>
        public MainMenu(ContentManager Content, GraphicsDevice graphics)
        {
            menuBackground = Content.Load<Texture2D>("MainMenu/Menu_BG");


            CarCursor1 = Content.Load<Texture2D>("MainMenu/Car1Right");
            CarCursor2 = Content.Load<Texture2D>("MainMenu/Car2Left");

            // Creating and positioning the buttons.
            //
            Vector2 buttonSize = new Vector2(graphics.Viewport.Width / 4, graphics.Viewport.Height / 10);

            buttonSP = new Button(Content.Load<Texture2D>("MainMenu/Button_SP"), buttonSize, new Vector2(480, 230));
            buttonMP = new Button(Content.Load<Texture2D>("MainMenu/Button_MP"), buttonSize, new Vector2(480, 320));
            buttonHTP = new Button(Content.Load<Texture2D>("MainMenu/Button_HTP"), buttonSize, new Vector2(480, 410));
            buttonE = new Button(Content.Load<Texture2D>("MainMenu/Button_E"), buttonSize, new Vector2(480, 500));
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, Game game)
        {
            // Menu Control with Arrows
            //
            if (currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down))
            {
                counter += 1;
            }
            if (counter > 4)
            {
                counter = 1;
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up))
            {
                counter -= 1;
            }
            if (counter < 1)
            {
                counter = 4;
            }
            if (counter == 1) carCursorP1 = 252;
            if (counter == 2) carCursorP1 = 345;
            if (counter == 3) carCursorP1 = 434;
            if (counter == 4) carCursorP1 = 520;
            if (counter == 1) carCursorP2 = 252;
            if (counter == 2) carCursorP2 = 345;
            if (counter == 3) carCursorP2 = 434;
            if (counter == 4) carCursorP2 = 520;

            // Get and respond to user input
            //
            if (currentKeyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter) && counter == 1)
            {
                game.startGame(1, 1);
                game.currentGameState = GameState.GameSP;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Enter) && counter == 2)
            {
                game.startGame(1, 2);
                game.currentGameState = GameState.GameMP;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Enter) && counter == 3) game.currentGameState = GameState.HowToPlay;
            if (currentKeyboardState.IsKeyDown(Keys.Enter) && counter == 4) game.Exit();

            if ((currentKeyboardState.IsKeyDown(Keys.Escape) && (previousKeyboardState.IsKeyUp(Keys.Escape)))) game.Exit();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(menuBackground, new Vector2(0.0f, 0.0f), Color.White);
            spriteBatch.Draw(CarCursor1, new Vector2(443.0f, carCursorP1), Color.White);
            spriteBatch.Draw(CarCursor2, new Vector2(805.0f, carCursorP2), Color.White);

            buttonSP.Draw(spriteBatch);
            buttonMP.Draw(spriteBatch);
            buttonHTP.Draw(spriteBatch);
            buttonE.Draw(spriteBatch);
        }
    }
}