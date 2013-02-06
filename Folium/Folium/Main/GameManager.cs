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

namespace Folium.Main
{
    public class GameManager : Microsoft.Xna.Framework.Game
    {
        public static int SCREENWIDTH       = 1280;
        public static int SCREENHEIGHT      = 720;
        public static Vector2 worldOrigin   = new Vector2(SCREENWIDTH/2, SCREENHEIGHT/2);
        public static Vector2 screenCenter  = new Vector2(SCREENWIDTH/2, SCREENHEIGHT/2);
        public static float zoomLevel       = 1;

        public float currentTimeMillis;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private float _scrollSpeed;
        private float _zoomSpeed;

        public GameManager()
        {
            _graphics               = new GraphicsDeviceManager(this);
            Content.RootDirectory   = "Content";
            currentTimeMillis       = 0;
            _scrollSpeed            = 512;
            _zoomSpeed              = 0.001f;
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
            currentTimeMillis   = (float)gameTime.TotalGameTime.TotalMilliseconds;

            InputManager.update();

            if (InputManager.isKeyReleased(Keys.Escape))
                this.Exit();

            #region Update world origin and scale
            //Update world scale (zoom level)
            if (InputManager.getDeltaScrollWheel() > 0) //Zooming in
            {
                Vector2 mouseToOrigin   = worldOrigin - InputManager.getMousePos();
                float lengthOverZoom    = mouseToOrigin.Length()/zoomLevel;

                if (lengthOverZoom != 0)
                    mouseToOrigin.Normalize();

                //Update zoom level
                zoomLevel *= 1 + _zoomSpeed * InputManager.getDeltaScrollWheel();

                //Set the world origin to the correct place
                worldOrigin = InputManager.getMousePos() + mouseToOrigin * lengthOverZoom * zoomLevel;
            }
            else if (InputManager.getDeltaScrollWheel() < 0) //Zooming out
            {
                Vector2 centerToOrigin  = worldOrigin - screenCenter;
                float lengthOverZoom    = centerToOrigin.Length()/zoomLevel;

                //Update zoom level
                zoomLevel /= 1 + -1 * _zoomSpeed * InputManager.getDeltaScrollWheel();

                if (lengthOverZoom != 0)
                    centerToOrigin.Normalize();

                //Set the world origin to the correct place
                worldOrigin = screenCenter + centerToOrigin * lengthOverZoom * zoomLevel;
            }

            //Update world origin
            if (InputManager.isKeyDown(Keys.W))
                worldOrigin.Y += _scrollSpeed * deltaTime;
            if (InputManager.isKeyDown(Keys.A))
                worldOrigin.X += _scrollSpeed * deltaTime;
            if (InputManager.isKeyDown(Keys.S))
                worldOrigin.Y -= _scrollSpeed * deltaTime;
            if (InputManager.isKeyDown(Keys.D))
                worldOrigin.X -= _scrollSpeed * deltaTime;
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
