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
    public class FontLine : EngineSceneObject
    {
        //Sprite principale
        private SpriteFont fontSprite = null;
        public SpriteFont FontSprite
        {
            get { return fontSprite; }
        }

        //Testo visualizzato
        protected string text = "No text";
        public virtual string Text
        {
            get { return text; }
            set { text = value; }
        }

        //Posizione
        private Vector2 position = Vector2.Zero;
        public Vector2 Position
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

        //Colore
        private Color color = Color.White;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        //Scalatura
        private float scale = 1.0f;
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        //Ombra
        private bool shadowEnabled = false;
        public bool ShadowEnabled
        {
            get { return shadowEnabled; }
            set { shadowEnabled = value; }
        }

        //Colore ombra
        private Color shadowColor = Color.Black;
        public Color ShadowColor
        {
            get { return shadowColor; }
            set { shadowColor = value; }
        }

        //Offset ombra
        private float shadowOffset = 1.0f;
        public float ShadowOffset
        {
            get { return shadowOffset; }
            set { shadowOffset = value; }
        }

        //Spazio tra i caratteri
	    public float CharactersSpacing
	    {
		    get { return this.fontSprite.Spacing;}
		    set { this.fontSprite.Spacing = value;}
	    }

        //Spazio tra le linee
        public int LineSpacing
        {
            get { return this.fontSprite.LineSpacing; }
        }

        //Larghezza del testo visualizzato
        public float Width
        {
            get { return this.fontSprite.MeasureString(this.text).X * this.scale;  }     
        }

        //Altezza del testo visualizzato
        public float Height
        {
            get { return this.fontSprite.MeasureString(this.text).Y * this.scale; }
        }

        public FontLine(string assetName, Scene parentScene):base(parentScene)
        {
            if (string.IsNullOrEmpty(assetName) == false)
            {
                try
                {
                    fontSprite = parentScene.SceneContent.Load<SpriteFont>(assetName);
                }
                catch
                {
                    fontSprite = parentScene.SceneContent.Load<SpriteFont>("Content\\Font\\VerdanaEngine");
                }
            }
            else
            {
                fontSprite = parentScene.SceneContent.Load<SpriteFont>("Content\\Font\\VerdanaEngine");
            }
        }
    }



    public class Font : Engine2DObject
    {
        //Batch
        private SpriteBatch fontBatch = null;
        private FontLine[] lines = null;

        public FontLine this[int id]
        {
            get
            {
                try
                {
                    return lines[id];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                try
                {
                    lines[id] = value;
                }
                catch
                { }
            }
        }

        public int LinesCount
        {
            get { return lines.Length; }
        }

        public Font(int totalLines, Scene sceneParent):base(sceneParent)
        {
            try
            {
                fontBatch = new SpriteBatch(Core.Graphics.GraphicsDevice);
                lines = new FontLine[totalLines];

                //Conto fonts totali
                Core.loadedFonts = Core.loadedFonts + 1;

                //Creato correttamente
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
            base.Update(gameTime);
        }

        public override void Draw()
        {
            if (IsCreated)
            {
                if (ToDraw)
                {
                    //Salvataggio RenderStates
                    CullMode old1 = Core.Graphics.GraphicsDevice.RenderState.CullMode;
                    bool old2 = Core.Graphics.GraphicsDevice.RenderState.DepthBufferEnable;
                    bool old3 = Core.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable;
                    bool old4 = Core.Graphics.GraphicsDevice.RenderState.AlphaTestEnable;
                    BlendFunction old5 = Core.Graphics.GraphicsDevice.RenderState.AlphaBlendOperation;
                    Blend old6 = Core.Graphics.GraphicsDevice.RenderState.SourceBlend;
                    Blend old7 = Core.Graphics.GraphicsDevice.RenderState.DestinationBlend;
                    bool old8 = Core.Graphics.GraphicsDevice.RenderState.SeparateAlphaBlendEnabled;
                    CompareFunction old9 = Core.Graphics.GraphicsDevice.RenderState.AlphaFunction;
                    int old10 = Core.Graphics.GraphicsDevice.RenderState.ReferenceAlpha;
                    TextureAddressMode old11 = Core.Graphics.GraphicsDevice.SamplerStates[0].AddressU;
                    TextureAddressMode old12 = Core.Graphics.GraphicsDevice.SamplerStates[0].AddressV;
                    TextureFilter old13 = Core.Graphics.GraphicsDevice.SamplerStates[0].MagFilter;
                    TextureFilter old14 = Core.Graphics.GraphicsDevice.SamplerStates[0].MinFilter;
                    TextureFilter old15 = Core.Graphics.GraphicsDevice.SamplerStates[0].MipFilter;
                    float old16 = Core.Graphics.GraphicsDevice.SamplerStates[0].MipMapLevelOfDetailBias;
                    int old17 = Core.Graphics.GraphicsDevice.SamplerStates[0].MaxMipLevel;//*/

                    //Conto totale
                    Core.currentFonts = Core.currentFonts + 1;

                    //Batch font
                    fontBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
                    {
                        for (int i = 0; i < lines.Length; i++)
                        {
                            //Ombra
                            if (lines[i] != null)
                            {
                                if (lines[i].ShadowEnabled)
                                {
                                    fontBatch.DrawString(lines[i].FontSprite, lines[i].Text, new Vector2(lines[i].PositionX + lines[i].ShadowOffset, lines[i].PositionY + lines[i].ShadowOffset), lines[i].ShadowColor, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0.0f);
                                }
                                fontBatch.DrawString(lines[i].FontSprite, lines[i].Text, new Vector2(lines[i].PositionX, lines[i].PositionY), lines[i].Color, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0.0f);
                            }
                        }
                    }
                    fontBatch.End();

                    //Reimposta i RenderStates che modifica lo spritebatch (vedere SpriteBatch.Begin Method() nell'MSDN)
                    Core.Graphics.GraphicsDevice.RenderState.CullMode = old1;
                    Core.Graphics.GraphicsDevice.RenderState.DepthBufferEnable = old2;
                    Core.Graphics.GraphicsDevice.RenderState.AlphaBlendEnable = old3;
                    Core.Graphics.GraphicsDevice.RenderState.AlphaTestEnable = old4;
                    Core.Graphics.GraphicsDevice.RenderState.AlphaBlendOperation = old5;
                    Core.Graphics.GraphicsDevice.RenderState.SourceBlend = old6;
                    Core.Graphics.GraphicsDevice.RenderState.DestinationBlend = old7;
                    Core.Graphics.GraphicsDevice.RenderState.SeparateAlphaBlendEnabled = old8;
                    Core.Graphics.GraphicsDevice.RenderState.AlphaFunction = old9;
                    Core.Graphics.GraphicsDevice.RenderState.ReferenceAlpha = old10;
                    Core.Graphics.GraphicsDevice.SamplerStates[0].AddressU = old11;
                    Core.Graphics.GraphicsDevice.SamplerStates[0].AddressV = old12;
                    Core.Graphics.GraphicsDevice.SamplerStates[0].MagFilter = old13;
                    Core.Graphics.GraphicsDevice.SamplerStates[0].MinFilter = old14;
                    Core.Graphics.GraphicsDevice.SamplerStates[0].MipFilter = old15;
                    Core.Graphics.GraphicsDevice.SamplerStates[0].MipMapLevelOfDetailBias = old16;
                    Core.Graphics.GraphicsDevice.SamplerStates[0].MaxMipLevel = old17;
                }
            }
            base.Draw();
        }


        ~Font()
        {
            //Conto fonts totali
            Core.loadedFonts = Core.loadedFonts - 1;

            //Rilascia le risorse
            //...
        }
    }
}



