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
        private List<Pickup> _pickups;
        private List<Heart> _hearts;

        private float _timeBetweenPulses;
        private float _lastPulseTime;

        public GameScreen(GameManager gameManager)
            : base(gameManager, "Game")
        {
            _pickups            = new List<Pickup>(32);
            _hearts             = new List<Heart>();
            _timeBetweenPulses  = Config.settings["HeartTimeBetweenPulses"];
            _lastPulseTime      = 0;
        }

        #region Getters/Setters
        public List<Pickup> getPickups() {return _pickups;}
        #endregion

        public override void initialize()
        {
            base.initialize();

            //Add primary heart
            addHeart(new Heart(_gameManager, this));
        }

        public void addHeart(Heart heart, bool doInit = true)
        {
            _hearts.Add(heart);
            addEntity(heart, doInit);
        }

        public override void update(float dT)
        {
            base.update(dT);

            //Pulse hearts
            if (GameManager.currentTime - _lastPulseTime >= _timeBetweenPulses)
            {
                _lastPulseTime = GameManager.currentTime;
                for (int i = 0; i < _hearts.Count; i++)
                    _hearts[i].pulse();
            }
        }
    }
}
