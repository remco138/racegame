using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace racegame
{
    class Hud
    {
        private SpriteFont font;
        private Texture2D debugTexture;
        Vector2 maxFontHeight;//the (roughly measured)maximal height and width of a character

        private Vector2 carHudLocation1, carHudLocation2;

        public Hud(ContentManager content)
        {
            carHudLocation1 = new Vector2(10, 10);
            carHudLocation1 = new Vector2(10, 50);

            font = content.Load<SpriteFont>("Fonts/Pericles Light");
            debugTexture = content.Load<Texture2D>("Tiles/White");

            maxFontHeight  = font.MeasureString("M");
            maxFontHeight.X = 0;
        }
        int temp;
        bool isAscending;
        public void draw(SpriteBatch spriteBatch, Track track)//List<Car> cars)
        {


            //Rectangle damage = track.cars[0].maxHealth / track.cars[0].damage;
            int barLength = 100;
            int barHeight = 10;
            Rectangle fuelLeft = new Rectangle((int)track.cars[0].position.X - 20, (int)track.cars[0].position.Y - 25, (int)(track.cars[0].fuel / track.cars[0].maxFuel * barLength), barHeight);
            Rectangle maxFuel = new Rectangle((int)track.cars[0].position.X - 20, (int)track.cars[0].position.Y - 25, (int)(100f / 100f * barLength), barHeight);

            Rectangle healthLeft = new Rectangle((int)track.cars[0].position.X - 20, (int)track.cars[0].position.Y - 13, (int)(track.cars[0].health / track.cars[0].maxHealth * barLength), barHeight);
            Rectangle maxHealth = new Rectangle((int)track.cars[0].position.X - 20, (int)track.cars[0].position.Y - 13, (int)(100f / 100f * barLength), barHeight);


            spriteBatch.DrawString(font, Convert.ToString("Speed km/h car 1: " + track.cars[0].acceleration.ToString()), carHudLocation1, Color.BlanchedAlmond);
            
            spriteBatch.DrawString(font, Convert.ToString("Speed km/h car 2: " + track.cars[1].acceleration.ToString()), carHudLocation2, Color.BlanchedAlmond);

            spriteBatch.Draw(debugTexture, maxFuel, Color.Yellow);
            spriteBatch.Draw(debugTexture, fuelLeft, Color.Green);

            spriteBatch.Draw(debugTexture, maxHealth, Color.Red);
            spriteBatch.Draw(debugTexture, healthLeft, Color.Green);
        }
    }
}
