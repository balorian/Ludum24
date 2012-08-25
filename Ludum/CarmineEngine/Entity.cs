using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CarmineEngine
{
    public class Entity : GraphicsComponent
    {
        public bool UseGeometry = false;
        public Sprite BoundingSprite;
        public Rectangle BoundingBox;
        public bool[,] BoundingGeometry;

        public Sprite Sprite;

        public Entity(GameScreen screen, Rectangle boundingBox, string sprite) : base(screen) 
        {
            Sprite = new Sprite(screen, sprite);
            Sprite.Tint = Tint;
            BoundingSprite = this.Sprite;
            BoundingBox = this.Sprite.Size;
            World.Entities.Add(this);
        }
        public Entity(GameScreen screen, string boundingSprite, string sprite) : base(screen)
        {
            BoundingSprite = new Sprite(screen, boundingSprite);
            BoundingSprite.Visible = false;
            BoundingBox = BoundingSprite.Size;
            BoundingGeometry = BoundingSprite.getGeometry();
            Sprite = new Sprite(screen, sprite);
            Sprite.Tint = Tint;
            World.Entities.Add(this);
        }

        public override void update()
        {
            base.update();
        }

        public override void draw()
        {
            base.draw();
        }

        public void moveTo(float x, float y)
        {
            Position = new Vector2(x, y);
            Sprite.Position = new Vector2(x, y);
        }

        public bool collidesWith(Entity entity)
        {
            if(collideBoxes(entity)){
                if (!UseGeometry && !entity.UseGeometry)
                    return true;
                else
                    return collideGeometries(entity);
            }
            return false;

        }

        bool collideBoxes(Entity entity)
        {
            Rectangle box1 = new Rectangle((int)Position.X, (int)Position.Y, BoundingBox.Width, BoundingBox.Height);
            Rectangle box2 = new Rectangle((int)entity.Position.X, (int)entity.Position.Y, entity.BoundingBox.Width, entity.BoundingBox.Height);
            return box1.Intersects(box2);
        }

        bool collideGeometries(Entity entity)
        {
            Matrix AtoB = BoundingSprite.getMatrix() * Matrix.Invert(entity.BoundingSprite.getMatrix());
            bool g1 = !UseGeometry;
            bool g2 = !entity.UseGeometry;

            for (int x = 0; x < BoundingBox.Width; x++)
            {
                for (int y = 0; y < BoundingBox.Height; y++)
                {
                    Vector2 position = Vector2.Transform(new Vector2(x, y), AtoB);
                    int X = (int)position.X;
                    int Y = (int)position.Y;
                    if(X >= 0  && X < entity.BoundingBox.Width && Y >= 0 && Y < entity.BoundingBox.Height)
                    {
                        if (!g1)
                            g1 =BoundingGeometry[x, y];
                        if (!g2)
                            g2 = entity.BoundingGeometry[x, y];
                        if (g1 && g2)
                            return true;
                    }
                }
            }
            return false;
        }

    }
}
