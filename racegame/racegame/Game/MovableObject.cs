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
    class Obstacle
    {
        protected Vector2 position;
        protected Texture2D texture; // Or Animation?
        public int Width { get { return texture.Width; } }
        public int Height { get { return texture.Height; } }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X,
                                        (int)position.Y,
                                        Width,
                                        Height);
            }
        }

        /// <summary>
        /// This is the bounding rectangle of the object located somehwere on the Track.
        /// </summary>
        public Rectangle BoundingRectangle
        {
            get
            {
                // Need to figure out what to do with rotated Objects (like a car for example)
                return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            }
        }



        public Obstacle(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.texture = texture;
        }
        public Obstacle(Rectangle rectangle)
        {
            this.position = new Vector2(rectangle.X, rectangle.Y);
            this.texture = null; // Maak hier nieuwe texture2D aan met de width en height van de retangle.
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }

    class MovableObject : Obstacle
    {
        public float Speed;
        public float Rotation; //doe hier dingen mee

        public Vector2 Velocity;
        public Vector2 LastPosition
        {
            get;
            protected set;
        }

        public MovableObject(Vector2 position, Texture2D texture, Vector2 velocity)
            : base(position, texture)
        {
            this.Velocity = velocity;
            LastPosition = new Vector2(0.0f, 0.0f);
        }

        public virtual void Update(GameTime gameTime)
        {
            CalculateMovement();
        }

        protected virtual void CalculateMovement()
        {
            LastPosition = position;
            position += Velocity;
            Velocity.X = Velocity.Y = 0;
        }

        
        

        public void reverseVelocity()
        {
            Velocity = Vector2.Negate(Velocity);
        }
        public void reverseVelocityX()
        {
            Velocity.X = -Velocity.X;
        }
        public void reverseVelocityY()
        {
            Velocity.Y = -Velocity.Y;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);
            //spriteBatch.Draw(texture, position, Rectangle.Empty ,Color.White, Rotation, new Vector2(0.0f,0.0f), new Vector2(1.0f,1.0f),SpriteEffects.None, 1.0f);
            spriteBatch.Draw(texture, this.Rectangle,
                new Rectangle(0,0, Width,Height), Color.White, Rotation, new Vector2((int)(Width/2), (int)(Height/2)),
                SpriteEffects.None, 0 );

        }
    }
}