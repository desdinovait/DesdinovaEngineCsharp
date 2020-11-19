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
using DesdinovaEngineX;
using DesdinovaEngineX.Helpers;

namespace DesdinovaEngineX
{
    public class Shadow 
    {
        //Modello per il rendering
        private XModel modelShadow = null;

        //Modello da cui prelevare le info
        private BoundingSphere referenceSphere;
        public BoundingSphere ReferenceSphere
        {
            get { return referenceSphere; }
            set { referenceSphere = value; }
        }

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
        private string tag = string.Empty;
        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public Shadow(ContentManager contentManager)
        {
            try
            {
                //Modello per il wireframe
                modelShadow = new XModel();
                modelShadow.Initialize("Content\\Model\\Shadow", contentManager);

                //Creazione avvenuta
                isCreated = true;
            }
            catch
            {
                //Creazione fallita
                isCreated = false;
            }
        }        
        
        
        public void Draw(LightEffect lightEffect, Camera camera)
        {
            if (isCreated)
            {
                if ((referenceSphere != null))
                {
                    //Disegna il modello (sfera o cerchio)
                    modelShadow.RenderProperties = new RenderProperties(FillMode.Solid, CullMode.None, BlendMode.AlphaBlend);
                    modelShadow.Position = new Vector3(referenceSphere.Center.X, 0.001f, referenceSphere.Center.Z);
                    modelShadow.Scale = new Vector3(referenceSphere.Radius);
                    modelShadow.Update(null, camera);   //L'update viene fatto qui per comodità (tanto verrà eseguito in DEBUG!)
                    modelShadow.Draw(lightEffect);
                }
            }
        }

        ~Shadow()
        {
            //Rilascia le risorse
        }
    }
}

