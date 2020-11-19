//Using di sistema
using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
//Using XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
//Using Desdinova Engine X
using DesdinovaModelPipeline;
using DesdinovaModelPipeline.Helpers;

namespace DesdinovaModelPipeline
{
    public class Sprite3D : Sprite
    {
        //Posizione 3D
        private Vector3 position = Vector3.Zero;
        public new Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        //Fattore di distanza (a questo valore di distanza lo sprite ha dimensione 1)
        private float distanceFactor = 20;
        public float DistanceFactor
        {
            get { return distanceFactor; }
            set { distanceFactor = value; }
        }

        public Sprite3D(Texture2D texture, Scene parentScene):base(texture, parentScene)
        {
            IsCreated = base.IsCreated;
        }

        public Sprite3D(string textureName, uint sprites, Scene parentScene): base(textureName, sprites, parentScene)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (IsCreated)
            {
                //Project the light position into 2D screen space.
                Vector3 projectedPosition = Core.Graphics.GraphicsDevice.Viewport.Project(position, this.ParentScene.SceneCamera.ProjectionMatrix, this.ParentScene.SceneCamera.ViewMatrix, Matrix.Identity);
                base.Position = new Vector2(projectedPosition.X, projectedPosition.Y);

                float sc = distanceFactor / Vector3.Distance(position, this.ParentScene.SceneCamera.Position);

                base.Scale = new Vector2(sc, sc);

                base.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw()
        {
            if (IsCreated)
            {
                base.Draw();
            }
        }
    }
}
