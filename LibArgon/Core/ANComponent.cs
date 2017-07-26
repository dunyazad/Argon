using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNature
{
    public abstract class ANComponent
    {
        public ANSceneEntity SceneEntity { get; protected set; }

        protected bool dirty = true;
        public bool Dirty { get { return dirty; } set { dirty = value; if (SceneEntity != null) SceneEntity.Dirty = true; } }

        public ANComponent(ANSceneEntity sceneEntity)
        {
            SceneEntity = sceneEntity;
        }

        public abstract void OnInitialize();
        public abstract void OnUpdate(double dt);
        public abstract void OnRender();
        public abstract void OnTerminate();
    }
}
