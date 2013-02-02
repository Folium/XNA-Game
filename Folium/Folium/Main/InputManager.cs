using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Folium.Main
{
    public class InputManager
    {
        private static KeyboardState currentKeyState;
        private static KeyboardState lastKeyState;

        private static MouseState currentMouseState;
        private static MouseState lastMouseState;

        private static int deltaMouseScrollWheel;

        private static int prevMouseScrollWheel;

        private GameManager gameManager;

        public InputManager(GameManager gameManager)
        {
            this.gameManager        = gameManager;

            currentKeyState         = Keyboard.GetState();
            lastKeyState            = currentKeyState;

            currentMouseState       = Mouse.GetState();
            lastMouseState          = currentMouseState;

            deltaMouseScrollWheel   = 0;
            prevMouseScrollWheel    = 0;
        }

        public static void update()
        {
            //Update input-device states
            lastKeyState    = currentKeyState;
            currentKeyState = Keyboard.GetState();

            //Update mouse state
            lastMouseState      = currentMouseState;
            currentMouseState   = Mouse.GetState();

            //Update scrollwheel state
            deltaMouseScrollWheel   = currentMouseState.ScrollWheelValue - prevMouseScrollWheel;
            prevMouseScrollWheel    = currentMouseState.ScrollWheelValue;
        }

        /// <summary>
        /// Returns true if the provided key is down.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool isKeyDown(Keys key)
        {
            return currentKeyState.IsKeyDown(key);
        }

        /// <summary>
        /// Returns true if the provided key is up.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool isKeyUp(Keys key)
        {
            return currentKeyState.IsKeyUp(key);
        }

        /// <summary>
        /// Returns true if the provided key was down during the previous update and up during the current update.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool isKeyReleased(Keys key)
        {
            return (lastKeyState.IsKeyDown(key) && currentKeyState.IsKeyUp(key));
        }

        public static bool isMouseLeftDown()
        {
            return (currentMouseState.LeftButton == ButtonState.Pressed);
        }

        public static bool isMouseMiddleDown()
        {
            return (currentMouseState.MiddleButton == ButtonState.Pressed);
        }

        public static bool isMouseRightDown()
        {
            return (currentMouseState.RightButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Returns true if the LMB was down during the previous update and up during the current update.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool isMouseLeftReleased()
        {
            return (lastMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released);
        }

        /// <summary>
        /// Returns true if the MMB was down during the previous update and up during the current update.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool isMouseMiddleReleased()
        {
            return (lastMouseState.MiddleButton == ButtonState.Pressed && currentMouseState.MiddleButton == ButtonState.Released);
        }

        /// <summary>
        /// Returns true if the RMB was down during the previous update and up during the current update.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool isMouseRightReleased()
        {
            return (lastMouseState.RightButton == ButtonState.Pressed && currentMouseState.RightButton == ButtonState.Released);
        }

        public static int getMouseX()
        {
            return currentMouseState.X;
        }

        public static int getMouseY()
        {
            return currentMouseState.Y;
        }

        /// <summary>
        /// Returns the difference in value of the scrollwheel between the last frame and the current frame.
        /// </summary>
        /// <returns></returns>
        public static float getDeltaScrollWheel()
        {
            return deltaMouseScrollWheel;
        }
    }
}
