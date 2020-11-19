using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SceneGraph
{
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


    public abstract class EngineSceneObject : EngineObject
    {
        private Scene parentScene;
        public Scene ParentScene { get { return parentScene; } }

        public EngineSceneObject(Scene parentScene)
        {
            this.parentScene = parentScene;
        }
    }

    public abstract class Engine2DObject : EngineSceneObject
    {
        public bool ToDraw { get; set; }
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

        public virtual void Update(GameTime gametime, Camera camera) {}
        public virtual void Draw(LightEffect lightEffect) { }   
    }


    public abstract class Engine3DObject : EngineSceneObject
    {
        public bool ToDraw { get; set; }
        public Matrix FinalMatrix { get; set; }

        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public BoundingBox BoundingBox { get; set; }

        private bool inFrustrum;
        public bool InFrustrum { get { return inFrustrum; } }

        public Engine3DObject(Scene parentScene): base(parentScene)
        {
            this.ToDraw = true;
            this.FinalMatrix = new Matrix();

            this.Position = new Vector3(0, 0, 0);
            this.Rotation = new Vector3(0, 0, 0);
            this.Scale = new Vector3(1, 1, 1);
            this.BoundingBox = new BoundingBox();
            this.inFrustrum = true;
        }

        public virtual void Update(GameTime gametime, Camera camera)
        {
            if (camera.Frustrum.Contains(this.BoundingBox) == ContainmentType.Disjoint)
            {
                this.inFrustrum = false;
            }
        }

        public virtual void Draw(LightEffect lightEffect) { }
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
