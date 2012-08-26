using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using CarmineEngine;

namespace Ludum
{
    public enum Side { None, Top, Bottom, Left, Right}

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
                if (!entity.UseGeometry)
                    return true;
                else
                    return collideGeometries(new Rectangle((int)Position.X, (int)Position.Y, BoundingBox.Width, BoundingBox.Height), entity);
            }
            return false;

        }

        public bool collideBoxes(Entity entity)
        {
            Rectangle box1 = new Rectangle((int)Position.X, (int)Position.Y, BoundingBox.Width, BoundingBox.Height);
            Rectangle box2 = new Rectangle((int)entity.Position.X, (int)entity.Position.Y, entity.BoundingBox.Width, entity.BoundingBox.Height);
            return box1.Intersects(box2);
        }

        public bool collideGeometries(Rectangle box1, Entity entity)
        {
            Rectangle box2 = new Rectangle((int)entity.Position.X, (int)entity.Position.Y, entity.BoundingBox.Width, entity.BoundingBox.Height);
            Rectangle intersect = Rectangle.Intersect(box1, box2);
            intersect.X -= box2.X;
            intersect.Y -= box2.Y;

            for (int x = 0; x < intersect.Width; x++)
                for (int y = 0; y < intersect.Height; y++)
                    if (entity.BoundingGeometry[x + intersect.X, y + intersect.Y])
                        return true;
            return false;
        }

    }
}
