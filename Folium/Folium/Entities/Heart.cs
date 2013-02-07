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
        private int _pulseStrength;

        public Heart(GameManager gameManager, Screen screen)
            : base(gameManager, screen)
        {
            _drawColor              = Color.IndianRed;
            _pulseStrength          = (int)Config.settings["HeartInitialPulseStrength"];
            _pulseSmallScale        = Config.settings["HeartPulseSmallScale"];
            _pulseLargeScale        = Config.settings["HeartPulseLargeScale"];
            _pulseSmallOutDuration  = Config.settings["HeartPulseSmallOutDuration"];
            _pulseSmallInDuration   = Config.settings["HeartPulseSmallInDuration"];
            _pulseLargeOutDuration  = Config.settings["HeartPulseLargeOutDuration"];
            _pulseLargeInDuration   = Config.settings["HeartPulseLargeInDuration"];
            _pulsePassOnTime        = Config.settings["HeartPulsePassOnTime"];
        }

        public void pulse()
        {
            base.pulse(_pulseStrength);
        }
    }
}
