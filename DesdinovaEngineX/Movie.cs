using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace DesdinovaModelPipeline
{
    public class Movie : Engine2DObject
    {
        //Proprietà
        Video video;
        VideoPlayer player;
        Texture2D videoTexture;
        SpriteBatch spriteBatch;

        public Movie(string name, Scene parentScene): base(parentScene)
        {
            try
            {
                spriteBatch = new SpriteBatch(Core.Graphics.GraphicsDevice);
                video = parentScene.SceneContent.Load<Video>(name);
                player = new VideoPlayer();
                IsCreated = true;
                ToDraw = true;
            }
            catch
            {
                IsCreated = false;
            }
        }

        public void Play()
        {
            if (IsCreated)
                player.Play(video);
        }

        public override void Update(GameTime gameTime)
        {
            //if (player.State == MediaState.Stopped)
            //{
            //    player.IsLooped = true;
            //    player.Play(video);
            //}
        }

        public override void Draw()
        {
            if ((IsCreated) && (ToDraw))
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

            IsCreated = false;
        }
    }
}
