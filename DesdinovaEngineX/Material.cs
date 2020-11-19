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
    public class Material : EngineObject
    {
        //Diffuse Texture
        private Texture2D diffuseTexture = null;
        public Texture2D DiffuseTexture
        {
            get { return diffuseTexture; }
            set { diffuseTexture = value; }
        }

        //Diffuse Texture
        private Texture2D normalTexture = null;
        public Texture2D NormalTexture
        {
            get { return normalTexture; }
            set { normalTexture = value; }
        }

        //Texture enabled
        private bool diffuseMapEnabled;
        public bool DiffuseMapEnabled
        {
            get { return diffuseMapEnabled; }
            set { diffuseMapEnabled = value; }
        }
	
        //Nomal abilitato
        private bool normalMapEnabled;
        public bool NormalMapEnabled
        {
            get { return normalMapEnabled; }
            set { normalMapEnabled = value; }
        }

        //Alpha
        private float alpha = 1.0f;
        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        //Colori
        private Color emissiveColor = Color.White;
        public Color EmissiveColor
        {
            get { return emissiveColor; }
            set { emissiveColor = value; }
        }

        private Color specularColor = Color.White;
        public Color SpecularColor
        {
            get { return specularColor; }
            set { specularColor = value; }
        }
        
        private Color diffuseColor = Color.White;
        public Color DiffuseColor
        {
            get { return diffuseColor; }
            set { diffuseColor = value; }
        }

        //Lucentezza
        private float shininess = 10.0f;
        public float Shininess
        {
            get { return shininess; }
            set { shininess = value; }
        }

        //Lucentezza
        private float reflection = 0.0f;    //!!!!!!!!!!!!!!!!!!!!!!
        public float Reflection
        {
            get { return reflection; }
            set { reflection = value; }
        }

        public Material()
        {
            try
            {
                //Texture vuota (se il materiale non ha defintio la texture diffusa)
                diffuseTexture = Core.NullTextureColor;

                //Creazione avvenuta
                IsCreated = true;
            }
            catch
            {
                //Creazione fallita
                IsCreated = false;
            }
        }

        public void MaterialFromMayaEffect(Effect sourceEffect, float opaqueReflection)
        {
            try
            {
                try
                {
                    this.diffuseTexture = sourceEffect.Parameters["colorMapTexture"].GetValueTexture2D();
                    if (this.diffuseTexture != null)
                    {
                        this.diffuseMapEnabled = true;
                    }
                    else
                    {
                        this.diffuseMapEnabled = false;
                    }
                }
                catch
                {
                    this.diffuseTexture = null;
                    this.diffuseMapEnabled = false;
                }

                try
                {
                    this.normalTexture = sourceEffect.Parameters["normalMapTexture"].GetValueTexture2D();
                    if (this.normalTexture != null)
                    {
                        this.normalMapEnabled = true;
                    }
                    else
                    {
                        this.normalMapEnabled = false;
                    }
                }
                catch
                {
                    this.normalTexture = null;
                    this.normalMapEnabled = false;
                }

                this.diffuseColor = new Color(sourceEffect.Parameters["materialDiffuse"].GetValueVector3());
                this.emissiveColor = new Color(sourceEffect.Parameters["materialEmissive"].GetValueVector3());
                this.SpecularColor = new Color(sourceEffect.Parameters["materialSpecular"].GetValueVector3());
                this.shininess = sourceEffect.Parameters["materialShininess"].GetValueSingle();
                this.alpha = sourceEffect.Parameters["materialAlpha"].GetValueSingle();
                this.reflection = opaqueReflection; //Viene prelevata esternamente dagli opaqueData
            }
            catch
            {
                this.MaterialFromMayaBasicEffect((BasicEffect)sourceEffect);
            }
        }

        public void MaterialFromMayaBasicEffect(BasicEffect sourceEffect)
        {
            //Da effetto
            if (sourceEffect.Texture == null)
            {
                this.diffuseTexture = null;
                this.diffuseMapEnabled = false;
            }
            else
            {
                this.diffuseTexture = sourceEffect.Texture;
                this.diffuseMapEnabled = true;
            }
            this.diffuseColor = new Color(sourceEffect.DiffuseColor);
            this.specularColor = new Color(sourceEffect.SpecularColor);
            this.emissiveColor = new Color(sourceEffect.EmissiveColor);
            this.shininess = sourceEffect.SpecularPower;
            this.alpha = (sourceEffect.Alpha + 2.0f) / 3.0f; //In Maya l'alpha va da 1(opaco) e -2(trasparente), qui lo converte da 0 a 1
            this.normalTexture = null;
            this.normalMapEnabled = false;
            this.reflection = 0.0f;
        }

        ~Material()
        {
            //if (diffuseTexture != null) diffuseTexture.Dispose();
        }
    }
}
