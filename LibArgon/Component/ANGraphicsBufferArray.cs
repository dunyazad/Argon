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
    public class ANGraphicsBufferArray : ANComponent
    {
        public ANShader Shader { get; set; }

        protected int vao;

        Dictionary<string, ANGraphicsBufferBase> buffers = new Dictionary<string, ANGraphicsBufferBase>();


        public override void OnInitialize()
        {
            GL.GenVertexArrays(1, out vao);

            foreach (var kvp in buffers)
            {
                kvp.Value.OnInitialize();
            }
        }

        public override void OnUpdate(double dt)
        {
        }

        public override void OnRender()
        {
        }

        public override void OnTerminate()
        {
            foreach (var kvp in buffers)
            {
                kvp.Value.OnTerminate();
            }

            GL.DeleteVertexArray(vao);
        }

        public void Bind()
        {
            GL.BindVertexArray(vao);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public ANGraphicsBuffer<T> CreateVBO<T>(string name) where T : struct
        {
            if (buffers.ContainsKey(name))
            {
                return buffers[name] as ANGraphicsBuffer<T>;
            }
            else
            {
                var vbo = new ANGraphicsBuffer<T>() { Name = name, VAO = this };
                buffers.Add(name, vbo);
                return vbo;
            }
        }

        public void BufferData()
        {
            Bind();
            foreach (var kvp in buffers)
            {
                kvp.Value.BufferData();
            }
            Unbind();
        }
    }
}
