using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Folium.Screens;
using System.IO;

namespace Folium.Main
{
    public class GameManager : Microsoft.Xna.Framework.Game
    {
        public enum LeafColorIndex
        {
            NORMAL,
            HEART
        }

        public static int NUM_COLORS            = 2;
        public static int SCREENWIDTH           = 1280;
        public static int SCREENHEIGHT          = 720;
        public static Vector2 WORLDOGIRIN       = new Vector2(SCREENWIDTH/2, SCREENHEIGHT/2);
        public static Vector2 SCREENCENTER      = new Vector2(SCREENWIDTH/2, SCREENHEIGHT/2);
        public static float ZOOMLEVEL           = 1;
        public static float CURRENTTIME         = 0;
        public static Color[] LEAFCOLORS        = new Color[NUM_COLORS];
        public static float[] MAX_DIST_TO_COLOR = new float[NUM_COLORS];

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private float _scrollSpeed;
        private float _zoomSpeed;

        public GameManager()
        {
            new Config("../../../settings.config");

            _graphics               = new GraphicsDeviceManager(this);
            Content.RootDirectory   = "Content";
            CURRENTTIME             = 0;
            _scrollSpeed            = Config.settings["ScrollSpeed"];
            _zoomSpeed              = Config.settings["ZoomSpeed"];
        }

        //LoadContent is called BEFORE Initialize
        protected override void Initialize()
        {
            base.Initialize();

            _graphics.PreferredBackBufferWidth   = SCREENWIDTH;
            _graphics.PreferredBackBufferHeight  = SCREENHEIGHT;
            _graphics.ApplyChanges();

            IsMouseVisible = true;

            new InputManager(this);
            new ScreenManager(this);

            //Init leaf colors
            NUM_COLORS = (int)Config.settings["Leaf.Color.Amount"];

            for (int i = 0; i < NUM_COLORS; i++)
                LEAFCOLORS[i] = Color.White;

            //Normal leaf color
            LEAFCOLORS[(int)LeafColorIndex.NORMAL].R         = (byte)Config.settings["Leaf.Color.Normal.R"];
            LEAFCOLORS[(int)LeafColorIndex.NORMAL].G         = (byte)Config.settings["Leaf.Color.Normal.G"];
            LEAFCOLORS[(int)LeafColorIndex.NORMAL].B         = (byte)Config.settings["Leaf.Color.Normal.B"];
            LEAFCOLORS[(int)LeafColorIndex.NORMAL].A         = (byte)Config.settings["Leaf.Color.Normal.A"];
            MAX_DIST_TO_COLOR[(int)LeafColorIndex.NORMAL]    = Config.settings["Leaf.Color.Heart.MaxDist"];

            //Heart color
            LEAFCOLORS[(int)LeafColorIndex.HEART].R         = (byte)Config.settings["Leaf.Color.Heart.R"];
            LEAFCOLORS[(int)LeafColorIndex.HEART].G         = (byte)Config.settings["Leaf.Color.Heart.G"];
            LEAFCOLORS[(int)LeafColorIndex.HEART].B         = (byte)Config.settings["Leaf.Color.Heart.B"];
            LEAFCOLORS[(int)LeafColorIndex.HEART].A         = (byte)Config.settings["Leaf.Color.Heart.A"];
            MAX_DIST_TO_COLOR[(int)LeafColorIndex.HEART]    = Config.settings["Leaf.Color.Heart.MaxDist"];

            ScreenManager.addScreen(new GameScreen(this));
            ScreenManager.getScreen("Game").initialize();
            ScreenManager.getScreen("Game").start();
        }

        //LoadContent is called BEFORE Initialize
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime     = (float)gameTime.ElapsedGameTime.TotalSeconds;
            CURRENTTIME         = (float)gameTime.TotalGameTime.TotalSeconds;

            InputManager.update();

            if (InputManager.isKeyReleased(Keys.Escape))
                this.Exit();

            #region Update world origin and scale
            //Update world scale (zoom level)
            if (InputManager.getDeltaScrollWheel() > 0) //Zooming in
            {
                Vector2 mouseToOrigin   = WORLDOGIRIN - InputManager.getMousePos();
                float lengthOverZoom    = mouseToOrigin.Length()/ZOOMLEVEL;

                if (lengthOverZoom != 0)
                    mouseToOrigin.Normalize();

                //Update zoom level
                ZOOMLEVEL *= 1 + _zoomSpeed * InputManager.getDeltaScrollWheel();

                //Set the world origin to the correct place
                WORLDOGIRIN = InputManager.getMousePos() + mouseToOrigin * lengthOverZoom * ZOOMLEVEL;
            }
            else if (InputManager.getDeltaScrollWheel() < 0) //Zooming out
            {
                Vector2 centerToOrigin  = WORLDOGIRIN - SCREENCENTER;
                float lengthOverZoom    = centerToOrigin.Length()/ZOOMLEVEL;

                //Update zoom level
                ZOOMLEVEL /= 1 + -1 * _zoomSpeed * InputManager.getDeltaScrollWheel();

                if (lengthOverZoom != 0)
                    centerToOrigin.Normalize();

                //Set the world origin to the correct place
                WORLDOGIRIN = SCREENCENTER + centerToOrigin * lengthOverZoom * ZOOMLEVEL;
            }

            //Update world origin
            if (InputManager.isKeyDown(Keys.W))
                WORLDOGIRIN.Y += _scrollSpeed * deltaTime;
            if (InputManager.isKeyDown(Keys.A))
                WORLDOGIRIN.X += _scrollSpeed * deltaTime;
            if (InputManager.isKeyDown(Keys.S))
                WORLDOGIRIN.Y -= _scrollSpeed * deltaTime;
            if (InputManager.isKeyDown(Keys.D))
                WORLDOGIRIN.X -= _scrollSpeed * deltaTime;
            #endregion

            //Let the screenmanager handle the updating of the screens (and their content)
            ScreenManager.update(deltaTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);

            _spriteBatch.Begin();

            //Let the screenmanager handle the drawing of the screens (and their content)
            ScreenManager.draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
