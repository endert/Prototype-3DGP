using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Prototype.GameStates
{
    class Credits : IGameState
    {
        int Score;
        SpriteFont font;
        SpriteBatch sBatch;
        bool canbeended = false;

        public Credits(int Score, GraphicsDeviceManager g, GraphicsDevice gD, ContentManager content)
        {
            this.Score = Score;
            sBatch = new SpriteBatch(gD);
            font = content.Load<SpriteFont>("Score");
        }

        public void Dispose()
        {
            
        }

        public void Draw()
        {
            sBatch.Begin();

            sBatch.DrawString(font, "Thanks for playing. This prototype was made by Joshua Endert and Mirko Ebert.", new Vector2(100, 20), Color.Black);
            sBatch.DrawString(font, "Your Score: " + Score, new Vector2(300, 100), Color.Black);

            sBatch.End();
        }

        public void Initialize()
        {
            
        }

        public void LoadContent()
        {
            
        }

        public void UnLoadContent()
        {
            
        }

        public EGameState Update(KeyboardState k, KeyboardState pk, GameTime gt, ref int Score)
        {
            if (canbeended && (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || k.IsKeyDown(Keys.Escape)))
            {
                return EGameState.None;
            }

            canbeended = k.IsKeyUp(Keys.Escape);

            return EGameState.Credits;
        }
    }
}
