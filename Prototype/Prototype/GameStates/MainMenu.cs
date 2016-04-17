using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
            
        }

        private void Initialize()
        {
            
        }

        private void LoadContent()
        {
            
        }

        public void UnLoadContent()
        {

        }

        public EGameState Update()
        {

            return EGameState.MainMenu;
        }
    }
}
