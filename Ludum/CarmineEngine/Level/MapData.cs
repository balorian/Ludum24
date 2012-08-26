using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CarmineEngine.Level
{
    public class MapData
    {
        public string name;
        public string mapImage;
        public string levelTemplate;
        public List<LevelDoorData> exitPoints;
        public List<SpawnPoint> spawnPoints;

    }
}
