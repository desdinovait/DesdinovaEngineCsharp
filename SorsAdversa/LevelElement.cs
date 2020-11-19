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
    public class LevelElement : XModel
    {
        private List<Quad3D> glows = null;

        public LevelElement(Scene parentScene) : base(parentScene)
        {
        }

        public override bool Initialize(string assetName, ContentManager contentManager)
        {
            //Crea il modello principale
            if (base.Initialize(assetName, contentManager))
            {
                glows = new List<Quad3D>();

                int counter = 0;
                while (true)
                {
                    bool ok = false;
                    Matrix newMatrix = base.GetBoneTransform("Glow" + counter.ToString(), true, out ok);
                    if (ok == false)
                    {
                        break;
                    }
                    else
                    {
                        Quad3D newGlow = new Quad3D("Content\\Texture\\Particle2", Quad3D.Quad3DGeneration.Center, contentManager);
                        Vector3 scale = Vector3.One;
                        Vector3 pos = Vector3.Zero;
                        Quaternion quat = Quaternion.Identity;
                        newMatrix.Decompose(out scale, out quat, out pos);
                        newGlow.Position = pos;
                        newGlow.Scale = new Vector2(scale.X, scale.Y);
                        newGlow.IsBillboard = true;
                        newGlow.BlendProperties = BlendMode.Additive;
                        glows.Add(newGlow);

                        counter = counter + 1;
                    }
                }
                return true;
            }
            return false;
        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            //Nuova posizione ( s = v * t )
            //Vector3 velocity = new Vector3(0, 0, 0);
            //base.PositionX = base.PositionX + (velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds);
            //base.PositionY = base.PositionY + (velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
            //base.PositionZ = base.PositionZ + (velocity.Z * (float)gameTime.ElapsedGameTime.TotalSeconds);

            //Update base
            base.Update(gameTime, camera);

            /*Update glows
            for (int i = 0; i < glows.Count; i++)
            {
                glows[i].ExternalMatrix = base.WorldMatrix;
                glows[i].Update(gameTime, camera);
            }//*/
        }

        public override void Draw(LightEffect lightEffect)
        {
            //Draw base
            base.Draw(lightEffect);

            /*Draw glows
            for (int i = 0; i < glows.Count; i++)
            {
                glows[i].Draw(lightEffect);
            }//*/
        }
    }
}