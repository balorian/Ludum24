using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CarmineEngine
{
    public class Actor : Entity
    {

        public Actor(GameScreen screen, Rectangle boundingBox, string sprite) : base(screen, boundingBox, sprite) 
        {
        }

        public override void update()
        {
            base.update();
        }

        public override void draw()
        {
            base.draw();
        }

        public void clipUp(Entity entity)
        {
            while (collidesWith(entity))
                Position += -Vector2.UnitY;
        }

    }
}
