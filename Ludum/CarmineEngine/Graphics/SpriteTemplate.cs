using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CarmineEngine
{
    public class SpriteTemplate
    {

        public string Name;
        public Rectangle Size;
        public Vector2 Offset;

        string sheet;
        public string Sheet
        {
            get { return sheet; }
            set
            {
                sheet = value;
                if (!Sprite.SpriteSheets.ContainsKey(sheet))
                    Sprite.LoadSheet(sheet);
            }
        }
        public Frame[] Frames;
        public bool Animated = false;
        Dictionary<string, Animation> animations = new Dictionary<string, Animation>();

        public static void Load(string dataDir)
        {
            SpriteTemplateData data = Engine.Content.Load<SpriteTemplateData>(dataDir);
            ProcessData(data);
        }
        public static void LoadFolder(string folderDir)
        {
            Dictionary<string, SpriteTemplateData> templates = Engine.LoadFolder<SpriteTemplateData>(folderDir);
            foreach (KeyValuePair<string, SpriteTemplateData> pair in templates)
                ProcessData(pair.Value);
        }
        private static void ProcessData(SpriteTemplateData data)
        {
            if (data.LoadType == SpriteTemplateData.FRAME)
            {
                new SpriteTemplate(data.Name, data.SheetDirectory);
                data.exctractData(Sprite.SpriteTemplates[data.Name]);
            }
            else if (data.LoadType == SpriteTemplateData.GRID)
            {
                new SpriteTemplate(data.Name, data.SheetDirectory, data.GridWidth, data.GridHeight, data.Animated);
                data.exctractData(Sprite.SpriteTemplates[data.Name]);
            }
            else if (data.LoadType == SpriteTemplateData.SHEET)
            {
                if (data.SpecifyBackground)
                    new SpriteTemplate(data.Name, data.SheetDirectory, data.Size, data.BackgroundColor, data.Animated);
                else
                    new SpriteTemplate(data.Name, data.SheetDirectory, data.Size, null, data.Animated);
                data.exctractData(Sprite.SpriteTemplates[data.Name]);
            }
            else
                throw new Exception("Failure processing " + data.Name + ", load type " + data.LoadType + " wasn't frame, grid or sheet.");
        }

        public SpriteTemplate(string name, string spriteDir)
        {
            Name = name;
            Sheet = spriteDir;
            Size = Sprite.SpriteSheets[sheet].Bounds;
            Frames = new Frame[1] { new Frame(Size) };
            Sprite.SpriteTemplates[Name] = this;
        }
        public SpriteTemplate(string name, string gridDir, int gridWidth, int gridHeight, bool animated)
        {
            Name = name;
            Sheet = gridDir;
            this.Animated = animated;
            Size = new Rectangle(0, 0, Sprite.SpriteSheets[gridDir].Bounds.Width / gridWidth, Sprite.SpriteSheets[sheet].Bounds.Height / gridHeight);
            Frames = new Frame[gridWidth * gridHeight];
            for (int x = 0; x < gridWidth; x++)
                for (int y = 0; y < gridHeight; y++)
                    Frames[x + y * gridWidth] = new Frame(new Rectangle(x * Size.Width, y * Size.Height, Size.Width, Size.Height));
            Animation idleAnimation = new Animation(Sprite.IDLE, 0, 1, new int[] { 0 }, false, true);
            animations[Sprite.IDLE] = idleAnimation;
            Sprite.SpriteTemplates[Name] = this;
        }
        public SpriteTemplate(string name, string sheetDir, Rectangle? spriteSize, Color? backgroundColor, bool animated)
        {
            Name = name;
            Sheet = sheetDir;
            this.Animated = animated;
            Size = spriteSize ?? new Rectangle(0, 0, 0, 0);

            Color[] firstPixel = new Color[1];
            Sprite.SpriteSheets[sheet].GetData<Color>(0, new Rectangle(0, 0, 1, 1), firstPixel, 0, 1);
            Color background = backgroundColor ?? firstPixel[0];

            getFrames(background);
            Animation idleAnimation = new Animation(Sprite.IDLE, 0, 1, new int[] { 0 }, false, true);
            animations[Sprite.IDLE] = idleAnimation;
            Sprite.SpriteTemplates[Name] = this;
        }

        private void getFrames(Color background)
        {
            Rectangle bounds = Sprite.SpriteSheets[sheet].Bounds;
            List<Frame> gatheredFrames = new List<Frame>();

            Color[] Colors1D = new Color[bounds.Width * bounds.Height];
            Sprite.SpriteSheets[sheet].GetData<Color>(Colors1D);
            Color[,] rawSheet = new Color[bounds.Width, bounds.Height];

            for (int i = 0; i < Colors1D.Length; i++)
                rawSheet[i % bounds.Width, i / bounds.Width] = Colors1D[i];

            for (int y = 0; y < rawSheet.GetUpperBound(1); y++)
            {
                for (int x = 0; x < rawSheet.GetUpperBound(0); x++)
                {
                    if (!rawSheet[x, y].Equals(background))
                    {
                        if (x == 0)
                        {
                            if (y == 0)
                                gatheredFrames.Add(new Frame(getSourceRectangle(background, rawSheet, new Point(x, y))));
                            else if (rawSheet[x, y - 1].Equals(background))
                                gatheredFrames.Add(new Frame(getSourceRectangle(background, rawSheet, new Point(x, y))));
                        }
                        else if (y == 0)
                        {
                            if (rawSheet[x - 1, y].Equals(background))
                                gatheredFrames.Add(new Frame(getSourceRectangle(background, rawSheet, new Point(x, y))));
                        }
                        else if (rawSheet[x, y - 1].Equals(background) && rawSheet[x - 1, y].Equals(background))
                        {
                            gatheredFrames.Add(new Frame(getSourceRectangle(background, rawSheet, new Point(x, y))));
                        }

                    }
                }
            }

            if (Size.Width == 0 && gatheredFrames.Count > 0)
                Size.Width = gatheredFrames[0].sourceRectangle.Width;
            if (Size.Height == 0 && gatheredFrames.Count > 0)
                Size.Height = gatheredFrames[0].sourceRectangle.Height;

            Frames = new Frame[gatheredFrames.Count];

            for (int i = 0; i < Frames.Length; i++)
                Frames[i] = gatheredFrames[i];
        }

        private Rectangle getSourceRectangle(Color background, Color[,] rawSheet, Point corner)
        {
            Rectangle source = new Rectangle(corner.X, corner.Y, 0, 0);

            for (int x = corner.X; x < rawSheet.GetLength(0); x++)
            {
                if (rawSheet[x, corner.Y].Equals(background))
                    break;
                source.Width++;
            }
            for (int y = corner.Y; y < rawSheet.GetLength(1); y++)
            {
                if (rawSheet[corner.X, y].Equals(background))
                    break;
                source.Height++;
            }

            return source;
        }

        internal void extractData(Sprite sprite)
        {
            sprite.Size = new Rectangle(Size.X, Size.Y, Size.Width, Size.Height);
            sprite.frames = new Frame[Frames.Length];
            Frames.CopyTo(sprite.frames, 0);
            sprite.animated = Animated;
            sprite.sheet = Sheet;
            sprite.Offset = Offset;

            foreach (KeyValuePair<string, Animation> pair in animations)
            {
                Animation copy = pair.Value.copy();
                sprite.addAnimation(copy);
            }
        }

        public void addAnimation(Animation animation)
        {
            animations[animation.Name] = animation;
        }
        public void removeAnimation(string name)
        {
            animations.Remove(name);
        }
    }
}
