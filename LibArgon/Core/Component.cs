using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNature
{
    public abstract class Component : ObjectBase
    {
        public SceneEntity SceneEntity { get; set; }

        protected bool dirty = true;
        public bool Dirty { get { return dirty; } set { dirty = value; if (SceneEntity != null) SceneEntity.Dirty = true; } }

        public Component(SceneEntity sceneEntity, string name)
            : base(name)
        {
            SceneEntity = sceneEntity;
        }

        ~Component()
        {
        }

        public abstract void OnUpdate(double dt);
        public abstract void OnRender();
    }
}
