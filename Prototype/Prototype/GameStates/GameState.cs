﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    interface IGameState
    {
        EGameState Update();
        void Draw();
        void UnLoadContent();
    }
}
