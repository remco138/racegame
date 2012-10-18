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
    enum TileCollision
    {
        Solid,
        Road,
        Grass,
        Dirt,
        Water,
        Checkpoint,
        Pitstop,
        StartFinish
    }

    struct Tile // verander naar class? -> gaat null pointer opleveren bij 2dim array
    {
        public Texture2D Texture;
        public TileCollision Collision;

        public const int Width = 16;
        public const int Height = 16;

        public static readonly Vector2 Size = new Vector2(Width, Height);

        public Tile(Texture2D texture, TileCollision collision)
        {
            Texture = texture;
            Collision = collision;
        }
    }
}