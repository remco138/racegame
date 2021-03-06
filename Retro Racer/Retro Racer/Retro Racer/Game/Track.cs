﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RetroRacer
{
    class Track
    {
        public Texture2D trackTexture;
        public Tile[,] tiles;

        int numberOfPlayers;

        private SpriteFont font;

        public int Width { get { return tiles.GetLength(0); } }
        public int Height { get { return tiles.GetLength(1); } }
        public int WidthInPixels { get { return Width * Tile.Width; } }
        public int HeightInPixels { get { return Height * Tile.Height; } }

        public List<Car> cars;
        private List<Powerup> powerups;
        private List<Obstacle> checkpoints;
        public Obstacle finish;

        public int MAX_LAPS = 1;

        ContentManager Content;

        bool isFinished = false;
        bool isGameOverSP = false;
        bool isGameOverMP = false;
        bool isStarted = false;
        Texture2D finishOverlay;

        TimeSpan timeStarted;
        public TimeSpan TimeElapsed;

        SoundEffect horn;
        SoundEffect gameOverSP;
        SoundEffect gameOverMP;
        SoundEffect winner;
        
        public Track(Texture2D trackTexture, ContentManager Content, int numberOfPlayers, Game game)
        {
            this.Content = Content;
            this.numberOfPlayers = numberOfPlayers;
            this.finishOverlay = Content.Load<Texture2D>("Overlay");
 
            cars = new List<Car>();
            checkpoints = new List<Obstacle>();
            powerups = new List<Powerup>();

            font = Content.Load<SpriteFont>("Fonts/Pericles Light");

            tiles = new Tile[trackTexture.Width, trackTexture.Height];
            this.trackTexture = trackTexture;

            // Load tiles into the tiles array
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    tiles[i, j] = LoadTile(trackTexture, i, j); // Loads the correct tile into the array with the LoadTile() method.
                }
            }
        }

        /// <summary>
        /// Returns different tiles depending on the color.
        /// </summary>
        /// <param name="trackTexture"></param>
        /// <param name="x">X coordinate inside the trackTexture</param>
        /// <param name="y">Y coordinate inside the trackTexture</param>
        /// <returns></returns>
        public Tile LoadTile(Texture2D trackTexture, int x, int y)
        {
            Color currentColor = GetPixelColor(trackTexture, x, y);

            horn = Content.Load<SoundEffect>("Sounds/horn"); // Loading the horn sound
            gameOverSP = Content.Load<SoundEffect>("Sounds/GameOverSP"); // Loading the GameOverSP sound
            gameOverMP = Content.Load<SoundEffect>("Sounds/GameOverMP"); // Loading the GameOverSP sound
            winner = Content.Load<SoundEffect>("Sounds/winner"); // Loading the winner sound

            if (currentColor.Equals(new Color(255, 127, 39)))
            {
                // Orange = A Car

                // Only add the amount of cars needed!
                if (cars.Count != numberOfPlayers)
                {
                    cars.Add(new Car(   new Vector2(x * Tile.Width, y * Tile.Height),
                                        Content.Load<Texture2D>("Cars/Car" + cars.Count),
                                        Content.Load<Texture2D>("Cars/CarDead"),
                                        100,
                                        100,
                                        0,
                                        1000.0f,
                                        this,
                                        cars.Count));
                }
                
                return new Tile(Content.Load<Texture2D>("Tiles/Road"), TileCollision.Road);
            }
            else if (currentColor.Equals(new Color(195, 195, 195)))
            {
                // Grey = the road
                return new Tile(Content.Load<Texture2D>("Tiles/Road"), TileCollision.Road);
            }
            /* Y: Could be used if we want Dirt on the map.
            else if (currentColor.Equals(new Color(239, 228, 176)))
            {
                // Beige = the dirt
                return new Tile(Content.Load<Texture2D>("Tiles/Dirt"), TileCollision.Road);
            }
            */
            else if (currentColor.Equals(new Color(000, 255, 000)))
            {
                // Green = Grass
                return new Tile(Content.Load<Texture2D>("Tiles/Grass"), TileCollision.Grass);
            }
            else if (currentColor.Equals(new Color(000, 162, 232)))
            {
                // Blue = Water
                return new Tile(Content.Load<Texture2D>("Tiles/Water"), TileCollision.Water);
            }
            else if (currentColor.Equals(new Color(185, 122, 87)))
            {
                // Beige = Powerup
                powerups.Add(new Powerup(PowerupType.Fuel, new Vector2(x * Tile.Width, y * Tile.Height), Content.Load<Texture2D>("Tiles/Powerup"), false, false));

                return new Tile(Content.Load<Texture2D>("Tiles/Road"), TileCollision.Road);
            }
            else if (currentColor.Equals(new Color(0, 0, 255)))
            {
                // Pure Blue = Health Pistop
                return new Tile(Content.Load<Texture2D>("Tiles/PitstopHealth"), TileCollision.PitstopHealth);
            }
            else if (currentColor.Equals(new Color(255, 0, 0)))
            {
                // Pure Red = Fuel Pistop
                return new Tile(Content.Load<Texture2D>("Tiles/PitstopFuel"), TileCollision.PitstopFuel);
            }
            else if (currentColor.Equals(new Color(163, 073, 164)))
            {
                // Purple = Strip
                return new Tile(Content.Load<Texture2D>("Tiles/Strip"), TileCollision.Strip);
            }
            else if (currentColor.Equals(new Color(255, 242, 0)))
            {
                // Yellow = Pitstop
                return new Tile(Content.Load<Texture2D>("Tiles/PitstopStripRed"), TileCollision.PitstopStrip);
            }
            else if (currentColor.Equals(new Color(202, 156, 0)))
            {
                // Dark Yellow = Pitstop
                return new Tile(Content.Load<Texture2D>("Tiles/PitstopStripWhite"), TileCollision.PitstopStrip);
            }
            else if (currentColor.Equals(new Color(0, 0, 0)))
            {
                // Black = Finish / Start

                if (finish != null && finish.BoundingRectangle.Contains(new Point(x * Tile.Width, y * Tile.Height)))
                    return new Tile(Content.Load<Texture2D>("Tiles/Finish"), TileCollision.Checkpoint);

                Point endFinishTile = getEndTile(x, y, currentColor);

                int widthFinish = endFinishTile.X - x;
                int heightFinish = endFinishTile.Y - y;

                finish = new Obstacle(new Rectangle(x * Tile.Width, y * Tile.Height, widthFinish * Tile.Width, heightFinish * Tile.Height));

                return new Tile(Content.Load<Texture2D>("Tiles/Finish"), TileCollision.Checkpoint);
            }
            else if (currentColor.Equals(new Color(255, 255, 255)))
            {
                // White = Checkpoint

                //check if this tile is already included in the checkpoints list
                //
                foreach (Obstacle iterator in checkpoints)
                {
                    if (iterator.BoundingRectangle.Contains(new Point(x * Tile.Width, y * Tile.Height)))
                        return new Tile(Content.Load<Texture2D>("Tiles/Checkpoint"), TileCollision.Checkpoint);
                }

                Color rightTileColor = GetPixelColor(trackTexture, x + 1, y);
                Color bottomTileColor = GetPixelColor(trackTexture, x, y + 1);

                Point endCheckpointTile = getEndTile(x, y, currentColor);

                // Make the Checkpoint Object and add it to the CheckPoints-List.
                int widthCheckpoint = endCheckpointTile.X - x;
                int heightCheckpoint = endCheckpointTile.Y - y;

                Obstacle checkpoint = new Obstacle(new Rectangle(x * Tile.Width, y * Tile.Height, widthCheckpoint * Tile.Width, heightCheckpoint * Tile.Height));
                checkpoints.Add(checkpoint);


                return new Tile(Content.Load<Texture2D>("Tiles/Checkpoint"), TileCollision.Checkpoint);
            }
            else if (currentColor.Equals(new Color(136, 0, 21)))
            {
                // Red = Wall / Solid

                return new Tile(Content.Load<Texture2D>("Tiles/Solid"), TileCollision.Solid);
            }
            else
            {
                //throw new NotSupportedException(String.Format("Unsupported tile Color {0} at position {1}, {2}.", tileColor, x, y));
                return new Tile(Content.Load<Texture2D>("Tiles/Road"), TileCollision.Road); // Instead of throwing an exception we load unknown tiles with a road texture.
            }
        }

        /// <summary>
        /// Starting from 1 tile: find out where the line of the same colored tiles stop
        /// </summary>
        /// <param name="startX">X position of the starting tile</param>
        /// <param name="startY">Y position of the starting tile</param>
        /// <param name="colorToTestFor">The color to test for</param>
        /// <returns>The x and y (Point) of the last tile in the line</returns>
        public Point getEndTile(int startX, int startY, Color colorToTestFor)
        {
            Point endTile = Point.Zero;

            for (int depthX = 0; depthX < (Width - startX); depthX++)
            {
                int currentTileX = startX + depthX;

                if (!GetPixelColor(currentTileX, startY).Equals(colorToTestFor))
                {
                    endTile.X = startX + depthX;
                    break;
                }
            }

            for (int depthY = 0; depthY < (Height - startY); depthY++)
            {
                int currentTileY = startY + depthY;

                if (!GetPixelColor(startX, currentTileY).Equals(colorToTestFor))
                {
                    endTile.Y = startY + depthY;
                    break;
                }
            }

            return endTile;
        }

        public void AddCar(Car car)
        {
            cars.Add(car);
        }

        /// <summary>
        /// Returns the color of the pixel located at the given x and y.
        /// </summary>
        public Color GetPixelColor(Texture2D texture, int x, int y)
        {
            Rectangle sourceRectangle = new Rectangle(x, y, 1, 1); // Make new rectangle with 1 pixel in width and height.
            Color[] retrievedColor = new Color[1];

            texture.GetData<Color>(0, sourceRectangle, retrievedColor, 0, 1);

            return retrievedColor[0]; // Return the color that was found
        }
        public Color GetPixelColor(int x, int y)
        {
            return GetPixelColor(trackTexture, x, y);
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState)
        {
            //
            // Check the following in Update:
            //
            //      - For each car do the following:
            //          1. update the cars
            //          2. check for collision between cars & powerups
            //      - Update all the powerups (used for the respawn timer)

            if (!isFinished && !isGameOverSP && !isGameOverMP)
            {
                foreach (Car car in cars)
                {
                    car.Update(gameTime, checkpoints, finish);

                    // Check if car has collided with a powerup
                    foreach (Powerup powerup in powerups)
                    {
                        if (powerup.isActive)
                        {
                            if (car.BoundingRectangle.Intersects(powerup.BoundingRectangle))
                            {
                                switch (powerup.PowerupType)
                                {
                                    case PowerupType.Fuel:
                                        Console.WriteLine("Picked up fuel powerup");
                                        car.IncreaseFuel(10);
                                        powerup.isPickedUp = true;
                                        break;

                                    case PowerupType.Health:
                                        //  ??
                                        break;
                                }
                            }
                        }
                    }

                    if (car.acceleration > 50 || car.acceleration < -50)
                    {
                        if (currentKeyboardState.IsKeyDown(Keys.Space) && (previousKeyboardState.IsKeyUp(Keys.Space)))
                        {
                            isStarted = true;
                        }
                    }

                    if (car.lapsDriven == MAX_LAPS)
                    {
                        isFinished = true;
                    }

                    if (cars.Count == 1 && (cars[0].fuel < 1 || cars[0].health < 1))
                    {
                        isGameOverSP = true;
                    }

                    if (cars.Count == 2 && (cars[0].fuel < 1 || cars[0].health < 1) && (cars[1].fuel < 1 || cars[1].health < 1))
                    {
                        isGameOverMP = true;
                    }

                }

                if (isStarted)
                {
                    horn.Play(); // Play the horn sound
                    isStarted = false;
                }
                if (cars.Count == 1)
                {
                    if (isGameOverSP)
                    {
                        gameOverSP.Play(); // Play the GameOverSP sound
                    }
                    else if (isFinished)
                    {
                        winner.Play(); // Play the winner sound
                    }
                }
                else if (cars.Count == 2)
                {
                    if (isGameOverMP)
                    {
                        gameOverMP.Play(); // Play the GameOverMP sound
                    }
                    else if (isFinished)
                    {
                        winner.Play(); // Play the winner sound
                    }
                }

                foreach (Powerup powerup in powerups)
                {
                    powerup.Update(gameTime);
                }

                // Set the startTime when game first starts && Calculate the timeElapsed after that.
                //
                if (timeStarted == TimeSpan.Zero) timeStarted = gameTime.TotalGameTime;
                TimeElapsed = gameTime.TotalGameTime - timeStarted;
                // Console.WriteLine("{0}, lapsed: {1}", timeStarted, TimeElapsed);  
            }
        }
        

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw all the tiles
            //
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Vector2 position = new Vector2(x, y) * Tile.Size;

                    spriteBatch.Draw(tiles[x, y].Texture,   //de texture van de tile
                                     position,
                                     Color.White);
                }
            }

            // Draw all the objects
            foreach (Car car in cars)
            {
                car.Draw(spriteBatch);
            }

            foreach (Powerup powerup in powerups)
            {
                powerup.Draw(spriteBatch);
            }

            if (cars.Count == 1)
            {
                if (isGameOverSP)
                {
                    spriteBatch.Draw(finishOverlay, new Rectangle(0, 0, 1280, 800), Color.Black);
                    spriteBatch.DrawString(font, "Game Over", new Vector2(370, 350), Color.White);
                }
                if (isFinished)
                {
                    spriteBatch.Draw(finishOverlay, new Rectangle(0, 0, 1280, 800), Color.Black);
                    spriteBatch.DrawString(font, "Finished in " + TimeElapsed.Minutes + " Minutes and " + TimeElapsed.Seconds + " Seconds.", new Vector2(300, 250), Color.White);
                }
            }
            else if (cars.Count == 2)
            {
                if (isGameOverMP)
                {
                    spriteBatch.Draw(finishOverlay, new Rectangle(0, 0, 1280, 800), Color.Black);
                    spriteBatch.DrawString(font, "You guys suck!", new Vector2(370, 350), Color.White);
                }
                if (isFinished && cars[0].lapsDriven > cars[1].lapsDriven)
                {
                    spriteBatch.Draw(finishOverlay, new Rectangle(0, 0, 1280, 800), Color.Black);
                    spriteBatch.DrawString(font, "Finished in " + TimeElapsed.Minutes + " Minutes and " + TimeElapsed.Seconds + " Seconds." +
                                                                                                                    Environment.NewLine +
                                                                                                                    Environment.NewLine +
                                                                                                                    Environment.NewLine + "The player of the red car has won.", new Vector2(300, 250), Color.White);
                }
                else if (isFinished && cars[0].lapsDriven < cars[1].lapsDriven)
                {
                    spriteBatch.Draw(finishOverlay, new Rectangle(0, 0, 1280, 800), Color.Black);
                    spriteBatch.DrawString(font, "Finished in " + TimeElapsed.Minutes + " Minutes and " + TimeElapsed.Seconds + " Seconds." +
                                                                                                                        Environment.NewLine +
                                                                                                                        Environment.NewLine +
                                                                                                                        Environment.NewLine + "The player of the blue car has won.", new Vector2(300, 250), Color.White);
                }
            }
        }

        /// <summary>
        /// This method takes an X and Y and returns the Collision Type of the tile.
        /// </summary>
        public TileCollision GetCollisionOfTile(int x, int y)
        {
            // Prevent escaping past the level boundaries.
            if ((x < 0 || x >= Width) ||
                (y < 0 || y >= Height))
            {
                return TileCollision.Solid;
            }

            return tiles[x, y].Collision;
        }
    }

}