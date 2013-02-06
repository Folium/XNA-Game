using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Folium.Main;
using Folium.Entities;

namespace Folium.Screens
{
    public class GameScreen : Screen
    {
        public GameScreen(GameManager gameManager)
            : base(gameManager, "Game")
        {
        }

        public override void initialize()
        {
            base.initialize();

            //Add test entity
            DrawableEntity testEnt = new DrawableEntity(_gameManager, this, "Textures/circle_white_320");
            addEntity(testEnt);
        }
    }
}
