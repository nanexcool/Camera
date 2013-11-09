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

namespace Camera
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        Player player;
        Map map;
        Camera camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            map = new Map(10, 10, Content.Load<Texture2D>("bg"));

            player = new Player()
            {
                Texture = Content.Load<Texture2D>("Octocat"),
                Position = new Vector2(GraphicsDevice.Viewport.Width/ 2, GraphicsDevice.Viewport.Height / 2)
            };

            camera = new Camera(this);
            camera.Focus = player;

            font = Content.Load<SpriteFont>("Font");

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 p = Vector2.Zero;

            if (state.IsKeyDown(Keys.A))
            {
                p.X = -1;
            }
            if (state.IsKeyDown(Keys.E))
            {
                p.X = 1;
            }
            if (state.IsKeyDown(Keys.OemComma))
            {
                p.Y = -1;
            }
            if (state.IsKeyDown(Keys.O))
            {
                p.Y = 1;
            }
            
            player.Position += p * 100 * elapsed;
            player.Position = Vector2.Clamp(player.Position, Vector2.Zero, new Vector2(GraphicsDevice.Viewport.Width - player.Width, GraphicsDevice.Viewport.Height - player.Height));

            p = Vector2.Zero;

            if (state.IsKeyDown(Keys.Left))
            {
                p.X = -1;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                p.X = 1;
            }
            if (state.IsKeyDown(Keys.Up))
            {
                p.Y = -1;
            }
            if (state.IsKeyDown(Keys.Down))
            {
                p.Y = 1;
            }

            map.Position += p * 100 * elapsed;

            camera.Update(elapsed);

            sb.Clear();
            sb.AppendLine(camera.Position.ToString());
            sb.AppendLine(map.Position.ToString());
            sb.AppendLine(player.Position.ToString());

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.Viewport = new Viewport(100, 100, 600, 380);
            

            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, camera.Transform);
            //spriteBatch.Begin();
            
            map.Draw(spriteBatch);

            player.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();

            DrawText(spriteBatch, sb);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void DrawText(SpriteBatch spriteBatch, System.Text.StringBuilder sb)
        {
            spriteBatch.DrawString(font, sb, Vector2.One, Color.Black);
            spriteBatch.DrawString(font, sb, Vector2.Zero, Color.White);
        }
    }
}
