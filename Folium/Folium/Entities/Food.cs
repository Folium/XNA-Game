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
        private GameManager _gameManager;
        private bool    _isBeingEaten;
        private int     _energyAmount;

        public Food(GameManager gameManager, Screen screen, Vector2 position)
            : base(gameManager, screen)
        {
            _gameManager    = gameManager;
            _texture        = _gameManager.Content.Load<Texture2D>("Textures/food_160");
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
                startBeingConsumed((Leaf)collider);
        }

        public void stopBeingConsumed() 
        {
            _texture = _gameManager.Content.Load<Texture2D>("Textures/food_160");
            _isBeingEaten = false; 
        }
        
        private void startBeingConsumed(Leaf collider) 
        {
            _texture = _gameManager.Content.Load<Texture2D>("Textures/food_being_consumed_160");
            _isBeingEaten = true;
        }
    }
}
