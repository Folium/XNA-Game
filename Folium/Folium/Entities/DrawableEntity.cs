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
        protected Color         _drawColor;
        protected float         _drawScale;
        protected float         _rotation;
        protected bool          _alive;

        public DrawableEntity(GameManager gameManager, Screen screen)
        {
            doDraw              = true;

            _gameManager        = gameManager;
            _screen             = screen;
            _texture            = null;
            _position           = Vector2.Zero;
            _drawColor          = Color.White;
            _drawScale          = 1;
            _rotation           = 0;
            _alive              = true;
        }

        public DrawableEntity(GameManager gameManager, Screen screen, String texture)
        {
            doDraw              = true;

            _gameManager        = gameManager;
            _screen             = screen;
            _texture            = gameManager.Content.Load<Texture2D>(texture);
            _position           = Vector2.Zero;
            _drawColor          = Color.White;
            _drawScale          = 1;
            _rotation           = 0;
            _alive              = true;
        }

        #region Getters/Setters
        public Vector2 getPosition() { return _position; }
        public bool isAlive() { return _alive; }

        public void setPosition(Vector2 a) { _position = a; }
        public void setScreen(Screen a) { _screen = a; }
        public void setScale(float a) { _drawScale = a; }
        public void setColor(Color a) { _drawColor = a; }
        #endregion

        public virtual void initialize()
        {
        }

        public virtual void kill()
        {
            _alive = false;
        }

        public virtual void update(float dT)
        {
        }

        public virtual void drawWorldSpace(SpriteBatch spriteBatch)
        {
            if(!doDraw || _texture == null)
                return;

            //This entity's position in screen space
            Vector2 posScreenSpace = GameManager.WORLDOGIRIN + _position * GameManager.ZOOMLEVEL;

            spriteBatch.Draw(_texture, posScreenSpace, null,
                             _drawColor, _rotation, new Vector2(_texture.Width/2, _texture.Height/2),
                             _drawScale * GameManager.ZOOMLEVEL, SpriteEffects.None, 0);
        }

        public virtual void drawScreenSpace(SpriteBatch spriteBatch)
        {
            if (!doDraw || _texture == null)
                return;

            spriteBatch.Draw(_texture, _position, null,
                             _drawColor, _rotation, new Vector2(_texture.Width/2, _texture.Height/2),
                             _drawScale, SpriteEffects.None, 0);
        }
    }
}
