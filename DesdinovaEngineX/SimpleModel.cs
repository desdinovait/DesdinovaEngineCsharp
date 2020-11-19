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
    public class SimpleModel : Engine3DObject
    {
        //Modello base
        private Model model = null;

        //Matrici interne
        private Matrix[] boneTransforms = null;

        //Effetto
        private Matrix viewMatrix = Matrix.Identity;
        private Matrix projMatrix = Matrix.Identity;

        //Texture
        private Texture2D texture = null;

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
        private Vector3 scale = Vector3.One;
        public Vector3 Scale
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
        public float ScaleZ
        {
            get { return scale.Z; }
            set { scale.Z = value; }
        }

        //Matrici
        private Matrix worldMatrix = Matrix.Identity;
        public Matrix WorldMatrix
        {
            get { return worldMatrix; }
        }

        //Proprietà di rendering 
        private BlendMode blendProperties = BlendMode.AlphaBlend;
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


        //Colori
        private Color diffuseColor = Color.White;
        public Color DiffuseColor
        {
            get { return diffuseColor; }
            set { diffuseColor = value; }
        }

        private Color specularColor = Color.White;
        public Color SpecularColor
        {
            get { return specularColor; }
            set { specularColor = value; }
        }

        private float specularPower = 10.0f;
        public float SpecularPower
        {
            get { return specularPower; }
            set { specularPower = value; }
        }

        private Color emissiveColor = Color.Black;
        public Color EmissiveColor
        {
            get { return emissiveColor; }
            set { emissiveColor = value; }
        }

        private bool vertexColorEnabled = false;
        public bool VertexColorEnabled
        {
            get { return vertexColorEnabled; }
            set { vertexColorEnabled = value; }
        }

        //Valore trasparenza 
        private float alpha = 1.0f;
        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        //Abilita Texture
        private bool textureEnabled = true;
        public bool TextureEnabled
        {
            get { return textureEnabled; }
            set { textureEnabled = value; }
        }

        //Abilita luci
        private bool lightingEnabled = false;
        public bool LightingEnabled
        {
            get { return lightingEnabled; }
            set { lightingEnabled = value; }
        }

        //Abilita nebbia
        private bool fogEnabled = false;
        public bool FogEnabled
        {
            get { return fogEnabled; }
            set { fogEnabled = value; }
        }

        //Conteggio
        private CountInfo countInfo = new CountInfo();
        public CountInfo CountInfo
        {
            get { return countInfo; }
        }

        //Questi 2 costruttori (uno interno e l'altro pubblico) permettono di stabilire se il SimpleModel caricato deve essere conteggiato o meno.
        //Viene usato sopratutto per il BoundingRenderer perchè in questo modo i bounding sphere/box caricati e visualizzati non vengono conteggiati
        //perchè storpierebbero il risultato del conteggio
        private bool mustCountInfo = true;
        internal SimpleModel(string modelName, string textureName, bool countInfoOK, Scene parentScene):base(parentScene)
        {
            mustCountInfo = countInfoOK;
            this.Initialize(modelName, textureName, parentScene);
        }

        public SimpleModel(string modelName, string textureName, Scene parentScene):base(parentScene)
        {
            mustCountInfo = true;
            this.Initialize(modelName, textureName, parentScene);
        }

        private void Initialize(string modelName, string textureName, Scene parentScene)
        {
            try
            {
                //Carica il modello da file
                model = ParentScene.SceneContent.Load<Model>(modelName);

                //Preleva le trasformazioni assolute
                boneTransforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(boneTransforms);

                //Texture
                if (textureName != string.Empty)
                {
                    texture = ParentScene.SceneContent.Load<Texture2D>(textureName);
                    textureEnabled = true;
                }
                else
                {
                    texture = null;
                    textureEnabled = false;
                }

                //Conteggio
                for (int m = 0; m < model.Meshes.Count; m++)
                {
                    for (int p=0; p<model.Meshes[m].MeshParts.Count; p++)
                    {
                        countInfo.LoadedVerticies = countInfo.LoadedVerticies + model.Meshes[m].MeshParts[p].NumVertices;
                        countInfo.LoadedPrimitives = countInfo.LoadedPrimitives + model.Meshes[m].MeshParts[p].PrimitiveCount;
                    }
                }
                countInfo.LoadedMeshes = model.Meshes.Count;
                countInfo.LoadedModels = 1;

                //Calcolo totale
                if (mustCountInfo)
                {
                    Core.loadedVertices = Core.loadedVertices + countInfo.LoadedVerticies;
                    Core.loadedPrimitives = Core.loadedPrimitives + countInfo.LoadedPrimitives;
                    Core.loadedMeshes = Core.loadedMeshes + countInfo.LoadedMeshes;
                    Core.loadedModels = Core.loadedModels + countInfo.LoadedModels;//*/
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
                //Imposta la matrice finale di posizione
                if (isBillboard)
                {
                    worldMatrix = Matrix.CreateScale(scale.X, scale.Y, scale.Z) * Matrix.CreateBillboard(position, base.ParentScene.SceneCamera.Position, base.ParentScene.SceneCamera.Up, null);
                }
                else
                {
                    worldMatrix = Matrix.CreateScale(scale.X, scale.Y, scale.Z) *
                                  Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(rotation.Y), MathHelper.ToRadians(rotation.X), MathHelper.ToRadians(rotation.Z)) *
                                  Matrix.CreateTranslation(position.X, position.Y, position.Z);
                }

                viewMatrix = base.ParentScene.SceneCamera.ViewMatrix;
                projMatrix = base.ParentScene.SceneCamera.ProjectionMatrix;
            }
            base.Update(gameTime);
        }    

        public override void Draw()
        {
            if (IsCreated)
            {
                //Conteggio
                countInfo.CurrentModels = 0;
                countInfo.CurrentMeshes = 0;
                countInfo.CurrentPrimitives = 0;
                countInfo.CurrentVerticies = 0;

                if (ToDraw)
                {
                    if (mustCountInfo)  countInfo.CurrentModels = 1;
                   
                    foreach (ModelMesh mesh in model.Meshes)
                    {
                        //Conteggio
                        if (mustCountInfo) countInfo.CurrentMeshes = countInfo.CurrentMeshes + model.Meshes.Count;

                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            //Texture
                            effect.TextureEnabled = textureEnabled;

                            //Trasparenza
                            effect.Alpha = alpha;
                            if (effect.Alpha < 1.0f)
                            {
                                if (blendProperties == BlendMode.None)
                                {
                                    //Nessun blend (forzato dall'utente)
                                    Core.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable = false;
                                    Core.Graphics.GraphicsDevice.RenderState.SourceBlend = Blend.One;
                                    Core.Graphics.GraphicsDevice.RenderState.DestinationBlend = Blend.Zero;
                                    Core.Graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
                                }
                                else if (blendProperties == BlendMode.AlphaBlend)
                                {
                                    //Blend normale (dipende dal canale alpha della texture, se il canale non c'è la trasparenza non viene applicata)
                                    Core.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable = true;
                                    Core.Graphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                                    Core.Graphics.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
                                    Core.Graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = false;
                                }
                                else if (blendProperties == BlendMode.Additive)
                                {
                                    //Blend additivo (ottimo per effetti grafici)
                                    Core.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable = true;
                                    Core.Graphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
                                    Core.Graphics.GraphicsDevice.RenderState.DestinationBlend = Blend.One;
                                    Core.Graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = false;
                                }
                            }
                            else
                            {
                                //Nessun blend
                                Core.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable = false;
                                Core.Graphics.GraphicsDevice.RenderState.SourceBlend = Blend.One;
                                Core.Graphics.GraphicsDevice.RenderState.DestinationBlend = Blend.Zero;
                                Core.Graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
                            }


                            //Colori
                            effect.VertexColorEnabled = vertexColorEnabled;
                            effect.DiffuseColor = diffuseColor.ToVector3();
                            effect.SpecularColor = specularColor.ToVector3();
                            effect.EmissiveColor = emissiveColor.ToVector3();
                            effect.SpecularPower = specularPower;

                            //Luci
                            effect.LightingEnabled = lightingEnabled;
                            if (effect.LightingEnabled)
                            {
                                effect.AmbientLightColor = base.ParentScene.SceneLightEffect.AmbientColor.ToVector3();
                                effect.DirectionalLight0.DiffuseColor = base.ParentScene.SceneLightEffect.DirectionalLight0.DiffuseColor.ToVector3();
                                effect.DirectionalLight0.Direction = base.ParentScene.SceneLightEffect.DirectionalLight0.Direction;
                                effect.DirectionalLight0.SpecularColor = base.ParentScene.SceneLightEffect.DirectionalLight0.SpecularColor.ToVector3();
                                effect.DirectionalLight0.Enabled = base.ParentScene.SceneLightEffect.DirectionalLight0.IsEnabled;
                                effect.DirectionalLight1.DiffuseColor = base.ParentScene.SceneLightEffect.DirectionalLight1.DiffuseColor.ToVector3();
                                effect.DirectionalLight1.Direction = base.ParentScene.SceneLightEffect.DirectionalLight1.Direction;
                                effect.DirectionalLight1.SpecularColor = base.ParentScene.SceneLightEffect.DirectionalLight1.SpecularColor.ToVector3();
                                effect.DirectionalLight1.Enabled = base.ParentScene.SceneLightEffect.DirectionalLight1.IsEnabled;
                                effect.DirectionalLight2.DiffuseColor = base.ParentScene.SceneLightEffect.DirectionalLight2.DiffuseColor.ToVector3();
                                effect.DirectionalLight2.Direction = base.ParentScene.SceneLightEffect.DirectionalLight2.Direction;
                                effect.DirectionalLight2.SpecularColor = base.ParentScene.SceneLightEffect.DirectionalLight2.SpecularColor.ToVector3();
                                effect.DirectionalLight2.Enabled = base.ParentScene.SceneLightEffect.DirectionalLight2.IsEnabled;
                            }

                            //Nebbia
                            if (fogEnabled)
                            {
                                effect.FogColor = base.ParentScene.SceneLightEffect.Fog.Color.ToVector3();
                                effect.FogEnabled = base.ParentScene.SceneLightEffect.Fog.Enable;
                                effect.FogStart = base.ParentScene.SceneLightEffect.Fog.Start;
                                effect.FogEnd = base.ParentScene.SceneLightEffect.Fog.End;
                            }

                            //Matrici
                            effect.View = viewMatrix;
                            effect.Projection = projMatrix;
                            effect.World = boneTransforms[mesh.ParentBone.Index] * this.worldMatrix;
                        }

                        //Conteggio
                        if (mustCountInfo)
                        {
                            for (int p = 0; p < mesh.MeshParts.Count; p++)
                            {
                                countInfo.CurrentPrimitives = countInfo.CurrentPrimitives + mesh.MeshParts[p].PrimitiveCount;
                                countInfo.CurrentVerticies = countInfo.CurrentVerticies + mesh.MeshParts[p].NumVertices;
                            }
                        }

                        mesh.Draw(SaveStateMode.SaveState);
                    }

                    //Info totali disegnate
                    if (mustCountInfo)
                    {
                        Core.currentVertices = Core.currentVertices + countInfo.CurrentVerticies;
                        Core.currentPrimitives = Core.currentPrimitives + countInfo.CurrentPrimitives;
                        Core.currentMeshes = Core.currentMeshes + countInfo.CurrentMeshes;
                        Core.currentModels = Core.currentModels + countInfo.CurrentModels;
                    }
                }
            }
            base.Draw();
        }


        ~SimpleModel()
        {
            //Rilascia le risorse
        }
    }
}
