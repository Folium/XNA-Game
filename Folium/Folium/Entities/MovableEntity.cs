using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Folium.Main;
using Folium.Screens;
using Microsoft.Xna.Framework;

namespace Folium.Entities
{
    public class MovableEntity : DrawableEntity
    {
        protected Vector2 _velocity;

        public MovableEntity(GameManager gameManager, Screen screen)
            : base(gameManager, screen)
        {
        }

        public override void update(float dT)
        {
            base.update(dT);

            _position += _velocity * dT;
        }

        #region Getters/Setters
        public Vector2 getVelocity() { return _velocity; }

        public void setVelocity(Vector2 velocity) { _velocity = velocity; }
        #endregion
    }
}
