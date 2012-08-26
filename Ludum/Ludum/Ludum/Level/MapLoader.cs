using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ludum.Level;
using Microsoft.Xna.Framework;
using CarmineEngine;
using CarmineEngine.Level;
using Microsoft.Xna.Framework.Graphics;

namespace Ludum.Level
{
    class MapLoader
    {
        public static Dictionary<Color, TileType> COLOR_DECODER = new Dictionary<Color, TileType>()
        {
            {new Color(0, 0, 0, 0), TileType.none},
            {new Color(127, 127, 0), TileType.block},
            {new Color(73, 73, 0), TileType.block_bg},
            {new Color(204, 204, 0), TileType.slope45},
            {new Color(239, 239, 0), TileType.slope18_left},
            {new Color(239, 186, 0), TileType.slope18_right},
            {new Color(148, 255, 0), TileType.water},
        };

        public static Map loadRawMap(MapData mapData, GameScreen parent)
        {
            Map map = new Map(mapData.name, parent);
            Texture2D mapTexture = Engine.Content.Load<Texture2D>("levels/maps/" + mapData.mapImage);
            Color[] colors1D = new Color[mapTexture.Width * mapTexture.Height];
            mapTexture.GetData<Color>(colors1D);
            Color[,] rawMap = new Color[mapTexture.Width, mapTexture.Height];
            for (int x = 0; x < mapTexture.Width; x++)
                for (int y = 0; y < mapTexture.Height; y++)
                    rawMap[x, y] = colors1D[x + y * mapTexture.Width];
            decodeMap(map, mapData, rawMap);

            Dictionary<String, SpawnPoint> spawns = new Dictionary<string,SpawnPoint>();
            foreach(SpawnPoint point in mapData.spawnPoints){
                spawns.Add(point.name, point);
            }
            map.SpawnPoints = spawns;
            map.LevelDoors = mapData.exitPoints;

            return map;
        }

        internal static void decodeMap(Map map, MapData mapData, Color[,] rawMap)
        {
            map.Width = rawMap.GetLength(0);
            map.Height = rawMap.GetLength(1);
            map.TileMap = new Tile[map.Width, map.Height];

            for (int y = 0; y < rawMap.GetLength(1); y++)
            {
                for (int x = 0; x < rawMap.GetLength(0); x++)
                {
                    TileType type = TileType.none;
                    COLOR_DECODER.TryGetValue(rawMap[x, y], out type);
                    map.TileMap[x, y] = new Tile(map, type, new Vector2(x * GameState.TILE_SIZE, y * GameState.TILE_SIZE), mapData.levelTemplate);
                }
            }

            chooseFrames(map);
        }

        public static void chooseFrames(Map map)
        {
            for (int x = 0; x < map.Width; x++)
                for (int y = 0; y < map.Height; y++)
                {
                    map.TileMap[x, y].updateTile(getNeighbors(map, x, y));
                }
        }


        public static TileType[,] getNeighbors(Map map, int x, int y)
        {
            TileType[,] result = new TileType[3, 3] { { TileType.none, TileType.none, TileType.none }, { TileType.none, TileType.none, TileType.none }, { TileType.none, TileType.none, TileType.none } };
            Rectangle mapRec = new Rectangle(0, 0, map.Width, map.Height);
            Rectangle neighborhood = new Rectangle(x - 1, y - 1, 3, 3);
            Rectangle intersect = Rectangle.Intersect(mapRec, neighborhood);


            for (int i = 0; i < intersect.Width; i++)
                for (int j = 0; j < intersect.Height; j++)
                {
                    result[i + intersect.X - neighborhood.X, j + intersect.Y - neighborhood.Y] = map.TileMap[i + intersect.X, j + intersect.Y].TileType;
                }
            return result;
        }
    }
}
