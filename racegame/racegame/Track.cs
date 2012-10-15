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

        public const int TileWidth = 32;
        public const int TileHeight = 32;

        public Tile(Texture2D texture, TileCollision collision)
        {
            Texture = texture;
            Collision = collision;
        }
    }

    class Track
    {
        public Tile[, ] tiles; //2-dimensionale array van de tiles(op dit moment alleen nog maar een wrapper voor Texture2D...)
        public int horizontalTiles, verticalTiles; //x tiles horizontaal, y tiles verticaal

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
                new Car(new Vector2(10.0f, 10.0f), Content.Load<Texture2D>("Crosshair"), 100, 100, 0, 1000.0f, 500.0f) { velocity = new Vector2(0.2f, 0.2f) }
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
            for (int x = 0; x < horizontalTiles; x++)
                for (int y = 0; y < verticalTiles; y++)
                    spriteBatch.Draw(tiles[x, y].Texture,   //de texture van de tile
                                     new Rectangle(x * Tile.TileWidth, y * Tile.TileHeight, Tile.TileWidth, Tile.TileHeight), //de recthoek waarin de texture in word geplaats/gerekt
                                     Color.White);

            // Draw all the objects
            //
            foreach (MovableObject obj in worldObjects)
            {
                obj.Draw(spriteBatch);
            }
        }
    }

}