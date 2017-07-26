using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNature
{
    public abstract class ANComponent
    {
        public string Name { get; set; }

        public ANSceneEntity SceneEntity { get; set; }

        protected bool dirty = true;
        public bool Dirty { get { return dirty; } set { dirty = value; if (SceneEntity != null) SceneEntity.Dirty = true; } }

        Dictionary<string, ANComponent> containingComponents = new Dictionary<string, ANComponent>();

        public ANComponent()
        {
        }

        public abstract void OnInitialize();
        public abstract void OnUpdate(double dt);
        public abstract void OnRender();
        public abstract void OnTerminate();
    }
}
