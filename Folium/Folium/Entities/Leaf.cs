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

        protected List<Leaf>    _parents;
        protected List<Leaf>    _children;
        protected int           _pulseDrain;
        protected float         _radius;

        protected bool          _doPulsePassOn;
        protected int           _pulseStrength;
        protected PulseStage    _pulseStage;
        protected float         _pulseSmallScale;
        protected float         _pulseLargeScale;
        protected float         _pulseSmallOutDuration;
        protected float         _pulseSmallInDuration;
        protected float         _pulseLargeOutDuration;
        protected float         _pulseLargeInDuration;
        protected float         _pulsePassOnTime;
        protected float         _lastPulseTime;

        private float           _lifeLossPerSecond;
        private float           _maxLife;
        private float           _life;

        private float           _scaleVelocity;
        private float           _targetScale;
        private bool            _doingScaleAnimation;

        public Leaf(GameManager gameManager, Screen screen)
            : base(gameManager, screen)
        {
            _texture                = gameManager.Content.Load<Texture2D>("Textures/circle_white_320");
            _radius                 = 160;
            _parents                = new List<Leaf>();
            _children               = new List<Leaf>();
            _pulseDrain             = 1;
            _pulseStrength          = 0;
            _pulseStage             = PulseStage.Done;
            _scaleVelocity          = 0;
            _doingScaleAnimation    = false;
            _targetScale            = 1;
            _drawScale              = _radius/_texture.Width * 2;
            _pulseSmallScale        = Config.settings["LeafPulseSmallScale"];
            _pulseLargeScale        = Config.settings["LeafPulseLargeScale"];
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

        /// <summary>
        /// Sets the target scale and the time it should take to reach this scale.
        /// </summary>
        /// <param name="targetScale"></param>
        /// <param name="effectDuration"></param>
        public void setTargetScale(float targetScale, float effectDuration)
        {
            _targetScale            = targetScale;
            _scaleVelocity          = (targetScale - _drawScale)/effectDuration;
            _doingScaleAnimation    = true;
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
                setTargetScale(_pulseSmallScale, _pulseSmallOutDuration);
            }
        }

        /// <summary>
        /// Should be called every update tick to update the scale animation.
        /// Animation parameters are set with 'setTargetScale'.
        /// </summary>
        /// <param name="dT"></param>
        public virtual void updateScaleAnimation(float dT)
        {
            if (_doingScaleAnimation)
            {
                _drawScale += _scaleVelocity * dT;

                if ((_scaleVelocity > 0 && _drawScale >= _targetScale) ||
                    (_scaleVelocity < 0 && _drawScale <= _targetScale))
                {
                    _drawScale              = _targetScale;
                    _doingScaleAnimation    = false;

                    switch (_pulseStage)
                    {
                        case PulseStage.outSmall:
                            _pulseStage = PulseStage.inSmall;
                            setTargetScale(1.0f, _pulseSmallInDuration);
                            break;
                        case PulseStage.inSmall:
                            _pulseStage = PulseStage.outLarge;
                            setTargetScale(_pulseLargeScale, _pulseLargeOutDuration);
                            break;
                        case PulseStage.outLarge:
                            _pulseStage = PulseStage.inLarge;
                            setTargetScale(1.0f, _pulseLargeInDuration);
                            break;
                        case PulseStage.inLarge:
                            _pulseStage = PulseStage.Done;
                            break;
                    }
                }
            }
        }

        public override void update(float dT)
        {
            base.update(dT);

            //Decrease life
            _life -= _lifeLossPerSecond * dT;
            
            if(_life <= 0)
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
