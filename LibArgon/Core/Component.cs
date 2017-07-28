using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNature
{
    public abstract class Component : ObjectBase
    {
        public bool Dirty { get; set; } = true;

        public Component(string name)
            : base(name)
        {
        }

        ~Component()
        {
        }

        public abstract void OnUpdate(double dt);
        public abstract void OnRender(SceneEntity entity);
    }
}
