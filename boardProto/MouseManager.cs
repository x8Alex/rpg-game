using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace boardProto
{
    class MouseManager
    {
        MouseState MOUSE_STATE;
        Vector2 WORLD_SCROLL = new Vector2(0,0);
        Vector2 MOUSE_POSITION = new Vector2 (0, 0);
        Vector2 LAST_MOUSE_POSITION = new Vector2 (0, 0);

        public MouseState GetMouseState()
        {
            MOUSE_STATE = Mouse.GetState();
            return MOUSE_STATE;
        }
    }
}
