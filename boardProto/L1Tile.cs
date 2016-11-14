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
        bool passable;
        EditorManager.L1TileType tileType;
        Texture2D tileTexture;

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

        public L1Tile(EditorManager.L1TileType _tileType , Vector2 _tilePosition, bool _passable, Texture2D _tileTexture)
        {
            tileType = _tileType;
            tilePosition = _tilePosition;
            passable = _passable;
            tileTexture = _tileTexture;
        }
    }
}
