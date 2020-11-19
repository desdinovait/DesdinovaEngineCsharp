using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace DesdinovaModelPipeline
{

    public interface IEngineChildCollector
    {
        bool AddChildren();
        bool RemoveChildren();    
    }


    public abstract class EngineObject
    {
        public string ID { get; set; }
        public Object Tag { get; set; }
        public bool IsCreated { get; set; }
        
        private DateTime timeStamp;
        public DateTime TimeStamp { get { return timeStamp; } }

        private string guid;
        public string Guid { get { return guid; } }

        public EngineObject()
        {
            this.ID = string.Empty;
            this.Tag = null;
            this.IsCreated = false;

            this.guid = System.Guid.NewGuid().ToString();
            this.timeStamp = DateTime.Now;
        }
    }

    public abstract class EngineScene : EngineObject
    {
        //Content Manager di base (per gli elementi da caricare)
        private ContentManager sceneContent;
        public ContentManager SceneContent
        {
            get { return sceneContent; }
            set { sceneContent = value; }
        }

        //Input di base
        private Input sceneInput;
        public Input SceneInput
        {
            get { return sceneInput; }
            set { sceneInput = value; }
        }

        //Camera di base
        private Camera sceneCamera;
        public Camera SceneCamera
        {
            get { return sceneCamera; }
            set { sceneCamera = value; }
        }

        //LightEffect
        private LightEffect sceneLightEffect;
        public LightEffect SceneLightEffect
        {
            get { return sceneLightEffect; }
            set { sceneLightEffect = value; }
        }
    }


    public abstract class EngineSceneObject : EngineObject
    {
        private Scene parentScene;
        public Scene ParentScene { get { return parentScene; } }

        public EngineSceneObject(Scene parentScene)
        {
            this.parentScene = parentScene;
        }
    }

    public abstract class EngineSceneDrawableObject : EngineSceneObject
    {
        public bool ToDraw { get; set; }
        public virtual void Update(GameTime gametime) { }
        public virtual void Draw() { }

        public EngineSceneDrawableObject(Scene parentScene):base(parentScene)
        {

        }
    }

    public abstract class Engine2DObject : EngineSceneDrawableObject
    {
        public Matrix FinalMatrix { get; set; }

        public Vector2 Position { get; set; }
        public Vector2 Rotation { get; set; }
        public Vector2 Scale { get; set; }
        public BoundingBox BoundingBox { get; set; }
        public int DrawOrder { get; set; }

        public Engine2DObject(Scene parentScene): base(parentScene)
        {
            this.ToDraw = true;
            this.FinalMatrix = new Matrix();

            this.Position = new Vector2(0, 0);
            this.Rotation = new Vector2(0, 0);
            this.Scale = new Vector2(1, 1);
            this.BoundingBox = new BoundingBox();
            this.DrawOrder = 0;
        }

        public override void Update(GameTime gametime) {}
        public override void Draw() { }   
    }


    public abstract class Engine3DObject : EngineSceneDrawableObject
    {
        public Matrix FinalMatrix { get; set; }

        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public BoundingSphere BoundingSphere { get; set; }

        private bool inFrustrum;
        public bool InFrustrum { get { return inFrustrum; } }

        public Engine3DObject(Scene parentScene): base(parentScene)
        {
            this.ToDraw = true;
            this.FinalMatrix = new Matrix();

            this.Position = new Vector3(0, 0, 0);
            this.Rotation = new Vector3(0, 0, 0);
            this.Scale = new Vector3(1, 1, 1);
            this.BoundingSphere = new BoundingSphere();
            this.inFrustrum = true;
        }

        public override void Update(GameTime gametime)
        {
            if (base.ParentScene.SceneCamera.Frustrum.Contains(this.BoundingSphere) == ContainmentType.Disjoint)
            {
                this.inFrustrum = false;
            }
        }

        public override void Draw() { }
    }


    public delegate void EngineTriggerEventHandler(object source, bool active);
    public abstract class EngineTriggerObject : EngineSceneObject
    {
        private bool isActive;
        public bool IsActive 
        { 
            get { return isActive; }
            set 
            {
                isActive = value;
                if (isActive == true)
                {
                    if (Activated != null) Activated(this, true);
                }
                else
                {
                    if (DeActivated != null) DeActivated(this, false);
                }
           } 
        }
        public event EngineTriggerEventHandler Activated;
        public event EngineTriggerEventHandler DeActivated;

        public EngineTriggerObject(Scene parentScene):base(parentScene)
        {
            this.IsActive = true;
        }
    }


    public abstract class EngineSoundObject : EngineSceneObject
    {
        public string Name { get; set; }

        public abstract void Play();
        public abstract void Stop();
        public abstract void Pause();

        public EngineSoundObject(Scene parentScene):base(parentScene)
        {

        }
    }
}
