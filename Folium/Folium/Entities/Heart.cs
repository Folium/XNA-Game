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
        private List<Leaf> _connectedFoodLeaves;

        public Heart(GameManager gameManager, Screen screen)
            : base(gameManager, screen)
        {
            _drawColor              = Color.IndianRed;
            _connectedFoodLeaves    = new List<Leaf>();
            _pulseStrength          = (int)Config.settings["Heart.InitialPulseStrength"];
            _pulseSmallRadius       = Config.settings["Heart.PulseSmallRadius"];
            _pulseLargeRadius       = Config.settings["Heart.PulseLargeRadius"];
            _pulseSmallOutDuration  = Config.settings["Heart.PulseSmallOutDuration"];
            _pulseSmallInDuration   = Config.settings["Heart.PulseSmallInDuration"];
            _pulseLargeOutDuration  = Config.settings["Heart.PulseLargeOutDuration"];
            _pulseLargeInDuration   = Config.settings["Heart.PulseLargeInDuration"];
            _pulsePassOnTime        = Config.settings["Heart.PulsePassOnTime"];
            _lifeLossPerSecond      = 0;

            setRadius(Config.settings["Heart.Radius"]);
            setDistToColor((int)GameManager.LeafColorIndex.NORMAL, -1);
            setDistToColor((int)GameManager.LeafColorIndex.HEART, 0);
            setColor(GameManager.LEAFCOLORS[(int)GameManager.LeafColorIndex.HEART]);
        }

        public void pulse()
        {
            base.pulse(_pulseStrength);
        }

        public override void registerFoodLeaf(Leaf leafToRegister, Food foodBeingEaten, int sequenceLength)
        {
            if (!_connectedFoodLeaves.Contains(leafToRegister) && sequenceLength < _pulseStrength + foodBeingEaten.getEnergyAmount())
            {
                leafToRegister.startEating(foodBeingEaten);
                foodBeingEaten.resolveCollision(this);
                _connectedFoodLeaves.Add(leafToRegister);
                _pulseStrength += foodBeingEaten.getEnergyAmount();
            }
        }

        public override void update(float dT)
        {
            FoodCheck();

            base.update(dT);
        }

        //Check connected foodleaves and remove some if necessary
        private void FoodCheck()
        { 
            for (int i = 0; i < _connectedFoodLeaves.Count; i++)
            {
                Leaf CFLeaf = _connectedFoodLeaves[i];
                Food foodBE = CFLeaf.getFoodBeingEaten();
                _pulseStrength -= CFLeaf.getFoodBeingEaten().getEnergyAmount();
                CFLeaf.stopEating();
                foodBE.stopBeingConsumed();
                _connectedFoodLeaves.RemoveAt(i);
                CFLeaf.registerFoodLeaf(CFLeaf, foodBE);
            }
        }
    }
}
