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
    class Car : MovableObject
    {
        Texture2D deathTexture;

        public float health;
        public float maxHealth;
        public float fuel;
        public float maxFuel;
        private int nitro;
        private int player;

        private float maxSpeed;
        public float acceleration;
        private float maxAcceleration;
        private float maxDecceleration;

        private bool isOnGrass;
        private bool isOnRoad;
        private bool isOnStrip;
        private bool isOnPSFuel;
        private bool isOnPSHealth;
        /* Y: Could be used if we want Dirt on the map. 
        private bool isOnDirt; 
        */

        private bool isDead = false;

        public int lapsDriven;
        private int checkpointsSurpassed;
        private Obstacle lastCheckpoint; //index of List<Obstacle> checkpoint in track

        Track track;

        public Car(Vector2 position, Texture2D texture, Texture2D deathTexture, int health, float fuel, int nitro, float maxSpeed, Track track, int player)
            : base(position, texture)
        {
            this.deathTexture = deathTexture;

            lastCheckpoint = track.finish;

            this.health = health;
            this.maxHealth = health;
            this.fuel = fuel;
            this.maxFuel = fuel;

            this.nitro = nitro;
            this.player = player;

            this.maxSpeed = maxSpeed;

            this.track = track;

            Rotation = 600;
        }

        public void Update(GameTime gameTime, List<Obstacle> checkpoints, Obstacle finish)
        {
            base.Update(gameTime);

            // Use fuel if acceleration higher or lower than 5.
            if (acceleration > 5 || acceleration < -5)
            {
                fuel = fuel - 0.05f;
            }

            // Stop the car if there is no more fuel.
            if (fuel < 1)
            {
                acceleration = 0;
            }

            CalculateMovement(gameTime);

            // Don't respond to user input when the car is dead
            if (!isDead)
            {
                GetInput(gameTime);
            }

            HandleCollisions();

            // Check if the car has reached the finish
            if (BoundingRectangle.Intersects(track.finish.BoundingRectangle))
            {
                lastCheckpoint = track.finish;
            }

            // Check for Checkpoint collision
            foreach (Obstacle checkpoint in checkpoints)
            {
                bool isTouching = BoundingRectangle.Intersects(checkpoint.BoundingRectangle);

                if (isTouching && lastCheckpoint != checkpoint)
                {
                    lastCheckpoint = checkpoint;
                    checkpointsSurpassed++;
                    Console.WriteLine("A car just drove over checkpoint No.:" + checkpointsSurpassed);
                }
            }

            // Check if the car has collided with the finish & if it has been to all the checkpoints.
            if (BoundingRectangle.Intersects(finish.BoundingRectangle) && checkpointsSurpassed >= checkpoints.Count() && checkpointsSurpassed != 0)
            {
                lapsDriven++;
                checkpointsSurpassed = 0;
                Console.WriteLine("Laps driven: " + lapsDriven);
            }

            // Car has suffered too much damage
            if (health < 1 && !isDead)
            {
                isDead = true;
                texture = deathTexture;
            }
        }

        public void GetInput(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (this.player == 0)
            {
                if (keyboardState.IsKeyDown(Keys.Up) && (fuel > 1 && health > 1)) // Y: Fuel and/or health must be greater than 1 so the car stops
                {
                    if (acceleration <= maxAcceleration) { acceleration += 5; }
                }
                else if (keyboardState.IsKeyDown(Keys.Down) && (fuel > 1 && health > 1)) //
                {
                    if (acceleration >= maxDecceleration) { acceleration -= 5; }
                }

                if (keyboardState.IsKeyDown(Keys.Left) && (acceleration > 50 || acceleration < -50)) // Y: Acceleration must be greater than (-)50 to be able to turn the car
                {
                    Rotation -= (float)(1 * 3.0f * gameTime.ElapsedGameTime.TotalSeconds);
                }

                else if (keyboardState.IsKeyDown(Keys.Right) && (acceleration > 50 || acceleration < -50)) //
                {
                    Rotation += (float)(1 * 3.0f * gameTime.ElapsedGameTime.TotalSeconds);
                }
                if (keyboardState.IsKeyDown(Keys.RightShift)) // Y: Cheating, speed goes up, but the health and fuel goes down faster then normal.
                {
                    acceleration = 300;
                    health -= 0.2f;
                    fuel -= 0.2f;
                }
            }
            else if (this.player == 1)
            {
                if (keyboardState.IsKeyDown(Keys.W) && (fuel > 1 && health > 1)) // Y: Fuel and/or health must be greater than 1 so the car stops
                {
                    if (acceleration <= maxAcceleration) { acceleration += 5; }
                }
                else if (keyboardState.IsKeyDown(Keys.S) && (fuel > 1 && health > 1)) //
                {
                    if (acceleration >= maxDecceleration) { acceleration -= 5; }
                }

                if (keyboardState.IsKeyDown(Keys.A) && (acceleration > 50 || acceleration < -50)) // Y: Acceleration must be greater than (-)50 to be able to turn the car
                {
                    Rotation -= (float)(1 * 3.0f * gameTime.ElapsedGameTime.TotalSeconds);
                }

                else if (keyboardState.IsKeyDown(Keys.D) && (acceleration > 50 || acceleration < -50)) // 
                {
                    Rotation += (float)(1 * 3.0f * gameTime.ElapsedGameTime.TotalSeconds);
                }
                if (keyboardState.IsKeyDown(Keys.Q)) // Y: Cheating, speed goes up, but the health and fuel goes down faster then normal.
                {
                    acceleration = 300;
                    health -= 0.2f;
                    fuel -= 0.2f;
                }
            }
        }

        protected void CalculateMovement(GameTime gameTime)
        {
            // Use a smooth-curve for acceleration & deceleration of the car

            //Setup the Movement increment.
            int moveAmount = (int)(acceleration * gameTime.ElapsedGameTime.TotalSeconds);

            this.position.X += (float)(moveAmount * Math.Cos(Rotation));
            this.position.Y += (float)(moveAmount * Math.Sin(Rotation));

            if (isOnRoad)
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
            else if (isOnGrass)
            {
                maxAcceleration = 100;
                maxDecceleration = -60;
                if (acceleration > 0)
                {
                    acceleration -= 2;
                }
                else if (acceleration < 0)
                {
                    acceleration += 2;
                }
            }
            else if (isOnStrip)
            {
                maxAcceleration = 100;
                maxDecceleration = -60;
                if (acceleration > 0)
                {
                    acceleration -= 2;
                }
                else if (acceleration < 0)
                {
                    acceleration += 2;
                }
            }
            else if (isOnPSFuel)
            {
                maxAcceleration = 125;
                maxDecceleration = -75;
                if (acceleration > 0)
                {
                    acceleration -= 2;
                }
                else if (acceleration < 0)
                {
                    acceleration += 2;
                }
            }
            else if (isOnPSHealth)
            {
                maxAcceleration = 125;
                maxDecceleration = -75;
                if (acceleration > 0)
                {
                    acceleration -= 2;
                }
                else if (acceleration < 0)
                {
                    acceleration += 2;
                }
            }
            /* Y: Could be used if we want Dirt on the map.
            else if (isOnDirt)
            {
                maxAcceleration = 100;
                maxDecceleration = -60;
                if (acceleration > 0)
                {
                    acceleration -= 10;
                }
                else if (acceleration < 0)
                {
                    acceleration += 10;
                }
            }
            */

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
                    Rectangle tileBoundingBox = new Rectangle(x * Tile.Width, y*Tile.Height, Tile.Width, Tile.Height);

                    if (!tileBoundingBox.Intersects(BoundingRectangle) || !BoundingRectangle.Intersects(tileBoundingBox))
                    {
                        break; // ?
                    }

                    switch (collision)
                    {
                        case TileCollision.Road:
                            isOnRoad = true;
                            isOnGrass = false;
                            isOnStrip = false;
                            isOnPSFuel = false;
                            isOnPSHealth = false;
                            /* Y: Could be used if we want Dirt on the map.
                            isOnDirt = false;
                            */
                            break;

                        case TileCollision.Grass:
                            isOnRoad = false;
                            isOnGrass = true;
                            isOnStrip = false;
                            isOnPSFuel = false;
                            isOnPSHealth = false;
                            /* Y: Could be used if we want Dirt on the map.
                            isOnDirt = false;
                            */
                            health -= 0.05f;
                            break;

                        case TileCollision.Strip:
                            isOnRoad = false;
                            isOnGrass = false;
                            isOnStrip = true;
                            isOnPSFuel = false;
                            isOnPSHealth = false;
                            /* Y: Could be used if we want Dirt on the map.
                            isOnDirt = false;
                            */                            
                            health -= 0.05f;
                            break;

                        case TileCollision.PitstopStrip:
                            this.position.X = lastCheckpoint.BoundingRectangle.Location.X;
                            this.position.Y = lastCheckpoint.BoundingRectangle.Location.Y;
                            position.X += lastCheckpoint.BoundingRectangle.Width / 2;
                            position.Y += lastCheckpoint.BoundingRectangle.Height / 2;
                            break;

                        case TileCollision.PitstopFuel:
                            isOnRoad = false;
                            isOnGrass = false;
                            isOnStrip = false;
                            isOnPSFuel = true;
                            isOnPSHealth = false;
                            /* Y: Could be used if we want Dirt on the map.
                            isOnDirt = false;
                            */
                            IncreaseFuel(1);
                            break;

                        case TileCollision.PitstopHealth:
                            isOnRoad = false;
                            isOnGrass = false;
                            isOnStrip = false;
                            isOnPSFuel = false;
                            isOnPSHealth = true;
                            /* Y: Could be used if we want Dirt on the map.
                            isOnDirt = false;
                            */
                            IncreaseHealth(1);
                            break;

                        /* Y: Could be used if we want Dirt on the map.
                        case TileCollision.Dirt:
                            isOnRoad = false;
                            isOnGrass = false;
                            isOnStrip = false;
                            isOnPSFuel = false;
                            isOnPSHealth = false;
                            isOnDirt = true;
                            break;
                         */

                        case TileCollision.Water:
                            this.position.X = lastCheckpoint.BoundingRectangle.Location.X;
                            this.position.Y = lastCheckpoint.BoundingRectangle.Location.Y;
                            position.X += lastCheckpoint.BoundingRectangle.Width / 2;
                            position.Y += lastCheckpoint.BoundingRectangle.Height / 2;
                            break;

                        case TileCollision.Solid:
                            this.position.X = lastCheckpoint.BoundingRectangle.Location.X;
                            this.position.Y = lastCheckpoint.BoundingRectangle.Location.Y;
                            position.X += lastCheckpoint.BoundingRectangle.Width / 2;
                            position.Y += lastCheckpoint.BoundingRectangle.Height / 2;
                            break;

                    }
                }

            }
        }

        public void IncreaseFuel(int amount)
        {
            fuel += amount;
            if (fuel > maxFuel) fuel = maxFuel;
        }

        public void IncreaseHealth(int amount)
        {
            health += amount;
            if (health > maxHealth) health = maxHealth;
        }
    }
}