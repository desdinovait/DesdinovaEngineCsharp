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
    public struct Line3D
    {
        public Vector3 startPosition;
        public Vector3 endPosition;
        public Color startColor;
        public Color endColor;

        public Line3D(Vector3 startPosition, Vector3 endPosition, Color startColor, Color endColor)
        {
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.startColor = startColor;
            this.endColor = endColor;
        }
    }

    public class Lines3D : Engine3DObject
    {
        //Effetto base
        private BasicEffect effect = null;

        //vertici
        private VertexDeclaration vertexDeclaration = null;
        private VertexPositionColor[] vertices = null;
        private int currentIndex = 0;

        //Indexer (è possibile prelevare o modificare dinamicamenteil valore dell'array)
        public Line3D this[int index]
        {
            get
            {
                if (index < 0 || index >= capacity)
                {
                    //Ritorna una linea minima
                    return new Line3D();
                }
                else
                {
                    return new Line3D(vertices[index * 2].Position, vertices[(index * 2) + 1].Position, vertices[index * 2].Color, vertices[(index * 2) + 1].Color);
                }
            }
            set
            {
                if (!(index < 0 || index >= capacity))
                {
                    vertices[index * 2] = new VertexPositionColor(value.startPosition, value.startColor);
                    vertices[(index * 2) + 1] = new VertexPositionColor(value.endPosition, value.endColor);
                }
            }
        }

        //Capienza
        private int lineCount = 0;
        public int Count
        {
            get { return lineCount; }
        }

        //Capacità massima
        private int capacity = 0;
        public int Capacity
        {
            get { return capacity; }
        }

        //Matrice generale
        private Matrix worldMatrix = Matrix.Identity;
        public Matrix WorldMatrix
        {
            get { return worldMatrix; }
            set { worldMatrix = value; }
        }

        public Lines3D(int totalCapacity, Scene sceneParent) : base(sceneParent)
        {
            try
            {
                //Effetto base
                effect = new BasicEffect(Core.Graphics.GraphicsDevice, null);
                effect.VertexColorEnabled = true;
                effect.TextureEnabled = false;
                effect.LightingEnabled = false;

                //Vertex declaration
                vertexDeclaration = new VertexDeclaration(Core.Graphics.GraphicsDevice, VertexPositionColor.VertexElements);

                //Calcolo capacità
                if (totalCapacity < 0)
                {
                    totalCapacity = 1;
                }
                if (totalCapacity * 2 > Core.Graphics.GraphicsDevice.GraphicsDeviceCapabilities.MaxPrimitiveCount)
                {
                    totalCapacity = Core.Graphics.GraphicsDevice.GraphicsDeviceCapabilities.MaxPrimitiveCount / 2;
                }

                capacity = totalCapacity;
                vertices = new VertexPositionColor[capacity * 2];

                //Creazione avvenuta
                IsCreated = true;
            }
            catch
            {
                //Creazione fallita
                IsCreated = false;
            }
        }


        public bool AddLine(Line3D newLine)
        {
            if (IsCreated)
            {
                if (currentIndex < (capacity * 2))
                {
                    VertexPositionColor v1 = new VertexPositionColor(newLine.startPosition, newLine.startColor);
                    VertexPositionColor v2 = new VertexPositionColor(newLine.endPosition, newLine.endColor);

                    vertices[currentIndex] = v1;
                    currentIndex++;
                    vertices[currentIndex] = v2;
                    currentIndex++;
                    lineCount++;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public override void Update(GameTime gameTime)
        {
            if (IsCreated)
            {
                effect.World = worldMatrix;
                effect.View = this.ParentScene.SceneCamera.ViewMatrix;
                effect.Projection = this.ParentScene.SceneCamera.ProjectionMatrix;
            }
            base.Update(gameTime);
        }


        public override void Draw()
        {
            if (IsCreated)
            {
                if ((ToDraw) && (lineCount >= 1))
                {
                    // Run the effect
                    effect.Begin(SaveStateMode.SaveState);

                    //Imposta la nebbia
                    if (this.ParentScene.SceneLightEffect != null)
                    {
                        effect.FogEnabled = this.ParentScene.SceneLightEffect.Fog.Enable;
                        if (this.ParentScene.SceneLightEffect.Fog.Enable)
                        {
                            effect.FogColor = this.ParentScene.SceneLightEffect.Fog.Color.ToVector3();
                            effect.FogStart = this.ParentScene.SceneLightEffect.Fog.Start;
                            effect.FogEnd = this.ParentScene.SceneLightEffect.Fog.End;
                        }
                    }
                    else
                    {
                        effect.FogEnabled = false;
                    }

                    // Configure the graphics device and effect to render our lines
                    Core.Graphics.GraphicsDevice.VertexDeclaration = vertexDeclaration;
                    Core.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable = false;

                    for (int i = 0; i < effect.CurrentTechnique.Passes.Count; ++i)
                    {
                        effect.CurrentTechnique.Passes[i].Begin();
                        Core.Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, lineCount);
                        effect.CurrentTechnique.Passes[i].End();
                    }

                    effect.End();
                }
            }
            base.Draw();
        }



        ~Lines3D()
        {
            /*if (effect != null)
            {
                effect.Dispose();
                effect = null;
            }

            if (vertexDeclaration != null)
            {
                vertexDeclaration.Dispose();
                vertexDeclaration = null;
            }*/
        }
    }
}