using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CarmineEngine
{
    public class SpriteTemplateData
    {
        public const string FRAME = "frame";
        public const string GRID = "grid";
        public const string SHEET = "sheet";

        public string Name;
        public string LoadType;
        public string SheetDirectory;
        public int GridWidth;
        public int GridHeight;
        public bool SpecifyBackground;
        public Color BackgroundColor;
        public bool Animated;

        public Rectangle Size;
        public Vector2 Offset;

        public List<FrameData> FrameOrigins;
        public List<AnimationData> Animations;

        internal void exctractData(SpriteTemplate template)
        {
            template.Animated = Animated;
            template.Size = Size;
            template.Offset = Offset;
            foreach (FrameData frame in FrameOrigins)
                template.Frames[frame.Index].spriteOrigin = frame.Origin;
            foreach (AnimationData animData in Animations)
                template.addAnimation(new Animation(animData.Name, animData.Frames.ToArray(), animData.Times.ToArray(), animData.Loops));
        }
    }
}
