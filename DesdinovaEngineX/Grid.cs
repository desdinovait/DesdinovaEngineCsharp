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
    public class Grid : Engine3DObject, IEngineChildCollector
    {
        //Linee usate per disegnare la  griglia
        private Lines3D lines = null;

        public Grid(int side, int subdivsions, Color color, Scene parentScene):base(parentScene)
        {
            try
            {
                lines = new Lines3D((subdivsions * 2) + 2 , parentScene);
                for (int i = 0; i < subdivsions + 1; i++)
                {
                    lines.AddLine(new Line3D(new Vector3(0 - (side / 2), 0, i * (side / subdivsions)), new Vector3(side - (side / 2), 0, i * (side / subdivsions)), color, color));
                }
                for (int i = 0; i < subdivsions + 1; i++)
                {
                    lines.AddLine(new Line3D(new Vector3((i * (side / subdivsions)) - (side / 2), 0, 0), new Vector3((i * (side / subdivsions)) - (side / 2), 0, side), color, color));
                }

                //Creazione avvenuta
                IsCreated = true;
            }
            catch
            {
                //Creazione fallita
                IsCreated = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (IsCreated)
            {
                lines.WorldMatrix = Matrix.CreateTranslation(this.Position.X, this.Position.Y, this.Position.Z);
            }
            base.Update(gameTime);
        }

        public override void Draw()
        {
            base.Draw();
        }

        ~Grid()
        {
            //Rilascia le risorse
        }

        public bool AddChildren()
        {                
            this.ParentScene.AddObject(lines);
            return true;
        }
        public bool RemoveChildren()
        {
            this.ParentScene.RemoveObject(lines);
            return true;
        }
    }
}
