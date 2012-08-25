using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CarmineEngine
{
    public struct Frame 
    {
        public Rectangle sourceRectangle;
        public Vector2 spriteOrigin;

        public Frame(Rectangle sourceRectangle)
        {
            this.sourceRectangle = sourceRectangle;
            spriteOrigin = Vector2.Zero;
        }

        public Frame(Rectangle sourceRectangle, Vector2 spriteOrigin)
        {
            this.sourceRectangle = sourceRectangle;
            this.spriteOrigin = spriteOrigin;
        }
    }

    public class Sprite : GraphicsComponent
    {
        public const string IDLE = "IDLE";
        public static Dictionary<string, Texture2D> SpriteSheets = new Dictionary<string, Texture2D>();
        public static Dictionary<string, SpriteTemplate> SpriteTemplates = new Dictionary<string, SpriteTemplate>();

        internal string sheet;
        internal string Sheet 
        {
            get {return sheet;}
            set 
            {
                sheet = value;
                if (!SpriteSheets.ContainsKey(sheet))
                    LoadSheet(sheet);
            }
        }

        internal Frame[] frames;
        int currentFrame = 0;
        internal bool animated = false;
        internal Dictionary<string, Animation> animations = new Dictionary<string,Animation>();
        Queue<string> animationQueue = new Queue<string>();
        string currentAnimation = IDLE;

        public static void LoadSheet(string sheetDir) 
        {
            Sprite.SpriteSheets[sheetDir] = Engine.Content.Load<Texture2D>(sheetDir);
        }

        public Sprite(GameScreen screen, string spriteTemplate) : base(screen) 
        {
            SpriteTemplate temp = SpriteTemplates[spriteTemplate];
            temp.extractData(this);
        }

        public override void update()
        {
            
            if (animated)
            {
                
                if (animations.ContainsKey(currentAnimation))
                {
                    animations[currentAnimation].update();
                    currentFrame = animations[currentAnimation].getFrame();
                }
                else
                {
                    animations[IDLE].update();
                    currentFrame = animations[IDLE].getFrame();
                }
            }
            base.update();
        }

        public override void draw() 
        {

            if (currentFrame > frames.Length || currentFrame < 0)
                throw new Exception("Frame " + currentFrame.ToString() + " was out of bounds");
            else{
                SpriteEffects flip = SpriteEffects.None;
                if (FlipHorizontally)
                    flip = SpriteEffects.FlipHorizontally;
                else if (FlipVertically)
                    flip = SpriteEffects.FlipVertically;
                Engine.SpriteBatch.Draw(SpriteSheets[sheet], Offset + Position + Pivot, frames[currentFrame].sourceRectangle, Tint, Rotation, Origin + Pivot + frames[currentFrame].spriteOrigin, Scale, flip, Layer);
            }

            base.draw();
        }

        public void setFrame(int i) 
        {
            currentFrame = i;
        }
        public Frame getCurrentFrame()
        {
            return frames[currentFrame];
        }
        public Frame getFrameAt(int index) 
        {
            if (index < 0 || index > frames.GetUpperBound(0))
                throw new Exception("Frame index " + index + " out of bounds");
            return frames[index];
        }

        public void pause()
        {
            animations[currentAnimation].Paused = true; ;
        }
        public void resume()
        {
            animations[currentAnimation].Paused = false;
        }

        public void addAnimation(Animation animation)
        {
            animations[animation.Name] = animation;
            animation.Parent = this;
        }
        public void removeAnimation(string name)
        {
            animations.Remove(name);
        }
        public void setSpeed(string name, float speed)
        {
            animations[name].Speed = speed;
        }

        public void play(string animation)
        {
            if (animations.ContainsKey(animation))
            {
                currentAnimation = animation;
                animations[currentAnimation].play();
            }else
                throw new Exception("animation " + animation + " doesn't exist");
        }
        public void playNext() 
        {
            if (animationQueue.Count > 0) 
            {
                currentAnimation = animationQueue.Dequeue();
                animations[currentAnimation].play();
            }
            else
            {
                currentAnimation = IDLE;
                animations[currentAnimation].play();
            }
        }
        public void playNext(int overflow) 
        {
            if (animationQueue.Count > 0)
            {
                currentAnimation = animationQueue.Dequeue();
                animations[currentAnimation].play(overflow);
            }
            else 
            {
                currentAnimation = IDLE;
                animations[currentAnimation].play(overflow);
            }
        }

        public void queue(string animation)
        {
            animationQueue.Enqueue(animation);
        }
        public void emptyQueue()
        { 
            animationQueue = new Queue<string>();
        }
        public void resetAnimation() 
        {
            animations[currentAnimation].reset();
        }

        public void loopAnimation(string animation)
        {
            animations[animation].Loops = true;
        }
        public void stopLoopingAnimation(string animation)
        {
            animations[animation].Loops = false;
        }

        public bool[,] getGeometry()
        {
            Rectangle sourceRectangle = getCurrentFrame().sourceRectangle;
            Color[] rawData = new Color[sourceRectangle.Width * sourceRectangle.Height];
            SpriteSheets[sheet].GetData<Color>(0, getCurrentFrame().sourceRectangle, rawData, 0, rawData.Length);
            bool[,] geometry = new bool[sourceRectangle.Width, sourceRectangle.Height];

            for (int x = 0; x < geometry.GetLength(0); x++)
                for (int y = 0; y < geometry.GetLength(1); y++)
                    geometry[x, y] = !(rawData[x + y * sourceRectangle.Width] == Color.Transparent);

            return geometry;
        }

        public Matrix getMatrix()
        {
            return Matrix.CreateTranslation(-(Origin.X + Pivot.X + frames[currentFrame].spriteOrigin.X), -(Origin.Y + Pivot.Y + frames[currentFrame].spriteOrigin.Y), 0) 
                * Matrix.CreateRotationZ(Rotation) * Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Offset.X + Position.X + Pivot.X, Offset.Y + Position.Y + Pivot.Y, 0);
        }

    }
}
