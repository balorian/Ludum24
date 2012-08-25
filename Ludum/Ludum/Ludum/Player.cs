using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarmineEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Ludum
{
    class Player : Entity
    {
        public Vector2 Acceleration = Vector2.Zero;
        public Vector2 Velocity = Vector2.Zero;
        public float MoveAcceleration = 0.005f;
        public float JumpImpulse = 1.5f;
        public Vector2 MaxSpeed = new Vector2(0.6f , 2f);

        public Player(GameScreen screen, Rectangle boundingBox, string sprite) : base(screen, boundingBox, sprite) 
        {
            Acceleration.Y = World.Gravity;
        }

        public override void update()
        {
            if (Engine.Keyboard.pressed(Key.W))
                Velocity = JumpImpulse * -Vector2.UnitY;

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
            Vector2 lastPosition = Position;
            Position += Velocity * (float)Engine.GameTime.ElapsedGameTime.TotalMilliseconds;

            List<Entity> collisions = getCollisions();
            if (collisions.Count > 0)
            {
                Vector2 deltaS = Position - lastPosition;
                Position = lastPosition;
                scanDeltaS(deltaS, 1);
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

        public void scanDeltaS(Vector2 deltaS, float step)
        {
            Vector2 unitS = Vector2.Normalize(deltaS);
            bool scanning = true;
            List<Entity> collisions = getCollisions();

            while (scanning)
            {
                deltaS -= unitS*step;
                if (deltaS.Length() < step)
                    scanning = false;

                {
                    Position += (unitS * step);

                    collisions = getCollisions();
                    if (collisions.Count > 0)
                    {
                        foreach(Entity e in collisions)
                        {
                            if (CheckEdge(Side.Bottom, e))
                            {
                                clipUp(e);
                                scanning = false;
                                Velocity.Y = 0;
                                Game1.Text += " BOTTOM";
                                Debug.WriteLine(" BOTTOM" + Velocity.ToString());
                            }
                            if (CheckEdge(Side.Top, e))
                            {
                                clipDown(e);
                                scanning = false;
                                Velocity.Y = 0;
                                Game1.Text += " TOP";
                                Debug.WriteLine(" TOP" + Velocity.ToString());
                            }
                            if (CheckEdge(Side.Left, e))
                            {
                                clipRight(e);
                                scanning = false;
                                Velocity.X = 0;
                                Game1.Text += " LEFT";
                                Debug.WriteLine(" LEFT" + Velocity.ToString());
                            }
                            if (CheckEdge(Side.Right, e))
                            {
                                clipLeft(e);
                                scanning = false;
                                Velocity.X = 0;
                                Game1.Text += " RIGHT";
                                Debug.WriteLine(" RIGHT" + Velocity.ToString());
                            }
                        }
                    }
                }
            }
        }

        List<Entity> getCollisions()
        {
            List<Entity> collisions = new List<Entity>();
            foreach (Entity e in World.Entities)
                if (!e.Equals(this) && collidesWith(e))
                    collisions.Add(e);
            return collisions;
        }

        public void clipUp(Entity entity)
        {
            while (collidesWith(entity))
                Position += -Vector2.UnitY;
            Debug.WriteLine(Velocity.ToString());
        }

        public void clipLeft(Entity entity)
        {
            while (collidesWith(entity))
                Position += -Vector2.UnitX;
            Debug.WriteLine(Velocity.ToString());
        }

        public void clipRight(Entity entity)
        {
            while (collidesWith(entity))
                Position += Vector2.UnitX;
            Debug.WriteLine(Velocity.ToString());
        }

        public void clipDown(Entity entity)
        {
            while (collidesWith(entity))
                Position += Vector2.UnitY;
            Debug.WriteLine(Velocity.ToString());
        }

        public bool CheckEdge(Side side, Entity entity)
        {
            switch(side){
                case Side.Bottom:
                    Rectangle bottom = new Rectangle((int)Position.X+2, (int)Position.Y + BoundingBox.Height - 1, BoundingBox.Width-4, 1);
                    if (bottom.Intersects(new Rectangle((int)entity.Position.X, (int)entity.Position.Y, entity.BoundingBox.Width, entity.BoundingBox.Height)))
                        return true;
                    break;
                case Side.Top:
                    Rectangle top = new Rectangle((int)Position.X+2, (int)Position.Y, BoundingBox.Width-4, 1);
                    if (top.Intersects(new Rectangle((int)entity.Position.X, (int)entity.Position.Y, entity.BoundingBox.Width, entity.BoundingBox.Height)))
                        return true;
                    break;
                case Side.Left:
                    Rectangle left = new Rectangle((int)Position.X, (int)Position.Y+2, 1, BoundingBox.Height-4);
                    if (left.Intersects(new Rectangle((int)entity.Position.X, (int)entity.Position.Y, entity.BoundingBox.Width, entity.BoundingBox.Height)))
                        return true;
                    break;
                case Side.Right:
                    Rectangle right = new Rectangle((int)Position.X + BoundingBox.Width - 1, (int)Position.Y+2, 1, BoundingBox.Height-4);
                    if (right.Intersects(new Rectangle((int)entity.Position.X, (int)entity.Position.Y, entity.BoundingBox.Width, entity.BoundingBox.Height)))
                        return true;
                    break;
            }
            return false;
        }

    }
}
