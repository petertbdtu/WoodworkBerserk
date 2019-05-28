using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using WoodworkBerserk.Client;
using WoodworkBerserk.Controllers;
using WoodworkBerserk.Message;
using WoodworkBerserk.Models;

namespace WoodworkBerserk
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game, IActionHandler
    {
        IGameClient gameServer;
        WBClient gameClient;
        State state;
        Controllers.IKeyboardInputHandler kinput;
        Models.ISettings settings;
        private GameMap map;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D character;
        //private SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        public void MoveUp()
        {
            gameServer.SendAction(PlayerAction.MoveUp);
        }
        public void MoveDown()
        {
            gameServer.SendAction(PlayerAction.MoveDown);
        }
        public void MoveRight()
        {
            gameServer.SendAction(PlayerAction.MoveRight);
        }
        public void MoveLeft()
        {
            gameServer.SendAction(PlayerAction.MoveLeft);
        }
        public void Interact()
        {
            gameServer.SendAction(PlayerAction.Interact);
        }
        public void Shutdown()
        {
            // TODO figure out why it's not exiting
            // might fail on a Join() inside.
            gameServer.Disconnect();
            Exit();
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

            gameServer = new AsyncGameClient();

            string username, password;
            do
            {
                Console.Write("Enter username: ");
                username = Console.ReadLine();
                //make readline async

                Console.Write("Enter password: ");
                password = Console.ReadLine();

            } while (gameServer.Connect(username, password));


            state = gameServer.GetState();
            map = new GameMap(GraphicsDevice, Content, 16, 16);
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
            
            // font = Content.Load<SpriteFont>("file");
            character = Content.Load<Texture2D>("characters");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

            //db.disconnect();
            // TODO: Unload any non ContentManager content here

            // Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();
            kinput.HandleInput(kstate);
            state = gameServer.GetState();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.TransparentBlack);
            map.Draw(spriteBatch, state);
            base.Draw(gameTime);
        }
    }
}
