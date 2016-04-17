using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Prototype.GameStates
{
    enum EGameState
    {
        None = -1,

        MainMenu,
        InGame,
        Credits,

        Count
    }

    interface IGameState : IDisposable
    {
        EGameState Update(KeyboardState k, KeyboardState pk);
        void Draw();
        void Initialize();
        void LoadContent();

        void UnLoadContent();
    }
}
