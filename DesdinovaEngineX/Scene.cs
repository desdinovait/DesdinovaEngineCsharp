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
    public enum SceneDrawMode
    {
        All = 1,
        Only2D = 2,
        Only3D = 3
    }

    public abstract class Scene : EngineScene
    {
        //Fase di caricamento
        private bool inLoading;
        public bool InLoading
        {
            get { return inLoading;}
        }

        //Caricato
        private bool loaded;
        public bool Loaded
        {
            get { return loaded; }
        }

        //Fase di caricamento
        private bool inRelease;
        public bool InRelease
        {
            get { return inRelease; }
        }

        //Caricato
        private bool released;
        public bool Released
        {
            get { return released; }
        }

        //Scene content creato correttamente
        //Altrimenti usa quello di base della classe Core che però non viene mai scaricato tramite Unload()
        public bool SceneContentCreated
        {
            get 
            {
                if (base.SceneContent != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        
        //Nome
        private string name;
        public string Name
        {
            get { return name; }
        }

        //Metodi astratti da implementare necessariamente
        public abstract bool Initialize();
        public abstract void Release();


        public Scene(string name)
        {
            this.name = name;
            DrawMode = SceneDrawMode.All;
            BackgroundColor = Color.Gray;
            IsCreated = true;
            SortObjects = true;
            DrawBoundings = false;

        }

        internal void InitializeBegin()
        {
            Core.Log(LogType.HtmlSubSection, "Scene '" + name + "' Initialization");
            Core.Log(LogType.HtmlInfo, "Scene Initialization Begin");

            //Funziona richiama internamente dal gestore delle scene prima di caricarla
            loaded = false;
            inLoading = true;
            
            try
            {
                base.SceneContent = new ContentManager(Core.Service);
                Core.Log(LogType.HtmlInfo, "Scene Content Manager Created Successfully");
            }
            catch
            {
                base.SceneContent = Core.Content;
                Core.Log(LogType.HtmlWarning, "Scene Content Manager NOT Created. Using Core Content Manager");
            }

            try
            {
                base.SceneCamera = new Camera();
                Core.Log(LogType.HtmlInfo, "Scene Camera Successfully");
            }
            catch
            {
                base.SceneCamera = null;
                Core.Log(LogType.HtmlWarning, "Scene Camera NOT Created.");
            }

            try
            {
                base.SceneInput = new Input();
                Core.Log(LogType.HtmlInfo, "Scene Input Created Successfully");
            }
            catch
            {
                base.SceneInput = null;
                Core.Log(LogType.HtmlWarning, "Scene Input NOT Created.");
            }

            try
            {
                base.SceneLightEffect = new LightEffect(true, this);
                Core.Log(LogType.HtmlInfo, "Scene Light Effect Successfully");
            }
            catch 
            {
                base.SceneLightEffect = null;
                Core.Log(LogType.HtmlWarning, "Scene Light Effect NOT Created.");
            }

            try
            {
                boundingRenderer = new BoundingRenderer(true, this);
                this.AddObject(boundingRenderer);
            }
            catch { }
        }


        internal void InitializeEnd()
        {
            //Avvia il Garbage Collector per sicurezza
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            //Funziona richiama internamente dal gestore delle scene dopo il caricamento
            loaded = true;
            inLoading = false;

            Core.Log(LogType.HtmlInfo, "Scene Initialization End");

        }

        internal void PreRelease()
        {
            inRelease = true;
        }

        internal void PostRelease()
        {
            //Funziona richiama internamente dal gestore delle scene dopo l'avvenuto rilascio
            try
            {
                //Se lo scene content è stato creato allora lo svuota
                //Se non è stato creato significa che si sta usando quello generale e quindi non lo rilascia
                if (base.SceneContent != null)
                {
                    base.SceneContent.Unload();
                    base.SceneContent = null;
                }
            }
            catch
            {
            }


            try
            {
                if (base.SceneInput != null)
                {
                    base.SceneInput = null;
                }
            }
            catch
            {
            }

            try
            {
                if (base.SceneCamera != null)
                {
                    base.SceneCamera = null;
                }
            }
            catch
            {
            }

            try
            {
                if (boundingRenderer != null)
                {
                    this.RemoveObject(boundingRenderer);
                    boundingRenderer = null;
                }

            }
            catch{}

            inRelease = false;
        }



        private List<Engine2DObject> objects2D = new List<Engine2DObject>();
        private List<Engine3DObject> objects3D = new List<Engine3DObject>();
        private List<EngineTriggerObject> objectsTrigger = new List<EngineTriggerObject>();
        private List<EngineSoundObject> objectsSound = new List<EngineSoundObject>();

        public SceneDrawMode DrawMode { get; set; }
        public Color BackgroundColor { get; set; }
        public bool SortObjects { get; set; }
        public bool DrawBoundings { get; set; }

        public int CountAllObjects { get { return objects2D.Count + objects3D.Count + objectsTrigger.Count + objectsSound.Count; } }
        public int Count2DObjects { get { return objects2D.Count; } }
        public int Count3DObjects { get { return objects3D.Count; } }
        public int CountTriggerObjects { get { return objectsTrigger.Count; } }
        public int CountSoundObjects { get { return objectsSound.Count; } }

        private BoundingRenderer boundingRenderer;

        public bool AddObject(EngineSceneObject newObject)
        {
            try
            {
                if ((IsCreated) && (newObject != null) && (newObject.IsCreated == true))
                {
                    if (newObject is Engine2DObject)
                    {
                        Engine2DObject currentObject = newObject as Engine2DObject;
                        currentObject.DrawOrder = objects2D.Count + 1;
                        objects2D.Add(currentObject);
                    }
                    else if (newObject is Engine3DObject)
                    {
                        Engine3DObject currentObject = newObject as Engine3DObject;
                        objects3D.Add(currentObject);                       
                    }
                    else if (newObject is EngineTriggerObject)
                    {
                        EngineTriggerObject currentObject = newObject as EngineTriggerObject;
                        objectsTrigger.Add(currentObject);
                    }
                    else if (newObject is EngineSoundObject)
                    {
                        EngineSoundObject currentObject = newObject as EngineSoundObject;
                        objectsSound.Add(currentObject);
                    }

                    //Aggiunge i figli
                    //if (newObject is IEngineChildCollector)
                    //    (newObject as IEngineChildCollector).AddChildren();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveObject(EngineSceneObject oldObject)
        {
            try
            {
                if ((IsCreated) && (oldObject != null) && (oldObject.IsCreated == true))
                {
                    //Rimuove i figli
                    //if (oldObject is IEngineChildCollector)
                    //    (oldObject as IEngineChildCollector).RemoveChildren();

                    if (oldObject is Engine2DObject)
                        objects2D.Remove(oldObject as Engine2DObject);
                    else if (oldObject is Engine3DObject)
                        objects3D.Remove(oldObject as Engine3DObject);
                    else if (oldObject is EngineTriggerObject)
                        objectsTrigger.Remove(oldObject as EngineTriggerObject);
                    else if (oldObject is EngineSoundObject)
                        objectsSound.Remove(oldObject as EngineSoundObject);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if ((IsCreated)&&(!InRelease))
            {
                SceneInput.Update(gameTime);
                SceneCamera.Update(gameTime);
                SceneLightEffect.Update(gameTime, base.SceneCamera);

                //Update 3D
                if (this.SortObjects) objects3D.Sort(delegate(Engine3DObject p1, Engine3DObject p2) { return p1.Position.Z.CompareTo(p2.Position.Z); });
                for (int i = 0; i < objects3D.Count; i++)
                {
                    objects3D[i].Update(gameTime);
                }

                //Update 2D
                if (this.SortObjects) objects2D.Sort(delegate(Engine2DObject p1, Engine2DObject p2) { return p1.DrawOrder.CompareTo(p2.DrawOrder); });
                for (int i = 0; i < objects2D.Count; i++)
                {
                    objects2D[i].Update(gameTime);
                }
            }
        }

        public virtual void Draw()
        {
            if ((IsCreated)&&(!InRelease))
            {
                Core.Graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, this.BackgroundColor, 1.0f, 0);

                if ((this.DrawMode == SceneDrawMode.All) || (this.DrawMode == SceneDrawMode.Only3D))
                {
                    for (int i = 0; i < objects3D.Count; i++)
                    {
                        objects3D[i].Draw();

                        boundingRenderer.ReferenceSphere = objects3D[i].BoundingSphere;
                        boundingRenderer.Color = Color.Red;
                        boundingRenderer.Update(null);
                        boundingRenderer.Draw();
                    }
                }

                if ((this.DrawMode == SceneDrawMode.All) || (this.DrawMode == SceneDrawMode.Only2D))
                {
                    for (int i = 0; i < objects2D.Count; i++)
                    {
                        objects2D[i].Draw();
                    }
                }
            }
        }
    }
}
