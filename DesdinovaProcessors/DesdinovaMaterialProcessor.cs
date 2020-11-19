#region File Description
//-----------------------------------------------------------------------------
// EnvironmentMappedMaterialProcessor.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System.IO;
using System.ComponentModel;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace DesdinovaProcessors
{
    /// <summary>
    /// Custom content pipeline processor derives from the built-in
    /// MaterialProcessor. This changes the material to use our custom
    /// environment mapping effect, and also builds the environment map
    /// texture in a special way.
    /// </summary>
    [ContentProcessor]
    public class DesdinovaMaterialProcessor : MaterialProcessor
    {
        /// <summary>
        /// Converts a material.
        /// </summary>
        public override MaterialContent Process(MaterialContent input,
                                                ContentProcessorContext context)
        {
            // Create a new effect material.
            EffectMaterialContent customMaterial = new EffectMaterialContent();

            // Point the new material at our custom effect file.
            string effectFile = Path.GetFullPath("Effect\\MainLighting.fx");

            customMaterial.Effect = new ExternalReference<EffectContent>(effectFile);

            //Vedere http://blogs.msdn.com/shawnhar/archive/2008/09/18/fbx-improvements-in-xna-game-studio-3-0.aspx

            //Diffuse texture
            ExternalReference<TextureContent> reference = null;
            input.Textures.TryGetValue("Texture", out reference);
            if (reference != null)
            {
                customMaterial.Textures.Add("colorMapTexture", reference);
            }

            //Bump texture
            ExternalReference<TextureContent> referenceBump = null;
            input.Textures.TryGetValue("Bump0", out referenceBump);
            if (referenceBump != null)
            {
                customMaterial.Textures.Add("normalMapTexture", referenceBump);
            }

            //Diffuse color
            try
            {
                customMaterial.OpaqueData.Add("materialDiffuse", (Vector3)input.OpaqueData["DiffuseColor"]);
            }
            catch
            {
                customMaterial.OpaqueData.Add("materialDiffuse", new Vector3(1.0f, 1.0f, 1.0f));
            }

            //Emissive color
            try
            {
                customMaterial.OpaqueData.Add("materialEmissive", (Vector3)input.OpaqueData["EmissiveColor"]);
            }
            catch
            {
                customMaterial.OpaqueData.Add("materialEmissive", new Vector3(0.0f, 0.0f, 0.0f));
            }

            

            //Specular color
            try
            {
                customMaterial.OpaqueData.Add("materialSpecular", (Vector3)input.OpaqueData["SpecularColor"]);
            }
            catch
            {
                customMaterial.OpaqueData.Add("materialSpecular", new Vector3(0.9f, 0.9f, 0.9f));
            }

            //Alpha
            try
            {
                customMaterial.OpaqueData.Add("materialAlpha", (float)input.OpaqueData["Alpha"]);
            }
            catch
            {
                customMaterial.OpaqueData.Add("materialAlpha", 1.0f);
            }

            //Alpha
            try
            {
                customMaterial.OpaqueData.Add("materialShininess", (float)input.OpaqueData["SpecularPower"]);
            }
            catch
            {
                customMaterial.OpaqueData.Add("materialShininess", 10.0f);
            }

            //customMaterial.OpaqueData.Add("materialReflection", 0.0f);//  ???????????????????????*/

            //System.Diagnostics.Debugger.Launch();

            // Chain to the base material processor.
            return base.Process(customMaterial, context);
        }


        /// <summary>
        /// Builds a texture for use by this material.
        /// </summary>
        protected override ExternalReference<TextureContent> BuildTexture(
                                            string textureName,
                                            ExternalReference<TextureContent> texture,
                                            ContentProcessorContext context)
        {

            // Apply default processing to all other textures.
            return base.BuildTexture(textureName, texture, context);
        }
    }
}
