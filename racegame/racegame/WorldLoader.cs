using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace racegame
{
    class tile {
       // int x, y;
        public Texture2D texture = null;
        //render options, 
    }

    class WorldLoader {
        public tile[, ] world;
        public int horizontalTiles, verticalTiles;        
        public ContentManager Content; // werkt misschien niet?

        public Texture2D GetTextureByRGB(Color data) {
            /*switch(data.R){
                case 255:
                    return Content.Load<Texture2D>("grass");
            
            }*/

            return Content.Load<Texture2D>(data.R.ToString()); //roodintensiteit van 255 -> "255"
        }

        public void Load(Texture2D mapData) {
            Color[, ] colorCodes = new Color[mapData.Width, mapData.Height];

            for (int x = 0; x < mapData.Width; x++)
                for (int y = 0; y < mapData.Height; y++) {
                    world[x, y].texture = GetTextureByRGB(colorCodes[x, y]);
                }

        }
        public void renderAll() {//tijdelijk, wereld zou eigen class moeten hebben ipl in world loader te zitten    
            for (int x = 0; x < mapData.Width; x++)
                for (int y = 0; y < mapData.Height; y++)
        }

    }
}
