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
    public class PostProcessing : Engine2DObject
    {
        //vertici
        private VertexDeclaration vertexDecl = null;
        private VertexPositionTexture[] verts = null;
        private short[] ib = null;

        //Resolve target (backbuffer)
        private ResolveTexture2D resolveTarget = null;

        //Effect parameters
        private readonly EffectParameter offscreenTextureEP = null;

        //Effetto base
        private Effect postprocessEffect = null;
        public Effect PostprocessEffect
        {
            get { return postprocessEffect; }
            set { postprocessEffect = value; }
        }

        //Tecnica corrente
        private string tecniqueName = string.Empty;
        public string TecniqueName
        {
            get { return tecniqueName; }
            set
            {
                tecniqueName = value;
                try
                {
                    postprocessEffect.CurrentTechnique = postprocessEffect.Techniques[tecniqueName];
                }
                catch
                {
                    postprocessEffect.CurrentTechnique = postprocessEffect.Techniques[0];
                    tecniqueName = postprocessEffect.Techniques[0].Name;
                }
            }
        }

        //Abilitato
        private bool isEnable = true;
        public bool IsEnable
        {
            get { return isEnable; }
            set { isEnable = value; }
        }


        public PostProcessing(string effectName, Scene parentScene) : base(parentScene)
        {
            //Post Process
            try
            {
                postprocessEffect = parentScene.SceneContent.Load<Effect>(effectName);

                // Look up the resolution and format of our main backbuffer.
                PresentationParameters pp = Core.Graphics.GraphicsDevice.PresentationParameters;
                int width = pp.BackBufferWidth;
                int height = pp.BackBufferHeight;
                SurfaceFormat format = pp.BackBufferFormat;

                // Create a texture for reading back the backbuffer contents.
                resolveTarget = new ResolveTexture2D(Core.Graphics.GraphicsDevice, width, height, 1, format);

                vertexDecl = new VertexDeclaration(Core.Graphics.GraphicsDevice, VertexPositionTexture.VertexElements);
                verts = new VertexPositionTexture[]
                        {
                            new VertexPositionTexture(
                                new Vector3(1,-1,0),
                                new Vector2(1,1)),
                            new VertexPositionTexture(
                                new Vector3(-1,-1,0),
                                new Vector2(0,1)),
                            new VertexPositionTexture(
                                new Vector3(-1,1,0),
                                new Vector2(0,0)),
                            new VertexPositionTexture(
                                new Vector3(1,1,0),
                                new Vector2(1,0))
                        };

                ib = new short[] { 0, 1, 2, 2, 3, 0 };

                postprocessEffect.CurrentTechnique = postprocessEffect.Techniques[0];
                tecniqueName = postprocessEffect.Techniques[0].Name.ToString();

                //Effect parameters
                offscreenTextureEP = postprocessEffect.Parameters["offscreenTexture"];

                //Creazione evvenuta
                IsCreated = true;
            }
            catch
            {
                //creazione fallita
                IsCreated = false;
            }
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
        }

        public override void Draw()
        {
            if (IsCreated)
            {
                if (isEnable)
                {
                    try
                    {
                        Core.Graphics.GraphicsDevice.ResolveBackBuffer(resolveTarget);

                        postprocessEffect.Begin(SaveStateMode.None);
                        {
                            foreach (EffectPass pass in postprocessEffect.CurrentTechnique.Passes)
                            {
                                //Effect parameters
                                offscreenTextureEP.SetValue(resolveTarget);

                                //Inizio
                                pass.Begin();
                                {
                                    Core.Graphics.GraphicsDevice.VertexDeclaration = vertexDecl;
                                    Core.Graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, verts, 0, 4, ib, 0, 2);
                                }
                                pass.End();
                            }
                        }
                        postprocessEffect.End();
                    }
                    catch
                    {
                    }
                }
            }
            base.Draw();
        }

        private bool isDisposed = false;
        public void Dispose()
        {
            //Avoid issues when multiple threads call Dispose at the same time.
            lock (this)
            {
                //Do nothing if already disposed of
                if (isDisposed)
                    return;

                //Dispose of all the disposable objects used by this instance
                //Including the one that wraps the unmanaged resource
                if (vertexDecl != null)
                {
                    vertexDecl.Dispose();
                    vertexDecl = null;
                }
                if (resolveTarget != null)
                {
                    resolveTarget.Dispose();
                    resolveTarget = null;
                }

                //Remember this object has been disposed of
                isDisposed = true;
            }
        }


        ~PostProcessing()
        {

        }

    }
}
