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

        public Hud(ContentManager content)
        {
            font = content.Load<SpriteFont>("Fonts/Pericles Light");
        }
        public void draw(SpriteBatch spriteBatch, Track track)//List<Car> cars)
        {
            //draw awesome art here

            //for (int i = 0; i < cars.Count(); i++)
            //{
            track.
                spriteBatch.DrawString(font, Convert.ToString(320), new Vector2(10, 10), Color.BlanchedAlmond);
            //}
        }
    }
}
