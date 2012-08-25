using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarmineEngine;
using Microsoft.Xna.Framework;

namespace Ludum
{
    class Player : Actor
    {
        public float MoveAcceleration = 0.005f;
        public float JumpImpulse = 1f;
        public Vector2 MaxSpeed = new Vector2(0.6f , 1f);

        public Player(GameScreen screen, Rectangle boundingBox, string sprite) : base(screen, boundingBox, sprite) 
        {
            Acceleration.Y = World.Gravity;
        }

        public override void update()
        {
            Ray r = new Ray(new Vector3(1, 0, 0), new Vector3(1, 0, 0));
            if (Engine.Keyboard.pressed(Key.W))
                Velocity += JumpImpulse * -Vector2.UnitY;

            if (Engine.Keyboard.down(Key.A))
            {
                Acceleration.X = -MoveAcceleration;
            }
            if (Engine.Keyboard.down(Key.D))
            {
                Acceleration.X = MoveAcceleration;
            }

            if (Engine.Keyboard.released(Key.D))
            {
                Acceleration.X = 0;
            }
            if (Engine.Keyboard.released(Key.A))
            {
                Acceleration.X = 0;
            }

            Velocity = Vector2.Clamp(Velocity, -MaxSpeed, MaxSpeed);

            base.update();
        }

        public override void draw()
        {
            base.draw();
        }

    }
}
