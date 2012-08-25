using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CarmineEngine
{
    public class GraphicsComponent : Component
    {
        public Vector2 Origin = Vector2.Zero;
        public Vector2 Position = Vector2.Zero;
        public Vector2 Offset = Vector2.Zero;
        public Color Tint = Color.White;
        public float Scale = 1f;
        public float Rotation = 0;
        public Vector2 Pivot = Vector2.Zero;
        public bool FlipHorizontally = false;
        public bool FlipVertically = false;
        public float Layer = 0.5f;
        public Rectangle Size;

        public GraphicsComponent(GameScreen screen) : base(screen)
        {

        }

        public void centerPivot()
        {
            Pivot = new Vector2(Size.Width / 2f, Size.Height / 2f);
        }
    }
}
