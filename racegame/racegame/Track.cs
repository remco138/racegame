using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace racegame
{
    enum TileCollision
    {
        /// <summary>
        /// Used for normal 'road' tiles
        /// </summary>
        Passable = 0,

        /// <summary>
        /// Bladiebla
        /// </summary>
        Impassable = 1,

        /// <summary>
        /// Cars will slow down when they enter a grass tile.
        /// </summary>
        Grass = 2,
    }

    struct Tile
    {
        public Texture2D Texture;
        public TileCollision Collision;

        public const int Width = 32;
        public const int Height = 32;

        public static readonly Vector2 Size = new Vector2(Width, Height);

        public Tile(Texture2D texture, TileCollision collision)
        {
            Texture = texture;
            Collision = collision;
        }
    }

    class Track
    {
        public Tile[,] tiles; //2-dimensionale array van de tiles(op dit moment alleen nog maar een wrapper voor Texture2D...)

        public int Width { get { return tiles.GetLength(0); } }
        public int Height { get { return tiles.GetLength(1); }  }
        public int WidthInPixels { get { return Width * Tile.Width; } }
        public int HeightInPixels { get { return Height * Tile.Height; } }

        List<MovableObject> worldObjects;

        public Track(Texture2D trackTexture, ContentManager Content)
        {
            //
            // Dit is de constructor, deze voert het volgende uit:
            // 
            //      - Leest de trackTexture in en laad aan de hand van de (kleur)-codes de verschillende objecten in. (bv een rode pixel = de start positie van een Car)   
            //
            tiles = new Tile[trackTexture.Width, trackTexture.Height];

            // Maak een aantal test-objecten aan. (dit is nu nog handmatig, kan later gedaan worden door de trackTexture uit te lezen)
            worldObjects = new List<MovableObject>()
            {
                new MovableObject(new Vector2(0.0f,0.5f), Content.Load<Texture2D>("Crosshair"), new Vector2(5.0f,5.0f)),
                new MovableObject(new Vector2(5.0f,0.5f), Content.Load<Texture2D>("Crosshair"), new Vector2(15.0f,15.0f)),
                new Car(new Vector2(10.0f, 10.0f), Content.Load<Texture2D>("Crosshair"), 100, 100, 0, 1000.0f, 500.0f, this) { velocity = new Vector2(0.2f, 0.2f) }
            }; 
        }

        public void Update()
        {
            //
            // Hier voeren we het volgende uit:
            //
            //      - Check op collisions
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
            if (x < 0 || x >= Width)
            {
                return TileCollision.Impassable;
            }

            return tiles[x, y].Collision;
        }
    }

}