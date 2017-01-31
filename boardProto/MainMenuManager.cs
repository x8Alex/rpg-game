using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace boardProto
{
    class MainMenuManager
    {
        bool exitGame = false;
        bool newMap = false;
        bool generateMapData = false;
        bool loadMapData = false;

        MapLoader mapLoader;
        MapSaver mapSaver;

        Texture2D buttonTexture;
        List<MainMenuButton> listMenuButtons = new List<MainMenuButton>();
        MouseState oldState;
        int menuButtonIndex = -1;

        public void Initialize(Texture2D _buttonTexture, SpriteFont _font)
        {
            mapLoader = new MapLoader();
            mapSaver = new MapSaver();

            buttonTexture = _buttonTexture;
            CreateButton("New Map", _font);
            CreateButton("Load Map", _font);
            CreateButton("Save Map", _font);
            CreateButton("Exit", _font);
        }

        // Create menu buttons
        public void CreateButton(String _buttonText, SpriteFont _font)
        {
            listMenuButtons.Add(new MainMenuButton(new Vector2(5, listMenuButtons.Count * 35 + 5), _buttonText, _font));
        }

        // Detects if any button was clicked
        public void MenuClickDetector(MouseState _mouseState)
        {
            if (_mouseState.LeftButton == ButtonState.Pressed &&
                oldState.LeftButton != ButtonState.Pressed)
            {
                // Finds and assigns the index of the button that is being clicked
                menuButtonIndex = listMenuButtons.FindIndex(a => a.ButtonRectangle.X <= _mouseState.X &&
                                                                 a.ButtonRectangle.Width + a.ButtonRectangle.X >= _mouseState.X &&
                                                                 a.ButtonRectangle.Y <= _mouseState.Y &&
                                                                 a.ButtonRectangle.Height + a.ButtonRectangle.Y >= _mouseState.Y);
                Console.WriteLine("Index of button clicked: {0}", menuButtonIndex);

                switch (menuButtonIndex)
                {
                    case 0:
                        {
                            newMap = true;
                            break;
                        }
                    case 1:
                        {
                            loadMapData = true;
                            break;
                        }
                    case 2:
                        {
                            generateMapData = true;
                            break;
                        }
                    case 3:
                        {
                            exitGame = true;
                            break;
                        }
                }
            }

            oldState = _mouseState;
        }

        // Draw menu buttons
        public void Draw(SpriteBatch _spriteBatch, SpriteFont _font)
        {
            foreach (var _button in listMenuButtons)
            {
                _spriteBatch.Draw(buttonTexture, _button.ButtonRectangle, Color.White);
                _spriteBatch.DrawString(_font, _button.ButtonText, _button.ButtonTextPosition, Color.WhiteSmoke);
            }
        }

        public Boolean ExitGame
        {
            get { return exitGame; }
        }
        public bool NewMap
        {
            get { return newMap; }
            set { newMap = value; }
        }
        public bool RetrieveMapData
        {
            get { return generateMapData; }
            set { generateMapData = value; }
        }
        public bool LoadMapData
        {
            get { return loadMapData; }
            set { loadMapData = value; }
        }
        internal MapSaver MapSaver
        {
            get { return mapSaver; }
            set { mapSaver = value; }
        }
        internal MapLoader MapLoader
        {
            get { return mapLoader; }
            set { mapLoader = value; }
        }

    }

    // Class for individual the individual button objects
    class MainMenuButton
    {
        Vector2 buttonPosition;
        Vector2 buttonTextPosition;
        Rectangle buttonRectangle = new Rectangle(0, 0, 145, 30);
        String buttonText;

        public MainMenuButton(Vector2 _buttonPosition, String _buttonText, SpriteFont _font)
        {
            buttonPosition = _buttonPosition;
            buttonText = _buttonText;
            // Centers the text in the button
            buttonTextPosition = new Vector2(buttonPosition.X + (buttonRectangle.Width / 2 - _font.MeasureString(buttonText).X / 2), 
                                             buttonPosition.Y + (buttonRectangle.Height / 2 - _font.MeasureString(buttonText).Y / 2));
            Console.WriteLine(_font.MeasureString(buttonText).X / 2);
            buttonRectangle.X = (int)(buttonPosition.X);
            buttonRectangle.Y = (int)(buttonPosition.Y);
        }

        public Rectangle ButtonRectangle
        {
            get { return buttonRectangle; }
        }
        public String ButtonText
        {
            get { return buttonText; }
        }
        public Vector2 ButtonTextPosition
        {
            get { return buttonTextPosition; }
        }
    }
}
