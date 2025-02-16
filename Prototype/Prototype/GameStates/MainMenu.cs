﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.GameStates
{
    class MainMenu : IGameState
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice gDevice;
        ContentManager Content;
        Texture2D pressEnter;

        public MainMenu(GraphicsDeviceManager g, GraphicsDevice gD, ContentManager content)
        {
            graphics = g;
            gDevice = gD;
            Content = content;

            //LoadContent();
            //Initialize();
        }

        public void Draw()
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(pressEnter, new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), origin: new Vector2(pressEnter.Width / 2, pressEnter.Height / 2));
            spriteBatch.End();
        }

        public void Initialize()
        {
            
        }

        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            pressEnter = Content.Load<Texture2D>("pressEnter");
        }

        public void UnLoadContent()
        {
            Content.Unload();
            Dispose();
        }

        public EGameState Update(KeyboardState kState, KeyboardState pk, GameTime gt, ref int Score)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kState.IsKeyDown(Keys.Escape))
            {
                return EGameState.Credits;
            }

            if (kState.IsKeyDown(Keys.Enter))
                return EGameState.InGame;
            else
                return EGameState.MainMenu;
        }

        public void Dispose()
        {
            graphics = null;
            spriteBatch = null;
            gDevice = null;
            Content = null;
            pressEnter = null;
        }
    }
}
