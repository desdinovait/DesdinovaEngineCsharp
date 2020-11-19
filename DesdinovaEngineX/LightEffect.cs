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
    public class LightEffect : EngineObject
    {
        private readonly EffectParameter material_Emissive;
        private readonly EffectParameter material_Diffuse;
        private readonly EffectParameter material_Specular;
        private readonly EffectParameter material_Shininess;
        private readonly EffectParameter material_Alpha;
        private readonly EffectParameter material_Reflection;
        private readonly EffectParameter material_DiffuseMapEnabled;
        private readonly EffectParameter material_NormalMapEnabled;

        private EffectParameter matrix_World;
        private EffectParameter matrix_View;
        private EffectParameter matrix_Projection;
        private readonly EffectParameter matrix_WorldInverseTranspose;
        private readonly EffectParameter matrix_WorldViewProjection;

        private readonly EffectParameter ambient_Global;
        private readonly EffectParameter camera_Position;
        private readonly EffectParameter texture_Color;
        private readonly EffectParameter texture_Environment;
        private readonly EffectParameter texture_Normal;
        private readonly EffectParameter dynamic_LightsUse;

        private readonly EffectParameter fog_Color;
        private readonly EffectParameter fog_Start;
        private readonly EffectParameter fog_End;
        private readonly EffectParameter fog_Altitude;
        private readonly EffectParameter fog_Thinning;
        private readonly EffectParameter fog_Enabled;

        private readonly EffectParameter[] dirLight_Direction;
        private readonly EffectParameter[] dirLight_Diffuse;
        private readonly EffectParameter[] dirLight_Specular;
        private readonly EffectParameter[] dirLight_Enabled;

        private readonly EffectParameter[] pointLight_Position;
        private readonly EffectParameter[] pointLight_Diffuse;
        private readonly EffectParameter[] pointLight_Specular;
        private readonly EffectParameter[] pointLight_Radius;
        private readonly EffectParameter[] pointLight_Enabled;

        private FogParameters fog;
        public FogParameters Fog
        {
            get { return fog; }
            set 
            { 
                fog = value;                
                fog_Color.SetValue(fog.Color.ToVector4());
                fog_Start.SetValue(fog.Start);
                fog_End.SetValue(fog.End);
                fog_Altitude.SetValue(fog.Altitude);
                fog_Thinning.SetValue(fog.Thinning);
                fog_Enabled.SetValue(fog.Enable);
            }
        }

        private Effect effect = null;
        internal Effect Effect
        {
            get { return effect; }
        }

        public Effect DrawEffect
        {
            get { return effect; }
        }
	
        public Color AmbientColor
        {
            get { return new Color(); }
            set 
            { 
                ambient_Global.SetValue(value.ToVector4());
            }
        }

        private Matrix currentWorldMatrix = Matrix.Identity;
        internal Matrix CurrentWorldMatrix
        {
            get { return currentWorldMatrix; }
            set
            {
                currentWorldMatrix = value;
                matrix_World.SetValue(currentWorldMatrix);
                matrix_WorldInverseTranspose.SetValue(Matrix.Transpose(Matrix.Invert(currentWorldMatrix)));
            }
        }

        private Material currentMaterial;
        internal Material CurrentMaterial
        {
            get { return currentMaterial; }
            set 
            { 
                currentMaterial = value;
                material_Diffuse.SetValue(currentMaterial.DiffuseColor.ToVector3());
                material_Emissive.SetValue(currentMaterial.EmissiveColor.ToVector3());
                material_Specular.SetValue(currentMaterial.SpecularColor.ToVector3());
                material_Shininess.SetValue(currentMaterial.Shininess);
                material_Alpha.SetValue(currentMaterial.Alpha);
                material_Reflection.SetValue(currentMaterial.Reflection);
                material_DiffuseMapEnabled.SetValue(currentMaterial.DiffuseMapEnabled);
                material_NormalMapEnabled.SetValue(currentMaterial.NormalMapEnabled);

                texture_Color.SetValue(currentMaterial.DiffuseTexture);
                texture_Normal.SetValue(currentMaterial.NormalTexture);
            }
        }

        private TextureCube environmentMap;
        public TextureCube EnvironmentMap
        {
            get { return environmentMap; }
            set 
            {
                environmentMap = value;
                texture_Environment.SetValue(environmentMap);
            }
        }
	

        private DirectionalLightParameters directionaLight0;
        public DirectionalLightParameters DirectionalLight0
        {
            get { return directionaLight0; }
            set
            {
                directionaLight0 = value;
                dirLight_Direction[0].SetValue(directionaLight0.Direction);
                dirLight_Diffuse[0].SetValue(directionaLight0.DiffuseColor.ToVector4());
                dirLight_Specular[0].SetValue(directionaLight0.SpecularColor.ToVector4());
                dirLight_Enabled[0].SetValue(directionaLight0.IsEnabled);
            }
        }

        private DirectionalLightParameters directionaLight1;
        public DirectionalLightParameters DirectionalLight1
        {
            get { return directionaLight1; }
            set
            {
                directionaLight1 = value;
                dirLight_Direction[1].SetValue(directionaLight1.Direction);
                dirLight_Diffuse[1].SetValue(directionaLight1.DiffuseColor.ToVector4());
                dirLight_Specular[1].SetValue(directionaLight1.SpecularColor.ToVector4());
                dirLight_Enabled[1].SetValue(directionaLight1.IsEnabled);
            }
        }

        private DirectionalLightParameters directionaLight2;
        public DirectionalLightParameters DirectionalLight2
        {
            get { return directionaLight2; }
            set
            {
                directionaLight2 = value;
                dirLight_Direction[2].SetValue(directionaLight2.Direction);
                dirLight_Diffuse[2].SetValue(directionaLight2.DiffuseColor.ToVector4());
                dirLight_Specular[2].SetValue(directionaLight2.SpecularColor.ToVector4());
                dirLight_Enabled[2].SetValue(directionaLight2.IsEnabled);
            }
        }

        private PointLightParameters[] pointLights = new PointLightParameters[10];
        public PointLightParameters[] PointLights
        {
            get { return pointLights; }
        }
	
        private Plane shadowPlane;
        public Plane ShadowPlane
        {
            get { return shadowPlane; }
            set { shadowPlane = value; }
        }

        private bool dynamicLightsCapable = false;
        public bool DynamicLightsCapable
        {
            get { return dynamicLightsCapable; }
        }
     
        public LightEffect(bool useDynamicLightsIfCapable, Scene parentScene )
        {
            try
            {
                effect = parentScene.SceneContent.Load<Effect>("Content\\Effect\\MainLighting");

                //Check capcità shader
                GraphicsDeviceCapabilities caps = Core.Graphics.GraphicsDevice.GraphicsDeviceCapabilities;
                if (caps.MaxPixelShaderProfile < ShaderProfile.PS_3_0)
                {
                    effect.CurrentTechnique = effect.Techniques["Lighting20"];

                    //Carica la tecnica shader 2.0
                    dynamicLightsCapable = false;
                }
                else
                {
                    //Carica la tecnica shader 3.0
                    effect.CurrentTechnique = effect.Techniques["Lighting30Bump"];

                    //Definisce se usare le luci dinamico o meno
                    //Si potrebbe caricare ancora lo shader del 2.0 ma sembra che compilato per 2.0 lo shader vada più lento (20fps su 300fps circa)
                    dynamicLightsCapable = useDynamicLightsIfCapable;
                    dynamic_LightsUse = effect.Parameters["useDynamicLights"];
                    dynamic_LightsUse.SetValue(useDynamicLightsIfCapable);
                }//*/

                //Piano per ombre
                shadowPlane = new Plane(Vector3.Up, 0);

                //Materiale
                material_Diffuse = effect.Parameters["materialDiffuse"];
                material_Emissive = effect.Parameters["materialEmissive"];
                material_Specular = effect.Parameters["materialSpecular"];
                material_Shininess = effect.Parameters["materialShininess"];
                material_Alpha = effect.Parameters["materialAlpha"];
                material_Reflection = effect.Parameters["materialReflection"];
                material_DiffuseMapEnabled = effect.Parameters["materialDiffuseMapEnabled"];
                material_NormalMapEnabled = effect.Parameters["materialNormalMapEnabled"];

                //Matrici
                matrix_World = effect.Parameters["worldMatrix"];
                matrix_View = effect.Parameters["viewMatrix"];
                matrix_Projection = effect.Parameters["projectionMatrix"];
                matrix_WorldInverseTranspose = effect.Parameters["worldInverseTransposeMatrix"];
                matrix_WorldViewProjection = effect.Parameters["worldViewProjectionMatrix"];

                //Altro
                ambient_Global = effect.Parameters["globalAmbient"];
                camera_Position = effect.Parameters["cameraPos"];
                texture_Color = effect.Parameters["colorMapTexture"];
                texture_Environment = effect.Parameters["environmentMapTexture"];
                texture_Normal = effect.Parameters["normalMapTexture"];

                //Nebbia
                fog_Color = effect.Parameters["fog"].StructureMembers["color"];
                fog_Start = effect.Parameters["fog"].StructureMembers["start"];
                fog_End = effect.Parameters["fog"].StructureMembers["end"];
                fog_Altitude = effect.Parameters["fog"].StructureMembers["altitude"];
                fog_Thinning = effect.Parameters["fog"].StructureMembers["thinning"];
                fog_Enabled = effect.Parameters["fog"].StructureMembers["enabled"];

                //Luci direzionali
                dirLight_Direction = new EffectParameter[3];
                dirLight_Diffuse = new EffectParameter[3];
                dirLight_Specular = new EffectParameter[3];
                dirLight_Enabled = new EffectParameter[3];
                dirLight_Direction[0] = effect.Parameters["dirLight"].Elements[0].StructureMembers["dir"];
                dirLight_Diffuse[0] = effect.Parameters["dirLight"].Elements[0].StructureMembers["diffuse"];
                dirLight_Specular[0] = effect.Parameters["dirLight"].Elements[0].StructureMembers["specular"];
                dirLight_Enabled[0] = effect.Parameters["dirLight"].Elements[0].StructureMembers["enabled"];
                dirLight_Direction[1] = effect.Parameters["dirLight"].Elements[1].StructureMembers["dir"];
                dirLight_Diffuse[1] = effect.Parameters["dirLight"].Elements[1].StructureMembers["diffuse"];
                dirLight_Specular[1] = effect.Parameters["dirLight"].Elements[1].StructureMembers["specular"];
                dirLight_Enabled[1] = effect.Parameters["dirLight"].Elements[1].StructureMembers["enabled"];
                dirLight_Direction[2] = effect.Parameters["dirLight"].Elements[2].StructureMembers["dir"];
                dirLight_Diffuse[2] = effect.Parameters["dirLight"].Elements[2].StructureMembers["diffuse"];
                dirLight_Specular[2] = effect.Parameters["dirLight"].Elements[2].StructureMembers["specular"];
                dirLight_Enabled[2] = effect.Parameters["dirLight"].Elements[2].StructureMembers["enabled"];

                //Luci dinamiche point
                if (dynamicLightsCapable)
                {
                    pointLight_Position = new EffectParameter[4];
                    pointLight_Diffuse = new EffectParameter[4];
                    pointLight_Specular = new EffectParameter[4];
                    pointLight_Radius = new EffectParameter[4];
                    pointLight_Enabled = new EffectParameter[4];
                    pointLight_Position[0] = effect.Parameters["pointLight"].Elements[0].StructureMembers["pos"];
                    pointLight_Diffuse[0] = effect.Parameters["pointLight"].Elements[0].StructureMembers["diffuse"];
                    pointLight_Specular[0] = effect.Parameters["pointLight"].Elements[0].StructureMembers["specular"];
                    pointLight_Radius[0] = effect.Parameters["pointLight"].Elements[0].StructureMembers["radius"];
                    pointLight_Enabled[0] = effect.Parameters["pointLight"].Elements[0].StructureMembers["enabled"];
                    pointLight_Position[1] = effect.Parameters["pointLight"].Elements[1].StructureMembers["pos"];
                    pointLight_Diffuse[1] = effect.Parameters["pointLight"].Elements[1].StructureMembers["diffuse"];
                    pointLight_Specular[1] = effect.Parameters["pointLight"].Elements[1].StructureMembers["specular"];
                    pointLight_Radius[1] = effect.Parameters["pointLight"].Elements[1].StructureMembers["radius"];
                    pointLight_Enabled[1] = effect.Parameters["pointLight"].Elements[1].StructureMembers["enabled"];
                    pointLight_Position[2] = effect.Parameters["pointLight"].Elements[2].StructureMembers["pos"];
                    pointLight_Diffuse[2] = effect.Parameters["pointLight"].Elements[2].StructureMembers["diffuse"];
                    pointLight_Specular[2] = effect.Parameters["pointLight"].Elements[2].StructureMembers["specular"];
                    pointLight_Radius[2] = effect.Parameters["pointLight"].Elements[2].StructureMembers["radius"];
                    pointLight_Enabled[2] = effect.Parameters["pointLight"].Elements[2].StructureMembers["enabled"];
                    pointLight_Position[3] = effect.Parameters["pointLight"].Elements[3].StructureMembers["pos"];
                    pointLight_Diffuse[3] = effect.Parameters["pointLight"].Elements[3].StructureMembers["diffuse"];
                    pointLight_Specular[3] = effect.Parameters["pointLight"].Elements[3].StructureMembers["specular"];
                    pointLight_Radius[3] = effect.Parameters["pointLight"].Elements[3].StructureMembers["radius"];
                    pointLight_Enabled[3] = effect.Parameters["pointLight"].Elements[3].StructureMembers["enabled"];
                }

                IsCreated = true;
            }
            catch
            {
                IsCreated = false;
            }
            
        }



        public void Update(GameTime gameTime, Camera camera)
        {
            if (camera != null)
            {
                camera_Position.SetValue(camera.Position);
                matrix_View.SetValue(camera.ViewMatrix);
                matrix_Projection.SetValue(camera.ProjectionMatrix);
            }

            for (int i = 0; i < pointLights.Length; i++)
            {
                pointLights[i].boundingSphere = new BoundingSphere(pointLights[i].Position, pointLights[i].Radius);
                if (camera.Frustrum.Contains(pointLights[i].boundingSphere) == ContainmentType.Disjoint)
                {
                    pointLights[i].inFrustrum = false;
                }
                else
                {
                    pointLights[i].inFrustrum = true;
                }
            }
        }

        /*public override void Draw(LightEffect lightEffect)
        {
            if (dynamicLightsCapable)
            {
                for (int i = 0; i < pointLights.Length; i++)
                {
                    if ((pointLights[i].inFrustrum)&&(pointLights[i].IsEnabled))
                    {
                        boundingRenderer.Color = pointLights[i].DiffuseColor;
                        boundingRenderer.ReferenceSphere = pointLights[i].boundingSphere;
                        boundingRenderer.Draw(this);

                        boundingRenderer.Color = pointLights[i].DiffuseColor;
                        boundingRenderer.ReferenceSphere = new BoundingSphere(pointLights[i].boundingSphere.Center, 5.0f);
                        boundingRenderer.Draw(this);
                    }
                }
            }
            base.Draw(this);
        }*/

        internal void CheckPointLight(BoundingSphere currentBoundingSphere)
        {
            if (dynamicLightsCapable)
            {
                //Spegne tutte le luci dinamiche dello shader (evita problemi di illuminazione parziale)
                pointLight_Enabled[0].SetValue(false);
                pointLight_Enabled[1].SetValue(false);
                pointLight_Enabled[2].SetValue(false);
                pointLight_Enabled[3].SetValue(false);

                int currentID = 0;
                for (int i = 0; i < pointLights.Length; i++)
                {
                    if (pointLights[i].inFrustrum)
                    {                    
                        if (pointLights[i].IsEnabled)
                        {
                            if (currentBoundingSphere.Contains(pointLights[i].boundingSphere) == ContainmentType.Intersects)
                            {
                                pointLight_Position[currentID].SetValue(pointLights[i].Position);
                                pointLight_Diffuse[currentID].SetValue(pointLights[i].DiffuseColor.ToVector4());
                                pointLight_Specular[currentID].SetValue(pointLights[i].SpecularColor.ToVector4());
                                pointLight_Radius[currentID].SetValue(pointLights[i].Radius);
                                pointLight_Enabled[currentID].SetValue(pointLights[i].IsEnabled);
                                currentID = currentID + 1;

                                //Non ci possono essere più di 4 luci a colpire il modello
                                if (currentID >= 4)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

            }//*/
        }
    }
}
