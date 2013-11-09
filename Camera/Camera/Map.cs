using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Camera
{
    class Map
    {
        public Texture2D Texture { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public int X { get { return (int)Position.X; } }
        public int Y { get { return (int)Position.Y; } }

        public Vector2 Position { get; set; }
        public Rectangle Bounds { get; set; }

        public int TileSize { get; set; }

        public Map(int width, int height, Texture2D texture)
        {
            Width = width;
            Height = height;

            Texture = texture;

            TileSize = 32;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    spriteBatch.Draw(Texture, new Rectangle(X + x * TileSize, Y + y * TileSize, TileSize, TileSize), Color.White);
                }
            }
        }
    }
}
