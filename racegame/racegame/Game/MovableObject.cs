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

        public virtual void Update()
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
    }
}