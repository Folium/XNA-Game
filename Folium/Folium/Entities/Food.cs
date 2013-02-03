using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Folium.Main;
using Folium.Screens;

namespace Folium.Entities
{
    class Food : DrawableEntity
    {
        public Food(GameManager gameManager, Screen screen)
            : base(gameManager, screen)
        {
        }
    }
}
