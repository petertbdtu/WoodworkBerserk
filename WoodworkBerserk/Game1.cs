using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using WoodworkBerserk.Models;

namespace WoodworkBerserk
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        Texture2D ballTexture;
        Vector2 ballPosition;
        KeyboardState ks;
        //float ballSpeed;
        Controllers.IWBKeyboardInputHandler kinput;
        Models.IWBSettings settingstest;
        Models.WBDefaultSettings settings;
        private GameMap map;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Animation animationDown, animationUp, animationLeft;
        private int direction;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

       // public void BallUp(float elapsedTimeInSeconds)
       // {
       //     ballPosition.Y -= ballSpeed * elapsedTimeInSeconds;
       // }
       // public void BallDown(float elapsedTimeInSeconds)
       // {
       //     ballPosition.Y += ballSpeed * elapsedTimeInSeconds;
       // }
       // public void BallLeft(float elapsedTimeInSeconds)
       // {
       //     ballPosition.X -= ballSpeed * elapsedTimeInSeconds;
       // }
       // public void BallRight(float elapsedTimeInSeconds)
       // {
       //     ballPosition.X += ballSpeed * elapsedTimeInSeconds;
       // }
        public void MoveUp(float elapsedTimeInSeconds)
        {
            map.Camera.Move(new Vector2(0, -1));
            animationUp.Draw(spriteBatch);
        }
        public void MoveDown(float elapsedTimeInSeconds)
        {
            map.Camera.Move(new Vector2(0, 1));
            animationDown.Draw(spriteBatch);
        }
        public void MoveRight(float elapsedTimeInSeconds)
        {
            map.Camera.Move(new Vector2(1, 0));
        }
        public void MoveLeft(float elapsedTimeInSeconds)
        {
            map.Camera.Move(new Vector2(-1, 0));
            animationLeft.Draw(spriteBatch);
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
            //ballPosition = new Vector2(graphics.PreferredBackBufferWidth / 2,
            //    graphics.PreferredBackBufferHeight / 2);
            //ballSpeed = 100f;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            map = new GameMap(GraphicsDevice, Content, 32, 32, 100, 100);
            kinput = new Controllers.WBKeyboardInputHandler();
            settingstest = new Models.WBDefaultSettings();
            settingstest.SetupKeyboardInputHandler(kinput);
            // Would like WBDefaultSettings to do this but I'm not sure where make delegates.
            //settingstest.TestingAddAction(Keys.Up, BallUp);
            //settingstest.TestingAddAction(Keys.Down, BallDown);
            //settingstest.TestingAddAction(Keys.Left, BallLeft);
            //settingstest.TestingAddAction(Keys.Right, BallRight);
            settingstest.TestingAddAction(Keys.Up, MoveUp);
            settingstest.TestingAddAction(Keys.Down, MoveDown);
            settingstest.TestingAddAction(Keys.Right, MoveRight);
            settingstest.TestingAddAction(Keys.Left, MoveLeft);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures

            // TODO: use this.Content to load your game content here
            //ballTexture = Content.Load<Texture2D>("ball");
            animationLeft = new Animation(Content.Load<Texture2D>("hero"), 3, 4, 0);
            animationUp = new Animation(Content.Load<Texture2D>("hero"), 3, 4, 16);
            animationDown = new Animation(Content.Load<Texture2D>("hero"), 3, 4, 32);
            animationLeft.updateTime = 1f / 5;
            animationUp.updateTime = 1f / 5;
            animationDown.updateTime = 1f / 5;
            Vector2 position = new Vector2(graphics.PreferredBackBufferWidth / 2,
                graphics.PreferredBackBufferHeight / 2);
            animationLeft.position = position;
            animationUp.position = position;
            animationDown.position = position;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void Up(float elapsedTimeinSeconds)
        {

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
            animationLeft.Update(gameTime);
            animationUp.Update(gameTime);
            animationDown.Update(gameTime);
            //ballPosition.X = Math.Min(Math.Max(ballTexture.Width / 2, ballPosition.X), graphics.PreferredBackBufferWidth - ballTexture.Width / 2);
            //ballPosition.Y = Math.Min(Math.Max(ballTexture.Height / 2, ballPosition.Y), graphics.PreferredBackBufferHeight - ballTexture.Height / 2);
            settings = new Models.WBDefaultSettings();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.TransparentBlack);

            // TODO: Add your drawing code here
            map.draw(spriteBatch);
            int[][] terrain = new int[100][100];
            spriteBatch.Begin();
            animationLeft.loop = true;
            animationLeft.Draw(spriteBatch);
            //spriteBatch.Draw(ballTexture, ballPosition, null, Color.White, 0f,
            //    new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
            //    Vector2.One, SpriteEffects.None, 0f );
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
