using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Folium.Main;
using Folium.Screens;
using Microsoft.Xna.Framework;

namespace Folium.Entities
{
    //alle the entities used in gameplay
    public class GameEntity : DrawableEntity
    {
        public GameEntity(GameManager gameManager, Screen screen)
            : base(gameManager, screen)
        {

        }

        public virtual void resolveCollision(GameEntity collider) {}
    }
}
