using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace boardProto
{
    class ToolTileButton
    {
        Texture2D buttonTextureON;
        Texture2D buttonTextureOFF;
        Texture2D buttonTileTexture;
        bool buttonStateON;
        Vector2 buttonPosition = new Vector2();
        Vector2 buttonTilePosition = new Vector2();
        int buttonRow;

        public ToolTileButton(int _tileIndex, List<Texture2D> _buttonTextures, Texture2D _texture)
        {
            buttonTextureOFF = _buttonTextures[0];
            buttonTextureON = _buttonTextures[1];
            buttonTileTexture = _texture;
            buttonStateON = false;

            buttonRow = _tileIndex / 5;     // 5 buttons per row
            // Figures out the position depending on the row and _tileIndex (the count for 1x1 tiles)
            buttonPosition.X = 17 + buttonTextureON.Width * (_tileIndex - (int)(_tileIndex / 5) * 5);
            buttonTilePosition.X = buttonPosition.X + (buttonTextureON.Width / 2 - buttonTileTexture.Width / 2);
            buttonPosition.Y = 12 + buttonRow * buttonTextureON.Height;
            buttonTilePosition.Y = buttonPosition.Y + (buttonTextureON.Height / 2 - buttonTileTexture.Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D _selectedTile)
        {
            // Omits the #x# from the end of the texture and checks if the button being drawn has
            // the same name as the selected tile name.
            if (_selectedTile.ToString().Remove(_selectedTile.ToString().Length - 3) ==
                buttonTileTexture.ToString().Remove(buttonTileTexture.ToString().Length - 3))
                spriteBatch.Draw(buttonTextureON, buttonPosition, null, Color.White, 0f, 
                                 Vector2.Zero, 1f, SpriteEffects.None, 0f);
            else
                spriteBatch.Draw(buttonTextureOFF, buttonPosition, null, Color.White, 0f, 
                                 Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Draws the tile inside the button
            spriteBatch.Draw(buttonTileTexture, buttonTilePosition, null, Color.White, 0f, 
                                 Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
