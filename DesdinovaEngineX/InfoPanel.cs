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
using System.Diagnostics;

namespace DesdinovaModelPipeline
{

    public class InfoPanel : Engine2DObject, IEngineChildCollector
    {
        //Modalità
        public enum InfoPanelMode
        {
            Hide = 0,
            Minimal = 1,
            Normal = 2,
            Extended = 3
        }   
     
        //Elementi info
        private Font fontInfo = null;
        private Sprite spriteInfo = null;
        private SpriteBatcher spriteBatcher = null;
        private Lines2D linesInfo = null;

        //Altre info
        private TimeSpan runningTime = TimeSpan.Zero;
        private bool runningSlowly = false;
        private bool toHide = false;

        //Modalità di visualizzazione
        private InfoPanelMode mode = InfoPanelMode.Normal;
        public InfoPanelMode Mode
        {
            get { return mode; }
            set 
            { 
                mode = value;
                if (mode == InfoPanelMode.Hide)
                {
                    toHide = true;
                }
                else if (mode == InfoPanelMode.Minimal)
                {
                    toHide = false;
                    float newHeight = 30;
                    linesInfo[0] = new Line2D(new Vector2(0, 0), new Vector2(0, newHeight), borderColor, borderColor);
                    linesInfo[1] = new Line2D(new Vector2(0, newHeight), new Vector2(250, newHeight), borderColor, borderColor);
                    linesInfo[2] = new Line2D(new Vector2(0, 0), new Vector2(250, 0), borderColor, borderColor);
                    linesInfo[3] = new Line2D(new Vector2(250, 0), new Vector2(250, newHeight), borderColor, borderColor);
                    linesInfo[4] = new Line2D(new Vector2(0, newHeight), new Vector2(250, newHeight), borderColor, borderColor);
                    spriteInfo.Scale = new Vector2(250, newHeight);
                }
                else if (mode == InfoPanelMode.Normal)
                {
                    toHide = false;
                    float newHeight = 140;
                    linesInfo[0] = new Line2D(new Vector2(0, 0), new Vector2(0, newHeight), borderColor, borderColor);
                    linesInfo[1] = new Line2D(new Vector2(0, newHeight), new Vector2(250, newHeight), borderColor, borderColor);
                    linesInfo[2] = new Line2D(new Vector2(0, 0), new Vector2(250, 0), borderColor, borderColor);
                    linesInfo[3] = new Line2D(new Vector2(250, 0), new Vector2(250, newHeight), borderColor, borderColor);
                    linesInfo[4] = new Line2D(new Vector2(0, 30), new Vector2(250, 30), borderColor, borderColor);
                    spriteInfo.Scale = new Vector2(250, newHeight);
                }
                else if (mode == InfoPanelMode.Extended)
                {
                    toHide = false;
                    float newHeight = 225;
                    linesInfo[0] = new Line2D(new Vector2(0, 0), new Vector2(0, newHeight), borderColor, borderColor);
                    linesInfo[1] = new Line2D(new Vector2(0, newHeight), new Vector2(250, newHeight), borderColor, borderColor);
                    linesInfo[2] = new Line2D(new Vector2(0, 0), new Vector2(250, 0), borderColor, borderColor);
                    linesInfo[3] = new Line2D(new Vector2(250, 0), new Vector2(250, newHeight), borderColor, borderColor);
                    linesInfo[4] = new Line2D(new Vector2(0, 30), new Vector2(250, 30), borderColor, borderColor);
                    spriteInfo.Scale = new Vector2(250, newHeight);
                }
            }
        }
	
        //Dimensioni
        private int width = 250;
        public int Width
        {
            get { return width; }
        }
        private int height = 160;
        public int Height
        {
            get { return height; }
        }

        //Colore
        private Color textColor = Color.Yellow;
        public Color TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        //Colore sfondo
        private Color backgroundColor = Color.Black;
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set 
            {
                backgroundColor = value; 
                spriteInfo.Color = backgroundColor;
            }
        }

        //Colore bordo
        private Color borderColor = Color.White;
        public Color BorderColor
        {
            get { return borderColor; }
            set 
            { 
                borderColor = value;
                linesInfo.Color = borderColor;
            }
        }
	
        //Posizione
        private Vector2 fontInfoCurrentPosition = Vector2.Zero;
        private Vector2 position = Vector2.Zero;
        public Vector2 Position
        {
            get { return position; }
            set 
            {
                linesInfo.PositionOffset = value;
                spriteInfo.Position = value;

                fontInfo.Position = value + new Vector2(10, 5);
                fontInfoCurrentPosition = fontInfo.Position;

                position = value; 
            }
        }

        public InfoPanel(Scene parentScene):base(parentScene)
        {
            try
            {
                //Sprite di sfondo colorato
                spriteInfo = new Sprite(Core.NullTextureColor, parentScene);
                spriteInfo.Color = new Color(0, 0, 0, 96);
                spriteInfo.Scale = new Vector2(250, 160);
                spriteInfo.Position = new Vector2(0, 0);

                //Batcher
                spriteBatcher = new SpriteBatcher(parentScene);
                spriteBatcher.BlendMode = SpriteBlendMode.AlphaBlend;
                spriteBatcher.SortMode = SpriteSortMode.Texture;
                spriteBatcher.TransformMatrix = Matrix.Identity;
                spriteBatcher.Add(spriteInfo);

                //Fonts
                fontInfo = new Font(12, this.ParentScene);
                fontInfo[0] = new FontLine("Content\\Font\\Verdana", this.ParentScene);
                fontInfo[1] = new FontLine("Content\\Font\\Verdana", this.ParentScene);
                fontInfo[2] = new FontLine("Content\\Font\\Verdana", this.ParentScene);
                fontInfo[3] = new FontLine("Content\\Font\\Verdana", this.ParentScene);
                fontInfo[4] = new FontLine("Content\\Font\\Verdana", this.ParentScene);
                fontInfo[5] = new FontLine("Content\\Font\\Verdana", this.ParentScene);
                fontInfo[6] = new FontLine("Content\\Font\\Verdana", this.ParentScene);
                fontInfo[7] = new FontLine("Content\\Font\\Verdana", this.ParentScene);
                fontInfo[8] = new FontLine("Content\\Font\\Verdana", this.ParentScene);
                fontInfo[9] = new FontLine("Content\\Font\\Verdana", this.ParentScene);
                fontInfo[10] = new FontLine("Content\\Font\\Verdana", this.ParentScene);
                fontInfo[11] = new FontLine("Content\\Font\\Verdana", this.ParentScene);

                //Line del bordo
                linesInfo = new Lines2D(5, parentScene);
                linesInfo.AddLine(new Line2D(new Vector2(0, 0), new Vector2(0, 160), Color.White, Color.White));        //Bordo
                linesInfo.AddLine(new Line2D(new Vector2(0, 160), new Vector2(250, 160), Color.White, Color.White));    //Bordo
                linesInfo.AddLine(new Line2D(new Vector2(0, 0), new Vector2(250, 0), Color.White, Color.White));        //Bordo
                linesInfo.AddLine(new Line2D(new Vector2(250, 0), new Vector2(250, 160), Color.White, Color.White));    //Bordo
                linesInfo.AddLine(new Line2D(new Vector2(0, 30), new Vector2(250, 30), Color.White, Color.White));      //Linea centrale

                //Stato iniziale
                this.Mode = InfoPanelMode.Normal;

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
                //Elementi
                fontInfo.Update(gameTime);
                spriteInfo.Update(gameTime);
                spriteBatcher.Update(gameTime);
                linesInfo.Update(gameTime);
                runningTime = gameTime.TotalRealTime;
                runningSlowly = gameTime.IsRunningSlowly;
            }
            base.Update(gameTime);
        }

        public override void Draw()
        {
            if (IsCreated)
            {
                if (ToDraw)
                {
                    if (toHide == false)
                    {                       
                        fontInfo[0].Text = "Current FPS: " + Core.FPS.ToString();
                        fontInfo[0].PositionX = fontInfoCurrentPosition.X;
                        fontInfo[0].PositionY = fontInfoCurrentPosition.Y;
                        fontInfo[0].Scale = 0.85f;
                        if (Core.FPS < 30)
                        {
                            fontInfo[0].Color = Color.Red;
                        }
                        else
                        {
                            fontInfo[0].Color = textColor;
                        }

                        if ((mode == InfoPanelMode.Normal) || (mode == InfoPanelMode.Extended))
                        {
                            fontInfo[1].Color = textColor;
                            fontInfo[1].Scale = 0.75f;
                            fontInfo[1].Text = "Models: " + Core.CurrentModels.ToString() + "/" + Core.LoadedModels.ToString();
                            fontInfo[1].PositionY = fontInfo[0].PositionY + 33;
                            fontInfo[1].PositionX = fontInfo[0].PositionX + 5;

                            fontInfo[2].Color = textColor;
                            fontInfo[2].Text = "Meshes: " + Core.CurrentMeshes.ToString() + "/" + Core.LoadedMeshes.ToString();
                            fontInfo[2].PositionY = fontInfo[1].PositionY + 15;
                            fontInfo[2].PositionX = fontInfo[0].PositionX + 5;

                            fontInfo[3].Color = textColor;
                            fontInfo[3].Text = "Primitives: " + Core.CurrentPrimitives.ToString() + "/" + Core.LoadedPrimitives.ToString();
                            fontInfo[3].PositionY = fontInfo[2].PositionY + 15;
                            fontInfo[3].PositionX = fontInfo[0].PositionX + 5;

                            fontInfo[4].Color = textColor;
                            fontInfo[4].Text = "Vertices: " + Core.CurrentVertices.ToString() + "/" + Core.LoadedVertices.ToString();
                            fontInfo[4].PositionY = fontInfo[3].PositionY + 15;
                            fontInfo[4].PositionX = fontInfo[0].PositionX + 5;

                            fontInfo[5].Color = textColor;
                            fontInfo[5].Text = "Sprites: " + Core.CurrentSprites.ToString() + "/" + Core.LoadedSprites.ToString();
                            fontInfo[5].PositionY = fontInfo[4].PositionY + 15;
                            fontInfo[5].PositionX = fontInfo[0].PositionX + 5;

                            fontInfo[6].Color = textColor;
                            fontInfo[6].Text = "Fonts: " + Core.CurrentFonts.ToString() + "/" + Core.LoadedFonts.ToString();
                            fontInfo[6].PositionY = fontInfo[5].PositionY + 15;
                            fontInfo[6].PositionX = fontInfo[0].PositionX + 5;
                        }

                        if (mode == InfoPanelMode.Extended)
                        {
                            fontInfo[7].Color = textColor;
                            fontInfo[7].Text = "Process MBytes: " + (((float)Process.GetCurrentProcess().WorkingSet64 / 1024.0f) / 1024.0f);
                            fontInfo[7].PositionY = fontInfo[6].PositionY + 25;
                            fontInfo[7].PositionX = fontInfo[0].PositionX + 5;

                            fontInfo[8].Text = "Running Time: " + runningTime.Hours.ToString() + "h : " + runningTime.Minutes.ToString() + "m : " + runningTime.Seconds.ToString() + "s";
                            fontInfo[8].PositionY = fontInfo[7].PositionY + 15;
                            fontInfo[8].PositionX = fontInfo[0].PositionX + 5;
                            if (runningSlowly)
                            {
                                fontInfo[8].Color = Color.Red;
                            }
                            else
                            {
                                fontInfo[8].Color = textColor;
                            }

                            fontInfo[9].Color = textColor;
                            fontInfo[9].Scale = 0.75f;
                            fontInfo[9].Text = "Resolution: " + Core.Graphics.GraphicsDevice.PresentationParameters.BackBufferWidth.ToString() + "x" + Core.Graphics.GraphicsDevice.PresentationParameters.BackBufferHeight.ToString() + "x" + Core.Graphics.GraphicsDevice.PresentationParameters.FullScreenRefreshRateInHz.ToString();
                            fontInfo[9].PositionY = fontInfo[8].PositionY + 15;
                            fontInfo[9].PositionX = fontInfo[0].PositionX + 5;

                            fontInfo[10].Color = textColor;
                            fontInfo[10].Scale = 0.75f;
                            fontInfo[10].Text = "VertexShader: " + Core.Graphics.GraphicsDevice.GraphicsDeviceCapabilities.VertexShaderVersion.Major.ToString() + "." + Core.Graphics.GraphicsDevice.GraphicsDeviceCapabilities.VertexShaderVersion.Minor.ToString();
                            fontInfo[10].PositionY = fontInfo[9].PositionY + 15;
                            fontInfo[10].PositionX = fontInfo[0].PositionX + 5;

                            fontInfo[11].Color = textColor;
                            fontInfo[11].Scale = 0.75f;
                            fontInfo[11].Text = "PixelShader: " + Core.Graphics.GraphicsDevice.GraphicsDeviceCapabilities.PixelShaderVersion.Major.ToString() + "." + Core.Graphics.GraphicsDevice.GraphicsDeviceCapabilities.PixelShaderVersion.Minor.ToString();
                            fontInfo[11].PositionY = fontInfo[10].PositionY + 15;
                            fontInfo[11].PositionX = fontInfo[0].PositionX + 5;
                        }
                    }
                }
            }
            base.Draw();
        }


        public InfoPanelMode SwithMode()
        {
            if (IsCreated)
            {
                if (mode == InfoPanelMode.Hide)
                {
                    this.Mode = InfoPanelMode.Minimal;
                }
                else if (mode == InfoPanelMode.Minimal)
                {
                    this.Mode = InfoPanelMode.Normal;
                }
                else if (mode == InfoPanelMode.Normal)
                {
                    this.Mode = InfoPanelMode.Extended;
                }
                else if (mode == InfoPanelMode.Extended)
                {
                    this.Mode = InfoPanelMode.Hide;
                }
            }
            return mode;
        }


        public bool AddChildren()
        {
            this.ParentScene.AddObject(spriteBatcher);
            this.ParentScene.AddObject(fontInfo);
            this.ParentScene.AddObject(linesInfo);
            return true;
        }

        public bool RemoveChildren()
        {
            this.ParentScene.RemoveObject(spriteBatcher);
            this.ParentScene.RemoveObject(fontInfo);
            this.ParentScene.RemoveObject(linesInfo);
            return true;
        }

    }
}
