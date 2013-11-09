using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Camera
{
    class Camera
    {

        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        
        public Vector2 Center { get; set; }
        public Vector2 Origin { get; set; }

        public float Rotation { get; set; }
        public Matrix Transform { get; set; }
        public Player Focus { get; set; }

        public Rectangle Bounds { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        float scale;

        public int X { get { return (int)Math.Floor(position.X); } }
        public int Y { get { return (int)Math.Floor(position.Y); } }

        public Camera(Game game)
        {
            Width = game.GraphicsDevice.Viewport.Width - 200;
            Height = game.GraphicsDevice.Viewport.Height - 100;
            Bounds = game.GraphicsDevice.Viewport.Bounds;
            Center = new Vector2(Width / 2, Height / 2);
            scale = 1;
            Rotation = 0;
        }

        public void Update(float elapsed)
        {
            Transform = Matrix.Identity *
                        Matrix.CreateTranslation(-X, -Y, 0) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                        Matrix.CreateScale(new Vector3(scale, scale, scale));

            Origin = Center / scale;

            //position.X += (Focus.Position.X - position.X) * 1.25f * elapsed;
            //position.Y += (Focus.Position.Y - position.Y) * 1.25f * elapsed;
            position.X = Focus.Position.X;
            position.Y = Focus.Position.Y;
        }

    }
}
