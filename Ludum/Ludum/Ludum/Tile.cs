using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarmineEngine;
using Microsoft.Xna.Framework;

namespace Ludum
{
    public enum TileType { none, water, block, slope14, slope45 }

    class Tile
    {
        public Vector2 Position { get { return position; } set { position = value; TileSprite.Position = value; } }
        public TileType TileType { get { return type; } }
        public Sprite TileSprite;

        TileType type;
        Vector2 position;

        public Tile(GameScreen screen, TileType type, Vector2 position)
        {
            this.type = type;
            TileSprite = new Sprite(screen, type.ToString());
            TileSprite.OverrideDraw = true;
            TileSprite.OverrideUpdate = true;
            Position = position;
        }

        public virtual void draw()
        {
            TileSprite.draw();
        }
    }
}
