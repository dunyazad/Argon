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
    public class ANGeometry : ANComponent
    {
        List<ANMaterial> materials = new List<ANMaterial>();

        ANVAO vao;
        ANVBO vboPosition;
        ANVBO vboColor;

        //int attribute_vertexColor;
        //int attribute_vertexPosition;
        int uniform_modelMatrix;
        int uniform_viewMatrix;
        int uniform_projectionMatrix;
        int uniform_mvp;
        //int vbo_position;
        //int vbo_color;
        int vbo_mview;
        Vector3[] vertdata;
        Vector4[] coldata;

        public ANGeometry()
            : base()
        {
        }

        public override void OnInitialize()
        {
            {
                var material = new ANMaterial() { Name = "Default" };
                materials.Add(material);

                vao = material.Shader.CreateVAO("Default");
                vboPosition = vao.CreateVBO("vPosition");
                vboColor = vao.CreateVBO("vColor");
            }

            foreach (var material in materials)
            {
                material.OnInitialize();
            }

            //vbo = new ANVBO();
            //vbo.OnInitialize();

            ///** In this function, we'll start with a call to the GL.CreateProgram() function,
            // * which returns the ID for a new program object, which we'll store in pgmID. */
            //program = GL.CreateProgram();

            //loadShader("./Resource/Shader/Default.vs", ShaderType.VertexShader, program, out vsID);
            //loadShader("./Resource/Shader/Default.fs", ShaderType.FragmentShader, program, out fsID);

            ///** Now that the shaders are added, the program needs to be linked.
            // * Like C code, the code is first compiled, then linked, so that it goes
            // * from human-readable code to the machine language needed. */
            //GL.LinkProgram(program);
            //Console.WriteLine(GL.GetProgramInfoLog(program));

            ///** We have multiple inputs on our vertex shader, so we need to get
            // * their addresses to give the shader position and color information for our vertices.
            // * 
            // * To get the addresses for each variable, we use the 
            // * GL.GetAttribLocation and GL.GetUniformLocation functions.
            // * Each takes the program's ID and the name of the variable in the shader. */
            //attribute_vertexPosition = GL.GetAttribLocation(program, "vPosition");
            //attribute_vertexColor = GL.GetAttribLocation(program, "vColor");
            //uniform_modelMatrix = GL.GetUniformLocation(program, "model");
            //uniform_viewMatrix = GL.GetUniformLocation(program, "view");
            //uniform_projectionMatrix = GL.GetUniformLocation(program, "projection");
            //uniform_mvp = GL.GetUniformLocation(program, "mvp");

            //attribute_vertexPosition = GL.GetAttribLocation(materials[0].Shader.Program, "vPosition");
            //attribute_vertexColor = GL.GetAttribLocation(materials[0].Shader.Program, "vColor");
            uniform_modelMatrix = GL.GetUniformLocation(materials[0].Shader.Program, "model");
            uniform_viewMatrix = GL.GetUniformLocation(materials[0].Shader.Program, "view");
            uniform_projectionMatrix = GL.GetUniformLocation(materials[0].Shader.Program, "projection");
            uniform_mvp = GL.GetUniformLocation(materials[0].Shader.Program, "mvp");




            /** Now our shaders and program are set up, but we need to give them something to draw.
             * To do this, we'll be using a Vertex Buffer Object (VBO).
             * When you use a VBO, first you need to have the graphics card create
             * one, then bind to it and send your information. 
             * Then, when the DrawArrays function is called, the information in
             * the buffers will be sent to the shaders and drawn to the screen. */
            //GL.GenBuffers(1, out vbo_position);
            //GL.GenBuffers(1, out vbo_color);
            GL.GenBuffers(1, out vbo_mview);


            vertdata = new Vector3[] { new Vector3(-0.8f, -0.8f, 0f), new Vector3(0.8f, -0.8f, 0f), new Vector3(0f, 0.8f, 0f) };
            coldata = new Vector4[] { new Vector4(1f, 0f, 0f, 0.5f), new Vector4(0f, 0f, 1f, 1f), new Vector4(0f, 1f, 0f, 1f) };

            Console.WriteLine("ANGeometry OnInitialize");
        }
        public override void OnUpdate(double dt)
        {
            if (Dirty)
            {
                materials[0].Shader.Use();

                vao.Bind();

                vboPosition.Bind();
                //GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(vboPosition.AttributeID, 3, VertexAttribPointerType.Float, false, 0, 0);

                vboColor.Bind();
                //GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color);
                GL.BufferData<Vector4>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector4.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(vboColor.AttributeID, 4, VertexAttribPointerType.Float, true, 0, 0);

                //GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

                vboPosition.Unbind();
                vboColor.Unbind();

                vao.Unbind();

                materials[0].Shader.Unuse();

                Console.WriteLine("ANGeometry OnUpdate, dt : " + dt.ToString());

                Dirty = false;
            }
        }

        public override void OnRender()
        {
            materials[0].Shader.Use();

            vao.Bind();

            vboPosition.Enable();
            vboColor.Enable();
            //GL.EnableVertexAttribArray(attribute_vertexPosition);
            //GL.EnableVertexAttribArray(attribute_vertexColor);



            var modelMatrix = SceneEntity.WorldMatrix;
            GL.UniformMatrix4(uniform_modelMatrix, false, ref modelMatrix);

            var viewMatrix = SceneEntity.Scene.MainCamera.ViewMatrix;
            GL.UniformMatrix4(uniform_viewMatrix, false, ref viewMatrix);

            var projectionMatrix = SceneEntity.Scene.MainCamera.ProjectionMatrix;
            GL.UniformMatrix4(uniform_projectionMatrix, false, ref projectionMatrix);

            var mvp = modelMatrix * viewMatrix * projectionMatrix;
            GL.UniformMatrix4(uniform_mvp, false, ref mvp);




            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            vboPosition.Disable();
            vboColor.Disable();

            //GL.DisableVertexAttribArray(attribute_vertexPosition);
            //GL.DisableVertexAttribArray(attribute_vertexColor);

            vao.Unbind();

            materials[0].Shader.Unuse();

            Console.WriteLine("ANGeometry OnRender");
        }

        public override void OnTerminate()
        {
            //GL.DeleteBuffer(vbo_position);
            //GL.DeleteBuffer(vbo_color);
            GL.DeleteBuffer(vbo_mview);

            vao.OnTerminate();

            Console.WriteLine("ANGeometry OnTerminate");
        }

        /// <summary>
        /// This creates a new shader (using a value from the ShaderType enum), loads code for it, compiles it, and adds it to our program.
        /// It also prints any errors it found to the console, which is really nice for when you make a mistake in a shader (it will also yell at you if you use deprecated code).
        /// </summary>
        /// <param name="filename">File to load the shader from</param>
        /// <param name="type">Type of shader to load</param>
        /// <param name="program">ID of the program to use the shader with</param>
        /// <param name="address">Address of the compiled shader</param>
        void loadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }
    }
}
