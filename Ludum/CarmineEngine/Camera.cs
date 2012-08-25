using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CarmineEngine
{
    public class Camera
    {
        public SamplerState SampleState = SamplerState.PointClamp;
        public Vector2 Position = Vector2.Zero;
        public Vector2 Velocity = Vector2.Zero;
        public Vector2 Acceleration = Vector2.Zero;
        public float MaxSpeed = 200;
        public bool Manual = false;
        public float Zoom = 1;
        public Vector2 goalPosition = Vector2.Zero;

        bool move = true;
        bool decelerate = true;
        Vector2 distance = Vector2.Zero;
        Vector2 goalDir = Vector2.Zero;

        public Camera()
        {

        }

        public void moveTo(Vector2 position)
        {
            stop();
            Position = position;
            Manual = false;
        }

        public void moveTo(Vector2 position, float speed)
        {
            stop();
            start();
            goalPosition = position;
            Vector2 unitDirection = goalPosition - Position;
            unitDirection.Normalize();
            Velocity = speed * unitDirection;
            Manual = false;

            if (goalPosition.X == Position.X && goalPosition.Y == Position.Y)
                stop();

        }

        public void moveTo(Vector2 position, float acceleration, float speed, bool decelerate)
        {
            stop();
            start();
            goalPosition = position;
            Vector2 unitDirection = goalPosition - Position;
            unitDirection.Normalize();
            Acceleration = acceleration * unitDirection;
            Velocity = speed * unitDirection;
            distance = goalPosition - Position;
            this.decelerate = decelerate;
            Manual = false;

            if (goalPosition.X == Position.X && goalPosition.Y == Position.Y)
                stop();
        }

        public void stop()
        {
            move = false;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            distance = Vector2.Zero;
            decelerate = false;
        }

        public void start()
        {
            move = true;
        }

        public Matrix crateTransformationMatrix()
        {
            Matrix transform = Matrix.CreateTranslation(new Vector3(-Position, 0));
            transform *= Matrix.CreateScale(Zoom);
            return transform;
        }

        public void update()
        {
            if (move)
            {
                if (!Manual)
                {

                    if (decelerate)
                    {
                        if ((goalPosition - Position).Length() < distance.Length() / 2)
                        {
                            Acceleration = -Acceleration;
                            decelerate = false;
                        }
                    }

                    Velocity += Acceleration * (float)Engine.GameTime.ElapsedGameTime.TotalMilliseconds;
                    if (Velocity.Length() > MaxSpeed)
                    {
                        Velocity.Normalize();
                        Velocity *= MaxSpeed;
                    }

                    Position += Velocity * (float)Engine.GameTime.ElapsedGameTime.TotalMilliseconds;
                    Vector2 goalDir = goalPosition - Position;
                    goalDir.Normalize();
                    Vector2 actualDir = Velocity;
                    actualDir.Normalize();
                    this.goalDir = goalDir;

                    if ((goalDir + actualDir).Length() < 0.1)
                    {
                        Position = goalPosition;
                        stop();
                    }
                }
                else
                {
                    Velocity += Acceleration * (float)Engine.GameTime.ElapsedGameTime.TotalMilliseconds;
                    if (Velocity.Length() > MaxSpeed)
                    {
                        Velocity.Normalize();
                        Velocity *= MaxSpeed;
                    }

                    Position += Velocity * (float)Engine.GameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
        }

        public override string ToString()
        {
            return "Position: " + Position.ToString() + "\r\nVelocity: " + Velocity.ToString() + "\r\nAcceleration: " + Acceleration.ToString() + "\r\nGoal Position: " + goalPosition.ToString() + "\r\nZoom: " + Zoom.ToString() + "\r\nSamplerState: " + SampleState.ToString();
        }
    }
}
