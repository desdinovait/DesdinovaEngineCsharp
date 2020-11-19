#region File Description
//-----------------------------------------------------------------------------
// EnvironmentMappedModelProcessor.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System.ComponentModel;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
#endregion

namespace DesdinovaProcessors
{
    /// <summary>
    /// Custom content pipeline processor derives from the built-in
    /// ModelProcessor, extending it to apply an environment mapping
    /// effect to the model as part of the build process.
    /// </summary>
    [ContentProcessor(DisplayName = "Model - Desdinova Engine Model Processor")]
    public class DesdinovaModelProcessor : ModelProcessor
    {
        Dictionary<string, Dictionary<string,object>> opaqueDictionary;

        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            opaqueDictionary = new Dictionary<string,  Dictionary<string, object>>();


            LookUpOpaqueData(input);


            ModelContent mc = base.Process(input, context);
            //System.Diagnostics.Debugger.Launch();

            for (int i = 0; i < mc.Meshes.Count; i++)
            {
                Dictionary<string, object> outDict = null;

                opaqueDictionary.TryGetValue(mc.Meshes[i].Name, out outDict);

                if (outDict != null)
                {
                    mc.Meshes[i].Tag = outDict as Dictionary<string, object>;
                }
            }

            return mc;
        }

        private void LookUpOpaqueData(NodeContent node)
        {
            MeshContent mesh = node as MeshContent;
            if (mesh != null)
            {
                Dictionary<string, object> tempDictionary = new Dictionary<string, object>();
                foreach (string key in mesh.OpaqueData.Keys)
                {
                    tempDictionary.Add(key, mesh.OpaqueData.GetValue<object>(key, null));
                }
                opaqueDictionary.Add(mesh.Name, tempDictionary);
            }

            foreach (NodeContent nodeContent in node.Children)
            {
                LookUpOpaqueData(nodeContent);
            }




            /*MeshContent mesh = node as MeshContent;
            if (mesh != null)
            {
                foreach (NodeContent child in node.Children)
                {
                    Dictionary<string,object> tempDictionary = new Dictionary<string, object>();
                    foreach (string a in child.OpaqueData.Keys)
                    {
                        tempDictionary.Add(a, child.OpaqueData.GetValue<object>(a, null));
                    }
                    opaqueDictionary.Add(child.Name.ToString(), tempDictionary);
                }
            }

            foreach (NodeContent child in node.Children)
            {
                LookUpOpaqueData(child);
            }*/
        }

        /// <summary>
        /// Use our custom EnvironmentMappedMaterialProcessor
        /// to convert all the materials on this model.
        /// </summary>
        protected override MaterialContent ConvertMaterial(MaterialContent material, ContentProcessorContext context)
        {
            //OpaqueDataDictionary processorParameters = new OpaqueDataDictionary();
            //processorParameters.Add("EnvironmentMap", EnvironmentMap);
            //return context.Convert<MaterialContent, MaterialContent>(material, "DesdinovaMaterialProcessor", processorParameters);

            return context.Convert<MaterialContent, MaterialContent>(material, "DesdinovaMaterialProcessor", null);
        }
    }
}
