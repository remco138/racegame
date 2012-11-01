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
    class FirstMenu
    {
        Texture2D menuBackground;

        Button buttonFS;
        Button buttonSS;

        Texture2D CarCursor1;
        Texture2D CarCursor2;


        float carCursorP1;// Variable for the 'Cursor' position.
        float carCursorP2;// Variable for the 'Cursor' position.
        int counter = 1;  // Variable for the Menu Control

        /// <summary>
        /// The Constructor of MainMenu, this code gets executed at the start of making a new object
        /// </summary>
        public FirstMenu(ContentManager Content, GraphicsDevice graphics)
        {
            menuBackground = Content.Load<Texture2D>("Menu/Menu_BG");


            CarCursor1 = Content.Load<Texture2D>("Menu/Car1Right");
            CarCursor2 = Content.Load<Texture2D>("Menu/Car2Left");
            // Creating and positioning the buttons.
            //
            Vector2 buttonSize = new Vector2(graphics.Viewport.Width / 4, graphics.Viewport.Height / 10);

            buttonFS = new Button(Content.Load<Texture2D>("Menu/Button_SS"), buttonSize, new Vector2(480, 230));
            buttonSS = new Button(Content.Load<Texture2D>("Menu/Button_FS"), buttonSize, new Vector2(480, 320));

        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, Game game)
        {
            // Menu Control with Arrows
            //
            if (currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down))
            {
                counter += 1;
            }
            if (counter > 2)
            {
                counter = 1;
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up))
            {
                counter -= 1;
            }
            if (counter < 1)
            {
                counter = 2;
            }
            if (counter == 1) carCursorP1 = 252;
            if (counter == 2) carCursorP1 = 345;
            if (counter == 1) carCursorP2 = 252;
            if (counter == 2) carCursorP2 = 345;


            // Get and respond to user input
            //
            if (currentKeyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter) && counter == 1)
            {
                game.currentGameState = GameState.MainMenu;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Enter) && counter == 2)
            {
                game.graphics.IsFullScreen = true;
                game.graphics.ApplyChanges();
                game.currentGameState = GameState.MainMenu;
            }

            if ((currentKeyboardState.IsKeyDown(Keys.Escape) && (previousKeyboardState.IsKeyUp(Keys.Escape)))) game.Exit();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(menuBackground, new Vector2(0.0f, 0.0f), Color.White);
            spriteBatch.Draw(CarCursor1, new Vector2(443.0f, carCursorP1), Color.White);
            spriteBatch.Draw(CarCursor2, new Vector2(805.0f, carCursorP2), Color.White);

            buttonSS.Draw(spriteBatch);
            buttonFS.Draw(spriteBatch);

        }
    }
}