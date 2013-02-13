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
    public class Food : SphereEntity
    {
        private bool    _isBeingEaten;
        private int     _energyAmount;

        public Food(GameManager gameManager, Screen screen, Vector2 position)
            : base(gameManager, screen)
        {
            _texture        = gameManager.Content.Load<Texture2D>("Textures/food_160");
            _radius         = _texture.Width/2;
            _isBeingEaten   = false;
            _energyAmount = (int)Config.settings["Food.Normal.EnergyAmount"];

            setPosition(position);
        }

        #region Getters/Setters
        public bool getIsBeingEaten() { return _isBeingEaten; }
        public int getEnergyAmount() { return _energyAmount; }
        #endregion

        public override void resolveCollision(GameEntity collider)
        {
            if (collider is Leaf)
                startBeingEating((Leaf)collider);
        }

        public void stopBeingEating() { _isBeingEaten = false; }
        
        private void startBeingEating(Leaf collider) { _isBeingEaten = true; }
    }
}
