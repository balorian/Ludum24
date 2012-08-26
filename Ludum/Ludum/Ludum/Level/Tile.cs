using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarmineEngine;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Ludum.Level
{
    public enum TileType { none, water, block, slope45, block_bg, slope18_left, slope18_right }

    class Tile
    {
        public Vector2 Position { get { return position; } set { position = value; if(TileSprite != null){TileSprite.Position = value;} } }
        public TileType TileType { get { return type; } }
        public Sprite TileSprite;

        TileType type;
        Vector2 position;

        public Tile(GameScreen screen, TileType type, Vector2 position, String template)
        {
            this.type = type;
            if (type != TileType.none)
            {
                TileSprite = new Sprite(screen, template);
            }
            Position = position;
        }

        internal void updateTile(TileType[,] n)
        {
            switch (type)
            {
                case TileType.none:
                    break;
                case TileType.block_bg:
                    TileSprite.setFrame(17);
                    if(n[0,1] == TileType.none){
                        TileSprite.setFrame(21);
                    }
                    break;
                case TileType.block:
                    TileSprite.setFrame(0);
                    break;
                case TileType.slope45:
                    if (n[1, 2] == TileType.none)
                    { // Below is clear
                        if (n[0, 1] == TileType.none)
                        { // Left is clear
                            TileSprite.setFrame(3);
                        }
                        else
                        {
                            TileSprite.setFrame(4);
                        }
                    }
                    else
                    { // Above is clear
                        if (n[0, 1] == TileType.none)
                        { // Left is clear
                            TileSprite.setFrame(1);
                        }
                        else
                        {
                            TileSprite.setFrame(2);
                        }
                    }
                    break;
                case TileType.slope18_left:
                    if (n[1, 0] == TileType.none)
                    { // Above is clear
                        if (n[0, 1] == TileType.none)
                        { // Left is clear
                            TileSprite.setFrame(5);
                        }
                        else if (n[0, 1] == TileType.slope18_left)
                        { // Left is sloap
                            if (n[2, 1] == TileType.slope18_left)
                            { // Right is sloap
                                TileSprite.setFrame(6);
                            }
                            else
                            {
                                TileSprite.setFrame(7);
                            }
                        }
                    }
                    else
                    {
                        if (n[0, 1] == TileType.none)
                        { // Left is clear
                            TileSprite.setFrame(12);
                        }
                        else if (n[0, 1] == TileType.slope18_left)
                        { // Left is sloap
                            if (n[2, 1] == TileType.slope18_left)
                            { // Right is sloap
                                TileSprite.setFrame(13);
                            }
                            else
                            {
                                TileSprite.setFrame(11);
                            }
                        }
                    }
                    
                    break;
                case TileType.slope18_right:
                    if (n[1, 0] == TileType.none)
                    { // Above is clear
                        if (n[2, 1] == TileType.none)
                        { // Right is clear
                            TileSprite.setFrame(10);
                        }
                        else if (n[2, 1] == TileType.slope18_right)
                        { // Left is sloap
                            if (n[0, 1] == TileType.slope18_right)
                            { // Left is sloap
                                TileSprite.setFrame(9);
                            }
                            else
                            {
                                TileSprite.setFrame(8);
                            }
                        }
                    }
                    else
                    {
                        if (n[2, 1] == TileType.none)
                        { // Right is clear
                            TileSprite.setFrame(16);
                        }
                        else if (n[2, 1] == TileType.slope18_right)
                        { // Right is sloap
                            if (n[0, 1] == TileType.slope18_right)
                            { // Left is sloap
                                TileSprite.setFrame(15);
                            }
                            else
                            {
                                TileSprite.setFrame(14);
                            }
                        }
                    }
                    break;
            }
        }
    }
}