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
    public class Sprite : Engine2DObject
    {
        //Animazione
        private int[] currentAnimation = null;
        private int currentFrameAnimation = 0;
        private bool playAnimation = false;
        private double timeAnimation = 50;
        private double oldTimer = 0;
        private Dictionary<string, int[]> animations = null;
        public Dictionary<string, int[]> Animations
        {
            get { return animations; }
            set { animations = value; }
        }
	        
        //Lista di textures
        private List<Texture2D> textures = null;
        public List<Texture2D> Textures
        {
            get { return textures; }
            set { textures = value; }
        }

        //Colore
        private Color color = Color.White;
        public Color Color
        {
            get { return color; }
            set { color = value; }
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

        //Rotazione
        private float rotation = 0.0f;
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        //Scalatura
        private Vector2 scale = new Vector2(1, 1);
        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        public float ScaleX
        {
            get { return scale.X; }
            set { scale.X = value; }
        }
        public float ScaleY
        {
            get { return scale.Y; }
            set { scale.Y = value; }
        }

        //Origine
        private Vector2 origin = Vector2.Zero;
        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        //Parte della texture da disegnare
        private Rectangle sourceRectangle;
        public Rectangle SourceRectangle
        {
            get { return sourceRectangle; }
            set { sourceRectangle = value; }
        }
	
        //Colore delle textures (serve per la collisione)
        private List<Color[]> texturesData = new List<Color[]>();
        public List<Color[]> TexturesData
        {
            get { return texturesData; }
            set { texturesData = value; }
        }

        //Frame corrente
        private int currentFrame = 0;
        public int CurrentFrame
        {
            get { return currentFrame; }
            set
            {
                currentFrame = value;
                if (currentFrame > textures.Count - 1) currentFrame = textures.Count - 1;
            }
        }

        //Profondità di layer (per il sorting: between 0 (front) and 1 (back))
        private float layerDepth = 0.0f;
        public float LayerDepth
        {
            get { return layerDepth; }
            set { layerDepth = value; }
        }

        //Effetto per lo sprite
        private SpriteEffects spriteEffect = SpriteEffects.None;
        public SpriteEffects SpriteEffect
        {
            get { return spriteEffect; }
            set { spriteEffect = value; }
        }
	

        //Rettangolo di influenza per l'intersezione (definisce solo una porzione rettangolare da calcolare)
        private Rectangle influenceRectangle;
        public Rectangle InfluenceRetangle
        {
            get { return influenceRectangle; }
            set { influenceRectangle = value; }
        }

        //Dimensioni
        private int width;
        public int Width
        {
            get { return (int)((float)width * scale.X); }
        }
        private int height;
        public int Height
        {
            get { return (int)((float)height * scale.Y); }
        }	

        //Accodamente nella lista del batch
        private bool accoded;
        public bool Accoded
        {
            get { return accoded; }
        }

       

        public Sprite(string textureName, uint sprites, Scene parentScene):base(parentScene)
        {
            try
            {
                //Animazioni e textures
                animations = new Dictionary<string, int[]>();
                textures = new List<Texture2D>();

                //Dimensioni
                if (sprites <= 0) sprites = 1;
                Texture2D spriteTexture = this.ParentScene.SceneContent.Load<Texture2D>(textureName);
                width = (int)(spriteTexture.Width / sprites);
                height = spriteTexture.Height;

                //Divide la texture principale in tante textures :-) e userà qualle per disegnare lo sprite
                //In questa struttura sono contemplate solo textures che hanno frames consecuitivi uno all'altro e non una mappa NxN di textures
                //Si è usata la dimensione in larghezza e non altezza perchè solitamente è più lunga la capacità di caricare texture lunghe che larghe
                for (int s = 0; s < sprites; s++)
                {
                    //Preleva i dati dalla texture nella zona specificata (avanza di s*width)
                    Color[] data = new Color[width * height];
                    Rectangle rect = new Rectangle(s * width, 0, width, height);
                    spriteTexture.GetData<Color>(0, rect, data, 0, data.Length);
                    texturesData.Add(data);

                    //Crea la nuova texture corrente e ne setta i dati appena prelevati
                    Texture2D tex = new Texture2D(Core.Graphics.GraphicsDevice, width, height, spriteTexture.LevelCount, spriteTexture.TextureUsage, spriteTexture.Format);
                    tex.SetData<Color>(data);
                    textures.Add(tex);
                }

                //Frame corrente, primo
                currentFrame = 0;

                //Rettangolo di influenza nella collisione (di default tutto)
                influenceRectangle = new Rectangle(0, 0, width, height);

                //Rettangolo della texture da disegnare (di default tutta)
                sourceRectangle = new Rectangle(0, 0, width, height);

                //Conto sprites totali
                Core.loadedSprites = Core.loadedSprites + 1;

                //Creato correttamente
                IsCreated = true;
            }
            catch
            {
                //Creazione fallita
                IsCreated = false;
            }
        }



        public Sprite(string textureName, uint spritesO, uint spritesV, bool isVertical, Scene parentScene): base(parentScene)
        {      
            try
            {
                //Animazioni e textures
                animations = new Dictionary<string, int[]>();
                textures = new List<Texture2D>();

                //Dimensioni
                if (spritesO <= 0) spritesO = 1;
                if (spritesV <= 0) spritesV = 1;
                Texture2D spriteTexture = this.ParentScene.SceneContent.Load<Texture2D>(textureName);
                width = (int)(spriteTexture.Width / spritesO);
                height = (int)(spriteTexture.Height / spritesV);

                if (isVertical)
                {
                    //Divide la texture principale in tante textures :-) e userà qualle per disegnare lo sprite
                    //In questa struttura sono contemplate textures che hanno una mappa NxN di textures
                    for (int o = 0; o < spritesO; o++)
                    {
                        for (int v = 0; v < spritesV; v++)
                        {
                            //Preleva i dati dalla texture nella zona specificata (avanza di s*width)
                            Color[] data = new Color[width * height];
                            Rectangle rect = new Rectangle(o * width, v * height, width, height);
                            spriteTexture.GetData<Color>(0, rect, data, 0, data.Length);
                            texturesData.Add(data);

                            //Crea la nuova texture corrente e ne setta i dati appena prelevati
                            Texture2D tex = new Texture2D(Core.Graphics.GraphicsDevice, width, height, spriteTexture.LevelCount, spriteTexture.TextureUsage, spriteTexture.Format);
                            tex.SetData<Color>(data);
                            textures.Add(tex);
                        }
                    }
                }
                else
                {
                    //Divide la texture principale in tante textures :-) e userà qualle per disegnare lo sprite
                    //In questa struttura sono contemplate textures che hanno una mappa NxN di textures
                    for (int v = 0; v < spritesV; v++)
                    {
                        for (int o = 0; o < spritesO; o++)
                        {
                            //Preleva i dati dalla texture nella zona specificata (avanza di s*width)
                            Color[] data = new Color[width * height];
                            Rectangle rect = new Rectangle(o * width, v * height, width, height);
                            spriteTexture.GetData<Color>(0, rect, data, 0, data.Length);
                            texturesData.Add(data);

                            //Crea la nuova texture corrente e ne setta i dati appena prelevati
                            Texture2D tex = new Texture2D(Core.Graphics.GraphicsDevice, width, height, spriteTexture.LevelCount, spriteTexture.TextureUsage, spriteTexture.Format);
                            tex.SetData<Color>(data);
                            textures.Add(tex);
                        }
                    }
                }
                //Frame corrente, primo
                currentFrame = 0;

                //Rettangolo di influenza nella collisione (di base tutto)
                influenceRectangle = new Rectangle(0, 0, width, height);

                //Rettangolo della texture da disegnare (di default tutta)
                sourceRectangle = new Rectangle(0, 0, width, height);

                //Conto sprites totali
                Core.loadedSprites = Core.loadedSprites + 1;

                //Creato correttamente
                IsCreated = true;
            }
            catch
            {
                //Creazione fallita
                IsCreated = false;
            }
        }


        public Sprite(Texture2D[] texturesArray, Scene parentScene):base(parentScene)
        {
            try
            {
                //Animazioni e textures
                animations = new Dictionary<string, int[]>();
                textures = new List<Texture2D>();

                //Dimensioni
                width = textures[0].Width;
                height = textures[0].Height;

                for (int s = 0; s < texturesArray.Length; s++)
                {
                    textures.Add(texturesArray[s]);
                }
                //Frame corrente, primo
                currentFrame = 0;

                //Rettangolo di influenza nella collisione (di base tutto)
                influenceRectangle = new Rectangle(0, 0, textures[0].Width, textures[0].Height);

                //Rettangolo della texture da disegnare (di default tutta)
                sourceRectangle = new Rectangle(0, 0, textures[0].Width, textures[0].Height);

                //Conto sprites totali
                Core.loadedSprites = Core.loadedSprites + 1;

                //Creato correttamente
                IsCreated = true;
            }
            catch
            {
                //Creazione fallita
                IsCreated = false;
            }
        }

        public Sprite(Texture2D texture, Scene parentScene):base(parentScene)
        {
            try
            {
                //Animazioni e textures
                animations = new Dictionary<string, int[]>();
                textures = new List<Texture2D>();

                //Dimensioni
                width = texture.Width;
                height = texture.Height;

                //Aggiunta texture
                textures.Add(texture);

                //Frame corrente, primo
                currentFrame = 0;

                //Rettangolo di influenza nella collisione (di base tutto)
                influenceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);

                //Rettangolo della texture da disegnare (di default tutta)
                sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);

                //Conto sprites totali
                Core.loadedSprites = Core.loadedSprites + 1;

                //Creato correttamente
                IsCreated = true;
            }
            catch
            {
                //Creazione fallita
                IsCreated = false;
            }
        }


        //Aggiunta animazioni (tramite array o indici)
        public void AddAnimation(string name, int[] animation)
        {
            if (IsCreated)
            {
                animations.Add(name, animation);
            }
        }
        public void AddAnimation(string name, int startIndex, int endIndex)
        {
            int[] a = new int[(endIndex+1) - startIndex];
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = (startIndex + i);
            }
            animations.Add(name, a);
        }

        //Controllo animazione
        public void PlayAnimation(string animationName, double time_ms)
        {
            if (IsCreated)
            {
                //Preleva l'array dei frame di animazione in base al nome dell'animazione specifica
                animations.TryGetValue(animationName, out currentAnimation);
                timeAnimation = time_ms;
                playAnimation = true;
            }
        }
        public void StopAnimation()
        {
            if (IsCreated)
            {
                playAnimation = false;
                currentFrameAnimation = 0;
            }
        }
        public void PauseAnimation()
        {
            if (IsCreated)
            {
                playAnimation = !playAnimation;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (IsCreated)
            {
                if (playAnimation)
                {
                    if (currentFrameAnimation >= currentAnimation.Length)
                        currentFrameAnimation = 0;
                    currentFrame = currentAnimation[currentFrameAnimation];

                    //manda avanti l'animazione di un frame solo se è passato il tempo specificato
                    if (oldTimer > timeAnimation)
                    {
                        currentFrameAnimation++;
                        oldTimer = 0;
                    }
                    oldTimer = oldTimer + gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                accoded = false;
            }
            base.Update(gameTime);
        }


        public override void Draw()
        {
            if (IsCreated)
            {
                if (ToDraw)
                {
                    accoded = true;
                    Core.currentSprites = Core.currentSprites + 1;
                }
            }
            base.Draw();
        }


        public bool IntersectSimple(Sprite spriteB)
        {
            if (IsCreated)
            {
                //Calcola la trasformazione del primo sprite
                Matrix transformA =
                    Matrix.CreateTranslation(new Vector3(-origin, 0.0f)) *
                    Matrix.CreateScale(new Vector3(scale.X, scale.Y, 1)) *
                    Matrix.CreateRotationZ(rotation) *
                    Matrix.CreateTranslation(new Vector3(position, 0.0f));

                //Calcola la trasformazione del secondo sprite
                Matrix transformB =
                    Matrix.CreateTranslation(new Vector3(-spriteB.origin, 0.0f)) *
                    Matrix.CreateScale(new Vector3(spriteB.scale.X, spriteB.scale.Y, 1)) *
                    Matrix.CreateRotationZ(spriteB.rotation) *
                    Matrix.CreateTranslation(new Vector3(spriteB.position, 0.0f));

                //Calcola i rettabgoli di occupazione in base alla trasformazione (potrebbero essere più grandi e comunque sempre AAB)
                Rectangle RectangleA = GraphicsHelper.CalculateBoundingRectangle(influenceRectangle, transformA);
                Rectangle RectangleB = GraphicsHelper.CalculateBoundingRectangle(spriteB.influenceRectangle, transformB);

                //Esegue l'intersezione tra i due rettangoli
                if (RectangleA.Intersects(RectangleB))
                {
                    //Intersezione avvenuta
                    return true;
                }
                else
                {
                    //Nessuna intersezione
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public bool Intersect(Sprite spriteB)
        {
            if (IsCreated)
            {
                //Calcola la trasformazione del primo sprite
                Matrix transformA =
                    Matrix.CreateTranslation(new Vector3(-origin, 0.0f)) *
                    Matrix.CreateScale(new Vector3(scale.X, scale.Y, 1)) *
                    Matrix.CreateRotationZ(rotation) *
                    Matrix.CreateTranslation(new Vector3(position, 0.0f));

                //Calcola la trasformazione del secondo sprite
                Matrix transformB =
                    Matrix.CreateTranslation(new Vector3(-spriteB.origin, 0.0f)) *
                    Matrix.CreateScale(new Vector3(spriteB.scale.X, spriteB.scale.Y, 1)) *
                    Matrix.CreateRotationZ(spriteB.rotation) *
                    Matrix.CreateTranslation(new Vector3(spriteB.position, 0.0f));

                //Calcola i rettabgoli di occupazione in base alla trasformazione (potrebbero essere più grandi e comunque sempre AAB)
                Rectangle RectangleA = GraphicsHelper.CalculateBoundingRectangle(influenceRectangle, transformA);
                Rectangle RectangleB = GraphicsHelper.CalculateBoundingRectangle(spriteB.influenceRectangle, transformB);

                //Esegue l'intersezione tra i due rettangoli
                if (RectangleA.Intersects(RectangleB))
                {
                    //Prima intersezione semplice avvenuta, procede con la seconda
                    if (GraphicsHelper.IntersectPixels(transformA, this.Textures[this.currentFrame].Width, this.Textures[this.currentFrame].Height, this.TexturesData[this.currentFrame], transformB, spriteB.Textures[spriteB.currentFrame].Width, spriteB.Textures[spriteB.currentFrame].Height, spriteB.TexturesData[spriteB.currentFrame]))
                    {
                        //Intersezione avvenuta
                        return true;
                    }
                    else
                    {
                        //Nessuna intersezione
                        return false;
                    }
                }
                else
                {
                    //Nessuna intersezione
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        ~Sprite()
        {
            //Conto sprites totali
            Core.loadedSprites = Core.loadedSprites - 1;

            /*Rilascia le risorse
            if (textures != null)
            {
                for (int i = 0; i < textures.Count; i++)
                {
                    textures[i].Dispose();
                    textures[i] = null;
                }
            }*/
        }
    }
}


/*

Uso della classe:

Sprite panel;
panel = new Sprite("Content\\Texture\\exp1", 45, 1);
panel.Position = new Vector2(200.0f, 200.0f);
panel.Origin = new Vector2(panel.Textures[0].Width/2.0f, panel.Textures[0].Height/2.0f);
panel.Scale = new Vector2(1.0f, 1.0f);
panel.Rotation = MathHelper.ToRadians(0.0f);
panel.CurrentFrame = 1;
panel.Color = new Color(255, 255, 255, 255);
panel.AddAnimation("prova1", 0, 20);
panel.AddAnimation("prova2", 21, 44);
panel.PlayAnimation("prova1", 25);
...
...
panel.Update();
...
panel.Draw();
 
*/
