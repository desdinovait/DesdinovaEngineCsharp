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
    public class SkySphere : Engine3DObject
    {
        //Modello base
        private Model model;

        //Effetto base
        private Effect effect;

        //Parametri effetto
        private readonly EffectParameter matrix_View;
        private readonly EffectParameter matrix_Proj;
        private readonly EffectParameter sky_color;

        //Colore generale
        private Color color = Color.White;
        public Color Color
        {
            get { return color; }
            set 
            { 
                color = value;
                sky_color.SetValue(color.ToVector4());
            }
        }

        private TextureCube cubeTexture;
        public TextureCube CubeTexture
        {
            get { return cubeTexture; }
        }

        public SkySphere(string cubemapName, Scene parentScene ):base(parentScene)
        {
            try
            {
                effect = parentScene.SceneContent.Load<Effect>("Content\\Effect\\SkySphere");
                model = parentScene.SceneContent.Load<Model>("Content\\Model\\BoundingSphere");
                cubeTexture = parentScene.SceneContent.Load<TextureCube>(cubemapName);
                effect.Parameters["colorMapTexture"].SetValue(cubeTexture);
                matrix_View = effect.Parameters["viewMatrix"];
                matrix_Proj = effect.Parameters["projectionMatrix"];
                sky_color = effect.Parameters["color"];

                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = effect;
                    }
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
                matrix_View.SetValue(this.ParentScene.SceneCamera.ViewMatrix);
                matrix_Proj.SetValue(this.ParentScene.SceneCamera.ProjectionMatrix);
            }
            base.Update(gameTime);
        }

        public override void Draw()
        {
            if (IsCreated)
            {
                if (ToDraw)
                {
                    //Render States (salvataggio)
                    CullMode old1 = Core.Graphics.GraphicsDevice.RenderState.CullMode;
                    bool old2 = Core.Graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable;

                    //Render States (settaggio)
                    Core.Graphics.GraphicsDevice.RenderState.CullMode = CullMode.CullClockwiseFace;
                    Core.Graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = false;

                    //Disegna
                    effect.Begin(SaveStateMode.None);
                    {
                        effect.CurrentTechnique.Passes[0].Begin();
                        foreach (ModelMesh mesh in model.Meshes)
                        {
                            foreach (ModelMeshPart part in mesh.MeshParts)
                            {
                                //Disegna i vertici correnti (in base alla parte della mesh)
                                Core.Graphics.GraphicsDevice.Vertices[0].SetSource(mesh.VertexBuffer, part.StreamOffset, part.VertexStride);
                                Core.Graphics.GraphicsDevice.VertexDeclaration = part.VertexDeclaration;
                                Core.Graphics.GraphicsDevice.Indices = mesh.IndexBuffer;
                                Core.Graphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.BaseVertex, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount);
                            }
                        }
                        effect.CurrentTechnique.Passes[0].End();
                    }
                    effect.End();//*/

                    //Reimposta i Render States
                    Core.Graphics.GraphicsDevice.RenderState.CullMode = old1;
                    Core.Graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = old2;
                }
            }
            base.Draw();
        }

        ~SkySphere()
        {
            //Rilascia le risorse
            //effect.Dispose();
        }
    }
}
