using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace racegame
{
    enum PowerupType
    {
        fuel,
        health
    }

    class Powerup : MovableObject
    {
        public PowerupType PowerupType
        {
            get { return powerupType; }
        }
        PowerupType powerupType;

        private bool isBouncy;
        private float bounce;

        public Vector2 BouncingPosition
        {
            get { return position + new Vector2(0.0f, bounce); }
        }
        public Rectangle BouncingRectangle
        {
            get { return new Rectangle((int)BouncingPosition.X, (int)BouncingPosition.Y, Width, Height); }
        }


        public bool isPickedUp = false;
        public bool isActive = true;
        private float respawnTimer = 0.0f;
        private const float POWERUP_RESPAWN_TIME = 25.0f;
        
        // Base Constructor
        public Powerup(PowerupType powerupType, Vector2 position, Texture2D texture, bool isTilePosition, bool isBouncy)
            : base(position, texture)
        {
            this.powerupType = powerupType;

            if (isTilePosition)
                this.position = new Vector2(position.X * Tile.Width, position.Y * Tile.Height);
            else
                this.position = position;

            this.isBouncy = isBouncy;
            this.isBouncy = true;
            this.bounce = 0.0f;
        }

        public new void Update(GameTime gameTime)
        {
            if (isBouncy)
            {
                // Bounce control constants
                const float BounceHeight = 0.18f;
                const float BounceRate = 6.2f;
                const float BounceSync = -0.75f;

                // Bounce along a sine curve over time.
                // Include the X coordinate so that neighboring gems bounce in a nice wave pattern.            
                double t = gameTime.TotalGameTime.TotalSeconds * BounceRate + position.X * BounceSync;
                bounce = (float)Math.Sin(t) * BounceHeight * Height;
            }

            // The timer used to respawn this powerup. It's triggered inside Track.cs by the isPickedUp variable
            //
            if (isPickedUp || respawnTimer > 0.0f)
            {
                respawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                isActive = false;

                if (respawnTimer > POWERUP_RESPAWN_TIME)
                {
                    isActive = true;
                    respawnTimer = 0.0f;
                    Console.WriteLine("Powerup is respawned");
                }
            }

            isPickedUp = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
               // base.Draw(spriteBatch);

                spriteBatch.Draw(texture,
                    BouncingRectangle,
                    null,
                    Color.White,
                    Rotation,
                    Origin,
                    SpriteEffects.None, 0);
            }
        }




    }
}
