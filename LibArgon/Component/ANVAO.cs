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
    public class ANVAO : ANComponent
    {
        public ANShader Shader { get; set; }

        int vao;

        Dictionary<string, ANVBO> vbos = new Dictionary<string, ANVBO>();


        public override void OnInitialize()
        {
            GL.GenVertexArrays(1, out vao);

            foreach (var kvp in vbos)
            {
                kvp.Value.OnInitialize();
            }
        }

        public override void OnRender()
        {
        }

        public override void OnTerminate()
        {
            foreach (var kvp in vbos)
            {
                kvp.Value.OnTerminate();
            }

            GL.DeleteVertexArray(vao);
        }

        public override void OnUpdate(double dt)
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

        public ANVBO CreateVBO(string name)
        {
            if (vbos.ContainsKey(name))
            {
                return vbos[name];
            }
            else
            {
                var vbo = new ANVBO() { Name = name, VAO = this };
                vbos.Add(name, vbo);
                return vbo;
            }
        }
    }
}
