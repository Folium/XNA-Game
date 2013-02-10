using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Folium.Main;
using Folium.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Folium.Screens
{
    public class GameScreen : Screen
    {
        private List<Pickup> _pickups;  //Contains all pickups
        private List<Heart> _hearts;    //Contains all hearts
        private List<Leaf> _leaves;     //Contains all leaves and hearts

        private DrawableEntity _seedCursor;

        private float _timeBetweenPulses;
        private float _lastPulseTime;

        public GameScreen(GameManager gameManager)
            : base(gameManager, "Game")
        {
            _pickups            = new List<Pickup>(32);
            _hearts             = new List<Heart>();
            _leaves             = new List<Leaf>(32);
            _seedCursor         = new DrawableEntity(gameManager, this, "Textures/placement_cursor");
            _timeBetweenPulses  = Config.settings["Heart.TimeBetweenPulses"];
            _lastPulseTime      = 0;

            _seedCursor.setScale(0.25f);
            _seedCursor.setColor(Color.DarkSlateGray);
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
            addLeaf(heart, doInit);
        }

        public void addLeaf(Leaf leaf, bool doInit = true)
        {
            _leaves.Add(leaf);
            addEntity(leaf, true, doInit);
        }

        public override void removeEntity(DrawableEntity entity)
        {
            base.removeEntity(entity);

            if(entity is Leaf)
                _leaves.Remove((Leaf)entity);
            if(entity is Heart)
                _hearts.Remove((Heart)entity);
            if(entity is Pickup)
                _pickups.Remove((Pickup)entity);
        }

        public override void update(float dT)
        {
            base.update(dT);

            #region Seed (cursor) placement
            Leaf closestLeaf        = null;
            float closestDist       = float.MaxValue;
            Vector2 mouseWorldPos   = InputManager.getMousePos() - GameManager.WORLDOGIRIN;
            for (int i = 0; i < _leaves.Count; i++) //Find closest leaf
            {
                _leaves[i].isSelected = false;

                float distToLeaf = (_leaves[i].getPosition() - mouseWorldPos).Length() - _leaves[i].getRadius();
                if (distToLeaf < closestDist && distToLeaf >= 0) //Find closest leaf
                {
                    closestLeaf = _leaves[i];
                    closestDist = distToLeaf;
                }
            }

            if (closestLeaf != null) //We have a winner!
            {
                closestLeaf.isSelected  = true;
                Vector2 leafToMouse     = mouseWorldPos - closestLeaf.getPosition();
                leafToMouse.Normalize();
                leafToMouse             *= closestLeaf.getRadius();
                Vector2 seedPos         = closestLeaf.getPosition() + leafToMouse;
                bool canPlace           = true;

                for (int i = 0; i < _leaves.Count; i++) //Find intersecting leaves
                {
                    if ((_leaves[i].getPosition() - seedPos).LengthSquared() < _leaves[i].getRadius() * _leaves[i].getRadius())
                        canPlace = false;
                }

                if(canPlace)
                    _seedCursor.setPosition(seedPos);

                //Handle input
                if (InputManager.isMouseLeftReleased())
                {
                    Leaf newLeaf = new Leaf(_gameManager, this);
                    newLeaf.setPosition(_seedCursor.getPosition());
                    addLeaf(newLeaf);
                    closestLeaf.addChild(newLeaf);
                }
            }
            #endregion

            _seedCursor.update(dT);

            //Pulse hearts
            if (GameManager.CURRENTTIME - _lastPulseTime >= _timeBetweenPulses)
            {
                _lastPulseTime = GameManager.CURRENTTIME;
                for (int i = 0; i < _hearts.Count; i++)
                    _hearts[i].pulse();
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);

            _seedCursor.drawWorldSpace(spriteBatch);
        }
    }
}
