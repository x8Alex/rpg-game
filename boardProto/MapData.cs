using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace boardProto
{
    class MapData
    {
        string mapName;
        int mapID;
        Vector2 centralRefPoint;
        List<L1Tile> l1Tiles = new List<L1Tile>();
        List<L2Tile> l2Tiles = new List<L2Tile>();
        List<Vector2> l3Tiles = new List<Vector2>();
        List<Vector3> npcs = new List<Vector3>();       // X, Y, ID
        List<Vector3> enemies = new List<Vector3>();
        List<Vector3> events = new List<Vector3>();

        public MapData(string _name, List<L1Tile> _l1Tiles, List<L2Tile> _l2Tiles)
        {
            mapName = _name;
            l1Tiles = _l1Tiles;
            l2Tiles = _l2Tiles;
        }

        // Constructor for loading
        public MapData()
        {

        }

        public string MapName
        {
            get { return mapName; }
            set { mapName = value; }
        }
        internal List<L1Tile> L1Tiles
        {
            get { return l1Tiles; }
            set { l1Tiles = value; }
        }
        internal List<L2Tile> L2Tiles
        {
            get { return l2Tiles; }
            set { l2Tiles = value; }
        }
    }
}
