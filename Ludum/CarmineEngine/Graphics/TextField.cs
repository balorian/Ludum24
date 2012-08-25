using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CarmineEngine
{
    public class TextField : GraphicsComponent
    {
        public SpriteFont Font = Engine.DefaultFont;
        public Image Background;

        public bool DrawShadow = false;
        public Color ShadowColor = Color.DimGray;
        public string Text { get { return text; } set { text = value; splitText(); } }
        public bool SplitWords = false;
        public int Line { get { return lineIndex; } set { if (value >= 0) lineIndex = value; splitText(); } }
        public int Index { get { return charIndex; } set { if (value >= 0) charIndex = value; splitText(); } }

        int lineIndex = 0;
        int charIndex = 0;
        string text = "No data";
        string drawText = "No data";

        public TextField(GameScreen screen, Rectangle size) : base(screen)
        {
            Size = size;
        }
        public TextField(GameScreen screen, SpriteFont font, Rectangle size) : base(screen)
        {
            Font = font;
            Size = size;
        }
        public TextField(GameScreen screen, SpriteFont font, Rectangle size, Image background) : base(screen)
        {
            Font = font;
            Size = size;
            Background = background;
            Background.OverrideDraw = true;
        }

        public override void update()
        {

            base.update();
        }

        public override void draw()
        {
            if (Background != null)
            {
                Background.Origin = Origin;
                Background.Pivot = Pivot;
                Background.FlipHorizontally = FlipHorizontally;
                Background.FlipVertically = FlipVertically;
                Background.Layer = Layer;
                Background.Rotation = Rotation;
                Background.Scale = Scale;
                Background.draw(new Rectangle((int)(Position.X + Origin.X), (int)(Position.Y + Origin.Y), Size.Width, Size.Height));
            }
            SpriteEffects flip = SpriteEffects.None;
            if (FlipHorizontally)
                flip = SpriteEffects.FlipHorizontally;
            else if (FlipVertically)
                flip = SpriteEffects.FlipVertically;
            if (DrawShadow)
                Engine.SpriteBatch.DrawString(Font, drawText, Offset + Position + Origin + new Vector2(1/Screen.Camera.Zoom), ShadowColor, Rotation, Origin + Pivot, Scale, flip, Layer);
            Engine.SpriteBatch.DrawString(Font, drawText, Offset + Position + Origin, Tint, Rotation, Origin + Pivot, Scale, flip, Layer);
            base.draw();
        }

        public void setSize(Rectangle size)
        {
            Size = size;
            splitText();
        }

        void splitText()
        {
            string result = "";
            string temp = Text;
            List<string> lines = new List<string>();
            float lineHeight = Font.MeasureString("A").Y;

            if (SplitWords)
            {
                int nextSplit = getNextSplit(temp);
                while (nextSplit != 0)
                {
                    lines.Add(temp.Substring(0, nextSplit));
                    temp = temp.Substring(nextSplit);
                    nextSplit = getNextSplit(temp);
                }
                lines.Add(temp);
            }
            else
            {
                string line = "";
                int nextSplit = getNextSplit(temp);
                while (nextSplit != 0)
                {
                    if (nextSplit + 1 != temp.Length)
                        line = temp.Substring(0, nextSplit + 1);
                    else
                        line = temp.Substring(0, nextSplit);
                    int split = line.LastIndexOf(' ');

                    if (split == -1)
                    {
                        lines.Add(line);
                        temp = temp.Substring(nextSplit);
                        nextSplit = getNextSplit(temp);
                    }
                    else
                    {
                        line = line.Substring(0, split);
                        lines.Add(line);
                        temp = temp.Substring(split+1);
                        nextSplit = getNextSplit(temp);
                    }
                }
                lines.Add(temp);
            }
            int breakcount = 0;
            for (int i = Line; i < lines.Count; i++)
            {
                breakcount += lines[i].Count(c => c == '\n');
                if ((i+breakcount) * lineHeight > Size.Height)
                    break;
                result += lines[i] + "\r\n";
            }

            if (Index > result.Length - 1)
                result = "";
            else
                result = result.Substring(Index);

            drawText = result;
        }

        int getNextSplit(string text)
        {
            float distance = Size.Width-Size.Width;
            for (int i = 0; i < text.Length; i++)
            {
                distance += Font.MeasureString(text[i].ToString()).X;
                if (distance > Size.Width)
                    return i;
            }
            return 0;
        }

    }
}
