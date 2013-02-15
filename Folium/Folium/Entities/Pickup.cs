using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Folium.Main;
using Folium.Screens;

namespace Folium.Entities
{
    public class Pickup : SphereEntity
    {
        public Pickup(GameManager gameManager, Screen screen)
            : base(gameManager, screen)
        {
        }
    }
}
