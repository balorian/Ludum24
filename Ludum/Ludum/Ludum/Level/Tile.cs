using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarmineEngine;
using Microsoft.Xna.Framework;

namespace Ludum.Level
{
    public enum TileType { none, water, block, slope45, block_bg, slope18_left, slope18_right }

    class Tile
    {
        public Vector2 Position { get { return position; } set { position = value; TileSprite.Position = value; } }
        public TileType TileType { get { return type; } }
        public Sprite TileSprite;

        TileType type;
        Vector2 position;

        public Tile(GameScreen screen, TileType type, Vector2 position, String template)
        {
            this.type = type;
            TileSprite = new Sprite(screen, template);
            TileSprite.OverrideDraw = true;
            TileSprite.OverrideUpdate = true;
            Position = position;
        }

        public virtual void draw()
        {
            TileSprite.draw();
        }

        internal void updateTile(TileType[,] n)
        {
            switch (type)
            {
                case TileType.block:
                    TileSprite.setFrame(19);
                    break;
                case TileType.slope45:
                    if (n[1, 2] == TileType.none)
                    { // Below is clear
                        if (n[0, 1] == TileType.none)
                        { // Left is clear
                            TileSprite.setFrame(8);
                        }
                        else
                        {
                            TileSprite.setFrame(9);
                        }
                    }
                    else
                    { // Above is clear
                        if (n[0, 1] == TileType.none)
                        { // Left is clear
                            TileSprite.setFrame(3);
                        }
                        else
                        {
                            TileSprite.setFrame(4);
                        }
                    }
                    break;
            }
        }
    }
}