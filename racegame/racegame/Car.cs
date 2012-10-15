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

        Track track;

        public Car(Vector2 position, Texture2D texture, int health, int fuel, int nitro, float maxSpeed, float accelerationSpeed, Track track)
            : base(position, texture, new Vector2(0.0f, 0.0f))
        {
            this.health = health;
            this.fuel = fuel;
            this.nitro = nitro;

            this.maxSpeed = maxSpeed;
            this.accelerationSpeed = accelerationSpeed;

            this.track = track;
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
            // 1. Haal de car-Rectangle op. Deze is op te halen via het veld: BoundingRectangle
            // 2. Zoek uit met welke tiles de Car in contact komt.
            // 3. Loop door deze tiles heen en controleer vervolgens welke collision-type deze tile heeft
            //      3b. Is deze tile passable?
            //      3c. Is deze tile NIET passable? dan is er een collision.
            //

            // this.BoundingRectangle wordt gebruikt om de tiles rondom de auto te vinden. 

            int xLeftTile = (int)Math.Floor((float)BoundingRectangle.Left / Tile.Width);
            int xRightTile = (int)Math.Ceiling((float)BoundingRectangle.Right / Tile.Width) - 1;
            int yTopTile = (int)Math.Floor((float)BoundingRectangle.Top / Tile.Height);
            int yBottomTile = (int)Math.Ceiling((float)BoundingRectangle.Bottom / Tile.Height) - 1;

            // Loop door de verticale tiles
            for(int y = yTopTile; y <= yBottomTile; ++y)
            {
                // En door de horizontale tiles
                for(int x= xLeftTile; x <= xRightTile; ++x)
                {
                    TileCollision collision = track.GetCollisionOfTile(x, y); // Haal collision-type op van de current Tile

                    if(collision != TileCollision.Passable)
                    {
                        // Een impassable object!
                    }
                    else if(collision == TileCollision.Grass)
                    {
                        // Slow down on grass?
                    }

                }

            }
        }
    }
}
