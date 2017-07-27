using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNature
{
    public abstract class SceneObject : ObjectBase
    {
        public Scene Scene { get; protected set; }

        public SceneObject(Scene scene, string name)
            : base(name)
        {
            Scene = scene;
        }

        ~SceneObject()
        {

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

        public virtual void Resize(float width, float height)
        {
            if (OnResize != null)
            {
                OnResize(this, width, height);
            }
        }

        public delegate void UpdateDelegate(SceneObject sceneObject, double dt);
        public event UpdateDelegate OnUpdate;

        public delegate void RenderDelegate(SceneObject sceneObject);
        public event RenderDelegate OnRender;

        public delegate void ResizeDelegate(SceneObject sceneObject, float width, float height);
        public event ResizeDelegate OnResize;
    }
}
