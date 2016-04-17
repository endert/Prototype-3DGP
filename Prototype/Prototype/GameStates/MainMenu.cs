using Microsoft.Xna.Framework;
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

            LoadContent();
            Initialize();
        }

        public void Draw()
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(pressEnter, new Vector2(graphics.PreferredBackBufferHeight / 2, graphics.PreferredBackBufferWidth / 2), origin: new Vector2(pressEnter.Width / 2, pressEnter.Height / 2));
            spriteBatch.End();
        }

        private void Initialize()
        {
            
        }

        private void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            pressEnter = Content.Load<Texture2D>("pressEnter");
        }

        public void UnLoadContent()
        {
            Content.Unload();
        }

        public EGameState Update(KeyboardState kState, KeyboardState pk)
        {
            if (kState.IsKeyDown(Keys.Enter))
                return EGameState.InGame;
            else
                return EGameState.MainMenu;
        }
    }
}
