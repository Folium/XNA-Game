using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Folium.Main;
using Folium.Screens;
using Microsoft.Xna.Framework.Graphics;

namespace Folium.Entities
{
    public class Leaf : DrawableEntity
    {
        protected enum PulseStage
        {
            Done,
            inSmall,
            outSmall,
            inLarge,
            outLarge
        }

        protected List<Leaf> _parents;
        protected List<Leaf> _children;
        protected int _pulseDrain;
        protected float _radius;
        protected float _drawRadius;
        protected float _normalRadius;

        protected bool _doPulsePassOn;
        protected int _pulseStrength;
        protected PulseStage _pulseStage;
        protected float _pulseSmallRadius;
        protected float _pulseLargeRadius;
        protected float _pulseSmallOutDuration;
        protected float _pulseSmallInDuration;
        protected float _pulseLargeOutDuration;
        protected float _pulseLargeInDuration;
        protected float _pulsePassOnTime;
        protected float _lastPulseTime;
        protected float _lifeLossPerSecond;

        private float _maxLife;
        private float _life;

        private float _radiusScaleVelocity;
        private float _targetRadius;
        private bool _doingRadiusAnimation;

        public Leaf(GameManager gameManager, Screen screen)
            : base(gameManager, screen)
        {
            _texture                = gameManager.Content.Load<Texture2D>("Textures/circle_white_320");
            _radius                 = Config.settings["LeafRadius"];
            _normalRadius           = _radius;
            _drawRadius             = _radius;
            _parents                = new List<Leaf>();
            _children               = new List<Leaf>();
            _pulseDrain             = 1;
            _pulseStrength          = 0;
            _pulseStage             = PulseStage.Done;
            _radiusScaleVelocity    = 0;
            _doingRadiusAnimation   = false;
            _targetRadius           = 1;
            _drawScale              = _radius/_texture.Width * 2;
            _pulseSmallRadius       = Config.settings["LeafPulseSmallRadius"];
            _pulseLargeRadius       = Config.settings["LeafPulseLargeRadius"];
            _pulseSmallOutDuration  = Config.settings["LeafPulseSmallOutDuration"];
            _pulseSmallInDuration   = Config.settings["LeafPulseSmallInDuration"];
            _pulseLargeOutDuration  = Config.settings["LeafPulseLargeOutDuration"];
            _pulseLargeInDuration   = Config.settings["LeafPulseLargeInDuration"];
            _lifeLossPerSecond      = Config.settings["LeafLifeLossPerSecond"];
            _maxLife                = Config.settings["LeafMaxLife"];
            _pulsePassOnTime        = Config.settings["LeafPulsePassOnTime"];
            _lastPulseTime          = 0;
            _life                   = _maxLife;
            _doPulsePassOn          = false;
        }

        #region Getters/Setters
        public int getPulseStrength() { return _pulseStrength; }
        public float getRadius() { return _radius; }

        public void setRadius(float radius) 
        {
            _radius         = radius;
            _drawRadius     = radius;
            _normalRadius   = radius;
            _drawScale      = _radius/_texture.Width * 2;
        }
        #endregion

        /// <summary>
        /// Sets the target scale and the time it should take to reach this scale.
        /// </summary>
        /// <param name="targetRadius"></param>
        /// <param name="effectDuration"></param>
        public void setTargetRadius(float targetRadius, float effectDuration)
        {
            _targetRadius           = targetRadius;
            _radiusScaleVelocity    = (targetRadius - _drawRadius)/effectDuration;
            _doingRadiusAnimation   = true;
        }

        /// <summary>
        /// Initiates a pulse.
        /// </summary>
        /// <param name="pulseStrength"></param>
        public virtual void pulse(int pulseStrength)
        {
            _pulseStrength = pulseStrength;

            if (pulseStrength > 0)
            {
                _lastPulseTime  = GameManager.currentTime;
                _doPulsePassOn  = true;
                _life           = _maxLife;
                _pulseStage     = PulseStage.outSmall;
                setTargetRadius(_pulseSmallRadius, _pulseSmallOutDuration);
            }
        }

        /// <summary>
        /// Should be called every update tick to update the scale animation.
        /// Animation parameters are set with 'setTargetScale'.
        /// </summary>
        /// <param name="dT"></param>
        public virtual void updateScaleAnimation(float dT)
        {
            if (_doingRadiusAnimation)
            {
                _drawRadius += _radiusScaleVelocity * dT;
                _drawScale  = _drawRadius/_texture.Width * 2;

                if ((_radiusScaleVelocity > 0 && _drawRadius >= _targetRadius) ||
                    (_radiusScaleVelocity < 0 && _drawRadius <= _targetRadius))
                {
                    _drawRadius             = _targetRadius;
                    _doingRadiusAnimation   = false;

                    switch (_pulseStage)
                    {
                        case PulseStage.outSmall:
                            _pulseStage = PulseStage.inSmall;
                            setTargetRadius(_normalRadius, _pulseSmallInDuration);
                            break;
                        case PulseStage.inSmall:
                            _pulseStage = PulseStage.outLarge;
                            setTargetRadius(_pulseLargeRadius, _pulseLargeOutDuration);
                            break;
                        case PulseStage.outLarge:
                            _pulseStage = PulseStage.inLarge;
                            setTargetRadius(_normalRadius, _pulseLargeInDuration);
                            break;
                        case PulseStage.inLarge:
                            _pulseStage = PulseStage.Done;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// This function is recursively called on the parents until it reaches a heart or a leaf that has no parents.
        /// </summary>
        /// <param name="?"></param>
        public virtual void registerEnergyLeaf(Leaf leafToRegister)
        {
            for (int i = 0; i < _parents.Count; i++)
                _parents[i].registerEnergyLeaf(leafToRegister);
        }

        public void addChild(Leaf child)
        {
            if (!_children.Contains(child))
                _children.Add(child);
            if (!child._parents.Contains(this))
                child._parents.Add(this);
        }

        public override void update(float dT)
        {
            base.update(dT);

            //Decrease life
            _life -= _lifeLossPerSecond * dT;

            if (_life <= 0)
                kill();

            //Scale animation
            updateScaleAnimation(dT);

            //Pass on pulse
            if (_doPulsePassOn && GameManager.currentTime - _lastPulseTime >= _pulsePassOnTime)
            {
                _doPulsePassOn = false;
                for (int i = 0; i < _children.Count; i++)
                    _children[i].pulse(_pulseStrength - _pulseDrain); //Pulse the children
            }
        }
    }
}
