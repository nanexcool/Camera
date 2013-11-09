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

        Viewport defaultViewport;
        Viewport gameplayViewport;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        Random random = new Random();

        Player player;
        Map map;
        Camera camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            defaultViewport = GraphicsDevice.Viewport;

            gameplayViewport = new Viewport(100, 100, defaultViewport.Width - 200, defaultViewport.Height - 100);
            
            map = new Map(20, 18, Content.Load<Texture2D>("bg"));
            map.Position = new Vector2(10, 10);

            player = new Player()
            {
                Texture = Content.Load<Texture2D>("Octocat"),
                Position = new Vector2(gameplayViewport.Width / 2, gameplayViewport.Height / 2)
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
            player.Position = Vector2.Clamp(player.Position, new Vector2(map.X, map.Y), new Vector2(map.Bounds.Width - player.Width, map.Bounds.Height - player.Height));

            p = Vector2.Zero;

            if (state.IsKeyDown(Keys.Left))
            {
                p.X = -1;
                gameplayViewport.X -= 1;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                p.X = 1;
                gameplayViewport.X += 1;
            }
            if (state.IsKeyDown(Keys.Up))
            {
                p.Y = -1;
                gameplayViewport.Y -= 1;
            }
            if (state.IsKeyDown(Keys.Down))
            {
                p.Y = 1;
                gameplayViewport.Y += 1;
            }

            if (random.NextDouble() < 0.005)
            {
                gameplayViewport.X = random.Next(0, 100);
                gameplayViewport.Y = random.Next(0, 100);
            }
            
            // Clamp gameplayViewport to Window
            gameplayViewport.X = (int)MathHelper.Clamp(gameplayViewport.X, 0, defaultViewport.Width - gameplayViewport.Width);
            gameplayViewport.Y = (int)MathHelper.Clamp(gameplayViewport.Y, 0, defaultViewport.Height - gameplayViewport.Height);

            camera.Update(elapsed);

            sb.Clear();
            sb.AppendLine(gameplayViewport.Bounds.ToString());
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

            GraphicsDevice.Viewport = gameplayViewport;

            spriteBatch.Begin();
            spriteBatch.Draw(map.Texture, new Rectangle(0, 0, gameplayViewport.Width, gameplayViewport.Height), Color.Black);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, camera.Transform);
            //spriteBatch.Begin();
            
            
            map.Draw(spriteBatch);

            player.Draw(spriteBatch);

            spriteBatch.End();

            GraphicsDevice.Viewport = defaultViewport;

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
