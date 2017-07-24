using System;
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
    struct ColouredVertex
    {
        public const int Size = (3 + 4) * 4; // size of struct in bytes

        private readonly Vector3 position;
        private readonly Color4 color;

        public ColouredVertex(Vector3 position, Color4 color)
        {
            this.position = position;
            this.color = color;
        }
    }

    sealed class Matrix4Uniform
    {
        private readonly string name;
        private Matrix4 matrix;

        public Matrix4 Matrix { get { return this.matrix; } set { this.matrix = value; } }

        public Matrix4Uniform(string name)
        {
            this.name = name;
        }

        public void Set(ShaderProgram program)
        {
            // get uniform location
            var i = program.GetUniformLocation(this.name);

            // set uniform value
            GL.UniformMatrix4(i, false, ref this.matrix);
        }
    }

    class View : GameWindow
    {
        private VertexBuffer<ColouredVertex> vertexBuffer;
        private ShaderProgram shaderProgram;
        private VertexArray<ColouredVertex> vertexArray;
        private Matrix4Uniform projectionMatrix;

        public View()
            : base(800, 600, GraphicsMode.Default, "Animation Editor")
        {
            VSync = VSyncMode.Off;
            WindowBorder = WindowBorder.Fixed;

            Debug.WriteLine(GL.GetString(StringName.Version));
        }


        protected override void OnKeyDown(KeyboardKeyEventArgs keyboardKeyEventArgs)
        {
            switch (keyboardKeyEventArgs.Key)
            {
                case Key.Space:
                case Key.Right:
                case Key.BackSpace:
                case Key.Left:
                case Key.Enter:
                case Key.Up:
                case Key.Down:
                case Key.F9:
                    break;
            }

            base.OnKeyDown(keyboardKeyEventArgs);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.vertexBuffer = new VertexBuffer<ColouredVertex>(ColouredVertex.Size);

            this.vertexBuffer.AddVertex(new ColouredVertex(new Vector3(-1, -1, -1.5f), Color4.Lime));
            this.vertexBuffer.AddVertex(new ColouredVertex(new Vector3(1, 1, -1.5f), Color4.Red));
            this.vertexBuffer.AddVertex(new ColouredVertex(new Vector3(1, -1, -1.5f), Color4.Blue));

            var vertexShader = new Shader(ShaderType.VertexShader,
@"#version 130
// a projection transformation to apply to the vertex' position
uniform mat4 projectionMatrix;
// attributes of our vertex
in vec3 vPosition;
in vec4 vColor;
out vec4 fColor; // must match name in fragment shader
void main()
{
    // gl_Position is a special variable of OpenGL that must be set
	gl_Position = projectionMatrix * vec4(vPosition, 1.0);
	fColor = vColor;
}"
                );
            var fragmentShader = new Shader(ShaderType.FragmentShader,
@"#version 130
in vec4 fColor; // must match name in vertex shader
out vec4 fragColor; // first out variable is automatically written to the screen
void main()
{
    fragColor = fColor;
}"
                );

            this.shaderProgram = new ShaderProgram(vertexShader, fragmentShader);

            this.vertexArray = new VertexArray<ColouredVertex>(
                this.vertexBuffer, this.shaderProgram,
                new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, ColouredVertex.Size, 0),
                new VertexAttribute("vColor", 4, VertexAttribPointerType.Float, ColouredVertex.Size, 12)
                );

            this.projectionMatrix = new Matrix4Uniform("projectionMatrix");
            this.projectionMatrix.Matrix = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.PiOver2, 16f / 9, 0.1f, 100f);

            GL.ClearColor(Color4.CornflowerBlue);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (Keyboard[Key.Escape])
                Exit();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            this.shaderProgram.Use();
            this.projectionMatrix.Set(this.shaderProgram);

            // bind vertex buffer and array objects
            this.vertexBuffer.Bind();
            this.vertexArray.Bind();

            // upload vertices to GPU and draw them
            this.vertexBuffer.BufferData();
            this.vertexBuffer.Draw();

            // reset state for potential further draw calls (optional, but good practice)
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.UseProgram(0);

            SwapBuffers();
        }

        [STAThread]
        static void Main()
        {
            using (View view = new View())
            {
                view.Run(30.0);
            }
        }
    }
}
