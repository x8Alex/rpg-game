using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

namespace boardProto
{
    class L1Tile
    {
        Vector2 tilePosition;
        Vector2 tileOffset;
        bool passable;
        EditorManager.L1TileType tileType;
        string textureID;

        public L1Tile(EditorManager.L1TileType _tileType , Vector2 _tilePosition, bool _passable, string _textureID)
        {
            tileType = _tileType;
            if (tileType == EditorManager.L1TileType.GrassThick)
                tileOffset = new Vector2(0, -5);
            else
                tileOffset = new Vector2(0, 0);

            tilePosition = _tilePosition;
            passable = _passable;
            textureID = _textureID;
        }

        // Constructor for when loading tiles
        public L1Tile()
        {
            tileType = EditorManager.L1TileType.Default;
            passable = true;
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
        internal EditorManager.L1TileType TileType
        {
            get { return tileType; }
            set { tileType = value; }
        }
        public string TextureID
        {
            get { return textureID; }
            set { textureID = value; }
        }
    }
}
