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
    public class Shader : Component
    {
        public static string ShaderRootPath = "./Resource/Shader/";
        private int program;
        public int Program { get { return program; } private set { } }

        //Dictionary<string, GraphicsBufferArray> vaos = new Dictionary<string, GraphicsBufferArray>();
        public Dictionary<string, int> UniformIDs { get; private set; } = new Dictionary<string, int>();
        public Dictionary<string, int> AttributeIDs { get; private set; } = new Dictionary<string, int>();

        public Shader(string name)
            : base(name)
        {
            var vsFileStream = Resources.GetFile(ShaderRootPath + Name + ".vs");
            string vsCode;
            using (var sr = new StreamReader(vsFileStream))
            {
                vsCode = sr.ReadToEnd();
            }
            int vs = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vs, vsCode);
            GL.CompileShader(vs);

            var fsFileStream = Resources.GetFile(ShaderRootPath + Name + ".fs");
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
                StringBuilder attributeName = new StringBuilder();

                GL.GetActiveAttrib(program, i, (int)ProgramInterfaceParameter.MaxNameLength, out length, out size, out type, attributeName);

                AttributeIDs.Add(attributeName.ToString(), GL.GetAttribLocation(program, attributeName.ToString()));
            }

            int uniformCount;
            GL.GetProgramInterface(program, ProgramInterface.Uniform, ProgramInterfaceParameter.ActiveResources, out uniformCount);
            for (int i = 0; i < uniformCount; i++)
            {
                int size;
                int length;
                ActiveUniformType type;
                StringBuilder uniformName = new StringBuilder();

                GL.GetActiveUniform(program, i, (int)ProgramInterfaceParameter.MaxNameLength, out length, out size, out type, uniformName);
                UniformIDs.Add(uniformName.ToString(), GL.GetUniformLocation(program, uniformName.ToString()));
            }
        }

        ~Shader()
        {
            
        }

        public override void OnUpdate(double dt)
        {
        }

        public override void OnRender(SceneEntity entity)
        {
        }

        public void Use()
        {
            GL.UseProgram(program);
        }

        public void Unuse()
        {
            GL.UseProgram(0);
        }

        public void BufferData(GraphicsBufferArray bufferArray)
        {
            GL.UseProgram(program);

            bufferArray.BufferData(this);

            GL.UseProgram(0);
        }

        public void BufferData(GraphicsBufferArray[] bufferArrays)
        {
            GL.UseProgram(program);

            foreach (var bufferArray in bufferArrays)
            {
                bufferArray.BufferData(this);
            }

            GL.UseProgram(0);
        }

        public void Render(GraphicsBufferArray bufferArray)
        {
            bufferArray.Render(this);
        }

        public void Render(GraphicsBufferArray[] bufferArrays)
        {
            foreach (var bufferArray in bufferArrays)
            {
                bufferArray.Render(this);
            }
        }

        public void SetUniformVector2(string uniformName, ref Vector2 value)
        {
            if (UniformIDs.ContainsKey(uniformName))
            {
                int uniformID = UniformIDs[uniformName];
                if (uniformID != -1)
                {
                    GL.Uniform2(uniformID, value);
                }
            }
        }

        public void SetUniformVector3(string uniformName, ref Vector3 value)
        {
            if (UniformIDs.ContainsKey(uniformName))
            {
                int uniformID = UniformIDs[uniformName];
                if (uniformID != -1)
                {
                    GL.Uniform3(uniformID, value);
                }
            }
        }

        public void SetUniformVector4(string uniformName, ref Vector4 value)
        {
            if (UniformIDs.ContainsKey(uniformName))
            {
                int uniformID = UniformIDs[uniformName];
                if (uniformID != -1)
                {
                    GL.Uniform4(uniformID, value);
                }
            }
        }

        public void SetUniformMatrix2(string uniformName, bool transpose, ref Matrix2 value)
        {
            if (UniformIDs.ContainsKey(uniformName))
            {
                int uniformID = UniformIDs[uniformName];
                if (uniformID != -1)
                {
                    GL.UniformMatrix2(uniformID, transpose, ref value);
                }
            }
        }

        public void SetUniformMatrix3(string uniformName, bool transpose, ref Matrix3 value)
        {
            if (UniformIDs.ContainsKey(uniformName))
            {
                int uniformID = UniformIDs[uniformName];
                if (uniformID != -1)
                {
                    GL.UniformMatrix3(uniformID, transpose, ref value);
                }
            }
        }

        public void SetUniformMatrix4(string uniformName, bool transpose, ref Matrix4 value)
        {
            if (UniformIDs.ContainsKey(uniformName))
            {
                int uniformID = UniformIDs[uniformName];
                if (uniformID != -1)
                {
                    GL.UniformMatrix4(uniformID, transpose, ref value);
                }
            }
        }

        public override void CleanUp()
        {
            AttributeIDs.Clear();

            UniformIDs.Clear();

            GL.DeleteProgram(program);
        }
    }
}
