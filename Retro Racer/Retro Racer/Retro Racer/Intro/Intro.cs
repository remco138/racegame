using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RetroRacer
{
    class Intro
    {
        // Creating the Texture.
        Texture2D Splash;

        // Declaring the variables for the Update method.
        int alphaValue = 1;
        int fadeIncrement = 3;
        double fadeDelay = 1;

        SoundEffect letsGetReadyToRumble;

        public Intro(ContentManager Content)
        {
            // Use the ContentManager to load the the Splash image into the Texture2D object
            Splash = Content.Load<Texture2D>("Intro/Splash");
            letsGetReadyToRumble = Content.Load<SoundEffect>("Sounds/LetsGetReadyToRumble"); // Loading the LetsGetReadyToRumble sound
        }

        /// <summary>
        /// Update will be used for the fading of the Splash image.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (gameTime.ElapsedGameTime.Milliseconds < 1)
            {
                letsGetReadyToRumble.Play(); // Play the LetsGetReadyToRumble sound
            }

            // Here the image will fade in. 
            fadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
            if (fadeDelay <= 0) // fadeDelay starts with value 1.
            {
                fadeDelay = .025;
                alphaValue += fadeIncrement; // alphaValue will increase by 3.
            }
            if (alphaValue >= 255 || alphaValue <= 0)
            {
                fadeIncrement *= 1;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Texture Splash will be set to white and then will be slowly drawn into original color (see Update method).
            spriteBatch.Draw(Splash, new Vector2(0.0f, 0.0f), new Color(255, 255, 255, (byte)MathHelper.Clamp(alphaValue, 0, 255)));
        }
    }
}
