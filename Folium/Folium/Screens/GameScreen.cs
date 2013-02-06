using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Folium.Main;
using Folium.Entities;
using Microsoft.Xna.Framework.Input;

namespace Folium.Screens
{
    public class GameScreen : Screen
    {
        private List<Pickup> pickups;

        public GameScreen(GameManager gameManager)
            : base(gameManager, "Game")
        {
            pickups = new List<Pickup>(32);
        }

        #region Getters/Setters
        public List<Pickup> getPickups() {return pickups;}
        #endregion

        public override void initialize()
        {
            base.initialize();

            //Add test entity
            Leaf testLeaf = new Leaf(_gameManager, this);
            addEntity(testLeaf);
        }

        public override void update(float dT)
        {
            base.update(dT);

            if(InputManager.isKeyReleased(Keys.Space))
                ((Leaf)_entities[0]).pulse(1);
        }
    }
}
