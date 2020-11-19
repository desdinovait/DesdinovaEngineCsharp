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
    public class MovieEffect : Engine2DObject
    {
        //Tipo di animazione
        public enum AnimationType
        {
            Fade = 0,
            FadeReverse = 1,
            Enter = 2,
            EnterReverse = 3
        }

        //Sprites
        private Sprite spriteUp = null;
        private Sprite spriteDown = null;
        private SpriteBatcher spriteBatcher = null;

        //Abilitazione effetto
        private bool enable = false;
        public bool Enable
        {
            get { return enable; }
            set { enable = value; }
        }

        //Rapporto schermo
        float cineRap = 0.0f;

        //Animazione
        private float animationVelocity = 1.5f;
        private float animationAlpha = 0.0f;
        private float animationPositionUp = 0.0f;
        private float animationPositionDown = 0.0f;
        private bool animationEnabled = false;
        private AnimationType animationType = AnimationType.Fade;
        public bool AnimationEnabled
        {
            get { return animationEnabled; }
        }

   
        public MovieEffect(Color panelColor, bool startDefaultAnim, bool useRap, int noRapHeight, Scene parentScene):base(parentScene)
        {
            try
            {
                //Dimensione dello sprite 16/9 in base al rapporto corrente dello schermo
                if (useRap)
                {
                    cineRap = ((float)Core.Graphics.GraphicsDevice.Viewport.Height - ((float)Core.Graphics.GraphicsDevice.Viewport.Width / (16.0f / 9.0f))) / 2.0f;
                }
                else
                {
                    cineRap = noRapHeight;
                }

                //Parte sopra
                spriteUp = new Sprite(Core.NullTextureColor, parentScene);
                spriteUp.PositionX = 0;
                spriteUp.PositionY = 0;
                spriteUp.ScaleX = Core.Graphics.GraphicsDevice.Viewport.Width;
                spriteUp.ScaleY = cineRap;
                spriteUp.Color = panelColor;

                //Parte sotto
                spriteDown = new Sprite(Core.NullTextureColor, parentScene);
                spriteDown.PositionX = 0;
                spriteDown.PositionY = Core.Graphics.GraphicsDevice.Viewport.Height - cineRap;
                spriteDown.ScaleX = Core.Graphics.GraphicsDevice.Viewport.Width;
                spriteDown.ScaleY = cineRap;
                spriteDown.Color = panelColor;

                //Aggiunge le 2 parti al batcher
                spriteBatcher = new SpriteBatcher(parentScene);
                spriteBatcher.BlendMode = SpriteBlendMode.AlphaBlend;
                spriteBatcher.SortMode = SpriteSortMode.Immediate;
                spriteBatcher.TransformMatrix = Matrix.Identity;
                spriteBatcher.Add(spriteUp);
                spriteBatcher.Add(spriteDown);

                
                //Esegue subito una animazione
                if (startDefaultAnim)
                {
                    enable = true;
                    PlayAnimation(0.5f, AnimationType.Enter);
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

        public void PlayAnimation(float velocity, AnimationType type)
        {
            if (IsCreated)
            {
                animationVelocity = velocity;
                animationType = type;
                animationEnabled = true;

                switch (animationType)
                {
                    case AnimationType.Fade:
                        {
                            animationAlpha = 0.0f;
                            animationPositionUp = 0;
                            animationPositionDown = Core.Graphics.GraphicsDevice.Viewport.Height - cineRap;
                            break;
                        }

                    case AnimationType.FadeReverse:
                        {
                            animationAlpha = 255.0f;
                            animationPositionUp = 0;
                            animationPositionDown = Core.Graphics.GraphicsDevice.Viewport.Height - cineRap;
                            break;
                        }

                    case AnimationType.Enter:
                        {
                            animationAlpha = 255.0f;
                            animationPositionUp = 0 - cineRap;
                            animationPositionDown = Core.Graphics.GraphicsDevice.Viewport.Height;
                            break;
                        }

                    case AnimationType.EnterReverse:
                        {
                            animationAlpha = 255.0f;
                            animationPositionUp = 0;
                            animationPositionDown = Core.Graphics.GraphicsDevice.Viewport.Height - cineRap;
                            break;
                        }
                }
            }
        }
        
        public override void Update(GameTime gameTime)
        {
            if (IsCreated)
            {
                if (enable)
                {
                    if (animationEnabled)
                    {
                        switch (animationType)
                        {
                            case AnimationType.Fade:
                                {
                                    animationAlpha = animationAlpha + animationVelocity;
                                    if (animationAlpha >= 255.0f)
                                    {
                                        animationEnabled = false;
                                    }
                                    break;
                                }

                            case AnimationType.FadeReverse:
                                {
                                    animationAlpha = animationAlpha - animationVelocity;
                                    if (animationAlpha <= 0.0f)
                                    {
                                        animationEnabled = false;
                                    }
                                    break;
                                }

                            case AnimationType.Enter:
                                {
                                    animationPositionUp = animationPositionUp + animationVelocity;
                                    animationPositionDown = animationPositionDown - animationVelocity;
                                    if (animationPositionUp >= 0)
                                    {
                                        animationEnabled = false;
                                        animationPositionUp = 0;
                                    }
                                    break;
                                }

                            case AnimationType.EnterReverse:
                                {
                                    animationPositionUp = animationPositionUp - animationVelocity;
                                    animationPositionDown = animationPositionDown + animationVelocity;
                                    if (animationPositionUp <= 0 - cineRap)
                                    {
                                        animationEnabled = false;
                                        animationPositionUp = -cineRap;
                                    }
                                    break;
                                }
                        }

                        //Imposta il nuovo colore
                        spriteUp.PositionY = animationPositionUp;
                        spriteUp.Color = new Color(spriteUp.Color.R, spriteUp.Color.G, spriteUp.Color.B, (byte)animationAlpha);

                        spriteDown.PositionY = animationPositionDown;
                        spriteDown.Color = new Color(spriteDown.Color.R, spriteDown.Color.G, spriteDown.Color.B, (byte)animationAlpha);
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw()
        {
            if (IsCreated)
            {
                if (enable)
                {
                    if (animationEnabled)
                    {
                    }
                }
            }
            base.Draw();
        }

        public bool AddChildren()
        {
            this.ParentScene.AddObject(spriteBatcher);
            return true;
        }
        public bool RemoveChildren()
        {
            this.ParentScene.RemoveObject(spriteBatcher);
            return true;
        }
    }
}


//Esempio per eseguire le animazioni
/*  
 * Effetto "normale"
 * cineEffect = new MovieEffect(false, true, 0);   
 * cineEffect.Enable = true;  
 * cineEffect.PlayAnimation(0.75f, MovieEffect.AnimationType.Enter);
 * 
 * Effetto tendina fade "tutto schermo"
 * cineEffect = new MovieEffect(new Color(0, 0, 0, 255), false, false, Core.Graphics.GraphicsDevice.Viewport.Height / 2);
 * cineEffect.Enable = true;  
 * cineEffect.PlayAnimation(1.25f, MovieEffect.AnimationType.Fade);
 * 
 */