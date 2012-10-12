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
       // int x, y;
        //int Width, Height;
        public Texture2D Texture;
        //render options ?
        //..
    }

    class WorldLoader 
    {
        World CurentWorld;  
        public ContentManager Content; // werkt misschien niet?

        public WorldLoader(ContentManager Content)
        {
            this.Content = Content;
        }

        public Texture2D GetTextureByRGB(Color data) 
        {
            /*switch(data.R){
                case 255:
                    return Content.Load<Texture2D>("grass");
            
            }*/

            return Content.Load<Texture2D>(data.R.ToString()); //roodintensiteit van 255 -> "255"(.png)
        }

        public World Load(Texture2D mapData) 
        {
            CurentWorld = new World(mapData.Width, mapData.Height);
            Color[] colorCodes = new Color[mapData.Width*mapData.Height];
            mapData.GetData(colorCodes);

            for (int x = 0; x < mapData.Width; x++)
                for (int y = 0; y < mapData.Height; y++)
                    CurentWorld.tiles[x, y].Texture = GetTextureByRGB(colorCodes[x + y*CurentWorld.horizontalTiles]);

            return CurentWorld;
        }


    }

    class World
    {
        public Tile[, ] tiles; //2-dimensionale array van de tiles(op dit moment alleen nog maar een wrapper voor Texture2D...)
        public int horizontalTiles, verticalTiles; //x tiles horizontaal, y tiles verticaal
        public const int TileWidth = 32; //je wilt niet oppeens van 32*32 tiles naar 64*64 tiles gaan
        public const int TileHeight = 32;

        public World(int tilesX, int tilesY)
        {
            horizontalTiles = tilesX;
            verticalTiles = tilesY;

            tiles = new Tile[horizontalTiles, verticalTiles];
            tiles.Initialize();
        }

        //renderd de textures van de tiles op het scherm, 
        public void render(GraphicsDevice graphicsDevice) 
        {
            SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);

            spriteBatch.Begin();

            for (int x = 0; x < horizontalTiles; x++)
                for (int y = 0; y < verticalTiles; y++)
                    spriteBatch.Draw(tiles[x, y].Texture,
                                     new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight),
                                     Color.White);

            spriteBatch.End();

        }
    }
    /*
    *
    * comment voor ome rutger :delegate
    */
}