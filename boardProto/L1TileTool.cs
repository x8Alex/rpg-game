using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace boardProto
{
    class L1TileTool
    {
        List<Texture2D> listTileTextures;

        Texture2D selectedTileTexture;
        bool tileSelectionMenuShow;

        public L1TileTool(List <Texture2D> _listTileTextures)
        {
            listTileTextures = _listTileTextures;
            selectedTileTexture = listTileTextures[0];
            tileSelectionMenuShow = true;
        }

        public void TileSelection()
        {

        }

        

        public List<Texture2D> ListTileTextures
        {
            get { return listTileTextures; }
            set { listTileTextures = value; }
        }

        public bool TileSelectionMenuShow
        {
            get { return tileSelectionMenuShow; }
            set { tileSelectionMenuShow = value; }
        }

        public Texture2D SelectedTileTexture
        {
            get { return selectedTileTexture; }
            set { selectedTileTexture = value; }
        }
    }
}
