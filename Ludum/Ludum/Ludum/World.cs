using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Ludum
{
    public static class World
    {
        public static float Gravity = 0.005f;
        public static float AirDrag = 0.0005f;
        public static float GroundDrag = 0.01f;
        public static List<Entity> Entities = new List<Entity>();

        public static Vector2 getForce(float x , float y)
        {
            return Vector2.UnitY*Gravity;
        }
    }
}
