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
    public class GraphicsBufferArray : Component
    {
        public Shader Shader { get; set; }

        protected int vao;

        Dictionary<string, GraphicsBufferBase> buffers = new Dictionary<string, GraphicsBufferBase>();

        public GraphicsBufferArray(SceneEntity sceneEntity, Shader shader, string name)
            : base(sceneEntity, name)
        {
            Shader = shader;
            GL.GenVertexArrays(1, out vao);
        }

        ~GraphicsBufferArray()
        {
        }

        public override void OnUpdate(double dt)
        {
        }

        public override void OnRender()
        {
        }

        
        public void Bind()
        {
            GL.BindVertexArray(vao);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public GraphicsBuffer<T> CreateBuffer<T>(string name) where T : struct
        {
            if (buffers.ContainsKey(name))
            {
                return buffers[name] as GraphicsBuffer<T>;
            }
            else
            {
                var vbo = new GraphicsBuffer<T>(SceneEntity, this, name);
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

        public override void CleanUp()
        {
            foreach (var kvp in buffers)
            {
                kvp.Value.CleanUp();
            }

            buffers.Clear();

            GL.DeleteVertexArray(vao);
        }
    }
}
