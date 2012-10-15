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

        Rectangle BoundingRectangle
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
            Console.WriteLine("car collisions handled");
        }
    }
}
