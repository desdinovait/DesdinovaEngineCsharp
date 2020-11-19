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
    public class Nebula
    {
        //Lista
        private Quad3D[] listNebula = null;

        //Posizione
        private Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set 
            { 
                position = value;
                for (int i = 0; i < listNebula.Length; i++)
                {
                    listNebula[i].Position = listNebula[i].Position + position;
                }

            }
        }
	

        public Nebula(ContentManager contentManager)
        {
            listNebula = new Quad3D[6];

            listNebula[0] = new Quad3D("Content\\Texture\\Nebula4", Quad3D.Quad3DGeneration.Center, contentManager);
            listNebula[1] = new Quad3D("Content\\Texture\\Nebula3", Quad3D.Quad3DGeneration.Center, contentManager);
            listNebula[2] = new Quad3D("Content\\Texture\\Nebula2", Quad3D.Quad3DGeneration.Center, contentManager);
            listNebula[3] = new Quad3D("Content\\Texture\\Nebula3", Quad3D.Quad3DGeneration.Center, contentManager);
            listNebula[4] = new Quad3D("Content\\Texture\\Nebula4", Quad3D.Quad3DGeneration.Center, contentManager);
            listNebula[5] = new Quad3D("Content\\Texture\\Nebula5", Quad3D.Quad3DGeneration.Center, contentManager);


            for (int i = 0; i < listNebula.Length; i++)
            {
                listNebula[i].Position = RandomHelper.GetRandomVector3(0,10);
                listNebula[i].Color = new Color(255, 255, 255, 154);
                listNebula[i].Scale = new Vector2(30.0f, 30.0f);
                listNebula[i].BlendProperties = BlendMode.Additive;
                listNebula[i].IsBillboard = true;
            }

        }

        public void Update(GameTime gameTime, Camera camera)
        {
            for (int i = 0; i < listNebula.Length; i++)
            {
                listNebula[i].Update(gameTime, camera);
            }
        }

        public void Draw(LightEffect lightEffect)
        {
            for (int i = 0; i < listNebula.Length; i++)
            {
                listNebula[i].Draw(lightEffect);
            }
        }
    }
}
