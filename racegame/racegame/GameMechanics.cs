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
    class StaticObject
    {
        protected Vector2 position;
        protected Texture2D texture; // Or Animation?

        public Rectangle BoundingRectangle
        {
            get
            {
                // Need to figure out what to do with rotated Objects (like a car for example)
                return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            }
        }

        public StaticObject(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.texture = texture;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }

    class MovableObject : StaticObject
    {
        public Vector2 velocity;
        public Vector2 LastPosition
        {
            get;
            protected set;
        }

        public MovableObject(Vector2 position, Texture2D texture, Vector2 velocity)
            : base(position, texture)
        {
            this.velocity = velocity;
            LastPosition = new Vector2(0.0f, 0.0f);
        }

        public virtual void Update()
        {
            CalculateMovement();
        }

        protected virtual void CalculateMovement()
        {
            LastPosition = position;
            position += velocity;
        }

        public void reverseVelocity()
        {
            velocity = -velocity;
        }
        public void reverseVelocityX()
        {
            velocity.X = -velocity.X;
        }
        public void reverseVelocityY()
        {
            velocity.Y = -velocity.Y;
        }
    }

    class Car : MovableObject
    {
        private int health;
        private int fuel;
        private int nitro;

        private float maxSpeed;
        private float accelerationSpeed;

        private bool isOnGrass;
        private bool isOnRoad;

        public Car(Vector2 position, Texture2D texture, int health, int fuel, int nitro, float maxSpeed, float accelerationSpeed)
            : base(position, texture, new Vector2(0.0f, 0.0f))
        {
            this.health = health;
            this.fuel = fuel;
            this.nitro = nitro;

            this.maxSpeed = maxSpeed;
            this.accelerationSpeed = accelerationSpeed;
        }

        public override void Update()
        {
            base.Update();

            HandleCollisions();
        }

        protected override void CalculateMovement()
        {
            // Use a smooth-curve for acceleration & deceleration of the car

            base.CalculateMovement();
        }

        public void HandleCollisions()
        {
            //vorm (de grootst mogelijke) vierhoek om de auto waarin alle tiles die collision kunnen veroorzaken in voorkomen
            //omdat ik lui ben en niet moeilijk wil doen met pythagoras, gewoon de langste zijde als omtrek voor alle zijden gebruiken
            //dit voorkomt dat je ALLE tiles checkt voor collision met de auto
            int maxSize = 0;
            //ipl van onhandige if/else, ternary operator!
            maxSize = (BoundingRectangle.Width > BoundingRectangle.Height) ? BoundingRectangle.Width : BoundingRectangle.Height;
            //niet alleen afronden van float naar int, ook moet er afgerond worden op buitenste 32 (of 16 of andere mogelijke tileSizes)
            int innerX = ((int)this.position.X - ((int)this.position.X % 32)) / 32; //tileSize moet hier staan, hoe staticObject deze waarde moet weten...
            int innerY = ((int)this.position.Y - ((int)this.position.X % 32)) / 32;     //x waarde van 33 =>  33 - (33 % 32) == 33 - 1 == 32; 32 / 32 = precies 1 tile!

            int outerX = ((int)this.position.X + (32 - (int)this.position.X % 32)) / 32; //x waarde van 66 => 66 + (32 - (66 % 32)) == 66 + (32 - 2) == 96; 96 / 32 = precies 3 tiles :D
            int outerY = ((int)this.position.Y + (32 - (int)this.position.X % 32)) / 32;

            Rectangle boundingBox = new Rectangle(innerX, innerY, outerX - innerX, outerY - innerY);

            for(int x = innerX; x < outerX; x++)
                for(int y = innerY; y < outerY; y++)
                    if(boundingBox.Intersects(World.tiles[x][y]) && // en andersom?
                        Console.WriteLine("Collision!");
        }
    }
}
