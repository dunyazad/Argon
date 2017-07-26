using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace ArtificialNature
{
    public class ANShader : ANComponent
    {
        bool initialized = false;

        public static string ShaderRootPath = "./Resource/Shader/";
        private int program;
        public int Program { get { return program; } private set { } }

        Dictionary<string, ANVAO> vaos = new Dictionary<string, ANVAO>();

        Dictionary<string, int> uniformIDs = new Dictionary<string, int>();

        public override void OnInitialize()
        {
            if (!initialized)
            {
                var vsFileStream = ANResources.GetFile(ShaderRootPath + Name + ".vs");
                string vsCode;
                using (var sr = new StreamReader(vsFileStream))
                {
                    vsCode = sr.ReadToEnd();
                }
                int vs = GL.CreateShader(ShaderType.VertexShader);
                GL.ShaderSource(vs, vsCode);
                GL.CompileShader(vs);

                var fsFileStream = ANResources.GetFile(ShaderRootPath + Name + ".fs");
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

                foreach (var kvp in vaos)
                {
                    kvp.Value.OnInitialize();
                }

                initialized = true;
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
            foreach (var kvp in vaos)
            {
                kvp.Value.OnTerminate();
            }
        }

        public void Use()
        {
            GL.UseProgram(program);
        }

        public void Unuse()
        {
            GL.UseProgram(0);
        }

        public ANVAO CreateVAO(string name)
        {
            if(vaos.ContainsKey(name))
            {
                return vaos[name];
            }
            else
            {
                var vao = new ANVAO() { Name = name, Shader = this };
                vaos.Add(name, vao);
                return vao;
            }
        }

        int GetUniformID(string uniformName)
        {
            if(uniformIDs.ContainsKey(uniformName))
            {
                return uniformIDs[uniformName];
            }
            else
            {
                int uniformID = GL.GetUniformLocation(program, uniformName);
                uniformIDs[uniformName] = uniformID;
                return uniformID;
            }
        }

        //public void SetAttribute(string attributeName, int size, VertexAttribPointerType type, int stride, int offset)
        //{
        //    // get location of attribute from shader program
        //    int index = GL.GetAttribLocation(program, attributeName);
        //    GL.VertexAttribPointer(index, size, type, false, stride, offset);
        //}

        public void SetUniformVector2(string uniformName, ref Vector2 value)
        {
            GL.Uniform2(GetUniformID(uniformName), value);
        }

        public void SetUniformVector3(string uniformName, ref Vector3 value)
        {
            GL.Uniform3(GetUniformID(uniformName), value);
        }

        public void SetUniformVector4(string uniformName, ref Vector4 value)
        {
            GL.Uniform4(GetUniformID(uniformName), ref value);
        }

        public void SetUniformMatrix2(string uniformName, bool transpose, ref Matrix2 value)
        {
            GL.UniformMatrix2(GetUniformID(uniformName), transpose, ref value);
        }

        public void SetUniformMatrix3(string uniformName, bool transpose, ref Matrix3 value)
        {
            GL.UniformMatrix3(GetUniformID(uniformName), transpose, ref value);
        }

        public void SetUniformMatrix4(string uniformName, bool transpose, ref Matrix4 value)
        {
            GL.UniformMatrix4(GetUniformID(uniformName), transpose, ref value);
        }
    }
}
