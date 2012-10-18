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

namespace GameProject
{
    class Menu
    {
        protected Game1 game;

        int previousWindowWidth = 0, previousWindowHeight = 0;

        Texture2D background;
        Vector2 menuPosition;

        public List<Button> buttons = new List<Button>();

        Point mousePosition;
        bool  mouse1_pressed;
        bool  mouse1_once;

        MouseState oldMouseState;

        public bool enabled;

        public Menu(ContentManager Content, Game1 game, GraphicsDeviceManager graphics)
            : base()
        {
            this.game = game;
            this.background = null;
            this.enabled = true;
        }
        public Menu(ContentManager Content, String background, Game1 game, GraphicsDeviceManager graphics)
            : this(Content, game, graphics)
        {
            if (background != null)
                this.background = Content.Load<Texture2D>(background);

            else
                this.background = null;
        }
        


        public void Update(GameTime gameTime, MouseState mouseState, GraphicsDeviceManager graphics)
        {
            getInput(mouseState);

            updateButtons();
        }

        public void getInput(MouseState mouseState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                //      pressed
                if (mouse1_once == false && mouse1_pressed == false) mouse1_once = true;
                else mouse1_once = false;

                mouse1_pressed = true;
            }
            else if (oldMouseState.LeftButton == ButtonState.Pressed)
            {
                //      released
                mouse1_once = false;
                mouse1_pressed = false;
            }
  
            mousePosition = new Point(mouseState.X, mouseState.Y);

            oldMouseState = mouseState;
        }

        private void updateButtons()
        {
            foreach (Button button in buttons)
                button.Update(mousePosition, mouse1_pressed);
        }

        /// <summary>
        /// Gets called whenever the window is resized in Game1.cs
        /// </summary>
        public void repositionMenu(int windowWidth, int windowHeight)
        {
            if (previousWindowWidth  != windowWidth ||
                previousWindowHeight != windowHeight)
            {
                int center = (windowWidth / 2); // Get the center of the window.
                int Ycenter = (windowHeight / 2);
                int centerButtons = center - (buttons[0].Width / 2);


                int centerBackground;
                int YcenterBackground;

                if (background != null)
                {
                    centerBackground = center - (background.Width / 2);
                    YcenterBackground = Ycenter - (background.Height / 2);
                }
                else
                {
                    centerBackground = centerButtons;
                    YcenterBackground = Ycenter - (buttons[0].Height / 2);
                }

                menuPosition = new Vector2(centerBackground, YcenterBackground);

                for (int i = 0; i < buttons.Count; i++)
                {
                    Button button = buttons[i];

                    float vertical_spacing = 40.0f;

                    if (i == 0)
                        button.Position = new Vector2(centerButtons, menuPosition.Y + 75.0f);
                    else
                    {
                        if(button.isVisible)
                            button.Position = new Vector2(centerButtons, buttons[i - 1].buttonRectangle.Bottom + vertical_spacing);
                        else
                            button.Position = new Vector2(centerButtons, buttons[i - 1].buttonRectangle.Top);
                    }
                }

                Console.WriteLine("menu repositioned!");
            }

            previousWindowWidth = windowWidth;
            previousWindowHeight = windowHeight;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(background != null)
                spriteBatch.Draw(background, menuPosition, null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
 
            foreach (Button button in buttons)
                button.Draw(spriteBatch);
        }


    }
}
