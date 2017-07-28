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
        public enum BufferType { Vertex, Index, Normal, Color, UV, Other };
        protected BufferType bufferType = BufferType.Other;

        public GraphicsBufferArray BufferArray { get; set; }

        public string AttributeName { get; set; }
        public abstract void BufferData(Shader shader);

        public abstract int DataCount();

        public abstract void Bind();
        public abstract void Unbind();

        public GraphicsBufferBase(GraphicsBufferArray bufferArray, string name, string attributeName, BufferType bufferType)
            : base(name)
        {
            BufferArray = bufferArray;
            AttributeName = attributeName;
            this.bufferType = bufferType;
        }

        ~GraphicsBufferBase()
        {

        }
    }

    public class GraphicsBuffer<T> : GraphicsBufferBase where T : struct
    {
        protected int vbo;

        public List<T> Datas { get; set; } = new List<T>();

        int dataUnitSize;
        int dataUnitCount;

        VertexAttribPointerType pointerType;

        public GraphicsBuffer(GraphicsBufferArray bufferArray, string name, string attributeName, BufferType bufferType)
            : base(bufferArray, name, attributeName, bufferType)
        {
            BufferArray = bufferArray;

            GL.GenBuffers(1, out vbo);

            if(typeof(T) == typeof(sbyte))
            {
                pointerType = VertexAttribPointerType.Byte;
                dataUnitSize = sizeof(sbyte);
                dataUnitCount = 1;
            }
            else if (typeof(T) == typeof(byte))
            {
                pointerType = VertexAttribPointerType.UnsignedByte;
                dataUnitSize = sizeof(byte);
                dataUnitCount = 1;
            }
            else if (typeof(T) == typeof(short))
            {
                pointerType = VertexAttribPointerType.Short;
                dataUnitSize = sizeof(short);
                dataUnitCount = 1;
            }
            else if (typeof(T) == typeof(ushort))
            {
                pointerType = VertexAttribPointerType.UnsignedShort;
                dataUnitSize = sizeof(ushort);
                dataUnitCount = 1;
            }
            else if (typeof(T) == typeof(int))
            {
                pointerType = VertexAttribPointerType.Int;
                dataUnitSize = sizeof(int);
                dataUnitCount = 1;
            }
            else if (typeof(T) == typeof(uint))
            {
                pointerType = VertexAttribPointerType.UnsignedInt;
                dataUnitSize = sizeof(uint);
                dataUnitCount = 1;
            }
            else if (typeof(T) == typeof(float))
            {
                pointerType = VertexAttribPointerType.Float;
                dataUnitSize = sizeof(float);
                dataUnitCount = 1;
            }
            else if (typeof(T) == typeof(double))
            {
                pointerType = VertexAttribPointerType.Double;
                dataUnitSize = sizeof(double);
                dataUnitCount = 1;
            }
            else if (typeof(T) == typeof(Vector3))
            {
                pointerType = VertexAttribPointerType.Float;
                dataUnitSize = sizeof(float);
                dataUnitCount = 3;
            }
            else if (typeof(T) == typeof(Vector4))
            {
                pointerType = VertexAttribPointerType.Float;
                dataUnitSize = sizeof(float);
                dataUnitCount = 4;
            }
        }

        ~GraphicsBuffer()
        {
        }

        public override void OnRender(SceneEntity entity)
        {
        }

        public override void OnUpdate(double dt)
        {
        }

        public override int DataCount()
        {
            return Datas.Count;
        }

        public override void Bind()
        {
            if(bufferType == BufferType.Index)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, vbo);
            }
            else
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            }
        }

        public override void Unbind()
        {
            if (bufferType == BufferType.Index)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            }
            else
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }
        }

        public void AddData(T data)
        {
            Datas.Add(data);
        }

        public void AddData(T[] datas)
        {
            this.Datas.AddRange(datas);
        }

        public override void BufferData(Shader shader)
        {
            if (shader.AttributeIDs.ContainsKey(AttributeName))
            {
                int attributeID = shader.AttributeIDs[AttributeName];
                if (attributeID != -1)
                {
                    Bind();
                    GL.BufferData<T>(BufferTarget.ArrayBuffer, (IntPtr)(Datas.Count * dataUnitSize * dataUnitCount), Datas.ToArray(), BufferUsageHint.StaticDraw);
                    GL.VertexAttribPointer(attributeID, dataUnitCount, pointerType, false, 0, 0);
                    Unbind();
                }
            } else if (bufferType == BufferType.Index)
            {
                Bind();
                GL.BufferData<T>(BufferTarget.ElementArrayBuffer, (IntPtr)(Datas.Count * dataUnitSize * dataUnitCount), Datas.ToArray(), BufferUsageHint.StaticDraw);
                Unbind();
            }
        }

        public override void CleanUp()
        {
            Datas.Clear();

            GL.DeleteBuffer(vbo);
        }
    }
}
