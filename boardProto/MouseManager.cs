using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace boardProto
{
    class MouseManager
    {
        MouseState MOUSE_STATE;
        Vector2 RESOLUTION;
        Vector2 WORLD_SCROLL = new Vector2(0,0);
        Vector2 MOUSE_POSITION = new Vector2 (0, 0);
        Vector2 LAST_MOUSE_POSITION = new Vector2 (0, 0);

        public void Initialize(Vector2 resolution)
        {
            RESOLUTION = resolution;
        }

        // Returns the mouse position adjusted for the virtual resolution
        public Vector2 GetMousePosition()
        {
            MOUSE_STATE = Mouse.GetState();
            return new Vector2(MOUSE_STATE.X * (RESOLUTION.X / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width),
                               MOUSE_STATE.Y * (RESOLUTION.Y / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));
        }

        // Returns the mouse state
        public MouseState GetMouseState()
        {
            MOUSE_STATE = Mouse.GetState();
            return MOUSE_STATE;
        }
    }
}
