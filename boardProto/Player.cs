using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace boardProto
{
    class Player
    {
        public Texture2D playerTexture;
        public Vector2 position;
        public bool active;
        public int health;

        public int Width
        {
            get { return playerTexture.Width; }
        }

        public int Height
        {
            get { return playerTexture.Height; }
        }

        public void Initialize(Texture2D texture, Vector2 position)
        {
            playerTexture = texture;
            position.X = position.X - texture.Width / 2;
            position.Y = position.Y - texture.Height;
            active = true;
            health = 100;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos)
        {
            spriteBatch.Draw(playerTexture, pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
