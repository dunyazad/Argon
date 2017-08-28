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
    public class GeometryRoundedRectangle : GraphicsObject
    {
        protected float width;
        protected float height;
        protected float w;
        protected float h;

        public float Width { get { return width; } set { width = value; Dirty = true; } }
        public float Height { get { return height; } set { height = value; Dirty = true; } }
        public float W { get { return w; } set { w = value; Dirty = true; } }
        public float H { get { return h; } set { h = value; Dirty = true; } }

        public GeometryRoundedRectangle(string name, float width, float height, float w, float h)
            : base(name)
        {
            this.width = width;
            this.height = height;
            this.w = w;
            this.h = h;

            var vboPosition = CreateBuffer<Vector3>("vPosition", GraphicsBufferBase.BufferType.Vertex);
            var vboColor = CreateBuffer<Vector4>("vColor", GraphicsBufferBase.BufferType.Color);
            //var vboUV = CreateBuffer<Vector2>("vUV", GraphicsBufferBase.BufferType.UV);
            var indices = CreateBuffer<uint>("index", GraphicsBufferBase.BufferType.Index);

            //var offset = Vector3.Zero;
            //float offsetW = width * 0.5f - w;
            //if (offsetW < 0) { offsetW = 0; w = width * 0.5f; }
            //float offsetH = height * 0.5f - h;
            //if (offsetH < 0) { offsetH = 0; h = height * 0.5f; }

            //uint segments = 36;
            //for (uint i = 0; i < segments; i++)
            //{
            //    var x = (float)Math.Cos(i * (360 / segments) * (Math.PI / 180));
            //    var y = (float)Math.Sin(i * (360 / segments) * (Math.PI / 180));

            //    var px = x > 0 ? x * w + offsetW : x * w - offsetW;
            //    var py = y > 0 ? y * h + offsetH : y * h - offsetH;
            //    vboPosition.AddData(new Vector3(px, py, 0));

            //    vboColor.AddData(new Vector4(1, 0, 0, 1));

            //    indices.AddData(segments); indices.AddData(i); indices.AddData(i + 1);
            //}
            //vboPosition.AddData(new Vector3(0, 0, 0));
            //vboColor.AddData(new Vector4(2, 2, 2, 1));

            //indices.AddData(segments); indices.AddData(segments - 1); indices.AddData(0);

            Dirty = true;
        }

        ~GeometryRoundedRectangle()
        {

        }

        public override void OnUpdate(double dt)
        {
            if (Dirty)
            {
                var vboPosition = Buffers[GraphicsBufferBase.BufferType.Vertex] as GraphicsBuffer<Vector3>;
                var vboColor = Buffers[GraphicsBufferBase.BufferType.Color] as GraphicsBuffer<Vector4>;
                //var vboUV = CreateBuffer<Vector2>("vUV", GraphicsBufferBase.BufferType.UV);
                var indices = Buffers[GraphicsBufferBase.BufferType.Index] as GraphicsBuffer<uint>;

                vboPosition.ClearData();
                vboColor.ClearData();
                indices.ClearData();

                var halfWidth = width * 0.5f;
                var halfHeight = height * 0.5f;

                if (W < 0) W = 0;
                if (H < 0) H = 0;

                var offset = Vector3.Zero;
                float offsetW = halfWidth - w;
                if (offsetW < 0) { offsetW = 0; w = halfWidth; }
                float offsetH = halfHeight - h;
                if (offsetH < 0) { offsetH = 0; h = halfHeight; }

                uint segments = 36;
                for (uint i = 0; i < segments; i++)
                {
                    var x = (float)Math.Cos(i * (360 / segments) * (Math.PI / 180));
                    var y = (float)Math.Sin(i * (360 / segments) * (Math.PI / 180));

                    var px = x > 0 ? x * w + offsetW : x * w - offsetW;
                    var py = y > 0 ? y * h + offsetH : y * h - offsetH;
                    vboPosition.AddData(new Vector3(px, py, 0));

                    vboColor.AddData(new Vector4(Color.R, Color.G, Color.B, Color.A));

                    indices.AddData(segments); indices.AddData(i); indices.AddData(i + 1);
                }
                vboPosition.AddData(new Vector3(0, 0, 0));
                vboColor.AddData(new Vector4(Color.R, Color.G, Color.B, Color.A));

                indices.AddData(segments); indices.AddData(segments - 1); indices.AddData(0);
            }

            base.OnUpdate(dt);
        }

        public override void OnRender(SceneEntity entity)
        {
            base.OnRender(entity);
        }
    }
}
