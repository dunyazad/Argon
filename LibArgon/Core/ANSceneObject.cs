using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNature
{
    public abstract class ANSceneObject
    {
        public ANScene Scene { get; protected set; }

        public string Name { get; set; }
        public ANSceneObject(ANScene scene, string name)
        {
            Name = name;
            Scene = scene;
        }

        protected Guid id;
        public ANSceneObject()
        {
            id = Guid.NewGuid();
        }

        public virtual void Initialize()
        {
            if (OnInitialize != null)
            {
                OnInitialize(this);
            }
        }
        public virtual void Update(double dt)
        {
            if (OnUpdate != null)
            {
                OnUpdate(this, dt);
            }
        }
        public virtual void Render()
        {
            if (OnRender != null)
            {
                OnRender(this);
            }
        }
        public virtual void Terminate()
        {
            if (OnTerminate != null)
            {
                OnTerminate(this);
            }
        }

        public virtual void Resize(float width, float height)
        {
            if (OnResize != null)
            {
                OnResize(this, width, height);
            }
        }

        public delegate void InitializeDelegate(ANSceneObject sceneObject);
        public event InitializeDelegate OnInitialize;

        public delegate void UpdateDelegate(ANSceneObject sceneObject, double dt);
        public event UpdateDelegate OnUpdate;

        public delegate void RenderDelegate(ANSceneObject sceneObject);
        public event RenderDelegate OnRender;

        public delegate void TerminateDelegate(ANSceneObject sceneObject);
        public event TerminateDelegate OnTerminate;

        public delegate void ResizeDelegate(ANSceneObject sceneObject, float width, float height);
        public event ResizeDelegate OnResize;
    }
}
