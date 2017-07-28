using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using QuickFont;
using QuickFont.Configuration;

namespace ArtificialNature
{
    public class GeometryCircle : GraphicsObject
    {
        public GeometryCircle(string name, float radius, uint segment)
            : base(name)
        {
            var vboPosition = BufferArrays[0].CreateBuffer<Vector3>("vPosition", GraphicsBufferBase.BufferType.Vertex);
            var vboColor = BufferArrays[0].CreateBuffer<Vector4>("vColor", GraphicsBufferBase.BufferType.Color);
            var indices = BufferArrays[0].CreateBuffer<uint>("index", GraphicsBufferBase.BufferType.Index);

            vboPosition.AddData(new Vector3(0, 0, 0));

            for (uint i = 0; i < segment; i++)
            {
                float x = (float)Math.Sin(i);
                float y = (float)Math.Cos(i);

                vboPosition.AddData(new Vector3(x * radius, y * radius, 0));
                vboColor.AddData(new Vector4(x, y, (x + y) / 2, 1));

                indices.AddData(0); indices.AddData(i); indices.AddData(i + 1);
            }
        }

        ~GeometryCircle()
        {

        }

        public override void OnUpdate(double dt)
        {
            base.OnUpdate(dt);
        }

        public override void OnRender(SceneEntity entity)
        {
            base.OnRender(entity);
        }
    }
}
