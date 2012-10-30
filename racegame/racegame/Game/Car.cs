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
    class Car : MovableObject
    {


        private int health;
        private float fuel;
        private int nitro;

        private float maxSpeed;
        private float acceleration;
        private float maxAcceleration;
        private float maxDecceleration;
        private bool goingForward;

        private bool isOnGrass;
        private bool isOnRoad;

        private int lapsDriven;
        private int currentCheckpoint;

        Track track;

        public Car(Vector2 position, Texture2D texture, int health, float fuel, int nitro, float maxSpeed, float acceleration, Track track)
            : base(position, texture, new Vector2(0.0f, 0.0f))
        {
            this.health = health;
            this.fuel = fuel;
            this.nitro = nitro;

            this.maxSpeed = maxSpeed;
            this.acceleration = acceleration;

            this.track = track;

            maxAcceleration = 200;
            maxDecceleration = -100;

        }

        public override void Update(GameTime gameTime)
        {
            if (acceleration > 5 || acceleration < -5)
            {
                fuel = fuel - 0.05f;
            }

            Console.WriteLine("fuel = " + fuel);
            Console.WriteLine("acc = " + acceleration);
            base.Update(gameTime);

            GetInput(gameTime);

            //Setup the Movement increment.
            int aMove = (int)(acceleration * gameTime.ElapsedGameTime.TotalSeconds);

            this.position.X += (float)(aMove * Math.Cos(Rotation));
            this.position.Y += (float)(aMove * Math.Sin(Rotation));
           
            if (isOnGrass)
            {
                maxAcceleration = 100;
                maxDecceleration = -50;
                if (acceleration > 0)
                {
                    acceleration -= 2;
                }
                else if (acceleration < 0)
                {
                    acceleration += 2;
                }
            }
            else if (isOnRoad)
            {
                maxAcceleration = 200;
                maxDecceleration = -100;
                if (acceleration > 0)
                {
                    acceleration--;
                }
                else if (acceleration < 0)
                {
                    acceleration++;
                }
            }

            HandleCollisions();
        }

        public void GetInput(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                if (acceleration <= maxAcceleration) { acceleration += 5; }                                            
            }
            else if(keyboardState.IsKeyDown(Keys.Down))
            {
                if (acceleration >= maxDecceleration) { acceleration -= 5; }
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                Rotation -= (float)(1 * 3.0f * gameTime.ElapsedGameTime.TotalSeconds);
            }

            else if(keyboardState.IsKeyDown(Keys.Right))
            {
                Rotation += (float)(1 * 3.0f * gameTime.ElapsedGameTime.TotalSeconds);
            }
            if(keyboardState.IsKeyDown(Keys.Space))
            {
            }
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
            for (int y = yTopTile; y <= yBottomTile; ++y)
            {
                // En door de horizontale tiles
                for (int x = xLeftTile; x <= xRightTile; ++x)
                {
                    TileCollision collision = track.GetCollisionOfTile(x, y); // Haal collision-type op van de current Tile

                    switch (collision)
                    {

                        case TileCollision.Road:
                            isOnGrass = false;
                            isOnRoad = true;

                            break;

                        case TileCollision.Grass:
                            isOnRoad = false;
                            isOnGrass = true;
                            break;

                        case TileCollision.Solid:
                            isOnRoad = false;
                            isOnGrass = true;

                            break;

                        case TileCollision.Pitstop:
                            break;
                        case TileCollision.Water:
                            this.position = new Vector2(950, 650);
                            break;

                    }
                }

            }
        }
    }
}