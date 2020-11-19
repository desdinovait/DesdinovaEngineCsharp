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
    public class SpriteBatcher : Engine2DObject
    {
        //Lista degli elementi 2D da disegnare
        private SpriteBatch batcher2D;
        private List<Sprite> batcher2DList;

        //Capienza
        public int Count
        {
            get { return batcher2DList.Count; }
        }


        public SpriteBatcher(Scene parentScene):base(parentScene)
	    {
            try
            {
                //Liste
                batcher2D = new SpriteBatch(Core.Graphics.GraphicsDevice);
                batcher2DList = new List<Sprite>();

                //Creazione avvenuta
                IsCreated = true;
            }
            catch
            {
                //Creazione fallita
                IsCreated = false;
            }
        }

        public int Add(Sprite newSprite)
        {
            if (IsCreated)
            {
                batcher2DList.Add(newSprite);
                this.ParentScene.AddObject(newSprite);
                return batcher2DList.Count - 1;
            }
            else
            {
                return 0;
            }
        }

        public bool Remove(Sprite spriteID)
        {
            if (IsCreated)
            {
                batcher2DList.Remove(spriteID);
                this.ParentScene.RemoveObject(spriteID);
                return true;
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
        
        private SpriteBlendMode blendMode;
        public SpriteBlendMode BlendMode
        {
            get { return blendMode;}
            set { blendMode = value;}
        }

        private SpriteSortMode sortMode;
        public SpriteSortMode SortMode
        {
            get { return sortMode;}
            set { sortMode = value;}
        }

        private Matrix transformMatrix;
        public Matrix TransformMatrix
        {
            get { return transformMatrix;}
            set { transformMatrix = value;}
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

                    //Disegna gli elementi 2D
                    batcher2D.Begin(blendMode, sortMode, SaveStateMode.None, transformMatrix);
                    for (int s = 0; s < batcher2DList.Count; s++)
                    {
                        if (batcher2DList[s] != null)
                        {
                            if (batcher2DList[s].Accoded)
                            {
                                batcher2D.Draw(batcher2DList[s].Textures[batcher2DList[s].CurrentFrame],
                                                batcher2DList[s].Position,
                                                batcher2DList[s].SourceRectangle,
                                                batcher2DList[s].Color,
                                                MathHelper.ToRadians(batcher2DList[s].Rotation),
                                                batcher2DList[s].Origin,
                                                batcher2DList[s].Scale,
                                                batcher2DList[s].SpriteEffect,
                                                batcher2DList[s].LayerDepth);
                            }
                        }
                    }
                    batcher2D.End();

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


        ~SpriteBatcher()
        {
            //Rilascia le risorse
            //batcher2D.Dispose();
        }
    }
}
