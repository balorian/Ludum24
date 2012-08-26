using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarmineEngine;

namespace Ludum
{
    class Scene : Entity
    {
        bool[,] CollisionArray;

        public Scene(GameScreen screen, string boundingSprite, string sprite) : base(screen, boundingSprite, sprite)
        {

        }

    }
}
