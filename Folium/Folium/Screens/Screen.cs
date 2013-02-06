using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Folium.Main;
using Microsoft.Xna.Framework.Graphics;
using Folium.Entities;

namespace Folium.Screens
{
    public class Screen
    {
        protected bool                  _doUpdate, _doDraw;
        protected String                _name;
        protected GameManager           _gameManager;
        protected List<DrawableEntity>  _entities;

        public Screen(GameManager gameManager, String name)
        {
            _gameManager    = gameManager;
            _doUpdate       = false;
            _doDraw         = false;
            _name           = name;
            _entities       = new List<DrawableEntity>();
        }

        #region Getters/Setters
        public String getName() { return _name; }
        #endregion

        public virtual void initialize()
        {
        }

        #region Screen commands
        /// <summary>
        /// Starts/resumes this screen and puts it on the screens stack, if this isn't already the case.
        /// </summary>
        public virtual void start()
        {
            _doUpdate   = true;
            _doDraw     = true;

            ScreenManager.addToUpdateList(_name);
        }

        /// <summary>
        /// Stops this screen and takes it off the screens stack. It will also reset the screen to its initialised state.
        /// </summary>
        public virtual void stop()
        {
            _doUpdate   = false;
            _doDraw     = false;

            ScreenManager.removeFromUpdateList(_name);
        }

        /// <summary>
        /// Pauses the screen but does not reset it or take it off the screens stack.
        /// </summary>
        /// <param name="pauseUpdate"></param>
        /// <param name="pauseDraw"></param>
        public virtual void pause(bool pauseUpdate = true, bool pauseDraw = true)
        {
            _doUpdate   = !pauseUpdate;
            _doDraw     = !pauseDraw;
        }
        #endregion

        public void addEntity(DrawableEntity entity, bool doInit = true)
        {
            _entities.Add(entity);
            entity.setScreen(this);

            if(doInit)
                entity.initialize();
        }

        public virtual void update(float dT)
        {
            if (!_doUpdate)
                return;

            for (int i = 0; i < _entities.Count; i++)
            {
                _entities[i].update(dT);

                if(!_entities[i].isAlive())
                    _entities.RemoveAt(i--);
            }
        }

        public virtual void draw(SpriteBatch spriteBatch)
        {
            if (!_doDraw)
                return;

            for (int i = 0; i < _entities.Count; i++)
                _entities[i].drawWorldSpace(spriteBatch);
        }
    }
}
