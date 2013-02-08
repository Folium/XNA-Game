using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Folium.Main;
using Folium.Screens;
using Microsoft.Xna.Framework.Graphics;

namespace Folium.Entities
{
    class FoodSource : DrawableEntity
    {
        private float _energyAmount;

        public FoodSource(GameManager gameManager, Screen screen)
            : base(gameManager, screen)
        {
            _texture        = gameManager.Content.Load<Texture2D>("Textures/energy_source_160");
            _energyAmount   = Config.settings["EnergySourceAmount"];
        }
    }
}
