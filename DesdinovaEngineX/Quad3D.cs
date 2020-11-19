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
    public class Quad3D : Engine3DObject
    {
        //Tipo di quad3D
        public enum Quad3DGeneration
        {
            Left = 0,
            Center = 1,
            Right = 2
        }

        //Vertici
        VertexDeclaration vertexDeclaration;
        VertexPositionColorTexture[] vertices;

        //Effetto
        private Effect effect = null;

        //Parametri effetto
        private EffectParameter matrix_World;
        private EffectParameter matrix_View;
        private EffectParameter matrix_Projection;
        private EffectParameter camera_Position;
        private EffectParameter billboard_Texture;
        private EffectParameter billboard_Color;
        private EffectParameter billboard_Alpha;
        private EffectParameter fog_Color;
        private EffectParameter fog_Start;
        private EffectParameter fog_End;
        private EffectParameter fog_Altitude;
        private EffectParameter fog_Thinning;
        private EffectParameter fog_Enabled;

        //Posizione
        private Vector3 position = Vector3.Zero;
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        public float PositionX
        {
            get { return position.X; }
            set { position.X = value; }
        }
        public float PositionY
        {
            get { return position.Y; }
            set { position.Y = value; }
        }
        public float PositionZ
        {
            get { return position.Z; }
            set { position.Z = value; }
        }

        //Rotazione
        private Vector3 rotation = Vector3.Zero;
        public Vector3 Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public float RotationX
        {
            get { return rotation.X; }
            set { rotation.X = value; }
        }
        public float RotationY
        {
            get { return rotation.Y; }
            set { rotation.Y = value; }
        }
        public float RotationZ
        {
            get { return rotation.Z; }
            set { rotation.Z = value; }
        }

        //Scalatura
        private Vector2 scale = Vector2.One;
        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        public float ScaleX
        {
            get { return scale.X; }
            set { scale.X = value; }
        }
        public float ScaleY
        {
            get { return scale.Y; }
            set { scale.Y = value; }
        }

        //Colore
        private Color color = Color.White;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        //Matrice
        private Matrix worldMatrix = Matrix.Identity;
        public Matrix WorldMatrix
        {
            get { return worldMatrix; }
        }

        //Raggio
        public float Radius
        {
            get { return Math.Max(scale.X, scale.Y); }
        }

        //Posizione 2D
        private Vector2 position2D = Vector2.Zero;
        public Vector2 Position2D
        {
            get { return position2D; }
        }

        //Matrice esterna (per figli)
        private Matrix externalMatrix = Matrix.Identity;
        public Matrix ExternalMatrix
        {
            get { return externalMatrix; }
            set { externalMatrix = value; }
        }

        //Nel frustrum
        private bool inFrustrum = true;
        public bool InFrustrum
        {
            get { return inFrustrum; }
        }

        //Proprietà di rendering 
        private BlendMode blendProperties = BlendMode.Additive;
        public BlendMode BlendProperties
        {
            get { return blendProperties; }
            set { blendProperties = value; }
        }
	
        //E' un billboard?
        private bool isBillboard = false;
        public bool IsBillboard
        {
            get { return isBillboard; }
            set { isBillboard = value; }
        }

        //Vertici visibili renderizzati
        private int currentVerticies = 0;
        public int CurrentVerticies
        {
            get { return currentVerticies; }
        }
        //Primitive visibili renderizzati
        private int currentPrimitives = 0;
        public int CurrentPrimitives
        {
            get { return currentPrimitives; }
        }
        //Mesh visibili renderizzate
        private int currentMeshes = 0;
        public int CurrentMeshes
        {
            get { return currentMeshes; }
        }

        //Vertici totali
        private int loadedVerticies = 0;
        public int LoadedVerticies
        {
            get { return loadedVerticies; }
        }
        //Primitive totali
        private int loadedPrimitives = 0;
        public int LoadedPrimitives
        {
            get { return loadedPrimitives; }
        }
        //Mesh caricate
        private int loadedMeshes = 0;
        public int LoadedMeshes
        {
            get { return loadedMeshes; }
        }
	
        //Bounding sphere
        public BoundingSphere BoundingSphere
        {
            get { return new BoundingSphere(this.Position, this.Radius); }
        }
	
        //Sfera di collisione
        public BoundingSphere CollisionSphere
        {
            get { return new BoundingSphere(this.Position, this.Radius * collisionValue); }
        }

        //Rapporto di collisione
        private float collisionValue;
        public float CollisionValue
        {
            get { return collisionValue; }
            set { collisionValue = value; }
        }	

        //Effect parameters
        private Matrix viewMatrix;
        private Matrix projMatrix;
        private Texture2D texture;

        public Quad3D(string filename, Quad3DGeneration generation, Scene parentScene) : base(parentScene)
        {
            Initialize(ParentScene.SceneContent.Load<Texture2D>(filename), generation, parentScene);
        }

        public Quad3D(Texture2D texture, Quad3DGeneration generation, Scene parentScene): base(parentScene)
        {
            Initialize(texture, generation, parentScene);
        }

        private void Initialize(Texture2D textureNew, Quad3DGeneration generation, Scene parentScene)
        {
            //Effetto
            effect = ParentScene.SceneContent.Load<Effect>("Content\\Effect\\Quad3D");

            //Texture generale
            texture = textureNew;

            //Vertex declaration
            vertexDeclaration = new VertexDeclaration(Core.Graphics.GraphicsDevice, VertexPositionColorTexture.VertexElements);

            //Generazione
            float modifier = 0;
            if (generation == Quad3DGeneration.Center)
            {
                modifier = 0;
            }
            else if (generation == Quad3DGeneration.Left)
            {
                modifier = 1;
            }
            else if (generation == Quad3DGeneration.Right)
            {
                modifier = -1;
            }

            //Vertici
            vertices = new VertexPositionColorTexture[4];
            vertices[0] = new VertexPositionColorTexture(new Vector3(-1 + modifier, -1, 0), Color.White, new Vector2(0, 1));
            vertices[1] = new VertexPositionColorTexture(new Vector3(-1 + modifier, 1, 0), Color.White, new Vector2(0, 0));
            vertices[2] = new VertexPositionColorTexture(new Vector3(1 + modifier, 1, 0), Color.White, new Vector2(1, 0));
            vertices[3] = new VertexPositionColorTexture(new Vector3(1 + modifier, -1, 0), Color.White, new Vector2(1, 1));

            //Conteggio
            loadedMeshes = 1;
            loadedPrimitives = 2;
            loadedVerticies = 4;

            //Parametri effetto
            matrix_World = effect.Parameters["worldMatrix"];
            matrix_View = effect.Parameters["viewMatrix"];
            matrix_Projection = effect.Parameters["projectionMatrix"];
            camera_Position = effect.Parameters["cameraPos"];

            fog_Color = effect.Parameters["fog"].StructureMembers["color"];
            fog_Start = effect.Parameters["fog"].StructureMembers["start"];
            fog_End = effect.Parameters["fog"].StructureMembers["end"];
            fog_Altitude = effect.Parameters["fog"].StructureMembers["altitude"];
            fog_Thinning = effect.Parameters["fog"].StructureMembers["thinning"];
            fog_Enabled = effect.Parameters["fog"].StructureMembers["enabled"];

            billboard_Texture = effect.Parameters["colorMapTexture"];
            billboard_Color = effect.Parameters["billboardColor"];
            billboard_Alpha = effect.Parameters["billboardAlpha"];


            //Calcolo totale
            Core.loadedVertices = Core.loadedVertices + loadedVerticies;
            Core.loadedPrimitives = Core.loadedPrimitives + loadedPrimitives;
            Core.loadedMeshes = Core.loadedMeshes + loadedMeshes;
        }


        public override void Update(GameTime gameTime)
        {
            camera_Position.SetValue(this.ParentScene.SceneCamera.Position);

            //Calcola la matrice (dipende se è un billboard o meno)
            Vector3 newPosition = Vector3.Zero;
            if (isBillboard)
            {
                newPosition = Vector3.Transform(position, externalMatrix);
                worldMatrix = Matrix.CreateScale(scale.X, scale.Y, 1.0f) * Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(rotation.Y), MathHelper.ToRadians(rotation.X), MathHelper.ToRadians(rotation.Z)) * Matrix.CreateBillboard(newPosition, this.ParentScene.SceneCamera.Position, this.ParentScene.SceneCamera.Up, null);
            }
            else
            {
                newPosition = position;
                worldMatrix = Matrix.CreateScale(scale.X, scale.Y, 1.0f) * Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(rotation.Y), MathHelper.ToRadians(rotation.X), MathHelper.ToRadians(rotation.Z)) * Matrix.CreateTranslation(newPosition.X, newPosition.Y, newPosition.Z) * externalMatrix;
            }

            //Matrici
            viewMatrix = this.ParentScene.SceneCamera.ViewMatrix;
            projMatrix = this.ParentScene.SceneCamera.ProjectionMatrix;

            //Posizione 2D
            Vector3 temp2D = Core.Graphics.GraphicsDevice.Viewport.Project(newPosition, this.ParentScene.SceneCamera.ProjectionMatrix, this.ParentScene.SceneCamera.ViewMatrix, Matrix.Identity);
            position2D.X = temp2D.X;
            position2D.Y = temp2D.Y;

            //Calcola se è nel frustrum
            if ((this.ParentScene.SceneCamera.Frustrum.Contains(new BoundingSphere(newPosition, Radius)) == ContainmentType.Disjoint))
            {
                //Fuori dal frustrum
                this.inFrustrum = false;
            }
            else
            {
                //Dentro il frustrum
                this.inFrustrum = true;
            }
            base.Update(gameTime);
        }


        public override void Draw()
        {
            //Azzera i vertici visbili
            currentVerticies = 0;
            currentPrimitives = 0;
            currentMeshes = 0;

            if ((ToDraw)&&(inFrustrum))
            {
                //Salvataggio Renderstates
                bool old1 = Core.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable;
                Blend old2 = Core.Graphics.GraphicsDevice.RenderState.SourceBlend;
                Blend old3 = Core.Graphics.GraphicsDevice.RenderState.DestinationBlend;
                bool old4 = Core.Graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable;
                CullMode old5 = Core.Graphics.GraphicsDevice.RenderState.CullMode;
                BlendFunction old6 = Core.Graphics.GraphicsDevice.RenderState.AlphaBlendOperation;
                bool old7 = Core.Graphics.GraphicsDevice.RenderState.SeparateAlphaBlendEnabled;
                bool old8 = Core.Graphics.GraphicsDevice.RenderState.AlphaTestEnable;
                CompareFunction old9 = Core.Graphics.GraphicsDevice.RenderState.AlphaFunction;
                int old10 = Core.Graphics.GraphicsDevice.RenderState.ReferenceAlpha;

                //Disegna il quadro 2D
                effect.Begin(SaveStateMode.None);
                {
                    effect.CurrentTechnique.Passes[0].Begin();
                    {
                        matrix_World.SetValue(worldMatrix);
                        matrix_View.SetValue(viewMatrix);
                        matrix_Projection.SetValue(projMatrix);

                        fog_Color.SetValue(this.ParentScene.SceneLightEffect.Fog.Color.ToVector4());
                        fog_Start.SetValue(this.ParentScene.SceneLightEffect.Fog.Start);
                        fog_End.SetValue(this.ParentScene.SceneLightEffect.Fog.End);
                        fog_Altitude.SetValue(this.ParentScene.SceneLightEffect.Fog.Altitude);
                        fog_Thinning.SetValue(this.ParentScene.SceneLightEffect.Fog.Thinning);
                        fog_Enabled.SetValue(this.ParentScene.SceneLightEffect.Fog.Enable);

                        float alphaValue = (float)color.A / 255.0f;
                        billboard_Texture.SetValue(texture);
                        billboard_Color.SetValue(color.ToVector4());
                        billboard_Alpha.SetValue((float)alphaValue);

                        if (isBillboard)
                        {
                            //Per qualche ragione se è un billboard viene disegnato con i vertici invertiti
                            if (Core.Graphics.GraphicsDevice.RenderState.CullMode == CullMode.CullCounterClockwiseFace)
                                Core.Graphics.GraphicsDevice.RenderState.CullMode = CullMode.CullClockwiseFace;
                            else if (Core.Graphics.GraphicsDevice.RenderState.CullMode == CullMode.CullClockwiseFace)
                                Core.Graphics.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
                        }
    
                        //Trasparenza
                        if ((alphaValue < 1.0f) || blendProperties == BlendMode.None)
                        {
                            if (blendProperties == BlendMode.AlphaBlend)
                            {
                                //Blend normale (dipende dal canale alpha della texture, se il canale non c'è la trasparenza non viene applicata)
                                Core.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable = true;
                                Core.Graphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                                Core.Graphics.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
                                Core.Graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = false;
                                Core.Graphics.GraphicsDevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
                                Core.Graphics.GraphicsDevice.RenderState.SeparateAlphaBlendEnabled = false;
                                Core.Graphics.GraphicsDevice.RenderState.AlphaTestEnable = true;
                                Core.Graphics.GraphicsDevice.RenderState.AlphaFunction = CompareFunction.Greater;
                                Core.Graphics.GraphicsDevice.RenderState.ReferenceAlpha = 0;                           
                            }
                            else if (blendProperties == BlendMode.Additive)
                            {
                                //Blend additivo (ottimo per effetti grafici)
                                Core.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable = true;
                                Core.Graphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                                Core.Graphics.GraphicsDevice.RenderState.DestinationBlend = Blend.One;
                                Core.Graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = false;
                                Core.Graphics.GraphicsDevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
                                Core.Graphics.GraphicsDevice.RenderState.SeparateAlphaBlendEnabled = false;
                                Core.Graphics.GraphicsDevice.RenderState.AlphaTestEnable = true;
                                Core.Graphics.GraphicsDevice.RenderState.AlphaFunction = CompareFunction.Greater;
                                Core.Graphics.GraphicsDevice.RenderState.ReferenceAlpha = 0;
                            }
                        }
                        else
                        {
                            //Nessun blend
                            Core.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable = false;
                            Core.Graphics.GraphicsDevice.RenderState.SourceBlend = Blend.One;
                            Core.Graphics.GraphicsDevice.RenderState.DestinationBlend = Blend.Zero;
                            Core.Graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
                            Core.Graphics.GraphicsDevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
                            Core.Graphics.GraphicsDevice.RenderState.SeparateAlphaBlendEnabled = false;
                            Core.Graphics.GraphicsDevice.RenderState.AlphaTestEnable = false;
                            Core.Graphics.GraphicsDevice.RenderState.AlphaFunction = CompareFunction.Always;
                            Core.Graphics.GraphicsDevice.RenderState.ReferenceAlpha = 0;
                        }

                        //Commit
                        effect.CommitChanges();

                        //Primitive
                        Core.Graphics.GraphicsDevice.VertexDeclaration = vertexDeclaration;
                        Core.Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleFan, vertices, 0, 2);

                        //Conteggio
                        currentMeshes = currentMeshes + 1;
                        currentPrimitives = currentPrimitives + 2;
                        currentVerticies = currentVerticies + 4;

                        //Restore
                        if (isBillboard)
                        {
                            if (Core.Graphics.GraphicsDevice.RenderState.CullMode == CullMode.CullCounterClockwiseFace)
                                Core.Graphics.GraphicsDevice.RenderState.CullMode = CullMode.CullClockwiseFace;
                            else if (Core.Graphics.GraphicsDevice.RenderState.CullMode == CullMode.CullClockwiseFace)
                                Core.Graphics.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
                        }
                    }
                    effect.CurrentTechnique.Passes[0].End();
                }
                effect.End();

                //Ripristino RenderStates
                Core.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable = old1;
                Core.Graphics.GraphicsDevice.RenderState.SourceBlend = old2;
                Core.Graphics.GraphicsDevice.RenderState.DestinationBlend = old3;
                Core.Graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = old4;
                Core.Graphics.GraphicsDevice.RenderState.CullMode = old5;
                Core.Graphics.GraphicsDevice.RenderState.AlphaBlendOperation = old6;
                Core.Graphics.GraphicsDevice.RenderState.SeparateAlphaBlendEnabled = old7;
                Core.Graphics.GraphicsDevice.RenderState.AlphaTestEnable = old8;
                Core.Graphics.GraphicsDevice.RenderState.AlphaFunction = old9;
                Core.Graphics.GraphicsDevice.RenderState.ReferenceAlpha = old10;

                //Info totali disegnate
                Core.currentVertices = Core.currentVertices + currentVerticies;
                Core.currentPrimitives = Core.currentPrimitives + currentPrimitives;
                Core.currentMeshes = Core.currentMeshes + currentMeshes;

            }
            base.Draw();
        }


        ~Quad3D()
        {
            //Calcolo totale
            Core.loadedVertices = Core.loadedVertices - loadedVerticies;
            Core.loadedPrimitives = Core.loadedPrimitives - loadedPrimitives;
            Core.loadedMeshes = Core.loadedMeshes - loadedMeshes;

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
