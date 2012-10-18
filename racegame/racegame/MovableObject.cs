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
        public float Acceleraion;
        public float MaxAcceleration;
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

        private float GetAcceleration(float speed)
        {
            //return acceleratie gebasseerd op huidige speed, later... eerst altijd 1, oftewel lineare speed
            return (Acceleraion < MaxAcceleration) ? 1 : 0;
        }
        public virtual void Update()
        {
            CalculateMovement();
        }

        protected virtual void CalculateMovement()
        {
            Speed += 5*GetAcceleration(Speed); // rekening houden met gametime?

            //speed naar coordinaten, gejat van elo..
            Velocity = new Vector2((float)(Speed * Math.Cos(Rotation)),
            (float)(Speed * Math.Sin(Rotation)));

            LastPosition = position;
            position += Velocity;
        }


        

        public void reverseVelocity()
        {
            Velocity = -Velocity;
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