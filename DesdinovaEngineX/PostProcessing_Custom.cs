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
    public class PostProcessing_BlackAndWhite : PostProcessing
    {
        public PostProcessing_BlackAndWhite(Scene parentScene)
            : base("Content\\Effect\\PostProcessing\\BlackAndWhite", parentScene)
        {
        }
    }


    public class PostProcessing_Bloom : PostProcessing
    {
        private float bloomScale = 1.5f;
        public float BloomScale
        {
            get { return bloomScale; }
            set
            {
                bloomScale = value;
                scaleEP.SetValue(bloomScale);
            }
        }

        private readonly EffectParameter scaleEP;

        public PostProcessing_Bloom(float bloomScale, Scene parentScene)
            : base("Content\\Effect\\PostProcessing\\Bloom", parentScene)
        {
            scaleEP = base.PostprocessEffect.Parameters["bloomScale"];
            scaleEP.SetValue(bloomScale);
        }
    }


    public class PostProcessing_Blur : PostProcessing
    {
        private Vector2 texelSize = new Vector2(0.0015f, 0.0015f);
        public Vector2 TexelSize
        {
            get { return texelSize; }
            set
            {
                texelSize = value;
                texelEP.SetValue(texelSize);
            }
        }

        private readonly EffectParameter texelEP;

        public PostProcessing_Blur(Vector2 texelSize, Scene parentScene)
            : base("Content\\Effect\\PostProcessing\\Blur", parentScene)
        {
            texelEP = base.PostprocessEffect.Parameters["blurTexelsize"];
            texelEP.SetValue(texelSize);
        }
    }


    public class PostProcessing_Brightness : PostProcessing
    {
        private float brightnessValue = 0.5f;
        public float BrightnessValue
        {
            get { return brightnessValue; }
            set
            {
                brightnessValue = value;
                brightnessEP.SetValue(brightnessValue);
            }
        }

        private readonly EffectParameter brightnessEP;

        public PostProcessing_Brightness(float texelSize, Scene parentScene)
            : base("Content\\Effect\\PostProcessing\\Brightness", parentScene)
        {
            brightnessEP = base.PostprocessEffect.Parameters["brightnessValue"];
            brightnessEP.SetValue(texelSize);
        }
    }


    public class PostProcessing_Color : PostProcessing
    {
        private Vector3 colorValue = new Vector3(0.5f, 0.5f, 1.0f);
        public Vector3 ColorValue
        {
            get { return colorValue; }
            set
            {
                colorValue = value;
                colorEP.SetValue(colorValue);
            }
        }

        private readonly EffectParameter colorEP;

        public PostProcessing_Color(Vector3 colorValue, Scene parentScene)
            : base("Content\\Effect\\PostProcessing\\Color", parentScene)
        {
            colorEP = base.PostprocessEffect.Parameters["colorValue"];
            colorEP.SetValue(colorValue);
        }
    }


    public class PostProcessing_EdgeDetection : PostProcessing
    {
        private Vector2 texelSize = new Vector2(0.0015f, 0.0015f);
        public Vector2 TexelSize
        {
            get { return texelSize; }
            set
            {
                texelSize = value;
                texelEP.SetValue(texelSize);
            }
        }

        private readonly EffectParameter texelEP;

        public PostProcessing_EdgeDetection(Vector2 texelSize, Scene parentScene)
            : base("Content\\Effect\\PostProcessing\\EdgeDetection", parentScene)
        {
            texelEP = base.PostprocessEffect.Parameters["edgeTexelsize"];
            texelEP.SetValue(texelSize);
        }
    }


    public class PostProcessing_Emboss : PostProcessing
    {
        private Vector2 texelSize = new Vector2(0.0015f, 0.0015f);
        public Vector2 TexelSize
        {
            get { return texelSize; }
            set
            {
                texelSize = value;
                texelEP.SetValue(texelSize);
            }
        }

        private readonly EffectParameter texelEP;

        public PostProcessing_Emboss(Vector2 texelSize, Scene parentScene)
            : base("Content\\Effect\\PostProcessing\\Emboss", parentScene)
        {
            texelEP = base.PostprocessEffect.Parameters["embossTexelsize"];
            texelEP.SetValue(texelSize);
        }
    }


    public class PostProcessing_Glass : PostProcessing
    {
        private Texture2D normalTexture;
        public Texture2D NormalTexture
        {
            get { return normalTexture; }
            set
            {
                normalTexture = value;
                normalEP.SetValue(normalTexture);
            }
        }

        private readonly EffectParameter normalEP;

        public PostProcessing_Glass(Texture2D normalTexture, Scene parentScene)
            : base("Content\\Effect\\PostProcessing\\Glass", parentScene)
        {
            normalEP = base.PostprocessEffect.Parameters["normalTexture"];
            normalEP.SetValue(normalTexture);
        }
    }


    public class PostProcessing_Inverse : PostProcessing
    {
        public PostProcessing_Inverse(Scene parentScene)
            : base("Content\\Effect\\PostProcessing\\Inverse", parentScene)
        {
        }
    }


    public class PostProcessing_Pixelate : PostProcessing
    {

        private float pixelNumber = 40.0f;
        public float PixelNumber
        {
            get { return pixelNumber; }
            set
            {
                pixelNumber = value;
                pixelNumberEP.SetValue(pixelNumber);
            }
        }

        private float edgeWidth = 0.15f;
        public float EdgeWidth
        {
            get { return edgeWidth; }
            set
            {
                edgeWidth = value;
                edgeWidthEP.SetValue(edgeWidth);
            }
        }

        private Vector3 edgeColor = new Vector3(0.7f, 0.7f, 0.7f);
        public Vector3 ColorValue
        {
            get { return edgeColor; }
            set
            {
                edgeColor = value;
                edgeColorEP.SetValue(edgeColor);
            }
        }


        private readonly EffectParameter pixelNumberEP;
        private readonly EffectParameter edgeWidthEP;
        private readonly EffectParameter edgeColorEP;

        public PostProcessing_Pixelate(float pixelNumber, float edgeWidth, Color edgeColor, Scene parentScene)
            : base("Content\\Effect\\PostProcessing\\Pixelate", parentScene)
        {
            pixelNumberEP = base.PostprocessEffect.Parameters["NumberOfPixels"];
            pixelNumberEP.SetValue(pixelNumber);

            edgeWidthEP = base.PostprocessEffect.Parameters["EdgeWidth"];
            edgeWidthEP.SetValue(edgeWidth);

            edgeColorEP = base.PostprocessEffect.Parameters["EdgeColor"];
            edgeColorEP.SetValue(edgeColor.ToVector3());

        }
    }


    public class PostProcessing_Sepia : PostProcessing
    {
        public PostProcessing_Sepia(Scene parentScene)
            : base("Content\\Effect\\PostProcessing\\Sepia", parentScene)
        {
        }
    }


    public class PostProcessing_Sharpen : PostProcessing
    {
        private float sharpenValue = 0.0001f;
        public float SharpenValue
        {
            get { return sharpenValue; }
            set
            {
                sharpenValue = value;
                sharpenEP.SetValue(sharpenValue);
            }
        }

        private readonly EffectParameter sharpenEP;

        public PostProcessing_Sharpen(float sharpenValue, Scene parentScene)
            : base("Content\\Effect\\PostProcessing\\Sharpen", parentScene)
        {
            sharpenEP = base.PostprocessEffect.Parameters["sharpenValue"];
            sharpenEP.SetValue(sharpenValue);
        }
    }


    public class PostProcessing_Tonemap : PostProcessing
    {
        private float luminance = 0.09f;
        public float Luminance
        {
            get { return luminance; }
            set
            {
                luminance = value;
                luminanceEP.SetValue(luminance);
            }
        }

        private readonly EffectParameter luminanceEP;

        public PostProcessing_Tonemap(float luminance, Scene parentScene)
            : base("Content\\Effect\\PostProcessing\\Tonemap", parentScene)
        {
            luminanceEP = base.PostprocessEffect.Parameters["tonemapLuminance"];
            luminanceEP.SetValue(luminance);
        }
    }



    public class PostProcessing_Underwater : PostProcessing
    {
        public PostProcessing_Underwater(Scene parentScene)
            : base("Content\\Effect\\PostProcessing\\Underwater", parentScene)
        {
        }
    }
}
