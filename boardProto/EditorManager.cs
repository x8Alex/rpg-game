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
    class EditorManager
    {
        Vector2 WORLD_OFFSET = new Vector2(0, 0);
        MouseState MOUSE_STATE;
        Vector2 LAST_MOUSE_POSITION;
        Vector2 MOUSE_POSITION;

        List<Texture2D> TEXTURE_LIST;
        Texture2D EMPTY_SPACE;
        Texture2D GRID_GRAY;
        Texture2D DIRT_TILE1;

        // Layers of the map
        List<Vector3> layer1;   // X, Y, PASSABLE
        List<Vector3> layer2;
        List<Vector3> layer3;


        public void Initialize(List<Texture2D> texture_list)
        {
            TEXTURE_LIST = texture_list;
            EMPTY_SPACE = TEXTURE_LIST[0];
            GRID_GRAY = TEXTURE_LIST[1];
            DIRT_TILE1 = TEXTURE_LIST[2];
        }

        public void ScrollWorld(MouseState mouseState)
        {
            MOUSE_STATE = mouseState;
            if (MOUSE_STATE.RightButton != ButtonState.Pressed)
            {
                LAST_MOUSE_POSITION.X = MOUSE_STATE.X;
                LAST_MOUSE_POSITION.Y = MOUSE_STATE.Y;
            }
            if (MOUSE_STATE.RightButton == ButtonState.Pressed)
            {
                WORLD_OFFSET.X += MOUSE_STATE.X - LAST_MOUSE_POSITION.X;
                WORLD_OFFSET.Y += MOUSE_STATE.Y - LAST_MOUSE_POSITION.Y;
                LAST_MOUSE_POSITION.X = MOUSE_STATE.X;
                LAST_MOUSE_POSITION.Y = MOUSE_STATE.Y;
            }
        }

        // Detects which tile the mouse is hovered over and returns the
        // calculated coordinates of the left corner.
        public Vector2 DetectClosestTilePosition(Vector2 mouseWorldPosition)
        {
            Vector2 MOUSE_POSITION = new Vector2(mouseWorldPosition.X, mouseWorldPosition.Y);
            double bottomBoundary = -0.36375 * Math.Abs((Math.Abs(mouseWorldPosition.X) -
                                    (int)(Math.Abs(mouseWorldPosition.X) / 80) * 80) - 40) + 14.56;
            double topBoundary = 0.36375 * Math.Abs((Math.Abs(mouseWorldPosition.X) - 
                                    (int)(Math.Abs(mouseWorldPosition.X) / 80) * 80) - 40) - 14.56;
            float calcYValue = (float)(80 * Math.Tan(20 * Math.PI / 180));

            if (Math.Abs(mouseWorldPosition.Y) <= 14.56)    // Checks for tiles at Y = 0
            {
                if (mouseWorldPosition.Y <= bottomBoundary
                    && mouseWorldPosition.Y > topBoundary)
                {
                    if (mouseWorldPosition.X > 0)
                    {   // When X is positive
                        return new Vector2((float)((int)(mouseWorldPosition.X / 80) * 80), 0f);
                    }
                    else
                    {   // When X is negative
                        return new Vector2((float)((int)(mouseWorldPosition.X / 80 - 1) * 80), 0f);
                    }
                }
            }
            else
            {   // When mouse y value is greater than the closest multiple of 29
                if (mouseWorldPosition.Y - ((int)(mouseWorldPosition.Y / calcYValue) * calcYValue) <= bottomBoundary    // Bottom boundary y value should be greater than mouse y value
                    && mouseWorldPosition.Y - ((int)(mouseWorldPosition.Y / calcYValue) * calcYValue) > topBoundary)    // Top boundary y value should be smaller than mouse y value
                {
                    /*if ((int)(mouseWorldPosition.Y / calcYValue) < (int)(mouseWorldPosition.Y / 29))    // 
                    {*/
                        if (mouseWorldPosition.X > 0)
                        {   // When X is positive
                            return new Vector2((float)((int)(mouseWorldPosition.X / 80) * 80),
                                           (int)(mouseWorldPosition.Y / calcYValue) * calcYValue);
                        }
                        else
                        {   // When X is negative
                            return new Vector2((float)((int)(mouseWorldPosition.X / 80 - 1) * 80),
                                           (int)(mouseWorldPosition.Y / calcYValue) * calcYValue);
                        }
                    /*}
                    else
                    {   // When X is positive
                        if (mouseWorldPosition.X > 0)
                        {
                            return new Vector2((float)((int)(mouseWorldPosition.X / 80) * 80),
                                               (int)(mouseWorldPosition.Y / calcYValue) * calcYValue);
                        }
                        else
                        {   // When X is negative
                            return new Vector2((float)((int)(mouseWorldPosition.X / 80 - 1) * 80),
                                               (int)(mouseWorldPosition.Y / calcYValue) * calcYValue);
                        }
                    }*/
                }
                // When mouse y value is less than the closest multiple of 29
                else if (((int)(mouseWorldPosition.Y / calcYValue + 1) * calcYValue) - mouseWorldPosition.Y <= bottomBoundary    // Bottom boundary y value should be greater than mouse y value
                    && mouseWorldPosition.Y - ((int)(mouseWorldPosition.Y / calcYValue + 1) * calcYValue) > topBoundary)    // Top boundary y value should be smaller than mouse y value
                {
                    if (mouseWorldPosition.X > 0)
                    {   // When X is positive
                        return new Vector2((float)((int)(mouseWorldPosition.X / 80) * 80),
                                           (int)(mouseWorldPosition.Y / calcYValue + 1) * calcYValue);
                    }
                    else
                    {   // When X is negative
                        return new Vector2((float)((int)(mouseWorldPosition.X / 80 - 1) * 80),
                                           (int)(mouseWorldPosition.Y / calcYValue + 1) * calcYValue);
                    }
                }
            }

            // Slope is 0.36375
            // Vertical gap between tiles is 29.1176
            return new Vector2(mouseWorldPosition.X, mouseWorldPosition.Y);
        }

        public void DrawEmptySpace(SpriteBatch spriteBatch, Viewport viewport){
            spriteBatch.Draw(EMPTY_SPACE, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);
        }

        public void DrawTiles(SpriteBatch spriteBatch, Vector2 pos)
        {
            spriteBatch.Draw(DIRT_TILE1, new Vector2(pos.X, pos.Y - DIRT_TILE1.Height / 2) + WORLD_OFFSET, 
                             null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public void DrawGrid(SpriteBatch spriteBatch, Viewport viewport)
        {
            for (int x = (int)(-0.5 * viewport.Width); x <= viewport.Width / 80; x++)
                DrawLine(spriteBatch, new Vector2(x * 80 + WORLD_OFFSET.X % 80, -29.1176f + WORLD_OFFSET.Y % 29.1176f),
                                                  new Vector2(viewport.Width, viewport.Height), 20f);

            for (int x = 0; x <= viewport.Width * 3 / 80; x++)
                DrawLine(spriteBatch, new Vector2(x * 80 + WORLD_OFFSET.X % 80, -29.1176f + WORLD_OFFSET.Y % 29.1176f),
                                                  new Vector2(0, viewport.Height), 160f);
        }

        // Method used to draw lines, takes in SpriteBatch, begining point and end point.
        private void DrawLine(SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, float angleDeg)
        {
            Vector2 line = point2 - point1;
            //float angle = (float)Math.Atan2(line.Y, line.X);    // Calculates angle
            float angleRad = (float)(angleDeg * Math.PI / 180f);    // Angle in raidans

            spriteBatch.Draw(GRID_GRAY,
                             new Rectangle((int)point1.X, (int)point1.Y, (int)line.Length(), 1),
                             null, Color.DarkGray, angleRad, new Vector2(0, 0), SpriteEffects.None, 0);
        }

        public Vector2 GetWorldOffset()
        {
            return WORLD_OFFSET;
        }
    }
}
