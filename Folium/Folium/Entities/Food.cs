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
        private bool        _isBeingConsumed;
        private int         _energyAmount;

        public Food(GameManager gameManager, Screen screen, Vector2 position, float radius)
            : base(gameManager, screen)
        {
            _gameManager    = gameManager;
            _texture        = _gameManager.Content.Load<Texture2D>("Textures/food_160");
            _isBeingConsumed   = false;
            _energyAmount = (int)Config.settings["Food.Normal.EnergyAmount"];

            setPosition(position);
            setRadius((float)radius);
        }

        #region Getters/Setters
        public bool getIsBeingEaten() { return _isBeingConsumed; }
        public int getEnergyAmount() { return _energyAmount; }
        #endregion

        public void resolveCollision(DrawableEntity collider)
        {
            if (collider is Leaf)
                startBeingConsumed((Leaf)collider);
        }

        public void stopBeingConsumed() 
        {
            _texture = _gameManager.Content.Load<Texture2D>("Textures/food_160");
            _isBeingConsumed = false; 
        }
        
        private void startBeingConsumed(Leaf collider) 
        {
            _texture = _gameManager.Content.Load<Texture2D>("Textures/food_being_consumed_160");
            _isBeingConsumed = true;
        }
    }
}
