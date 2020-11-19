using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SceneGraph
{
    public partial class Core : Microsoft.Xna.Framework.Game
    {
        //Device manager per egli elementi grafici
        protected static GraphicsDeviceManager gameGraphics = null;
        public static GraphicsDeviceManager Graphics
        {
            get { return gameGraphics; }
        }

        //Service container
        protected static GameServiceContainer gameService = null;
        public static GameServiceContainer Service
        {
            get { return gameService; }
        }

        //Content manager per gli elementi da caricare
        protected static ContentManager gameContent = null;
        public new static ContentManager Content
        {
            get { return gameContent; }
        }

        public Scene CurrentScene { get; set; }

        public Core()
        {
            //Crea il service
            gameService = Services;

            //Crea il Content
            gameContent = new ContentManager(gameService);

            //Crea il Device
            gameGraphics = new GraphicsDeviceManager(this);
        }

        protected override void Update(GameTime gameTime)
        {
            CurrentScene.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            CurrentScene.Draw();
            base.Draw(gameTime);
        }
    }
}
