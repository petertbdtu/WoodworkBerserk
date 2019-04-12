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
        //float ballSpeed;
        Controllers.IWBKeyboardInputHandler kinput;
        Models.IWBSettings settings;
        private GameMap map;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Animation animationDown, animationUp, animationLeft;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }
        
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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            map = new GameMap(GraphicsDevice, Content, 32, 32, 100, 100);
            kinput = new Controllers.WBKeyboardInputHandler();
            settings = new Models.WBDefaultSettings(this);
            settings.SetupKeyboardInputHandler(kinput);

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
            int[,] terrain = new int[20,20];
            for (int i = 0; i < 20*20; i++)
            {
                terrain[i / 20, i % 20] = 0;
            }
            Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
            entities.Add(0,new Entity(new Vector2(5,5), 1, true));
            State state = new State(0, terrain, entities);

            map.Draw(spriteBatch, state);
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
