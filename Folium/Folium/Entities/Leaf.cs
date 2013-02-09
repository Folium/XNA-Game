using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Folium.Main;
using Folium.Screens;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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

        public bool isSelected;

        protected List<Leaf>    _parents;
        protected List<Leaf>    _children;
        protected int           _pulseDrain;
        protected float         _radius;        //Radius in logic code, used for distance checking etc.
        protected float         _drawRadius;    //Radius used for drawing, has no effect on logic
        protected float         _normalRadius;  //Radius in normal state

        protected bool          _doPulsePassOn;
        protected int           _pulseStrength;
        protected PulseStage    _pulseStage;
        protected float         _pulseSmallRadius;
        protected float         _pulseLargeRadius;
        protected float         _pulseSmallOutDuration;
        protected float         _pulseSmallInDuration;
        protected float         _pulseLargeOutDuration;
        protected float         _pulseLargeInDuration;
        protected float         _pulsePassOnTime;
        protected float         _lastPulseTime;
        protected float         _lifeLossPerSecond;

        private float           _maxLife;
        private float           _life;

        private float           _radiusScaleVelocity;
        private float           _targetRadius;
        private bool            _doingRadiusAnimation;

        private float           _selectedTextureScale;
        private Texture2D       _selectedTexture;

        private float[]         _distToColors;

        public Leaf(GameManager gameManager, Screen screen)
            : base(gameManager, screen)
        {
            _texture                = gameManager.Content.Load<Texture2D>("Textures/circle_white_320");
            _selectedTexture        = gameManager.Content.Load<Texture2D>("Textures/circle_edge_white_320");
            _radius                 = Config.settings["Leaf.Radius"];
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
            _selectedTextureScale   = _normalRadius/_selectedTexture.Width * 2;
            _pulseSmallRadius       = Config.settings["Leaf.PulseSmallRadius"];
            _pulseLargeRadius       = Config.settings["Leaf.PulseLargeRadius"];
            _pulseSmallOutDuration  = Config.settings["Leaf.PulseSmallOutDuration"];
            _pulseSmallInDuration   = Config.settings["Leaf.PulseSmallInDuration"];
            _pulseLargeOutDuration  = Config.settings["Leaf.PulseLargeOutDuration"];
            _pulseLargeInDuration   = Config.settings["Leaf.PulseLargeInDuration"];
            _lifeLossPerSecond      = Config.settings["Leaf.LifeLossPerSecond"];
            _maxLife                = Config.settings["Leaf.MaxLife"];
            _pulsePassOnTime        = Config.settings["Leaf.PulsePassOnTime"];
            _lastPulseTime          = 0;
            _life                   = _maxLife;
            _doPulsePassOn          = false;
            isSelected              = false;
            _distToColors           = new float[GameManager.NUM_COLORS];
        }

        #region Getters/Setters
        public int getPulseStrength() { return _pulseStrength; }
        public float getRadius() { return _radius; }

        public void setRadius(float radius) 
        {
            _radius                 = radius;
            _drawRadius             = radius;
            _normalRadius           = radius;
            _drawScale              = _radius/_texture.Width * 2;
            _selectedTextureScale   = _radius/_selectedTexture.Width * 2;
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

        public override void drawWorldSpace(SpriteBatch spriteBatch)
        {
            base.drawWorldSpace(spriteBatch);

            if (isSelected && _selectedTexture != null)
            {
                //This entity's position in screen space
                Vector2 posScreenSpace = GameManager.worldOrigin + _position * GameManager.zoomLevel;

                spriteBatch.Draw(_selectedTexture, posScreenSpace, null,
                                 Color.FromNonPremultiplied(47, 79, 79, 80), _rotation, 
                                 new Vector2(_selectedTexture.Width/2, _selectedTexture.Height/2),
                                 _selectedTextureScale * GameManager.zoomLevel, SpriteEffects.None, 0);
            }
        }
    }
}
