using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using WoodworkBerserk.Controllers;
using WoodworkBerserk.Models;

namespace WoodworkBerserk
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game, IActionHandler
    {
        IGameServer gameServer;
        State state;
        Controllers.IKeyboardInputHandler kinput;
        Models.ISettings settings;
        private GameMap map;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Animation animationDown, animationUp, animationLeft;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }
        
        public void MoveUp()
        {
            gameServer.Send(PlayerAction.MoveUp);
        }
        public void MoveDown()
        {
            gameServer.Send(PlayerAction.MoveDown);
        }
        public void MoveRight()
        {
            gameServer.Send(PlayerAction.MoveRight);
        }
        public void MoveLeft()
        {
            gameServer.Send(PlayerAction.MoveLeft);
        }
        public void Interact()
        {
            gameServer.Send(PlayerAction.Interact);
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
            gameServer = new LocalGameServer();
            gameServer.Connect();
            state = gameServer.Receive();
            map = new GameMap(GraphicsDevice, Content, 32, 32, 100, 100);
            kinput = new KeyboardInputHandler();
            settings = new DefaultSettings(this);
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
            kinput.HandleInput(kstate);

            /*
             * HAVE TO SET UP CONNECTION ON STARTUP
             * maintain connection?
             * 
             * Receive updated game state from server (incl. other user behavior)
             *  May fail, will be received again later.
             */
            state = gameServer.Receive();

            animationLeft.Update(gameTime);
            animationUp.Update(gameTime);
            animationDown.Update(gameTime);
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
