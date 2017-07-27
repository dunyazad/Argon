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
    public abstract class GraphicsBufferBase : Component
    {
        public abstract void BufferData();

        public GraphicsBufferBase(SceneEntity sceneEntity, string name)
            : base(sceneEntity, name)
        {

        }

        ~GraphicsBufferBase()
        {

        }
    }

    public class GraphicsBuffer<T> : GraphicsBufferBase where T : struct
    {
        public GraphicsBufferArray BufferArray { get; set; }
        public int AttributeID { get; private set; }

        protected int vbo;

        public List<T> Datas { get; set; } = new List<T>();

        int dataUnitSize;

        VertexAttribPointerType pointerType;

        public GraphicsBuffer(SceneEntity sceneEntity, GraphicsBufferArray bufferArray, string name)
            : base(sceneEntity, name)
        {
            BufferArray = bufferArray;

            dataUnitSize = System.Runtime.InteropServices.Marshal.SizeOf(default(T));

            GL.GenBuffers(1, out vbo);
            AttributeID = GL.GetAttribLocation(BufferArray.Shader.Program, Name);

            if(typeof(T) == typeof(sbyte))
            {
                pointerType = VertexAttribPointerType.Byte;
            }
            else if (typeof(T) == typeof(byte))
            {
                pointerType = VertexAttribPointerType.UnsignedByte;
            }
            else if (typeof(T) == typeof(short))
            {
                pointerType = VertexAttribPointerType.Short;
            }
            else if (typeof(T) == typeof(ushort))
            {
                pointerType = VertexAttribPointerType.UnsignedShort;
            }
            else if (typeof(T) == typeof(int))
            {
                pointerType = VertexAttribPointerType.Int;
            }
            else if (typeof(T) == typeof(uint))
            {
                pointerType = VertexAttribPointerType.UnsignedInt;
            }
            else if (typeof(T) == typeof(float))
            {
                pointerType = VertexAttribPointerType.Float;
            }
            else if (typeof(T) == typeof(double))
            {
                pointerType = VertexAttribPointerType.Double;
            }
            else if (typeof(T) == typeof(Vector3))
            {
                pointerType = VertexAttribPointerType.Float;
            }
            else if (typeof(T) == typeof(Vector4))
            {
                pointerType = VertexAttribPointerType.Float;
            }
        }

        ~GraphicsBuffer()
        {
        }

        public override void OnRender()
        {
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

        public void AddData(T data)
        {
            Datas.Add(data);
        }

        public void AddData(T[] datas)
        {
            this.Datas.AddRange(datas);
        }

        public override void BufferData()
        {
            Bind();
            GL.BufferData<T>(BufferTarget.ArrayBuffer, (IntPtr)(Datas.Count * dataUnitSize), Datas.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(AttributeID, dataUnitSize / sizeof(float), pointerType, false, 0, 0);
            Unbind();
        }

        public override void CleanUp()
        {
            Datas.Clear();

            GL.DeleteBuffer(vbo);
        }
    }
}
