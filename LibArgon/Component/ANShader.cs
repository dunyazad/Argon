using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

using ArtificialNature.Core;

namespace ArtificialNature.Component
{
    public class ANShader
    {
        public static string ShaderRootPath = "./Resource/Shader/";
        private readonly int program;
        public int Program { get { return program; } private set { } }

        public ANShader(string name)
        {
            var vsFileStream = ANResources.GetFile(ShaderRootPath + name + ".vs");
            string vsCode;
            using (var sr = new StreamReader(vsFileStream))
            {
                vsCode = sr.ReadToEnd();
            }
            int vs = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vs, vsCode);
            GL.CompileShader(vs);

            var fsFileStream = ANResources.GetFile(ShaderRootPath + name + ".fs");
            string fsCode;
            using (var sr = new StreamReader(fsFileStream))
            {
                fsCode = sr.ReadToEnd();
            }
            int fs = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fs, fsCode);
            GL.CompileShader(fs);

            program = GL.CreateProgram();
            GL.AttachShader(program, vs);
            GL.AttachShader(program, fs);
            GL.LinkProgram(program);
            GL.DetachShader(program, vs);
            GL.DetachShader(program, fs);
        }

        public void SetAttribute(string attributeName, int size, VertexAttribPointerType type, int stride, int offset)
        {
            // get location of attribute from shader program
            int index = GL.GetAttribLocation(program, attributeName);
            GL.VertexAttribPointer(index, size, type, false, stride, offset);
        }

        public void SetUniform1<T>(string uniformName, T value)
        {
            int location = GL.GetUniformLocation(program, uniformName);
        }

        public void Use()
        {
            GL.UseProgram(program);
        }

    }
}
