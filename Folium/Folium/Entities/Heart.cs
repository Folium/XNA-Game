using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Folium.Main;
using Folium.Screens;
using Microsoft.Xna.Framework;

namespace Folium.Entities
{
    public class Heart : Leaf
    {
        private List<Leaf> _connectedEnergyLeaves;
        private int _pulseStrength;

        public Heart(GameManager gameManager, Screen screen)
            : base(gameManager, screen)
        {
            _drawColor              = Color.IndianRed;
            _connectedEnergyLeaves  = new List<Leaf>();
            _pulseStrength          = (int)Config.settings["HeartInitialPulseStrength"];
            _pulseSmallRadius       = Config.settings["HeartPulseSmallRadius"];
            _pulseLargeRadius       = Config.settings["HeartPulseLargeRadius"];
            _pulseSmallOutDuration  = Config.settings["HeartPulseSmallOutDuration"];
            _pulseSmallInDuration   = Config.settings["HeartPulseSmallInDuration"];
            _pulseLargeOutDuration  = Config.settings["HeartPulseLargeOutDuration"];
            _pulseLargeInDuration   = Config.settings["HeartPulseLargeInDuration"];
            _pulsePassOnTime        = Config.settings["HeartPulsePassOnTime"];
            _lifeLossPerSecond      = 0;

            setRadius(Config.settings["HeartRadius"]);
        }

        public void pulse()
        {
            base.pulse(_pulseStrength);
        }

        public override void registerEnergyLeaf(Leaf leafToRegister)
        {
            if (!_connectedEnergyLeaves.Contains(leafToRegister))
            {
                _connectedEnergyLeaves.Add(leafToRegister);
                _pulseStrength += leafToRegister.getPulseStrength();
            }
        }

        public override void update(float dT)
        {
            //Check connected energy leaves and remove some if necessary
            for (int i = 0; i < _connectedEnergyLeaves.Count; i++)
            {
                Leaf temp = _connectedEnergyLeaves[i];
                _pulseStrength -= temp.getPulseStrength();
                _connectedEnergyLeaves.RemoveAt(i);
                temp.registerEnergyLeaf(temp);
            }

            base.update(dT);
        }
    }
}
