using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarmineEngine;
using Ludum.Level;

namespace Ludum.Level
{
    class Level : GameScreen
    {
        public Map Map;

        public Level(String name, GameScreen state)
            : base(name, 100, state)
        {

        }

    }
}
