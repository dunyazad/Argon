using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNature
{
    public class Material : Component
    {
        public Shader Shader { get; set; } = Resources.GetShader("Default");

        public List<Texture> Textures { get; protected set; } = new List<Texture>();

        public Material(string name)
            : base(name)
        {
        }

        ~Material()
        {
        }

        public override void OnUpdate(double dt)
        {
        }

        public override void OnRender(SceneEntity entity)
        {
        }

        public override void CleanUp()
        {
        }
    }
}
