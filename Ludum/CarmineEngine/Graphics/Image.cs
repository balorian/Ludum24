using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CarmineEngine
{
    public class Image : GraphicsComponent
    {
        public static Dictionary<string, Texture2D> Images = new Dictionary<string, Texture2D>();


        string imageTexture;
        string ImageTexture {
            get { return imageTexture; }
            set
            { 
                imageTexture = value;
                if (!Images.ContainsKey(imageTexture))
                    Images[imageTexture] = Engine.Content.Load<Texture2D>(imageTexture);
            }
        }

        public Image(GameScreen screen, string image) : base(screen)
        {
            ImageTexture = image;
            Size = Images[ImageTexture].Bounds;
        }
        public Image(GameScreen screen, string image, Rectangle size) : base(screen)
        {
            ImageTexture = image;
            Size = size;
        }

        public override void draw()
        {
            SpriteEffects flip = SpriteEffects.None;
            if (FlipHorizontally)
                flip = SpriteEffects.FlipHorizontally;
            else if (FlipVertically)
                flip = SpriteEffects.FlipVertically;
            Engine.SpriteBatch.Draw(Images[imageTexture], new Rectangle((int)(Offset.X+Position.X), (int)(Offset.Y+Position.Y), Size.Width, Size.Height), null, Tint, Rotation, Origin + Pivot, flip, Layer);
            base.draw();
        }

        public void draw(Rectangle destination)
        {
            SpriteEffects flip = SpriteEffects.None;
            if (FlipHorizontally)
                flip = SpriteEffects.FlipHorizontally;
            else if (FlipVertically)
                flip = SpriteEffects.FlipVertically;
            Engine.SpriteBatch.Draw(Images[imageTexture], destination, null, Tint, Rotation, Origin + Pivot, flip, Layer);
        }
    }
}
