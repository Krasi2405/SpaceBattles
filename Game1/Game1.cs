using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        List<Invader> invaders;
        int cooldown;
        List<Bullet> bullets = new List<Bullet>();
        float scale;

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
            // TODO: Add your initialization logic here
            /*
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width - 160;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height - 90;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            */
            scale = GraphicsDevice.Viewport.Width / 800;
            invaders = new List<Invader>();

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
            Texture2D ship = Content.Load<Texture2D>("ship_2");
            Texture2D invader = Content.Load<Texture2D>("invader_1");
            player = new Player(ship, new Vector2(GraphicsDevice.Viewport.Width / 2 - (ship.Width / 2) * scale, GraphicsDevice.Viewport.Height - ship.Height * scale));
            player.SetScale(scale);
            int invaderCount = 8;
            for(int i = 0; i < invaderCount; i++)
            {
                invaders.Add(new Invader(invader, new Vector2(((GraphicsDevice.Viewport.Width / invaderCount) * i), 10)));
            }
            

            /*
            this.graphics.PreferredBackBufferWidth = this.backgroundTexture.Width;
            this.graphics.PreferredBackBufferHeight = this.backgroundTexture.Height;
            this.graphics.ApplyChanges();
            */

            // TODO: use this.Content to load your game content here
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


            // Moving Logic
            KeyboardState state = Keyboard.GetState();
            if(state.IsKeyDown(Keys.A))
            {
                player.Position.X -= 2 + GraphicsDevice.Viewport.Width / 160;
            }

            if(state.IsKeyDown(Keys.D))
            {
                player.Position.X += 2 + GraphicsDevice.Viewport.Width / 160;
            }
            
            // Collision Logic
            if(player.Position.X >= GraphicsDevice.Viewport.Width - player.GetWidth())
            {
                player.Position.X = GraphicsDevice.Viewport.Width - player.GetWidth();
            }
            else if(player.Position.X <= 0)
            {
                player.Position.X = 0;
            }


            // Fullscreen on or off
            if(state.IsKeyDown(Keys.F))
            {
                if(graphics.IsFullScreen == true)
                {
                    graphics.IsFullScreen = false;
                }
                else if(graphics.IsFullScreen == false)
                {
                    graphics.IsFullScreen = true;
                }
                graphics.ApplyChanges();
            }

            // Shooting

            if(state.IsKeyDown(Keys.Space) && cooldown >= 60)
            {
                Texture2D bullet = Content.Load<Texture2D>("blast_shot_blue");
                bullets.Add(
                    new Bullet(bullet,
                    new Vector2(player.Position.X + player.GetWidth() / 2 - bullet.Width / 2,player.Position.Y),
                    player, 
                    new Vector2(0, -(5 + GraphicsDevice.Viewport.Width / 200))
                    ));
                cooldown = 0;
            }

            // Bullets Logic
            foreach(Bullet bullet in bullets)
            {
                
                bullet.Move();
                bullet.UpdateLogic();
                if (bullet.Position.Y <= 0 && bullet.Position.Y >= GraphicsDevice.Viewport.Width + 100)
                {
                    bullets.Remove(bullet);
                    
                }

                foreach(Invader invader in invaders)
                {
                    invader.UpdateLogic();
                    if (bullet.Intersects(invader.Box)) {
                        invader.IsShot = true;
                        bullet.isUsed = true;
                    }
                }
            }



            cooldown += 1;
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
            foreach(Bullet bullet in bullets)
            {
                if (!bullet.isUsed)
                {
                    bullet.Draw(spriteBatch);
                }
            }

            foreach(Invader invader in invaders)
            {
                if (!invader.IsShot)
                {
                    invader.Draw(spriteBatch);
                }
            }

            player.Draw(spriteBatch);
            base.Draw(gameTime);
        }
    }
}
