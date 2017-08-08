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
        public GeometryCircle(string name, float radius, uint segments)
            : base(name)
        {
            var vboPosition = BufferArrays[0].CreateBuffer<Vector3>("vPosition", GraphicsBufferBase.BufferType.Vertex);
            var vboColor = BufferArrays[0].CreateBuffer<Vector4>("vColor", GraphicsBufferBase.BufferType.Color);
            var indices = BufferArrays[0].CreateBuffer<uint>("index", GraphicsBufferBase.BufferType.Index);

            for (uint i = 0; i < segments; i++)
            {
                var x = (float)Math.Cos(i * (360 / segments) * (Math.PI / 180));
                var y = (float)Math.Sin(i * (360 / segments) * (Math.PI / 180));

                vboPosition.AddData(new Vector3(x * radius, y * radius, 0));

                vboColor.AddData(new Vector4(1, 0, 0, 1));

                indices.AddData(segments); indices.AddData(i); indices.AddData(i + 1);
            }
            vboPosition.AddData(new Vector3(0, 0, 0));
            vboColor.AddData(new Vector4(2, 2, 2, 1));

            indices.AddData(segments); indices.AddData(segments - 1); indices.AddData(0);
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
