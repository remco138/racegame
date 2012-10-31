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
    class Button
    {
        // Creating the Texture's.
        Texture2D texture;

        // Declaring a variable for the Vector2 and the Rectangle.
        Vector2 position;
        Vector2 size;

        public Rectangle Rectangle
        {
            get { return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y); }
        }

        // Declaring a color.
        Color color = new Color(255, 255, 255, 255);

        /// <summary>
        /// Button will be used for creating the Buttons and give them a size.
        /// </summary>
        public Button(Texture2D newTexture, Vector2 size, Vector2 position)
        {
            texture = newTexture;
            this.size = size;
            this.position = position;
        }

        /// <summary>
        /// Update will be used for the fading of the Button images.
        /// </summary>
        /// <param name="mouse"></param>
        public void Update()
        {
            // De onderstaande regel is niet nodig:  
            //rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }

        /// <summary>
        /// setPosition will be used in Game for setting the position of the Buttons.
        /// </summary>
        /// <param name="newPosition"></param>
        public void setPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Rectangle, color);
        }
    }
}
