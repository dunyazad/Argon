using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNature.Core
{
    public abstract class ANComponent
    {
        public ANSceneObject SceneObject { get; protected set; }

        public ANComponent(ANSceneObject sceneObject)
        {
            SceneObject = sceneObject;
        }

        public abstract void OnInitialize();
        public abstract void OnUpdate(double dt);
        public abstract void OnRender();
        public abstract void OnTerminate();
    }
}
