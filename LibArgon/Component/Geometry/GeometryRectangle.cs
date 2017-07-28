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
    public class GeometryRectangle : GraphicsObject
    {
        public GeometryRectangle(string name)
            : base(name)
        {
            var vboPosition = BufferArrays[0].CreateBuffer<Vector3>("vPosition", GraphicsBufferBase.BufferType.Vertex);
            var vboColor = BufferArrays[0].CreateBuffer<Vector4>("vColor", GraphicsBufferBase.BufferType.Color);

            vboPosition.AddData(new Vector3[] {
                new Vector3(-0.4f, -0.4f, 0f), new Vector3(0.4f, -0.4f, 0f), new Vector3( 0.4f, 0.4f, 0f),
                new Vector3(-0.4f, -0.4f, 0f), new Vector3(0.4f,  0.4f, 0f), new Vector3(-0.4f, 0.4f, 0f)
            });
            vboColor.AddData(new Vector4[] {
                new Vector4(1f, 0f, 0f, 0.5f), new Vector4(0f, 0f, 1f, 1f), new Vector4(0f, 1f, 0f, 1f),
                new Vector4(1f, 0f, 0f, 0.5f), new Vector4(0f, 0f, 1f, 1f), new Vector4(0f, 1f, 0f, 1f)
            });
        }

        ~GeometryRectangle()
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
