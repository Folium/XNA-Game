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

        private float           _lifeLossPerSecond;
        private float           _maxLife;
        private float           _life;

        private float           _scaleVelocity;
        private float           _targetScale;
        private bool            _doingScaleAnimation;

        private int             _pulseStrength;
        private PulseStage      _pulseStage;
        private float           _pulseSmallScale;
        private float           _pulseLargeScale;
        private float           _pulseSmallOutDuration;
        private float           _pulseSmallInDuration;
        private float           _pulseLargeOutDuration;
        private float           _pulseLargeInDuration;

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
            _pulseSmallScale        = Config.settings["PulseSmallScale"];
            _pulseLargeScale        = Config.settings["PulseLargeScale"];
            _pulseSmallOutDuration  = Config.settings["PulseSmallOutDuration"];
            _pulseSmallInDuration   = Config.settings["PulseSmallInDuration"];
            _pulseLargeOutDuration  = Config.settings["PulseLargeOutDuration"];
            _pulseLargeInDuration   = Config.settings["PulseLargeInDuration"];
            _lifeLossPerSecond      = Config.settings["LifeLossPerSecond"];
            _maxLife                = Config.settings["MaxLife"];
            _life                   = _maxLife;
        }

        public void setTargetScale(float targetScale, float effectDuration)
        {
            _targetScale            = targetScale;
            _scaleVelocity          = (targetScale - _drawScale)/effectDuration;
            _doingScaleAnimation    = true;
        }

        public virtual void pulse(int pulseStrength)
        {
            _pulseStrength = pulseStrength;

            if (pulseStrength > 0)
            {
                _life       = _maxLife;
                _pulseStage = PulseStage.outSmall;
                setTargetScale(_pulseSmallScale, _pulseSmallOutDuration);
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
                            for (int i = 0; i < _children.Count; i++)
                                _children[i].pulse(_pulseStrength - _pulseDrain); //Pulse the children
                            break;
                    }
                }
            }
        }
    }
}
