using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Folium.Main;
using Microsoft.Xna.Framework.Graphics;

namespace Folium.Screens
{
    public class ScreenManager
    {
        private static GameManager                  _gameManager;
        private static List<Screen>                 _updateList;
        private static Dictionary<String, Screen>   _screens;

        public ScreenManager(GameManager gameManager)
        {
            _gameManager    = gameManager;
            _updateList     = new List<Screen>();
            _screens        = new Dictionary<string,Screen>();
        }

        #region Screen commands
        /// <summary>
        /// Adds the screen to the manager.
        /// </summary>
        /// <param name="a"></param>
        public static void addScreen(Screen a)
        {
            _screens.Add(a.getName(), a);
        }

        /// <summary>
        /// Remove screen from the manager.
        /// </summary>
        /// <param name="a"></param>
        public static void removeScreen(Screen a)
        {
            _screens.Remove(a.getName());
        }

        /// <summary>
        /// Returns the screen with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Screen getScreen(String name)
        {
            return _screens[name];
        }

        /// <summary>
        /// Puts the screen at the end of the update list.
        /// </summary>
        /// <param name="a"></param>
        public static void addToUpdateList(String name)
        {
            _updateList.Add(_screens[name]);
        }
        
        /// <summary>
        /// Removes the screen from the update list.
        /// </summary>
        /// <param name="name"></param>
        public static void removeFromUpdateList(String name)
        {
            _updateList.Remove(_screens[name]);
        }
        #endregion

        public static void update(float dT)
        {
            for (int i = 0; i < _updateList.Count; i++)
                _updateList[i].update(dT);
        }

        public static void draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _updateList.Count; i++)
                _updateList[i].draw(spriteBatch);
        }
    }
}
