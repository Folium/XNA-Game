using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Folium.Main;
using Folium.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Folium.Entities
{
    public class SphereEntity : DrawableEntity
    {
        protected float _radius;        //Radius in logic code, used for distance checking etc.
        
        public SphereEntity(GameManager gameManager, Screen screen)
            : base(gameManager, screen)
        {
           
        }

        #region Getters/Setters
        public float getRadius() { return _radius; }

        public virtual void setRadius(float radius) { _radius = radius; }
        #endregion
    }
}
