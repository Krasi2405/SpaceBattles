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
        List<List<Invader>> invaders;
        List<Bullet> invaderBullets = new List<Bullet>();
        List<Bullet> playerBullets = new List<Bullet>();
        List<Powerup> availablePowerups = new List<Powerup>();
        Texture2D shield;

        SpriteFont font;
        Sprite backgroundImage;

        int restartingCounter;
        float scale;
        int cooldown;
        int invaderCooldown;

        enum GameState { Playing, GameOver, Restarting, Victory};
        GameState currentState;

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
            restartingCounter = 180;
            scale = GraphicsDevice.Viewport.Width / 800;
            invaderCooldown = 0;
            invaders = new List<List<Invader>>();
            currentState = GameState.Playing;
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
            int rows = 4;
            Random random = new Random();
            for (int a = 0; a < rows; a++) {
                invaders.Add(new List<Invader>());
                for (int i = 0; i < invaderCount; i++)
                {
                    invaders[a].Add(new Invader(
                        invader,
                        new Vector2(((GraphicsDevice.Viewport.Width / invaderCount) * i), 10 - (invader.Height + 20) * a),
                        new Vector2(4, 0.25f),
                        random.Next(160, 300)
                        ));
                }
            }
            Texture2D background = this.Content.Load<Texture2D>("galaxy");
            backgroundImage = new Sprite(background, new Vector2(0, 0));
            font = this.Content.Load<SpriteFont>("Font");

            availablePowerups.Add(new Powerup(this.Content.Load<Texture2D>("shield"),new Vector2(player.Position.X, player.Position.Y), "shield", 60 * 30));

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

            KeyboardState state = Keyboard.GetState();
            if (currentState == GameState.Playing || currentState == GameState.Victory)
            {
                
                // Moving Logic
                
                if (state.IsKeyDown(Keys.A))
                {
                    player.Position.X -= 7;
                }

                if (state.IsKeyDown(Keys.D))
                {
                    player.Position.X += 7;
                }

                // Collision Logic
                if (player.Position.X >= GraphicsDevice.Viewport.Width - player.GetWidth())
                {
                    player.Position.X = GraphicsDevice.Viewport.Width - player.GetWidth();
                }
                else if (player.Position.X <= 0)
                {
                    player.Position.X = 0;
                }


                

                // Shooting

                if (state.IsKeyDown(Keys.Space) && cooldown >= 30)
                {
                    Texture2D bullet = Content.Load<Texture2D>("blast_shot_blue");
                    playerBullets.Add(
                        new Bullet(bullet,
                        new Vector2(player.Position.X + player.GetWidth() / 2 - bullet.Width / 2, player.Position.Y),
                        player,
                        new Vector2(0, -8)
                        ));
                    cooldown = 0;
                }

                // Player Bullets Logic

                for (int i = 0; i < playerBullets.Count; i++)
                {
                    Bullet bullet = playerBullets[i];
                    bullet.Move();
                    bullet.UpdateLogic();
                    if (bullet.Position.Y <= 0 - bullet.GetHeight() || bullet.Position.Y >= GraphicsDevice.Viewport.Height)
                    {
                        playerBullets.Remove(bullet);

                    }

                    foreach (List<Invader> invadersList in invaders)
                    {
                        foreach (Invader invader in invadersList)
                        {
                            invader.UpdateLogic();
                            if (bullet.Intersects(invader.Box) && invader.IsShot == false)
                            {
                                invader.IsShot = true;
                                bullet.isUsed = true;
                                break;
                            }
                        }
                    }

                    for (int a = 0; a < invaders.Count; a++)
                    {
                        List<Invader> invadersList = invaders[a];
                        for (int b = 0; b < invadersList.Count; b++)
                        {
                            if (invadersList[b].IsShot)
                            {
                                invadersList.Remove(invadersList[b]);
                            }
                        }
                    }

                    if (bullet.isUsed)
                    {
                        playerBullets.Remove(bullet);
                    }

                }



                // Invaders moving
                foreach (List<Invader> invadersList in invaders)
                {
                    foreach (Invader invader in invadersList)
                    {
                        invader.Move();
                        if (invader.Position.X >= GraphicsDevice.Viewport.Width - invader.GetWidth())
                        {
                            invader.Position.X = GraphicsDevice.Viewport.Width - invader.GetWidth();
                            invader.ChangeVelocityXSign();
                        }
                        if (invader.Position.X <= 0)
                        {
                            invader.Position.X = 0;
                            invader.ChangeVelocityXSign();
                        }

                        invader.UpdateLogic();
                    }
                }

                // Invaders Shooting
                Random random = new Random();
                foreach (List<Invader> invadersList in invaders)
                {
                    foreach (Invader invader in invadersList)
                    {
                        if (invader.Position.Y >= 0 - invader.GetHeight() && invader.Cooldown >= invader.MaxCooldown)
                        {
                            Texture2D bullet = Content.Load<Texture2D>("blast_shot_red");
                            invaderBullets.Add(
                                new Bullet(bullet,
                                new Vector2(invader.Position.X + invader.GetWidth() / 2 - bullet.Width / 2, invader.Position.Y + invader.GetHeight()),
                                invader,
                                new Vector2(0, 8)
                            ));
                            invader.ResetCooldown();
                        }
                        else if (invader.Position.Y >= 0 - invader.GetHeight())
                        {
                            invader.IncreaseCooldown();
                        }
                    }
                }

                // Invaders Bullets Logic
                for (int i = 0; i < invaderBullets.Count; i++)
                {
                    Bullet bullet = invaderBullets[i];
                    bullet.Move();
                    bullet.UpdateLogic();

                    if (bullet.Position.Y <= 0 - bullet.GetHeight() || bullet.Position.Y >= GraphicsDevice.Viewport.Height)
                    {
                        invaderBullets.Remove(bullet);

                    }

                    if (player.IntersectsCut(bullet.Box, 30))
                    {
                        bullet.isUsed = true;
                        currentState = GameState.GameOver;
                    }

                    if (bullet.isUsed)
                    {
                        invaderBullets.Remove(bullet);
                    }
                }

                // Check if row is empty
                for (int a = 0; a < invaders.Count; a++)
                {
                    if (invaders[a].Count == 0)
                    {
                        invaders.Remove(invaders[a]);
                    }
                }

                // Victory Conditions
                if(invaders.Count == 0)
                {
                    currentState = GameState.Victory;
                }

                player.UpdateLogic();
                invaderCooldown += 1;
                cooldown += 1;
            }

            // Fullscreen on or off
            if (state.IsKeyDown(Keys.F))
            {
                if (graphics.IsFullScreen == true)
                {
                    graphics.IsFullScreen = false;
                }
                else if (graphics.IsFullScreen == false)
                {
                    graphics.IsFullScreen = true;
                }
                graphics.ApplyChanges();
            }

            // Runs this if it is game over.
            if(currentState == GameState.GameOver || currentState == GameState.Victory)
            {
                if(state.IsKeyDown(Keys.R))
                {
                    currentState = GameState.Restarting;
                }
            }

            // Runs when restarting
            if(currentState == GameState.Restarting)
            {
                if(restartingCounter == 0)
                {
                    restartingCounter = 180;
                    currentState = GameState.Playing;
                }

                if(restartingCounter == 180)
                {

                    Texture2D ship = Content.Load<Texture2D>("ship_2");
                    Texture2D invader = Content.Load<Texture2D>("invader_1");
                    player = new Player(ship, new Vector2(GraphicsDevice.Viewport.Width / 2 - (ship.Width / 2) * scale, GraphicsDevice.Viewport.Height - ship.Height * scale));
                    player.SetScale(scale);
                    int invaderCount = 8;
                    int rows = 4;
                    invaders = new List<List<Invader>>();
                    playerBullets = new List<Bullet>();
                    invaderBullets = new List<Bullet>();
                    Random random = new Random();
                    for (int a = 0; a < rows; a++)
                    {
                        invaders.Add(new List<Invader>());
                        for (int i = 0; i < invaderCount; i++)
                        {
                            invaders[a].Add(new Invader(
                                invader,
                                new Vector2(((GraphicsDevice.Viewport.Width / invaderCount) * i), 10 - (invader.Height + 20) * a),
                                new Vector2(4, 0.25f),
                                random.Next(160, 300)
                                ));
                        }
                    }
                }

                


                restartingCounter -= 1;
            }

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
            //backgroundImage.Draw(spriteBatch);

            foreach (Bullet bullet in playerBullets)
            {
                bullet.Draw(spriteBatch);
            }

            foreach(Bullet bullet in invaderBullets)
            {
                bullet.Draw(spriteBatch);
            }

            foreach (List<Invader> invadersList in invaders)
            {
                foreach (Invader invader in invadersList)
                {
                    invader.Draw(spriteBatch);
                }
            }

            player.Draw(spriteBatch);

            if (currentState == GameState.GameOver) {
                spriteBatch.Begin();
                string text = "PRESS 'R' IN ORDER TO RESTART";
                spriteBatch.DrawString(
                    font,
                    text,
                    new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2) - (font.MeasureString(text) / 2),
                    Color.Black);
                spriteBatch.End();
            }

            if(currentState == GameState.Restarting)
            {
                spriteBatch.Begin();
                string text = "RESTARTING IN " + Math.Round((double)restartingCounter / 60) + "!";
                spriteBatch.DrawString(
                    font,
                    text,
                    new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2) - (font.MeasureString(text) / 2),
                    Color.Black);
                spriteBatch.End();
            }

            if(currentState == GameState.Victory)
            {
                spriteBatch.Begin();
                string text = "VICTORY!!!";
                spriteBatch.DrawString(
                    font,
                    text,
                    new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2) - (font.MeasureString(text) / 2),
                    Color.Black);

                string text2 = "PRESS 'R' IN ORDER TO RESTART";
                spriteBatch.DrawString(
                    font,
                    text2,
                    new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 + 30) - (font.MeasureString(text2) / 2),
                    Color.Black);
                spriteBatch.End();
            }
            
            
            
            base.Draw(gameTime);
        }
    }
}
