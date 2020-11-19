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
    public struct Line2D
    {
        public Vector2 startPosition;
        public Vector2 endPosition;
        public Color startColor;
        public Color endColor;

        public Line2D(Vector2 startPosition, Vector2 endPosition, Color startColor, Color endColor)
        {
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.startColor = startColor;
            this.endColor = endColor;
        }
    }

    public class Lines2D : Engine2DObject
    {
        //Effetto di base
        private BasicEffect effect = null;

        //vertici
        private VertexDeclaration vertexDeclaration = null;
        private VertexPositionColor[] vertices = null;
        private VertexPositionColor[] verticesFinal = null;
        private int currentIndex = 0;

        //Indexer (è possibile prelevare o modificare dinamicamente il valore dell'array)
        public Line2D this[int index]
        {
            get
            {
                if (index < 0 || index >= capacity)
                {
                    //Ritorna una linea minima
                    return new Line2D();
                }
                else
                {
                    return new Line2D(new Vector2(vertices[index * 2].Position.X, vertices[index * 2].Position.Y), new Vector2(vertices[(index * 2) + 1].Position.X, vertices[(index * 2) + 1].Position.Y), vertices[index * 2].Color, vertices[(index * 2) + 1].Color);
                }
            }
            set
            {
                if (!(index < 0 || index >= capacity))
                {
                    vertices[index * 2] = new VertexPositionColor(new Vector3(value.startPosition.X, value.startPosition.Y, 0.0f), value.startColor);
                    vertices[(index * 2) + 1] = new VertexPositionColor(new Vector3(value.endPosition.X, value.endPosition.Y, 0.0f), value.endColor);
                }

                //Ricalcola i vertici
                PositionOffset = positionOffset;
            }
        }

        //Linee presenti
        private int lineCount = 0;
        public int Count
        {
            get { return lineCount; }
        }

        //Capienza massima
        private int capacity = 0;
        public int Capacity
        {
            get { return capacity; }
        }

        //Spostamento di tutte le linee
        private Vector2 positionOffset = Vector2.Zero;
        public Vector2 PositionOffset
        {
            get { return positionOffset; }
            set 
            { 
                positionOffset = value;

                for (int i = 0; i < currentIndex; i++)
                {
                    verticesFinal[i].Position.X = vertices[i].Position.X + positionOffset.X;
                    verticesFinal[i].Position.Y = vertices[i].Position.Y + positionOffset.Y;
                    verticesFinal[i].Color = vertices[i].Color;
                }
            }
        }

        //Colore generale
        private Color color;
        public Color Color
        {
            get { return color; }
            set 
            { 
                color = value;
                for (int i = 0; i < currentIndex; i++)
                {
                    verticesFinal[i].Color = color;
                }         
            }
        }

        public Lines2D(int totalCapacity, Scene parentScene):base(parentScene)
        {
            try
            {
                //Effetto base
                effect = new BasicEffect(Core.Graphics.GraphicsDevice, null);
                effect.VertexColorEnabled = true;
                effect.TextureEnabled = false;
                effect.LightingEnabled = false;

                //Matrici
                effect.World = Matrix.Identity;
                effect.View = Matrix.CreateLookAt(Vector3.Zero, Vector3.Forward, Vector3.Up);
                effect.Projection = Matrix.CreateOrthographicOffCenter(0.0f, Core.Graphics.GraphicsDevice.Viewport.Width, Core.Graphics.GraphicsDevice.Viewport.Height, 0.0f, 0.0f, 1.0f);

                //Vertex Declaration
                vertexDeclaration = new VertexDeclaration(Core.Graphics.GraphicsDevice, VertexPositionColor.VertexElements);

                //Conto capacità
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
                verticesFinal = new VertexPositionColor[capacity * 2];

                //Creazione avvenuta
                IsCreated = true;
            }
            catch
            {
                //Creazione fallita
                IsCreated = false;
            }
        }


        public bool AddLine(Line2D newLine)
        {
            if (IsCreated)
            {
                if (currentIndex < (capacity * 2))
                {
                    VertexPositionColor v1 = new VertexPositionColor(new Vector3(newLine.startPosition, 0f), newLine.startColor);
                    VertexPositionColor v2 = new VertexPositionColor(new Vector3(newLine.endPosition, 0f), newLine.endColor);

                    vertices[currentIndex] = v1;
                    verticesFinal[currentIndex] = v1;
                    currentIndex++;
                    vertices[currentIndex] = v2;
                    verticesFinal[currentIndex] = v2;
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

                    // Configure the graphics device and effect to render our lines
                    Core.Graphics.GraphicsDevice.VertexDeclaration = vertexDeclaration;
                    Core.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable = false;


                    for (int i = 0; i < effect.CurrentTechnique.Passes.Count; ++i)
                    {
                        effect.CurrentTechnique.Passes[i].Begin();
                        Core.Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, verticesFinal, 0, lineCount);
                        effect.CurrentTechnique.Passes[i].End();
                    }

                    effect.End();
                }
            }
            base.Draw();
        }



        ~Lines2D()
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