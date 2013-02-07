using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Folium.Main;
using Folium.Screens;
using Microsoft.Xna.Framework.Graphics;

namespace Folium.Entities
{
    class Food : DrawableEntity
    {
        private float _energyAmount;

        public Food(GameManager gameManager, Screen screen)
            : base(gameManager, screen)
        {
            _texture        = gameManager.Content.Load<Texture2D>("Textures/energy_source_160");
            _energyAmount   = Config.settings["EnergySourceAmount"];
        }
    }
}
