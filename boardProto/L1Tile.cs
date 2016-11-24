using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace boardProto
{
    class L1Tile
    {
        Vector2 tilePosition;
        Vector2 tileOffset;
        bool passable;
        EditorManager.L1TileType tileType;
        Texture2D tileTexture;

        public L1Tile(EditorManager.L1TileType _tileType , Vector2 _tilePosition, bool _passable, Texture2D _tileTexture)
        {
            tileType = _tileType;
            if (tileType == EditorManager.L1TileType.GrassThick)
                tileOffset = new Vector2(0, -5);
            else
                tileOffset = new Vector2(0, 0);
            tilePosition = _tilePosition;
            passable = _passable;
            tileTexture = _tileTexture;
        }

        public Vector2 TileOffset
        {
            get { return tileOffset; }
            set { tileOffset = value; }
        }
        public Vector2 TilePosition
        {
            get { return tilePosition; }
            set { tilePosition = value; }
        }
        public Texture2D TileTexture
        {
            get { return tileTexture; }
            //set { tileTexture = value; }
        }
    }
}
