using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ArtificialNature.Core;

namespace ArtificialNature.Component
{
    public class ANMaterial
    {
        public string Name { get; set; }

        protected ANShader shader = ANResources.GetShader("Default");
        public ANShader Shader { get { return shader; } set { shader = value; } }

        public ANMaterial(string name)
        {
            Name = name;
        }
    }
}
