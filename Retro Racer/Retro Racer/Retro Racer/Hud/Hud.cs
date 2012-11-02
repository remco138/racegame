using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RetroRacer
{
    class Hud
    {
        private SpriteFont font;
        private Texture2D debugTexture;
        Vector2 maxFontHeight;//the (roughly measured)maximal height and width of a character

        private Vector2[] hudLocation;
        private Rectangle[] fuelLeft;
        private Rectangle[] maxFuel;
        private Rectangle[] healthLeft;
        private Rectangle[] maxHealth;

        public Hud(ContentManager content)
        {
            
            font = content.Load<SpriteFont>("Fonts/Pericles Light");
            debugTexture = content.Load<Texture2D>("Tiles/White");

            maxFontHeight  = font.MeasureString("M");
            maxFontHeight.X = 0;

            fuelLeft = new Rectangle[2]; //hardcoded
            maxFuel = new Rectangle[2];

            healthLeft = new Rectangle[2];
            maxHealth = new Rectangle[2]; 
        }

        public void draw(SpriteBatch spriteBatch, Track track)//List<Car> cars)
        {
            int barLength = 100;
            int barHeight = 10;

            hudLocation = new Vector2[] { new Vector2(18, 18), new Vector2(track.WidthInPixels - 255, 18) };

            /*
            Rectangle[] fuelLeft = new Rectangle[track.cars.Count()];
            Rectangle[] maxFuel = new Rectangle[track.cars.Count()];

            Rectangle[] healthLeft = new Rectangle[track.cars.Count()];
            Rectangle[] maxHealth = new Rectangle[track.cars.Count()]; 
            */
            for(int i = 0; i < track.cars.Count(); i++)
            {
                //fuel and health bars
                fuelLeft[i] = new Rectangle((int)track.cars[i].position.X - 20, (int)track.cars[i].position.Y - 13, (int)(track.cars[i].fuel / track.cars[0].maxFuel * barLength), barHeight);
                maxFuel[i] = new Rectangle((int)track.cars[i].position.X - 20, (int)track.cars[i].position.Y - 13, (int)(100f / 100f * barLength), barHeight);

                healthLeft[i] = new Rectangle((int)track.cars[i].position.X - 20, (int)track.cars[i].position.Y - 25, (int)(track.cars[i].health / track.cars[0].maxHealth * barLength), barHeight);
                maxHealth[i] = new Rectangle((int)track.cars[i].position.X - 20, (int)track.cars[i].position.Y - 25, (int)(100f / 100f * barLength), barHeight);

                //Other data which gets printed as text on screen
                Vector2 currentHeight = new Vector2(0, - 10);

                string carNotifier = "car " + i;
                spriteBatch.DrawString(font, carNotifier, hudLocation[i] + currentHeight, Color.BlanchedAlmond);//ADJUST
                currentHeight.Y += (int)font.MeasureString(carNotifier).Y - 10;

                string speedNotifier = "Speed:  " + track.cars[i].acceleration.ToString();
                spriteBatch.DrawString(font, speedNotifier, hudLocation[i] + currentHeight, Color.BlanchedAlmond);//ADJUST
                currentHeight.Y += (int)font.MeasureString(speedNotifier).Y - 10;

                string lapsNotifier = "Rounds: " + track.cars[i].lapsDriven + "/" + track.MAX_LAPS;
                spriteBatch.DrawString(font, lapsNotifier, hudLocation[i] + currentHeight, Color.BlanchedAlmond);//ADJUST
                currentHeight.Y += (int)font.MeasureString(lapsNotifier).Y - 10;


                spriteBatch.Draw(debugTexture, maxFuel[i], Color.Yellow);
                spriteBatch.Draw(debugTexture, fuelLeft[i], Color.Green);

                spriteBatch.Draw(debugTexture, maxHealth[i], Color.Red);
                spriteBatch.Draw(debugTexture, healthLeft[i], Color.Green);
            }

            String timeString = "Time elapsed: "+track.TimeElapsed.Hours+ ":"+track.TimeElapsed.Minutes+":"+track.TimeElapsed.Seconds;
            spriteBatch.DrawString(font, timeString, new Vector2(640.0f, 300.0f), Color.Black);
        }

        void drawText(string text)
        {

        }
    }
}
