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
    public class GeometryTriangle : GraphicsObject
    {
        public GeometryTriangle(string name)
            : base(name)
        {
            var vboPosition = CreateBuffer<Vector3>("vPosition", GraphicsBufferBase.BufferType.Vertex);
            var vboColor = CreateBuffer<Vector4>("vColor", GraphicsBufferBase.BufferType.Color);
            //var indices = CreateBuffer<uint>("index", GraphicsBufferBase.BufferType.Index);

            vboPosition.AddData(new Vector3[] { new Vector3(-0.4f, -0.4f, 0f), new Vector3(0.4f, -0.4f, 0f), new Vector3(0f, 0.4f, 0f) });
            vboColor.AddData(new Vector4[] { new Vector4(1f, 0f, 0f, 0.5f), new Vector4(0f, 0f, 1f, 1f), new Vector4(0f, 1f, 0f, 1f) });
            //indices.AddData(0); indices.AddData(1); indices.AddData(2);
        }

        ~GeometryTriangle()
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
