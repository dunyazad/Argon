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
    public class ANVBO : ANComponent
    {
        public ANVAO VAO { get; set; }
        public int AttributeID { get; private set; }

        int vbo;

        public override void OnInitialize()
        {
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
    }
}
