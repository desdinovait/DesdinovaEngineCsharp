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
    public class BoundingRenderer : Engine3DObject
    {
        //Modello per il rendering
        private SimpleModel modelSphere = null;

        //Modello da cui prelevare le info
        private BoundingSphere referenceSphere;
        public BoundingSphere ReferenceSphere
        {
            get { return referenceSphere; }
            set { referenceSphere = value; }
        }

        //Colore
        private Color color = Color.White;
        public Color Color
        {
            get { return color; }
            set
            { 
                color = value;
                modelSphere.DiffuseColor = color;
            }
        }


        public BoundingRenderer(bool useCircle, Scene parentScene):base(parentScene)
        {
            try
            {
                //Modello per il wireframe
                if (useCircle)
                {
                    modelSphere = new SimpleModel("Content\\Model\\BoundingCircle", "", false, parentScene);
                    modelSphere.IsBillboard = true;
                    modelSphere.BlendProperties = BlendMode.AlphaBlend;
                }
                else
                {
                    modelSphere = new SimpleModel("Content\\Model\\BoundingSphere", "", false, parentScene);
                    modelSphere.IsBillboard = false;
                    modelSphere.BlendProperties = BlendMode.AlphaBlend;
                }

                //Impostazioni di base
                modelSphere.LightingEnabled = false;
                modelSphere.FogEnabled = false;
                modelSphere.Alpha = 1.0f;

                //Creazione avvenuta
                IsCreated = true;
            }
            catch
            {
                //Creazione fallita
                IsCreated = false;
            }
        }


        public override void Update(GameTime gametime)
        {
            modelSphere.Position = referenceSphere.Center;
            modelSphere.Scale = new Vector3(referenceSphere.Radius);
            base.Update(gametime);
        }
        
        
        public override void Draw()
        {
            base.Draw();
        }

        ~BoundingRenderer()
        {
            //Rilascia le risorse
        }

        public bool AddChildren()
        {
            this.ParentScene.AddObject(modelSphere);
            return true;
        }
        public bool RemoveChildren()
        {
            this.ParentScene.RemoveObject(modelSphere);
            return true;
        }
    }
}

