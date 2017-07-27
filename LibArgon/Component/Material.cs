using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNature
{
    public class Material : Component
    {
        protected Shader shader = Resources.GetShader("Default");
        public Shader Shader { get { return shader; } set { shader = value; } }

        public Material(SceneEntity sceneEntity, string name)
            : base(sceneEntity, name)
        {
        }

        ~Material()
        {
        }

        public override void OnUpdate(double dt)
        {
        }

        public override void OnRender()
        {
        }
    }
}
