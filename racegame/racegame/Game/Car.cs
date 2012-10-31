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

        public int lapsDriven;
        private int checkpointsSurpassed;
        private Obstacle lastCheckpoint; //index of List<Obstacle> checkpoint in track

        Track track;

        public Car(Vector2 position, Texture2D texture, int health, float fuel, int nitro, float maxSpeed, Track track, int player)
            : base(position, texture)
        {
            this.health = health;
            this.maxHealth = health;
            this.fuel = fuel;
            this.maxFuel = fuel;

            this.nitro = nitro;
            this.player = player;

            this.maxSpeed = maxSpeed;

            this.track = track;

            maxAcceleration = 200;
            maxDecceleration = -100;

            Rotation = 600;
        }

        public void Update(GameTime gameTime, List<Obstacle> checkpoints, Obstacle finish)
        {
            base.Update(gameTime);

            if (acceleration > 5 || acceleration < -5)
            {
                fuel = fuel - 0.05f;
            }

            CalculateMovement(gameTime);

            GetInput(gameTime);

            HandleCollisions();

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

            // Check for finish collision
            if (BoundingRectangle.Intersects(finish.BoundingRectangle) && checkpointsSurpassed >= checkpoints.Count() && checkpointsSurpassed != 0)
            {
                lapsDriven++;
                checkpointsSurpassed = 0;
                Console.WriteLine("Laps driven: " + lapsDriven);
            }
        }

        public void GetInput(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (this.player == 0)
            {
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    if (acceleration <= maxAcceleration) { acceleration += 5; }
                }
                else if (keyboardState.IsKeyDown(Keys.Down))
                {
                    if (acceleration >= maxDecceleration) { acceleration -= 5; }
                }

                if (keyboardState.IsKeyDown(Keys.Left) && (acceleration > 0 || acceleration < 0))
                {
                    Rotation -= (float)(1 * 3.0f * gameTime.ElapsedGameTime.TotalSeconds);
                }

                else if (keyboardState.IsKeyDown(Keys.Right) && (acceleration > 0 || acceleration < 0))
                {
                    Rotation += (float)(1 * 3.0f * gameTime.ElapsedGameTime.TotalSeconds);
                }
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                }
            }
            else if (this.player == 1)
            {
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    if (acceleration <= maxAcceleration) { acceleration += 5; }
                }
                else if (keyboardState.IsKeyDown(Keys.S))
                {
                    if (acceleration >= maxDecceleration) { acceleration -= 5; }
                }

                if (keyboardState.IsKeyDown(Keys.A) && (acceleration > 0 || acceleration < 0))
                {
                    Rotation -= (float)(1 * 3.0f * gameTime.ElapsedGameTime.TotalSeconds);
                }

                else if (keyboardState.IsKeyDown(Keys.D) && (acceleration > 0 || acceleration < 0))
                {
                    Rotation += (float)(1 * 3.0f * gameTime.ElapsedGameTime.TotalSeconds);
                }
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                }
            }
        }

        protected void CalculateMovement(GameTime gameTime)
        {
            // Use a smooth-curve for acceleration & deceleration of the car
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
                        break;
                    }

                    switch (collision)
                    {

                        case TileCollision.Road:
                            isOnGrass = false;
                            isOnRoad = true;

                            break;

                        case TileCollision.Grass:
                            isOnRoad = false;
                            isOnGrass = true;
                            health -= 0.5f;

                            break;

                        case TileCollision.Strip:

                            KeyboardState keyboardState = Keyboard.GetState();
                            if (keyboardState.IsKeyDown(Keys.Up))
                            {
                                if (acceleration > 60)
                                {
                                    acceleration -= 50;
                                }
                                else if (acceleration < 60)
                                {
                                    acceleration += 50;
                                }
                            }
                            if (keyboardState.IsKeyDown(Keys.Down))
                            {
                                if (acceleration < 0)
                                {
                                    acceleration = 05;
                                }
                            }
                            health -= 0.05f;
                            break;

                        case TileCollision.PitstopFuel:
                            increaseFuel(1);
                            break;


                        case TileCollision.PitstopHealth:
                            increaseHealth(1);
                            break;

                        case TileCollision.Water:
                            this.position = new Vector2(950, 650);
                            break;

                    }
                }

            }
        }

        public void increaseFuel(int amount)
        {
            fuel += amount;
            if (fuel > maxFuel) fuel = maxFuel;
        }

        public void increaseHealth(int amount)
        {
            health += amount;
            if (health > maxHealth) health = maxHealth;
        }
    }
}