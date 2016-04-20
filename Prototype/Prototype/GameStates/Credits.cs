using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Prototype.GameStates
{
    class Credits : IGameState
    {
        public void Dispose()
        {
            
        }

        public void Draw()
        {
            
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
            return EGameState.Credits;
        }
    }
}
