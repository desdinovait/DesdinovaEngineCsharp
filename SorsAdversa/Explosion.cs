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
//Using DesdinovaEngineX
using DesdinovaModelPipeline;
using DesdinovaModelPipeline.Helpers;

namespace SorsAdversa
{
    public class Explosion
    {
        private Quad3D quadHalo;
        private float currentScale = 0.0f;
        private float currentAlpha = 255;
        private bool isActive = true;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
	

        public Explosion(string filename, ContentManager contentManager)
        {
            quadHalo = new Quad3D(filename, Quad3D.Quad3DGeneration.Center, contentManager);
            quadHalo.RotationX = 90;
            quadHalo.Color = Color.YellowGreen;
            quadHalo.ToDraw = true;

        }

        public void Update(GameTime gameTime, Camera camera)
        {
            currentAlpha = currentAlpha - 0.25f;
            if (currentAlpha > 0.0f)
            {
                isActive = true;
                currentScale = currentScale + 0.1f;
                quadHalo.Color = new Color(quadHalo.Color.R, quadHalo.Color.G, quadHalo.Color.B, (byte)currentAlpha);
                quadHalo.Scale = new Vector2(currentScale, currentScale);
                quadHalo.Update(gameTime, camera);
            }
            else
            {
                isActive = false;
            }
        }

        public void Draw(LightEffect lightEffect)
        {
            if (isActive)
            {
                quadHalo.Draw(lightEffect);
            }
        }

    }
}
