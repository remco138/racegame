using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;


namespace GameProject
{
    class MenuStart : Menu
    {
        private Button start, opties, afsluiten;

        public MenuStart(ContentManager Content, String background, Game1 game, GraphicsDeviceManager graphics)
            : base(Content, background, game, graphics)
        {
            start = new Button(Content, "Start", "Menu/Buttons/start", button_Start);
            opties = new Button(Content, "Opties", "Menu/Buttons/opties", button_Opties);
            afsluiten = new Button(Content, "Opties", "Menu/Buttons/afsluiten", button_Afsluiten);

            buttons.Add(start);
            buttons.Add(opties);
            buttons.Add(afsluiten);

            repositionMenu(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }


        private void button_Start(object sender, EventArgs e)
        {
            Console.WriteLine("start");
            game.isRunning = true;
        }

        private void button_Opties(object sender, EventArgs e)
        {
            Console.WriteLine("opties");
        }

        private void button_Afsluiten(object sender, EventArgs e)
        {
            Console.WriteLine("afsluiten");
            game.Exit();
        }

    }
}
