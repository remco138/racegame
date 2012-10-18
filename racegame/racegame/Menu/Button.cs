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
    enum State
    {
        Hover,
        Up,
        Released,
        Down
    }

    class Button
    {
        public Vector2 Position
        {
            get;
            set;
        }

        String label;
        Texture2D textureNormal, textureHover, texturePressed;
        State state;

        public event EventHandler ButtonClicked;

        bool mouse1_was_pressed;

        public bool isEnabled { get; set; }
        public bool isVisible { get; set; }

        public int Width
        {
            get { return textureNormal.Width; }
        }
        public int Height
        {
            get { return textureNormal.Height; }
        }
        public Rectangle buttonRectangle
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Width, Height); }
        }

        private ContentManager Content;



        public Button(ContentManager Content, String label, String textureFolder)
            : base()
        {
            this.Content = Content;
            this.label = label;
            this.isVisible = true;

            this.isEnabled = true;
            this.Position = new Vector2(0.1f, 0.1f);

            this.ButtonClicked = empty;
            getTextures(textureFolder);
        }

        public Button(ContentManager Content, String label, String textureFolder, EventHandler evt)
            : this(Content, label, textureFolder)
        {
            this.ButtonClicked = evt;
        }

        public Button(ContentManager Content, String label, String textureFolder, Vector2 position, bool enabled)
            : this(Content, label, textureFolder)
        {
            this.isEnabled = enabled;
            this.Position = position;
        }

        public Button(ContentManager Content, String label, String textureFolder, Vector2 position, bool enabled, EventHandler evt)
            : this(Content, label, textureFolder, position, enabled)
        {
            this.ButtonClicked = evt;
        }



        public void getTextures(String folder)
        {
            this.textureNormal = Content.Load<Texture2D>(folder + "/normal");
            this.textureHover = Content.Load<Texture2D>(folder + "/hover");
            this.texturePressed = Content.Load<Texture2D>(folder + "/pressed");
        }

        public void Update(Point mousePosition, bool mouse1Pressed)
        {
            if (hit_button(buttonRectangle, mousePosition))
            {
                state = State.Hover;

                if (mouse1Pressed)
                {
                    state = State.Down;
                }
                else if (mouse1_was_pressed)
                {
                    ButtonClicked(this, new EventArgs());
                    state = State.Released;
                }
            }
            else
                state = State.Up;

            mouse1_was_pressed = mouse1Pressed;
        }

        private Boolean hit_button(Rectangle buttonRectangle, Point mousePos)
        {
            return (mousePos.X >= buttonRectangle.Left &&
                    mousePos.X <= buttonRectangle.Right &&
                    mousePos.Y >= buttonRectangle.Top &&
                    mousePos.Y <= buttonRectangle.Bottom);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
                switch (state)
                {
                    case State.Up:
                        spriteBatch.Draw(textureNormal, Position, null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                        break;

                    case State.Down:
                        spriteBatch.Draw(texturePressed, Position, null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                        break;

                    case State.Hover:
                        spriteBatch.Draw(textureHover, Position, null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                        break;

                    default:
                        spriteBatch.Draw(textureNormal, Position, null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                        break;
                }
            }
        }

        private void empty(object sender, EventArgs e)
        {
            // No action.

            // Is used for buttons without an eventHandler.
        }
    }
}
