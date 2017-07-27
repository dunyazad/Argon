using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
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

        Dictionary<string, ANGraphicsBufferArray> vaos = new Dictionary<string, ANGraphicsBufferArray>();
        public Dictionary<string, int> UniformIDs { get; private set; } = new Dictionary<string, int>();
        public Dictionary<string, int> AttributeIDs { get; private set; } = new Dictionary<string, int>();

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


                int attributeCount;
                GL.GetProgramInterface(program, ProgramInterface.ProgramInput, ProgramInterfaceParameter.ActiveResources, out attributeCount);
                for (int i = 0; i < attributeCount; i++)
                {
                    int size;
                    int length;
                    ActiveAttribType type;
                    StringBuilder name = new StringBuilder();

                    GL.GetActiveAttrib(program, i, (int)ProgramInterfaceParameter.MaxNameLength, out length, out size, out type, name);
                    AttributeIDs.Add(name.ToString(), i);
                }

                int uniformCount;
                GL.GetProgramInterface(program, ProgramInterface.Uniform, ProgramInterfaceParameter.ActiveResources, out uniformCount);
                for (int i = 0; i < uniformCount; i++)
                {
                    int size;
                    int length;
                    ActiveUniformType type;
                    StringBuilder name = new StringBuilder();

                    GL.GetActiveUniform(program, i, (int)ProgramInterfaceParameter.MaxNameLength, out length, out size, out type, name);
                    UniformIDs.Add(name.ToString(), i);
                }

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

        public ANGraphicsBufferArray GetBufferArray(string name)
        {
            if(vaos.ContainsKey(name))
            {
                return vaos[name];
            }
            else
            {
                var vao = new ANGraphicsBufferArray() { Name = name, Shader = this };
                vaos.Add(name, vao);
                return vao;
            }
        }

        int GetUniformID(string uniformName)
        {
            if(UniformIDs.ContainsKey(uniformName))
            {
                return UniformIDs[uniformName];
            }
            else
            {
                return -1;
            }
        }

        int AttributeID(string attributeName)
        {
            if (AttributeIDs.ContainsKey(attributeName))
            {
                return AttributeIDs[attributeName];
            }
            else
            {
                return -1;
            }
        }

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
