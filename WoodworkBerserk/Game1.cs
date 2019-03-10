using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace WoodworkBerserk
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        Texture2D ballTexture;
        Vector2 ballPosition;
        float ballSpeed;
        Controllers.IWBKeyboardInputHandler kinput;
        Models.IWBSettings settingstest;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public void BallUp(float elapsedTimeInSeconds)
        {
            ballPosition.Y -= ballSpeed * elapsedTimeInSeconds;
        }
        public void BallDown(float elapsedTimeInSeconds)
        {
            ballPosition.Y += ballSpeed * elapsedTimeInSeconds;
        }
        public void BallLeft(float elapsedTimeInSeconds)
        {
            ballPosition.X -= ballSpeed * elapsedTimeInSeconds;
        }
        public void BallRight(float elapsedTimeInSeconds)
        {
            ballPosition.X += ballSpeed * elapsedTimeInSeconds;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ballPosition = new Vector2(graphics.PreferredBackBufferWidth / 2,
                graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 100f;

            kinput = new Controllers.WBKeyboardInputHandler();
            settingstest = new Models.WBDefaultSettings();
            settingstest.SetupKeyboardInputHandler(kinput);
            // Would like WBDefaultSettings to do this but I'm not sure where make delegates.
            settingstest.TestingAddAction(Keys.Up, BallUp);
            settingstest.TestingAddAction(Keys.Down, BallDown);
            settingstest.TestingAddAction(Keys.Left, BallLeft);
            settingstest.TestingAddAction(Keys.Right, BallRight);
           
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
            ballTexture = Content.Load<Texture2D>("ball");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();
            kinput.HandleInput(kstate, (float)gameTime.ElapsedGameTime.TotalSeconds);

            ballPosition.X = Math.Min(Math.Max(ballTexture.Width / 2, ballPosition.X), graphics.PreferredBackBufferWidth - ballTexture.Width / 2);
            ballPosition.Y = Math.Min(Math.Max(ballTexture.Height / 2, ballPosition.Y), graphics.PreferredBackBufferHeight - ballTexture.Height / 2);


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(ballTexture, ballPosition, null, Color.White, 0f,
                new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                Vector2.One, SpriteEffects.None, 0f );
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
