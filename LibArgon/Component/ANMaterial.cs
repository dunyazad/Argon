using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNature
{
    public class ANMaterial : ANComponent
    {
        protected ANShader shader = ANResources.GetShader("Default");
        public ANShader Shader { get { return shader; } set { shader = value; } }

        public ANMaterial()
        {
        }

        public override void OnInitialize()
        {
            Shader.OnInitialize();
        }

        public override void OnUpdate(double dt)
        {
        }

        public override void OnRender()
        {
        }

        public override void OnTerminate()
        {
        }
    }
}
