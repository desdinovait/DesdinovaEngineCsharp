using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SceneGraph
{
    public enum DrawMode
    { 
        All = 1,
        Only2D = 2,
        Only3D = 3
    }
    
    public class Scene : EngineObject
    {
        public string Name { get; set; }

        public GraphicsDevice Device { get; set; }

        public ContentManager Content { get; set; }
        
        public Camera Camera { get; set; }

        private List<Engine2DObject> objects2D = new List<Engine2DObject>();
        private List<Engine3DObject> objects3D = new List<Engine3DObject>();
        private List<EngineTriggerObject> objectsTrigger = new List<EngineTriggerObject>();
        private List<EngineSoundObject> objectsSound = new List<EngineSoundObject>();

        public DrawMode DrawMode { get; set; }
        public Color BackgroundColor { get; set; }
        public LightEffect LightEffect { get; set; }

        public int CountAllObjects { get { return objects2D.Count + objects3D.Count + objectsTrigger.Count + objectsSound.Count; } }
        public int Count2DObjects { get { return objects2D.Count; } }
        public int Count3DObjects { get { return objects3D.Count; } }
        public int CountTriggerObjects { get { return objectsTrigger.Count; } }
        public int CountSoundObjects { get { return objectsSound.Count; } }

        public Scene(string name, GraphicsDevice device, GameServiceContainer service)
        {
            try
            {
                Content = new ContentManager(service);
                Content.RootDirectory = "Content";
                Device = device;
                Name = name;
                IsCreated = true;
                DrawMode = DrawMode.All;
                BackgroundColor = Color.Gray;
            }
            catch
            {
                IsCreated = false;
            }
        }

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

        public virtual void Update(GameTime gametime)
        {
            if (IsCreated)
            {
                //Update 3D
                objects3D.Sort(delegate(Engine3DObject p1, Engine3DObject p2) { return p1.Position.Z.CompareTo(p2.Position.Z); });
                objects3D.ForEach(delegate(Engine3DObject currentObject)
                {
                    currentObject.Update(gametime, this.Camera);
                });

                //Update 2D
                objects2D.Sort(delegate(Engine2DObject p1, Engine2DObject p2) { return p1.DrawOrder.CompareTo(p2.DrawOrder); });
                objects2D.ForEach(delegate(Engine2DObject currentObject)
                {
                    currentObject.Update(gametime, this.Camera);
                });
            }
        }

        public virtual void Draw()
        {
            if (IsCreated)
            {
                this.Device.Clear(this.BackgroundColor);

                if ((this.DrawMode == DrawMode.All) || (this.DrawMode == DrawMode.Only3D))
                {
                    objects3D.ForEach(delegate(Engine3DObject currentObject)
                    {
                        currentObject.Draw(this.LightEffect);
                    });
                }

                if ((this.DrawMode == DrawMode.All) || (this.DrawMode == DrawMode.Only2D))
                {
                    objects2D.ForEach(delegate(Engine2DObject currentObject)
                    {
                        currentObject.Draw(this.LightEffect);
                    });
                }
            }
        }
    }
}
