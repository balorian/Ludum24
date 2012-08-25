using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarmineEngine;
using Microsoft.Xna.Framework;

namespace Ludum
{
    class Player : Entity
    {
        public Vector2 Acceleration = Vector2.Zero;
        public Vector2 Velocity = Vector2.Zero;
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
                Velocity.X = 0;
            }
            if (Engine.Keyboard.released(Key.A))
            {
                Acceleration.X = 0;
                Velocity.X = 0;
            }

            Velocity += Acceleration * (float)Engine.GameTime.ElapsedGameTime.TotalMilliseconds;
            Vector2 lastPosition = new Vector2(Position.X, Position.Y);
            Position += Velocity * (float)Engine.GameTime.ElapsedGameTime.TotalMilliseconds;

            foreach (Entity e in World.Entities)
                if (!e.Equals(this) && collidesWith(e))
                {
                    clipUp(e);
                    Velocity.Y = 0;
                }

            moveTo((int)Position.X, (int)Position.Y);

            base.update();

            Velocity = Vector2.Clamp(Velocity, -MaxSpeed, MaxSpeed);



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
