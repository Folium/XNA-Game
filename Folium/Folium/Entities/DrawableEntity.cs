using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Folium.Main;
using Folium.Screens;

namespace Folium.Entities
{
    public class DrawableEntity
    {
        public bool             doDraw;

        protected GameManager   _gameManager;
        protected Screen        _screen;
        protected Texture2D     _texture;
        protected Vector2       _position;

        DrawableEntity(GameManager gameManager, Screen screen)
        {
            doDraw          = true;

            _gameManager    = gameManager;
            _screen         = screen;
            _texture        = null;
            _position       = Vector2.Zero;
        }

        #region Getters/Setters
        public Vector2 getPosition() { return _position; }

        public virtual void setPosition(Vector2 a) { _position = a; }
        #endregion

        public virtual void initialize(bool doLoadContent = true)
        {
            if (doLoadContent)
                loadContent();
        }

        public virtual void loadContent()
        {
        }

        public virtual void update(double dT)
        {
        }

        public virtual void draw(SpriteBatch spriteBatch)
        {
            if(!doDraw)
                return;
        }
    }
}
