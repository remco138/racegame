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

namespace GameProject
{
    enum DrawOption
    {
        /// <summary>
        /// Draw everything in the world including the objects like the ladders for example.
        /// </summary>
        All = 0,

        /// <summary>
        /// Draw only the world Tiles. (Not the ladder & switches etc.
        /// </summary>
        World = 1,

        /// <summary>
        /// Draw only the 'Objects' like ladders & switches.
        /// </summary>
        Objects = 2,
    }


    class WorldLoader
    {
        World world;

        Texture2D worldTexture;

        private Block[,] blocks; // Contains all the different blocks, its a 2d array filled according to an image. (Where different pixel colors represent the different blocks.)

        /// <summary>
        /// Width of the world measured in blocks.
        /// </summary>
        public int Width
        {
            get { return blocks.GetLength(0); }
        }

        /// <summary>
        /// Height of level measured in blocks.
        /// </summary>
        public int Height
        {
            get { return blocks.GetLength(1); }
        }

        /// <summary>
        /// Width of the world in pixels
        /// </summary>
        public int WidthPixel
        {
            get { return Width * Block.Width; }
        }

        /// <summary>
        /// Height of the world in pixels
        /// </summary>
        public int HeightPixel
        {
            get { return Height * Block.Height; }
        }

        /// <summary>
        /// Get the rectangle of the current world.
        ///
        public Rectangle WorldRectangle
        {
            get { return new Rectangle(0, 0, WidthPixel, HeightPixel); }
        }


        

        # region Loading

        public WorldLoader(World world, Texture2D worldTexture)
        {
            this.worldTexture = worldTexture;
            this.world = world; // Get the current world, so the game can acceses the contentManager and thereby load the character sprite.
            loadContent();
        }


        public void loadContent()
        {
            // 1. Load world texture
            //      - Set the window height & width accoring to the level image
            // 2. Scan the whole picture and according to the color of the pixel add the different blocks to the world.
            // 3. 

            blocks = new Block[worldTexture.Width, worldTexture.Height]; // Set the width & height of the blocks.

            
            // 2. Scan the whole picture and according to the color of the pixel add the different blocks to the world.
            for (int i = 0; i < Width; i++) // Loop trough the (horizontal) width of the worldTexture.
            {
                for (int j = 0; j < Height; j++)  // Loop trough the (vertical) height of the worldTexture.
                {
                    // LOAD ALL THE BLOCKS INTO THE BLOCKS ARRAY[][] !!!!!
                    Color currentColor = GetPixelColor(worldTexture, i, j);
                    blocks[i, j] = LoadBlock(currentColor, i, j);  // Load each block depending on the color of the pixel
                }
            }
            Console.WriteLine("World Width in blocks:  " + Width +       "      World Height in blocks: " + Height);
            Console.WriteLine("World Width in pixels: " + WidthPixel +   "      World height in pixels: " + HeightPixel);
        }


        /// <summary>
        /// Get the pixel color at the given coordinates.
        /// </summary>
        /// <param name="texture">
        /// The texture containing the color to get.
        /// </param>
        /// <param name="x">
        /// The x position of the color in the texture.
        /// </param>
        /// <param name="7">
        /// The y position of the color in the texture.
        /// </param>
        public Color GetPixelColor(Texture2D texture, int x, int y)
        {
            Rectangle sourceRectangle = new Rectangle(x, y, 1, 1); // Make new rectangle with 1 pixel in width and height.
            Color[] retrievedColor = new Color[1];

            texture.GetData<Color>(0, sourceRectangle, retrievedColor, 0, 1);

            return retrievedColor[0]; // Return the color that was found
        }

        /// <summary>
        /// Loads an individual block's appearance and behavior.
        /// </summary>
        /// <param name="blockType">
        /// The character loaded from the structure file which
        /// indicates what should be loaded.
        /// </param>
        /// <param name="x">
        /// The X location of this block in block space.
        /// </param>
        /// <param name="y">
        /// The Y location of this block in block space.
        /// </param>
        /// <returns>The loaded block.</returns>
        private Block LoadBlock(Color blockColor, int x, int y)
        {
            Color nothing = new Color(000, 000, 000);

            Color start = new Color(000, 255, 000); // green
            Color exit  = new Color(000, 255, 255); // greenish

            Color ak47_ammo = new Color(000, 100, 250); // Blue ish
            Color usp_ammo  = new Color(000, 150, 250);
            Color health    = new Color(100, 100, 250);
            Color armor     = new Color(100, 150, 250);
            Color points    = new Color(150, 100, 250);

            Color metal     = new Color(255, 255, 255); // White
            Color dirt      = new Color(080, 040, 000); // Brown
            Color dirtGrass = new Color(030, 050, 025); // Brown-dark
            Color dirtTop   = new Color(085, 075, 050); // Green-dark

            List<Color> corneredBlockColors = new List<Color>(new Color[] { metal, dirt, dirtGrass, dirtTop });
            BlockCorner cornerType = getCornerType(blockColor, x, y, corneredBlockColors);

            Color ladder    = new Color(200, 100, 000); // Orange

            Color enemy       = new Color(255, 000, 000);
            Color enemyRanged = new Color(255, 100, 000);

  
                // START / EXIT
            if (blockColor.Equals(nothing))           // Color black, nothing, passable
                return new Block(null, BlockCollision.Passable);

            else if (blockColor.Equals( start ))      // Color Green, starting position
                return LoadStart(x, y);

            else if (blockColor.Equals( exit ))       // Color Green, starting position
                return LoadExit(x, y);
            

                // POWER UPS    

            if (blockColor.Equals(ak47_ammo ))       
                return LoadPowerup(PickupType.ammo_762_39mm, world.Content.Load<Texture2D>("Pickups/762_39mm_ammo"), x, y);

            else if (blockColor.Equals(usp_ammo))      
                return LoadPowerup(PickupType.ammo_9mm,      world.Content.Load<Texture2D>("Pickups/9mm_ammo"), x, y);

            else if (blockColor.Equals(health))      
                return LoadPowerup(PickupType.health,        world.Content.Load<Texture2D>("Pickups/health"), x, y);

            else if (blockColor.Equals(armor))     
                return LoadPowerup(PickupType.armor,         world.Content.Load<Texture2D>("Pickups/Armor"), x, y);

            else if (blockColor.Equals(points))
            {
                Animation animation = new Animation(world.Content.Load<Texture2D>("Pickups/PointsAnimationSmall"), 0.3f, true);
                return LoadBouncyPowerup(PickupType.points_small, animation, x, y);
            }


                // BLOCKS

            if (blockColor.Equals(metal))      
                return LoadBlock("Metal", BlockCollision.Impassable, cornerType);

            else if (blockColor.Equals(dirt))       
                return LoadBlock("Dirt", BlockCollision.Impassable, cornerType);

            else if (blockColor.Equals(dirtGrass))       
                return LoadBlock("DirtGrass", BlockCollision.GrassyTop, cornerType);

            else if (blockColor.Equals(dirtTop))    
                return LoadBlock("DirtTop", BlockCollision.Impassable, cornerType);

            else if (blockColor.Equals(ladder))       // Color Orange 200,100,0, Ladder Block
                return LoadBlock("3025 Ladder", BlockCollision.Ladder);


                // ENEMIES

            if (blockColor.Equals(enemy))       // Color Red Enemy Block Block
                return LoadEnemy(EnemyType.unarmed, "Sprites/Enemy Normal A/", x, y);

            else if (blockColor.Equals(enemyRanged))       // Color Red-ish Enemy Block Block
                return LoadEnemy(EnemyType.ranged, "Sprites/Enemy Normal A Weapon/", x, y);

            else
                throw new NotSupportedException(String.Format("Unsupported block Color {0} at position {1}, {2}.", blockColor, x, y));
        }


        /// <summary>
        /// ONLY gets used in LoadBlock() to get the cornerType
        /// </summary>
        private BlockCorner getCornerType(Color blockColor, int x, int y, List<Color> exceptionBlocks)
        {
  
            int previous_x = (int)MathHelper.Clamp(x - 1, 0, Width);
            int next_x = (int)MathHelper.Clamp(x + 1, 0, Width - 1);
            Color previousBlockColor = GetPixelColor(worldTexture, previous_x, y);
            Color nextBlockColor = GetPixelColor(worldTexture, next_x, y);

            BlockCorner cornerType = BlockCorner.none;

            bool prevIsException = false; // if previous block-color is one of the exception blocks, this sets to true
            bool nextIsException = false; // if next block-color is one of the exception blocks, this sets to true

            foreach (Color excColor in exceptionBlocks)
            {
                if (previousBlockColor.Equals(excColor)) 
                    prevIsException = true;

                if (nextBlockColor.Equals(excColor))
                    nextIsException = true;
            }

                    // if previous is NOT one of these two                   && if next IS one of these two
            if ( !(previousBlockColor.Equals(blockColor) || prevIsException) && (nextBlockColor.Equals(blockColor) || nextIsException))
                cornerType = BlockCorner.left;

                    // if next is NOT one of these two                   && if previous IS one of these two
            if ( !(nextBlockColor.Equals(blockColor) || nextIsException) && (previousBlockColor.Equals(blockColor) || prevIsException))
                cornerType = BlockCorner.right;

                    // if previous is NOT one of these two                   && if next is NOT of these two
            if ( !(previousBlockColor.Equals(blockColor) || prevIsException) && !(nextBlockColor.Equals(blockColor) || nextIsException))
                cornerType = BlockCorner.top;

            return cornerType;
        }

        /// <summary>
        /// Creates a new block. The other block loading methods typically chain to this
        /// method after performing their special logic.
        /// </summary>
        /// <param name="name">
        /// Path to a block texture relative to the Content/Blocks directory.
        /// </param>
        /// <param name="collision">
        /// The block collision type for the new block.
        /// </param>
        /// <returns>The new block.</returns>
        private Block LoadBlock(string name, BlockCollision collision)
        {
            return new Block(world.Content.Load<Texture2D>("Blocks/" + name), collision);
        }

        private Block LoadBlock(string folder, BlockCollision collision, BlockCorner cornerType)
        {
            int random = RandomNumber(0, 1000);

            switch (cornerType)
            {
                case BlockCorner.left:
                    if (random < 700)
                    {
                        if (RandomNumber(0, 2000) < 1000)
                            return LoadBlock(folder + "/leftb", collision);
                        else
                            return LoadBlock(folder + "/left", collision);
                    }
                    else
                        return LoadBlock(folder + "/none", collision);


                case BlockCorner.right:
                    if (random < 700)
                    {
                        if (RandomNumber(0, 2000) < 1000)
                            return LoadBlock(folder + "/rightb", collision);
                        else
                            return LoadBlock(folder + "/right", collision);
                    }
                    else
                        return LoadBlock(folder + "/none", collision);


                case BlockCorner.top:
                    return LoadBlock(folder + "/top", collision);

                default:
                    return LoadBlock(folder + "/none", collision);
            }
        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }


        private Block LoadStart(int x, int y)
        {
            Rectangle tempBlockRect = GetBlockRectangle(x, y);
            world.Start = new Vector2(tempBlockRect.Left, tempBlockRect.Top);

            return new Block(null, BlockCollision.Passable);
        }
        private Block LoadExit(int x, int y)
        {
            Rectangle tempBlockRect = GetBlockRectangle(x, y);
            world.Exit = new Point(tempBlockRect.Left, tempBlockRect.Top);

            return new Block(world.Content.Load<Texture2D>("Exit"), BlockCollision.Passable);
        }

        private Block LoadEnemy(EnemyType type, string spriteFolder, int x, int y)
        {
            // Find the starting position
            Rectangle tempBlockRect = GetBlockRectangle(x, y);
            Vector2 startPosition = RectangleExtensions.GetBottomCenter(tempBlockRect);

            world.enemies.Add(new Enemy(type, 10, startPosition, FaceDirection.Right, world, spriteFolder));

            // return an empty & passable block.
            return new Block(null, BlockCollision.Passable);
        }

        private Block LoadPowerup(PickupType pickupType, Texture2D texture, int x, int y)
        {
            world.pickups.Add(new Pickup(pickupType, new Vector2(x, y),true, false, texture));

            return new Block(null, BlockCollision.Passable);
        }
        private Block LoadBouncyPowerup(PickupType pickupType, Animation animation, int x, int y)
        {
            world.pickups.Add(new Pickup(pickupType, new Vector2(x, y), true, true, animation));

            return new Block(null, BlockCollision.Passable);
        }

        #endregion


        #region Collision methods

        /// <summary>
        /// Gets the bounding rectangle of a block in world space.
        /// If the Block has collision GrassyTop then the height and y of this block is altered.
        /// </summary>        
        public Rectangle GetBlockRectangle(int x, int y)
        {
            // Make sure the player is still inside the world before accessing the blocks array.
            if (x >= 0 && x < Width)
            {
                if (blocks[x, y].Collision == BlockCollision.GrassyTop)
                {
                    // The altered rectangle for the grassyTop Block.
                    return new Rectangle(x * Block.Width, (y * Block.Height) + 9, Block.Width, Block.Height);
                }
            }

            // Return the standard block Rectangle.
            return new Rectangle(x * Block.Width, y * Block.Height, Block.Width, Block.Height);
        }

        /// <summary>
        /// Gets the collision mode of the block at a particular location.
        /// This method handles blocks outside of the levels boundries by making it
        /// impossible to escape past the left or right edges, but allowing things
        /// to jump beyond the top of the level and fall off the bottom.
        /// </summary>
        public BlockCollision GetCollision(int x, int y)
        {
            // Prevent escaping past the level ends.
            if (x < 0 || x >= Width)
            {
                return BlockCollision.Impassable;
            }
            // Allow jumping past the level top and falling through the bottom.
            if (y < 0 || y >= Height)
                return BlockCollision.Passable;

            return blocks[x, y].Collision;
        }

        public BlockCollision GetBlockCollisionBelowPlayer(Vector2 characterOrigin)
        {
            // Get the number of the block to get it from the blocks Array.

            int x = (int)Math.Round(characterOrigin.X / Block.Width);
            int y = (int)Math.Round(characterOrigin.Y / Block.Height);
          
            // Prevent escaping past the level ends.
            if (x < 0 || x >= Width)
                return BlockCollision.Impassable;
            // Allow jumping past the level top and falling through the bottom.
            if (y < 0 || y >= Height)
                return BlockCollision.Passable;

            return blocks[x, y].Collision;
        }


        public BlockCollision GetBlockCollisionTwoBelowPlayer(Vector2 characterOrigin)
        {
            // Get the number of the block to get it from the blocks Array.

            int x = (int)Math.Round(characterOrigin.X / Block.Width);
            int y = (int)Math.Round(characterOrigin.Y / Block.Height);
            y += 3;
            // Prevent escaping past the level ends.
            if (x < 0 || x >= Width)
                return BlockCollision.Impassable;
            // Allow jumping past the level top and falling through the bottom.
            if (y < 0 || y >= Height)
                return BlockCollision.Passable;

            return blocks[x, y].Collision;
        }

        public BlockCollision GetBlockCollisionBehindPlayer(Vector2 characterOrigin)
        {
            // Get the number of the block to get it from the blocks Array.
            int x = (int)Math.Round(characterOrigin.X / Block.Width);
            int y = (int)Math.Ceiling(characterOrigin.Y / Block.Height);
            y += 1;

            // Prevent escaping past the level ends.
            if (x < 0 || x >= Width)
                return BlockCollision.Impassable;
            // Allow jumping past the level top and falling through the bottom.
            if (y < 0 || y >= Height)
                return BlockCollision.Passable;

            return blocks[x, y].Collision; 
        }

        public void DebugSetBlockTexture(int x, int y)
        {
            blocks[x, y].Texture = world.Content.Load<Texture2D>("Pixel");
        }

        #endregion


        #region Draw

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, DrawOption drawOption)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Texture2D texture = blocks[x, y].Texture;
                    if (texture != null)
                    {  
                        // Find out what to draw.
                        DrawChoice(drawOption, x, y, blocks[x, y], spriteBatch);

                    }
                }

            } 
        }


        /// <summary>
        /// This method contains the various options the worldLoader can draw. 
        /// Depending on the drawOption, this method draws one of the following things: 
        /// 1. Only objects, 2. Only static world objects or 3. All of the previously mentioned.
        /// This method is used to draw the objects on a different layer in the World Class.
        /// For example, the grass of the grass block needs to be drawed in front of the character, while the ladder
        /// on the other hand needs to be drawn behind the character.
        /// </summary>
        /// <param name="drawOption">The option to draw.</param>
        /// <param name="x">x Position of the Block</param>
        /// <param name="y">y Position of the Block</param>
        /// <param name="currentBlock">The Block that needs to be drawn</param>
        /// <param name="spriteBatch">Spritebatch</param>
        public void DrawChoice(DrawOption drawOption, int x, int y, Block currentBlock, SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(x, y) * Block.Size;

            if (drawOption == DrawOption.Objects || drawOption == DrawOption.All)
            {
                // Contains all the Blocks that are considered 'objects':
                //
                //      - The Ladders
                //

                // Draw the ladder object
                if (currentBlock.Collision == BlockCollision.Ladder || currentBlock.Collision == BlockCollision.Passable)
                {
                    float cameraDistance = 0.6f;
                    spriteBatch.Draw(currentBlock.Texture, position, null, Color.White, 0.0f, new Vector2(0.0f ,0.0f), 1.0f, SpriteEffects.None, cameraDistance);
                }

            }
            if (drawOption == DrawOption.World || drawOption == DrawOption.All)
            {
                // Contains all the Blocks that are considered to be part of the solid 'World':
                //
                //      - Anything that is not a ladder 
                //

                // Draw everything that is not a ladder
                if (currentBlock.Collision != BlockCollision.Ladder && currentBlock.Collision != BlockCollision.Passable)
                {
                    float cameraDistance = 0.4f;
                    spriteBatch.Draw(currentBlock.Texture, position, null, Color.White, 0.0f, new Vector2(0.0f, 0.0f), 1.0f, SpriteEffects.None, cameraDistance);
                }
            }

        }

        #endregion
    }
}
