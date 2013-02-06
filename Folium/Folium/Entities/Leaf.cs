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

        private float           _scaleVelocity;
        private float           _targetScale;
        private bool            _doingScaleAnimation;

        private int             _pulseStrength;
        private PulseStage      _pulseStage;

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
            _targetScale            = _radius;
            _drawScale              = _radius/_texture.Width * 2;
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
                _pulseStage = PulseStage.outSmall;
                setTargetScale(1.025f, 0.1f);
            }
        }

        public override void update(float dT)
        {
            base.update(dT);

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
                            setTargetScale(1.0f, 0.1f);
                            break;
                        case PulseStage.inSmall:
                            _pulseStage = PulseStage.outLarge;
                            setTargetScale(1.05f, 0.1f);
                            break;
                        case PulseStage.outLarge:
                            _pulseStage = PulseStage.inLarge;
                            setTargetScale(1.0f, 0.2f);
                            break;
                        case PulseStage.inLarge:
                            _pulseStage = PulseStage.Done;
                            break;
                    }
                }
            }
        }
    }
}
