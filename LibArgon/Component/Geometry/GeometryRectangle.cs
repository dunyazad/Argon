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
            var vboPosition = CreateBuffer<Vector3>("vPosition", GraphicsBufferBase.BufferType.Vertex);
            var vboColor = CreateBuffer<Vector4>("vColor", GraphicsBufferBase.BufferType.Color);
            var vboUV = CreateBuffer<Vector2>("vUV", GraphicsBufferBase.BufferType.UV);
            //var indices = BufferArrays[0].CreateBuffer<uint>("index", GraphicsBufferBase.BufferType.Index);

            vboPosition.AddData(new Vector3[] {
                new Vector3(-0.4f, -0.4f, 0f), new Vector3(0.4f, -0.4f, 0f), new Vector3( 0.4f, 0.4f, 0f),
                new Vector3(-0.4f, -0.4f, 0f), new Vector3(0.4f,  0.4f, 0f), new Vector3(-0.4f, 0.4f, 0f)
            });
            vboColor.AddData(new Vector4[] {
                new Vector4(1f, 0f, 0f, 0.5f), new Vector4(0f, 0f, 1f, 1f), new Vector4(0f, 1f, 0f, 1f),
                new Vector4(1f, 0f, 0f, 0.5f), new Vector4(0f, 0f, 1f, 1f), new Vector4(0f, 1f, 0f, 1f)
            });
            vboUV.AddData(new Vector2[] {
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1),
                new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 1),
            });
            //indices.AddData(0); indices.AddData(1); indices.AddData(2);
            //indices.AddData(3); indices.AddData(4); indices.AddData(5);
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
