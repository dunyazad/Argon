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
    public abstract class ANGraphicsBufferBase : ANComponent
    {
        public abstract void BufferData();
    }

    public class ANGraphicsBuffer<T> : ANGraphicsBufferBase where T : struct
    {
        public ANGraphicsBufferArray VAO { get; set; }
        public int AttributeID { get; private set; }

        protected int vbo;

        public List<T> Datas { get; set; } = new List<T>();

        int dataUnitSize;

        public override void OnInitialize()
        {
            Datas = new List<T>();

            dataUnitSize = System.Runtime.InteropServices.Marshal.SizeOf(default(T));

            GL.GenBuffers(1, out vbo);
            AttributeID = GL.GetAttribLocation(VAO.Shader.Program, Name);
        }

        public override void OnRender()
        {
        }

        public override void OnTerminate()
        {
            GL.DeleteBuffer(vbo);
        }

        public override void OnUpdate(double dt)
        {
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Enable()
        {
            GL.EnableVertexAttribArray(AttributeID);
        }

        public void Disable()
        {
            GL.DisableVertexAttribArray(AttributeID);
        }

        public void AddData(T data)
        {
            Datas.Add(data);
        }

        public void AddData(T[] datas)
        {
            this.Datas.AddRange(datas);
        }

        public override void BufferData()
        {
            Bind();
            GL.BufferData<T>(BufferTarget.ArrayBuffer, (IntPtr)(Datas.Count * dataUnitSize), Datas.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(AttributeID, dataUnitSize / sizeof(float), VertexAttribPointerType.Float, false, 0, 0);
            Unbind();
        }
    }
}
