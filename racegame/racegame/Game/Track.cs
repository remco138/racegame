using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace racegame
{
    class Track
    {
        public Texture2D trackTexture;
        public Tile[,] tiles; //2-dimensionale array van de tiles(op dit moment alleen nog maar een wrapper voor Texture2D...)

        public int Width { get { return tiles.GetLength(0); } }
        public int Height { get { return tiles.GetLength(1); } }
        public int WidthInPixels { get { return Width * Tile.Width; } }
        public int HeightInPixels { get { return Height * Tile.Height; } }

        List<Car> cars;
        List<Powerup> powerups;
        List<Obstacle> checkpoints;
        Obstacle finish;

        ContentManager Content;

        int numberOfPlayers;
        
        public Track(Texture2D trackTexture, ContentManager Content, int numberOfPlayers)
        {
            //
            // Dit is de constructor, deze voert het volgende uit:
            // 
            //      - Leest de trackTexture in en laad aan de hand van de (kleur)-codes de verschillende objecten in. (bv een rode pixel = de start positie van een Car)   
            //
            this.Content = Content;
            this.numberOfPlayers = numberOfPlayers;

            cars = new List<Car>();
            checkpoints = new List<Obstacle>();
            powerups = new List<Powerup>();

            tiles = new Tile[trackTexture.Width, trackTexture.Height];
            this.trackTexture = trackTexture;

            // Load tiles into the tiles array
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    tiles[i, j] = LoadTile(trackTexture, i, j); // Zoek uit wat voor Tile dit is en zet deze in de tiles[] array.
                }
            }

            // Maak een aantal test-objecten aan. (dit is nu nog handmatig, kan later gedaan worden door de trackTexture uit te lezen)
        }

        public Tile LoadTile(Texture2D trackTexture, int x, int y)
        {
            Color currentColor = GetPixelColor(trackTexture, x, y);

            if (currentColor.Equals(new Color(255, 127, 39)))
            {
                // Orange = A Car

                // Only add the amount of cars needed!
                if (cars.Count != numberOfPlayers)
                {
                    cars.Add(new Car(new Vector2(x * Tile.Width, y * Tile.Height), Content.Load<Texture2D>("Car" + cars.Count), 100, 100, 0, 1000.0f, this, cars.Count));
                }
                
                return new Tile(Content.Load<Texture2D>("Tiles/Road"), TileCollision.Road);
            }
            else if (currentColor.Equals(new Color(195, 195, 195)))
            {
                // Grey = the road
                return new Tile(Content.Load<Texture2D>("Tiles/Road"), TileCollision.Road);
            }
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
                powerups.Add(new Powerup(PowerupType.fuel, new Vector2(x * Tile.Width, y * Tile.Height), Content.Load<Texture2D>("Tiles/Powerup"), false, false));

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

                //
                // Maak heir het Checkpoint Object aan & de range van deze tiles opslaan zodat dit bij de volgende keer wordt overgeslagen
                //

                return new Tile(Content.Load<Texture2D>("Tiles/Checkpoint"), TileCollision.Checkpoint);
            }
            else if (currentColor.Equals(new Color(136, 0, 21)))
            {
                // Red = Wall / Solid

                return new Tile(Content.Load<Texture2D>("Tiles/Solid"), TileCollision.Solid);
            }
            else
            {
                //throw new NotSupportedException(String.Format("Unsupported tole Color {0} at position {1}, {2}.", tileColor, x, y));
                return new Tile(Content.Load<Texture2D>("Tiles/Road"), TileCollision.Road);
            }
        }

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

        public void Update(GameTime gameTime)
        {
            //
            // Check the following in Update:
            //
            //      - For each car do the following:
            //          1. update the cars
            //          2. check for collision between cars & powerups
            //      - Update all the powerups (used for the respawn timer)

            foreach (Car car in cars)
            {
                car.Update(gameTime, checkpoints, finish);
                
                // Check if car has collided with a powerup
                foreach(Powerup powerup in powerups)
                {
                    if (powerup.isActive)
                    {
                        if (car.BoundingRectangle.Intersects(powerup.BoundingRectangle))
                        {
                            Console.WriteLine("Picked up fuel powerup");
                            car.increaseFuel(20);
                            powerup.isActive = false;
                        }
                    }
                }
            }

            foreach (Powerup powerup in powerups)
            {
                powerup.Update(gameTime);
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
            //
            foreach (Car car in cars)
            {
                car.Draw(spriteBatch);
            }
            foreach (Powerup powerup in powerups)
            {
                powerup.Draw(spriteBatch);
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