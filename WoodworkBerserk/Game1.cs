﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        Animation animationDown, animationUp, animationLeft;
        Texture2D character;
        DatabaseConnector db;
        int count = 0;
        enum GameState
        {
            Menu,
            Game
        }

        bool loggedIn = false;
        string text = "";
        GameState gameState;
        private SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            gameState = GameState.Menu;
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
            gameServer.Connect();
            state = gameServer.GetState();
            map = new GameMap(GraphicsDevice, Content, 16, 16);
            kinput = new KeyboardInputHandler();
            settings = new DefaultSettings(this);
            settings.SetupKeyboardInputHandler(kinput);
            db = new DatabaseConnector();
 

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures

            // use this.Content to load your game content here
            //animationLeft = new Animation(Content.Load<Texture2D>("characters"), 3, 4, 0);
            //animationUp = new Animation(Content.Load<Texture2D>("hero"), 3, 4, 16);
            //animationDown = new Animation(Content.Load<Texture2D>("hero"), 3, 4, 32);
            //animationLeft.updateTime = 1f / 5;
            //animationUp.updateTime = 1f / 5;
            //animationDown.updateTime = 1f / 5;
            Vector2 position = new Vector2(graphics.PreferredBackBufferWidth / 2,
                graphics.PreferredBackBufferHeight / 2);
            //animationLeft.position = position;
            //animationUp.position = position;
            //animationDown.position = position;
            font = Content.Load<SpriteFont>("file");
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
            if (gameState == GameState.Menu)
            {
                //code for menu
                var kstate = Keyboard.GetState();
                KeyboardInputHandler handler = new KeyboardInputHandler();
                handler.HandleInput(kstate);
                text += kstate.GetPressedKeys();
                
                if(loggedIn) //update loggedIn to true on succesfull login
                {
                    gameState = GameState.Game;
                    
                }
            }
            else
            {
                var kstate = Keyboard.GetState();
                kinput.HandleInput(kstate);

                /*
                 * HAVE TO SET UP CONNECTION ON STARTUP
                 * maintain connection?
                 * 
                 * Receive updated game state from server (incl. other user behavior)
                 *  May fail, will be received again later.
                 */
                 if (count == 0)
                {
                    db.updatePlayer_active(2, true);
                    Console.WriteLine(db.getPlayer_active("1", "1"));
                    //db.createPlayer("asd", "asd");
                    //Console.WriteLine(db.Authenticate("asd", "asd"));
                    count++;
                }
                //Console.WriteLine(db.Authenticate("asfd","gfd"));
                state = gameServer.GetState();

                //animationLeft.Update(gameTime);
                //animationUp.Update(gameTime);Herud
                //animationDown.Update(gameTime);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.TransparentBlack);
            if (gameState == GameState.Menu)
            {
                int switcher = 0;
                spriteBatch.Begin();
                if(switcher == 0)
                spriteBatch.DrawString(font, "Enter username", new Vector2(100, 100), Color.White);
                //make readline async
                string username = Console.ReadLine();
                Console.WriteLine(username);
                if (username == "123")
                {
                    switcher++;
                    spriteBatch.DrawString(font, "Enter password", new Vector2(100, 200), Color.White);
                    //make readline async

                    string password = Console.ReadLine();
                    if (password == "321")
                    {   
                        gameState = GameState.Game;
                        ClientMessage message = new ClientMessageConnect(username, password);
                        //gameClient.Send(message);
                    }
                }
                spriteBatch.End();
            } else
            {

            
                map.Draw(spriteBatch, state);
            spriteBatch.Begin();
            //animationLeft.loop = false;
            //animationLeft.Draw(spriteBatch);
            //spriteBatch.Draw(ballTexture, ballPosition, null, Color.White, 0f,
            //    new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
            //    Vector2.One, SpriteEffects.None, 0f );
            spriteBatch.Draw(character, new Vector2(graphics.PreferredBackBufferWidth/2, graphics.PreferredBackBufferHeight/2), null, new Rectangle(Point.Zero, new Point(16, 16)), null, 0f, null, Color.White, SpriteEffects.None, 0.1f);
            
            spriteBatch.End();
            base.Draw(gameTime);
            }
        }
    }
}
