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
    class Obstacle
    {
        public Vector2 position;
        protected Texture2D texture; // Or Animation?
        public int Width;
        public int Height;

        /// <summary>
        /// This is the bounding rectangle of the object located somehwere on the Track.
        /// </summary>
        public Rectangle BoundingRectangle
        {
            get
            {
                // Need to figure out what to do with rotated Objects (like a car for example)
                return new Rectangle((int)position.X, (int)position.Y, Width, Height);
            }
        }

        /// <summary>
        /// This is the Rectangle with the added adjustements fix the origin of rotating (used for the collision)
        /// </summary>
        public Rectangle RealBoundingRectangle
        {
            get
            {
                return new Rectangle((int)position.X + Width / 2, (int)position.Y + Height / 2, Width, Height);
            }
        }

        public Vector2 Origin
        {
            get { return new Vector2((int)(Width / 2), (int)(Height / 2)); }
        }



        public Obstacle(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.texture = texture;

            Width = texture.Width;
            Height = texture.Height;
        }
        public Obstacle(Rectangle rectangle)
        {
            this.position = new Vector2(rectangle.X, rectangle.Y);
            this.texture = null; // Maak hier nieuwe texture2D aan met de width en height van de retangle.

            Width = rectangle.Width;
            Height = rectangle.Height;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }  
}