using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarmineEngine;

namespace Ludum
{
    class Scene : Entity
    {

        public Scene(GameScreen screen, string boundingSprite, string sprite) : base(screen, boundingSprite, sprite)
        {
            Sprite.BlockUpdate = true;
            Sprite.Visible = false;
        }

    }
}
