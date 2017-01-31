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
        bool managerActive = true;
        MouseState oldMouseState;
        MouseState MOUSE_STATE;
        Vector2 RESOLUTION;
        Vector2 WORLD_SCROLL = new Vector2(0,0);
        Vector2 MOUSE_POSITION = new Vector2 (0, 0);
        Vector2 LAST_MOUSE_POSITION = new Vector2 (0, 0);
        Vector2 VIEWPORT;

        public void Initialize(Vector2 resolution, Vector2 _viewport)
        {
            RESOLUTION = resolution;
            VIEWPORT = _viewport;
            MOUSE_STATE = Mouse.GetState();
        }

        // Returns the mouse position adjusted for the virtual resolution
        public Vector2 GetMousePosition()
        {
            if (managerActive)
            {
            MOUSE_STATE = Mouse.GetState();
            if (VIEWPORT.X < RESOLUTION.X && VIEWPORT.Y < RESOLUTION.Y)
                return new Vector2(MOUSE_STATE.X / (VIEWPORT.X / RESOLUTION.X),
                                   MOUSE_STATE.Y / (VIEWPORT.Y / RESOLUTION.Y));
            else
                return new Vector2(MOUSE_STATE.X,
                                   MOUSE_STATE.Y);
            }
            else
            {
                return new Vector2();
            }
        }

        // Returns the mouse state
        public MouseState GetMouseState()
        {
            MOUSE_STATE = Mouse.GetState();
            return MOUSE_STATE;
        }
        public bool ManagerActive
        {
            get { return managerActive; }
            set { managerActive = value; }
        }
        public MouseState MouseState
        {
            get { return MOUSE_STATE; }
            set { MOUSE_STATE = value; }
        }
        public MouseState OldMouseState
        {
            get { return oldMouseState; }
            set { oldMouseState = value; }
        }
    }
}
