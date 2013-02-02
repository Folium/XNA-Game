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
        public static int SCREENWIDTH   = 1280;
        public static int SCREENHEIGHT  = 720;

        public float currentTimeMillis;

        private GraphicsDeviceManager   graphics;
        private SpriteBatch             spriteBatch;

        public GameManager()
        {
            graphics                = new GraphicsDeviceManager(this);
            Content.RootDirectory   = "Content";
            currentTimeMillis       = 0;
        }

        //LoadContent is called BEFORE Initialize
        protected override void Initialize()
        {
            base.Initialize();

            graphics.PreferredBackBufferWidth   = SCREENWIDTH;
            graphics.PreferredBackBufferHeight  = SCREENHEIGHT;
            graphics.ApplyChanges();

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
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime     = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            currentTimeMillis   = (float)gameTime.TotalGameTime.TotalMilliseconds;

            InputManager.update();

            if (InputManager.isKeyReleased(Keys.Escape))
                this.Exit();

            //Let the screenmanager handle the updating of the screens (and their content)
            ScreenManager.update(deltaTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);

            //Let the screenmanager handle the drawing of the screens (and their content)
            ScreenManager.draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
