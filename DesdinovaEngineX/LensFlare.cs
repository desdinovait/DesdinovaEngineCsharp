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
    public class Flare
    {
        public Flare(float position, Vector2 scale, Color color, string textureName)
        {
            Position = position;
            Scale = scale;
            Color = color;
            TextureName = textureName;
        }

        public float Position;
        public Vector2 Scale;
        public Color Color;
        public string TextureName;
        public Texture2D Texture;
    }


    public class LensFlare : Engine2DObject
    {
        private SpriteBatch spriteBatch = null;
        private BasicEffect basicEffect = null;
        private VertexDeclaration vertexDeclaration = null;
        private VertexPositionColor[] queryVertices = null;
        OcclusionQuery occlusionQuery = null;
        bool occlusionQueryActive = false;
        float occlusionAlpha = 0.0f;

        //Flash
        Texture2D flashTexture = null;

        //Lista dei flare
        private List<Flare> flares = new List<Flare>();

        //Dimensioni dell'occlusione
        private float occlusionSize = 50.0f;
        public float OcclusionSize
        {
            get { return occlusionSize; }
            set { occlusionSize = value; }
        }
	
        //Direzione della luce
        private Vector3 lightDirection = Vector3.Normalize(new Vector3(-0.4f, -0.4f, 1.0f));
        public Vector3 LightDirection
        {
            get { return lightDirection; }
            set { lightDirection = value; }
        }

        //Posizione 2D
        private Vector2 position2D = Vector2.Zero;
        public Vector2 Position2D
        {
            get { return position2D; }
        }

        //Colore flash
        private Color flashColor = Color.White;
        public Color FlashColor
        {
            get { return flashColor; }
            set { flashColor = value; }
        }


        public LensFlare(Scene parentScene):base(parentScene)
        {
            try
            {
                //Query di occlusione
                occlusionQuery = new OcclusionQuery(Core.Graphics.GraphicsDevice);
                if (occlusionQuery.IsSupported)
                {
                    // Create a SpriteBatch for drawing the glow and flare sprites.
                    spriteBatch = new SpriteBatch(Core.Graphics.GraphicsDevice);

                    // Effect and vertex declaration for drawing occlusion query polygons.
                    basicEffect = new BasicEffect(Core.Graphics.GraphicsDevice, null);

                    basicEffect.View = Matrix.Identity;
                    basicEffect.VertexColorEnabled = true;

                    vertexDeclaration = new VertexDeclaration(Core.Graphics.GraphicsDevice, VertexPositionColor.VertexElements);

                    // Create vertex data for the occlusion query polygons.
                    queryVertices = new VertexPositionColor[4];

                    queryVertices[0].Position = new Vector3(-occlusionSize / 2, -occlusionSize / 2, -1);
                    queryVertices[1].Position = new Vector3(occlusionSize / 2, -occlusionSize / 2, -1);
                    queryVertices[2].Position = new Vector3(occlusionSize / 2, occlusionSize / 2, -1);
                    queryVertices[3].Position = new Vector3(-occlusionSize / 2, occlusionSize / 2, -1);

                    //Flash
                    flashTexture = new Texture2D(Core.Graphics.GraphicsDevice, 1, 1, 1, TextureUsage.None, SurfaceFormat.Color);
                    Color[] whiteTextureData = new Color[1];
                    whiteTextureData[0] = new Color(255, 255, 255, 255);
                    flashTexture.SetData<Color>(whiteTextureData);

                    //Creazione avvenuta
                    IsCreated = true;
                }
                else
                {
                    //Creazione fallita (quary non supportate)
                    IsCreated = false;
                }
            }
            catch
            {
                //creazione fallita
                IsCreated = false;
            }
        }


        public void AddFlare(Flare flare)
        {
            // Array describes the position, size, color, and texture for each individual
            // flare graphic. The position value lies on a line between the sun and the
            // center of the screen. Zero places a flare directly over the top of the sun,
            // one is exactly in the middle of the screen, fractional positions lie in
            // between these two points, while negative values or positions greater than
            // one will move the flares outward toward the edge of the screen. Changing
            // the number of flares, or tweaking their positions and colors, can produce
            // a wide range of different lensflare effects without altering any other code.

            if (IsCreated)
            {
                flare.Texture = this.ParentScene.SceneContent.Load<Texture2D>(flare.TextureName);
                flares.Add(flare);
            }
        }
        

        public override void Update(GameTime gameTime)
        {
            if (IsCreated)
            {
                // The sun is infinitely distant, so it should not be affected by the
                // position of the camera. Floating point math doesn't support infinitely
                // distant vectors, but we can get the same result by making a copy of our
                // view matrix, then resetting the view translation to zero. Pretending the
                // camera has not moved position gives the same result as if the camera
                // was moving, but the light was infinitely far away. If our flares came
                // from a local object rather than the sun, we would use the original view
                // matrix here.
                Matrix infiniteView = this.ParentScene.SceneCamera.ViewMatrix;
                infiniteView.Translation = Vector3.Zero;

                //Project the light position into 2D screen space.
                Vector3 projectedPosition = Core.Graphics.GraphicsDevice.Viewport.Project(-LightDirection, this.ParentScene.SceneCamera.ProjectionMatrix, infiniteView, Matrix.Identity);

                // Don't draw any flares if the light is behind the camera.
                if ((projectedPosition.Z < 0) || (projectedPosition.Z > 1))
                {
                    //return;
                }

                //Posizione 2D
                position2D = new Vector2(projectedPosition.X, projectedPosition.Y);
            }
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
                    int old17 = Core.Graphics.GraphicsDevice.SamplerStates[0].MaxMipLevel;
                    ColorWriteChannels old18 = Core.Graphics.GraphicsDevice.RenderState.ColorWriteChannels;//*/


                    // Check whether the light is hidden behind the scenery.
                    if (occlusionQueryActive)
                    {
                        // If the previous query has not yet completed, wait until it does.
                        if (!occlusionQuery.IsComplete)
                            goto prova;

                        // Use the occlusion query pixel count to work
                        // out what percentage of the sun is visible.
                        float queryArea = occlusionSize * occlusionSize;

                        occlusionAlpha = Math.Min(occlusionQuery.PixelCount / queryArea, 1);
                        if (occlusionAlpha < 1.0f)
                        {
                            //int hhh = 0;
                        }
                    }


                    // Set renderstates for drawing the occlusion query geometry. We want depth
                    // tests enabled, but depth writes disabled, and we set ColorWriteChannels
                    // to None to prevent this query polygon actually showing up on the screen.
                    Core.Graphics.GraphicsDevice.RenderState.DepthBufferEnable = true;
                    Core.Graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = false;
                    Core.Graphics.GraphicsDevice.RenderState.AlphaTestEnable = false;
                    Core.Graphics.GraphicsDevice.RenderState.ColorWriteChannels = ColorWriteChannels.None;


                    basicEffect.Begin(SaveStateMode.None);
                    {
                        basicEffect.CurrentTechnique.Passes[0].Begin();
                        Core.Graphics.GraphicsDevice.VertexDeclaration = vertexDeclaration;

                        // Issue the occlusion query.
                        occlusionQuery.Begin();
                        Core.Graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleFan, queryVertices, 0, 2);
                        occlusionQuery.End();

                        // Set up our BasicEffect to center on the current 2D light position.
                        basicEffect.World = Matrix.CreateTranslation(position2D.X, position2D.Y, 0);
                        basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, Core.Graphics.GraphicsDevice.Viewport.Width, Core.Graphics.GraphicsDevice.Viewport.Height, 0, 0, 1);

                        basicEffect.CurrentTechnique.Passes[0].End();
                    }
                    basicEffect.End();

                    // Reset renderstates.
                    Core.Graphics.GraphicsDevice.RenderState.ColorWriteChannels = ColorWriteChannels.All;
                    Core.Graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;

                    occlusionQueryActive = true;




                prova:

                    Vector2 screenCenter = new Vector2(Core.Graphics.GraphicsDevice.Viewport.Width, Core.Graphics.GraphicsDevice.Viewport.Height) / 2;
                    Vector2 flareVector = screenCenter - position2D;

                    // Draw the flare sprites using additive blending.
                    spriteBatch.Begin(SpriteBlendMode.Additive, SpriteSortMode.Texture, SaveStateMode.None);
                    {
                        for (int i = 0; i < flares.Count; i++)
                        {
                            // Compute the position of this flare sprite.
                            Vector2 flarePosition = position2D + flareVector * flares[i].Position;

                            // Set the flare alpha based on the previous occlusion query result.
                            Vector4 flareColor = flares[i].Color.ToVector4();
                            flareColor.W *= occlusionAlpha;

                            // Center the sprite texture.
                            Vector2 flareOrigin = new Vector2(flares[i].Texture.Width, flares[i].Texture.Height) / 2;

                            // Draw the flare.
                            spriteBatch.Draw(flares[i].Texture, flarePosition, null, new Color(flareColor), 1, flareOrigin, flares[i].Scale, SpriteEffects.None, 0);
                        }

                        //Flash
                        //Vector4 flashColorVect = flashColor.ToVector4();
                        //Vettore direzione (dal centro schermo)
                        //Vector2 screenCoordMiddle = new Vector2(Core.Graphics.GraphicsDevice.Viewport.Width / 2.0f, Core.Graphics.GraphicsDevice.Viewport.Height / 2.0f);
                        //Vector2 direction = Vector2.Subtract(screenCoordMiddle, position2D);
                        //byte alpha = (byte)(255.0f - ((direction.Length() * 255.0f) / screenCoordMiddle.X));
                        //flashColorVect.W = alpha;
                        //spriteBatch.Draw(flashTexture, new Rectangle(0, 0, Core.Graphics.GraphicsDevice.Viewport.Width, Core.Graphics.GraphicsDevice.Viewport.Height - 200), new Color(flashColorVect)); 
                    }
                    spriteBatch.End();


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
                    Core.Graphics.GraphicsDevice.RenderState.ColorWriteChannels = old18;//*/
                }
            }
            base.Draw();
        }



        /// <summary>
        /// Mesures how much of the sun is visible, by drawing a small rectangle,
        /// centered on the sun, but with the depth set to as far away as possible,
        /// and using an occlusion query to measure how many of these very-far-away
        /// pixels are not hidden behind the terrain.
        /// 
        /// The problem with occlusion queries is that the graphics card runs in
        /// parallel with the CPU. When you issue drawing commands, they are just
        /// stored in a buffer, and the graphics card can be as much as a frame delayed
        /// in getting around to processing the commands from that buffer. This means
        /// that even after we issue our occlusion query, the occlusion results will
        /// not be available until later, after the graphics card finishes processing
        /// these commands.
        /// 
        /// It would slow our game down too much if we waited for the graphics card,
        /// so instead we delay our occlusion processing by one frame. Each time
        /// around the game loop, we read back the occlusion results from the previous
        /// frame, then issue a new occlusion query ready for the next frame to read
        /// its result. This keeps the data flowing smoothly between the CPU and GPU,
        /// but also causes our data to be a frame out of date: we are deciding
        /// whether or not to draw our lensflare effect based on whether it was
        /// visible in the previous frame, as opposed to the current one! Fortunately,
        /// the camera tends to move slowly, and the lensflare fades in and out
        /// smoothly as it goes behind the scenery, so this out-by-one-frame error
        /// is not too noticeable in practice.
        /// </summary>

    }
}
