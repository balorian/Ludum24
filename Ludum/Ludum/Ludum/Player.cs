﻿using System;
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
        public Vector2 Force = Vector2.Zero;
        public float Drag = World.AirDrag;
        public Vector2 Gravity = World.Gravity * Vector2.UnitY;

        public Vector2 Acceleration = Vector2.Zero;
        public Vector2 Velocity = Vector2.Zero;
        public float MoveAcceleration = 0.005f;
        public float JumpImpulse = 1.5f;
        public Vector2 MaxSpeed = new Vector2(0.6f , 2f);

        public bool onGround;

        public Player(GameScreen screen, Rectangle boundingBox, string sprite) : base(screen, boundingBox, sprite) 
        {

        }

        public override void update()
        {
            if (Engine.Keyboard.pressed(Key.W))
                Velocity = JumpImpulse * -Vector2.UnitY;

            if (Engine.Keyboard.down(Key.A))
            {
                Force.X = -MoveAcceleration;
            }
            if (Engine.Keyboard.down(Key.D))
            {
                Force.X = MoveAcceleration;
            }

            if (Engine.Keyboard.released(Key.D))
            {
                Force.X = 0;
            }
            if (Engine.Keyboard.released(Key.A))
            {
                Force.X = 0;
            }

            Acceleration = (Force + Gravity + (Drag * -Velocity));

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
                {
                    scanning = false;
                    Position += deltaS;
                }
                {
                    Position += (unitS * step);

                    collisions = getCollisions();
                    if (collisions.Count > 0)
                    {
                        foreach(Entity e in collisions)
                        {
                            if (checkEdge(Side.Bottom, e))
                            {
                                clipUp(e);
                                Velocity.Y = 0;
                                Game1.Text += " BOTTOM";
                                Debug.WriteLine(" BOTTOM" + Velocity.ToString());
                            }
                            if (checkEdge(Side.Top, e))
                            {
                                clipDown(e);
                                Velocity.Y = 0;
                                Game1.Text += " TOP";
                                Debug.WriteLine(" TOP" + Velocity.ToString());
                            }
                            if (checkEdge(Side.Left, e))
                            {
                                clipRight(e);
                                Velocity.X = 0;
                                Game1.Text += " LEFT";
                                Debug.WriteLine(" LEFT" + Velocity.ToString());
                            }
                            if (checkEdge(Side.Right, e))
                            {
                                clipLeft(e);
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
            while (checkEdge(Side.Bottom, entity))
                Position += -Vector2.UnitY;
            Debug.WriteLine(Velocity.ToString());
        }

        public void clipLeft(Entity entity)
        {
            while (checkEdge(Side.Right, entity))
                Position += -Vector2.UnitX;
            Debug.WriteLine(Velocity.ToString());
        }

        public void clipRight(Entity entity)
        {
            while (checkEdge(Side.Left, entity))
                Position += Vector2.UnitX;
            Debug.WriteLine(Velocity.ToString());
        }

        public void clipDown(Entity entity)
        {
            while (checkEdge(Side.Top, entity))
                Position += Vector2.UnitY;
            Debug.WriteLine(Velocity.ToString());
        }

        public bool checkEdge(Side side, Entity entity)
        {
            switch(side){
                case Side.Bottom:
                    Rectangle bottom = new Rectangle((int)Position.X+2, (int)Position.Y + BoundingBox.Height - 1, BoundingBox.Width-4, 1);
                    if (!entity.UseGeometry)
                    {
                        if (bottom.Intersects(new Rectangle((int)entity.Position.X, (int)entity.Position.Y, entity.BoundingBox.Width, entity.BoundingBox.Height)))
                            return true;
                    }
                    else
                    {
                        if (collideGeometries(bottom, entity))
                            return true;
                    }
                    break;
                case Side.Top:
                    Rectangle top = new Rectangle((int)Position.X+2, (int)Position.Y, BoundingBox.Width-4, 1);
                    if (!entity.UseGeometry)
                    {
                        if (top.Intersects(new Rectangle((int)entity.Position.X, (int)entity.Position.Y, entity.BoundingBox.Width, entity.BoundingBox.Height)))
                            return true;
                    }
                    else
                    {
                        if (collideGeometries(top, entity))
                            return true;
                    }
                    break;
                case Side.Left:
                    Rectangle left = new Rectangle((int)Position.X, (int)Position.Y+2, 1, BoundingBox.Height-4);
                    if (!entity.UseGeometry)
                    {
                        if (left.Intersects(new Rectangle((int)entity.Position.X, (int)entity.Position.Y, entity.BoundingBox.Width, entity.BoundingBox.Height)))
                            return true;
                    }
                    else
                    {
                        if (collideGeometries(left, entity))
                            return true;
                    }
                    break;
                case Side.Right:
                    Rectangle right = new Rectangle((int)Position.X + BoundingBox.Width - 1, (int)Position.Y + 2, 1, BoundingBox.Height - 4);
                    if (!entity.UseGeometry)
                    {
                        if (right.Intersects(new Rectangle((int)entity.Position.X, (int)entity.Position.Y, entity.BoundingBox.Width, entity.BoundingBox.Height)))
                            return true;
                    }
                    else
                    {
                        if (collideGeometries(right, entity))
                            return true;
                    }
                    break;
            }
            return false;
        }

    }
}
