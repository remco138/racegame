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
        public Tile[,] tiles; //2-dimensionale array van de tiles(op dit moment alleen nog maar een wrapper voor Texture2D...)

        public int Width { get { return tiles.GetLength(0); } }
        public int Height { get { return tiles.GetLength(1); }  }
        public int WidthInPixels { get { return Width * Tile.Width; } }
        public int HeightInPixels { get { return Height * Tile.Height; } }

        List<MovableObject> worldObjects;

        ContentManager Content;

        public Track(Texture2D trackTexture, ContentManager Content)
        {
            //
            // Dit is de constructor, deze voert het volgende uit:
            // 
            //      - Leest de trackTexture in en laad aan de hand van de (kleur)-codes de verschillende objecten in. (bv een rode pixel = de start positie van een Car)   
            //
            this.Content = Content;

            tiles = new Tile[trackTexture.Width, trackTexture.Height];

            // Load tiles into the tiles array
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Color currentColor = GetPixelColor(trackTexture, i, j);
                    tiles[i, j] = LoadTile(currentColor, i, j); // Zoek uit wat voor Tile dit is en zet deze in de tiles[] array.
                }
            }

            // Maak een aantal test-objecten aan. (dit is nu nog handmatig, kan later gedaan worden door de trackTexture uit te lezen)
            worldObjects = new List<MovableObject>();
        }

        public Tile LoadTile(Color tileColor, int x, int y)
        {
            if(tileColor.Equals(new Color(195, 195, 195)))
            {
                // Grey = the road
                return new Tile(Content.Load<Texture2D>("Tiles/Road"), TileCollision.Road);
            }
            else if (tileColor.Equals(new Color(000, 255, 000)))
            {
                // Green = Grass
                return new Tile(Content.Load<Texture2D>("Tiles/Grass"), TileCollision.Grass);
            }
            else if (tileColor.Equals(new Color(000, 162, 232)))
            {
                // Blue = Water
                return new Tile(Content.Load<Texture2D>("Tiles/Water"), TileCollision.Water);
            }
            else if(tileColor.Equals(new Color(163,073,164)))
            {
                return new Tile(Content.Load<Texture2D>("Tiles/Solid"), TileCollision.Solid);
            }
            else
            {
                //throw new NotSupportedException(String.Format("Unsupported tole Color {0} at position {1}, {2}.", tileColor, x, y));
                return new Tile(Content.Load<Texture2D>("Tiles/Road"), TileCollision.Road);
            }
        }
         
     public void AddObject(MovableObject Object){
         worldObjects.Add(Object);
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

        public void Update()
        {
            //
            // Hier voeren we het volgende uit:
            //
            //      - Check of de auto een powerup aanraakt.
            //      - Update de objecten (zoals bv. cars, powerups etc.)
            //

            foreach (MovableObject obj in worldObjects)
            { 
                obj.Update(); 
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
            foreach (MovableObject obj in worldObjects)
            {
                obj.Draw(spriteBatch);
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