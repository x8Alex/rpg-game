using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

namespace boardProto
{
    class L2Tile
    {
        Vector2 tilePosition;
        Vector2 tileOffset;
        bool passable;
        EditorManager.L2TileType tileType;
        int textureIndex;

        public L2Tile(EditorManager.L2TileType _tileType, Vector2 _tilePosition, bool _passable, int _textureIndex)
        {
            tileType = _tileType;
            if (tileType == EditorManager.L2TileType.RockS)
                tileOffset = new Vector2(0, 0);
            else if (tileType == EditorManager.L2TileType.RockM)
                tileOffset = new Vector2(0, -75);
            else if (tileType == EditorManager.L2TileType.RockL)
                tileOffset = new Vector2(0, -260);
            else
                tileOffset = new Vector2(0, 0);

            tilePosition = _tilePosition;
            passable = _passable;
            textureIndex = _textureIndex;
        }

        // Constructor for when loading tiles
        public L2Tile()
        {
            tileType = EditorManager.L2TileType.Default;
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
        public int TextureIndex
        {
            get { return textureIndex; }
            set { textureIndex = value; }
        }
        internal EditorManager.L2TileType TileType
        {
            get { return tileType; }
            set { tileType = value; }
        }
    }
}
