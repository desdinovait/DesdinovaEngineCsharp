using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace WindowsGameLibrary1
{
    public class Movie
    {
        //Proprietà
        Video video;
        VideoPlayer player;
        Texture2D videoTexture;
        SpriteBatch spriteBatch;

        //Visualizzazione
        private bool toDraw = true;
        public bool ToDraw
        {
            get { return toDraw; }
            set { toDraw = value; }
        }

        //Creazione
        private bool isCreated = false;
        public bool IsCreated
        {
            get { return isCreated; }
        }

        //Tag
        private Object tag = null;
        public Object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public Movie(GraphicsDevice device, ContentManager contentManager)
        {
            try
            {
                spriteBatch = new SpriteBatch(device);
                video = contentManager.Load<Video>("Star");
                player = new VideoPlayer();
                isCreated = true;
            }
            catch    
            {
                isCreated = false;
            }
        }

        public void Play()
        {
            if (isCreated)
                player.Play(video);
        }

        public void Update(GameTime gameTime)
        {
            //if (player.State == MediaState.Stopped)
            //{
            //    player.IsLooped = true;
            //    player.Play(video);
            //}
        }

        public void Draw()
        {
            if ((isCreated)&&(toDraw))
            {
                //Sole se è in play o pausa
                if (player.State != MediaState.Stopped)
                    videoTexture = player.GetTexture();

                //Disegna il video
                if (videoTexture != null)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(videoTexture, new Rectangle(0, 0, 800, 600), Color.White);
                    spriteBatch.End();
                }
            }
        }

        public void Dispose()
        {
            try
            {
                video = null;
                player.Dispose();
                player = null;

                spriteBatch.Dispose();
                spriteBatch = null;

                videoTexture.Dispose();
                videoTexture = null;
            }
            catch
            { }

            isCreated = false;
        }

    }
}
