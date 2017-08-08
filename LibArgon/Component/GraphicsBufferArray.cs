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
    public class GraphicsBufferArray : Component
    {
        protected int vao;

        public Dictionary<GraphicsBufferBase.BufferType, GraphicsBufferBase> Buffers { get; private set; } = new Dictionary<GraphicsBufferBase.BufferType, GraphicsBufferBase>();

        public GraphicsBufferArray(string name)
            : base(name)
        {
            GL.GenVertexArrays(1, out vao);
        }

        ~GraphicsBufferArray()
        {
        }

        public override void OnUpdate(double dt)
        {
            if (!Buffers.ContainsKey(GraphicsBufferBase.BufferType.Index))
            {
                CreateBuffer<uint>("index", GraphicsBufferBase.BufferType.Index);
            }

            var indices = Buffers[GraphicsBufferBase.BufferType.Index] as GraphicsBuffer<uint>;
            if (indices.DataCount() == 0)
            {
                for (int i = 0; i < Buffers[GraphicsBufferBase.BufferType.Vertex].DataCount(); i++)
                {
                    indices.AddData((uint)i);
                }
            }
        }

        public override void OnRender(SceneEntity entity)
        {
        }

        public void Render(Shader shader)
        {
            Bind();

            foreach (var kvp in Buffers)
            {
                if (shader.AttributeIDs.ContainsKey(kvp.Value.AttributeName))
                {
                    int attributeID = shader.AttributeIDs[kvp.Value.AttributeName];
                    if (attributeID != -1)
                    {
                        GL.EnableVertexAttribArray(attributeID);
                    }
                }
                else
                {
                    if (Buffers.ContainsKey(GraphicsBufferBase.BufferType.Index))
                    {
                        Buffers[GraphicsBufferBase.BufferType.Index].Bind();
                    }
                }
            }

            if (Buffers.ContainsKey(GraphicsBufferBase.BufferType.Index))
            {
                //GL.DrawElements<uint>(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt, (Buffers[GraphicsBufferBase.BufferType.Index]as GraphicsBuffer<uint>).Datas.ToArray());
                GL.DrawElements(PrimitiveType.Triangles, Buffers[GraphicsBufferBase.BufferType.Index].DataCount(), DrawElementsType.UnsignedInt, 0);// (Buffers[GraphicsBufferBase.BufferType.Index] as GraphicsBuffer<uint>).Datas.ToArray());
            }
            else
            {
                GL.DrawArrays(PrimitiveType.Triangles, 0, Buffers[GraphicsBufferBase.BufferType.Vertex].DataCount());
            }

            foreach (var kvp in Buffers)
            {
                if (shader.AttributeIDs.ContainsKey(kvp.Value.AttributeName))
                {
                    int attributeID = shader.AttributeIDs[kvp.Value.AttributeName];
                    if (attributeID != -1)
                    {
                        GL.DisableVertexAttribArray(attributeID);
                    }
                }
                {
                    if (Buffers.ContainsKey(GraphicsBufferBase.BufferType.Index))
                    {
                        Buffers[GraphicsBufferBase.BufferType.Index].Unbind();
                    }
                }
            }

            Unbind();
        }
        
        public void Bind()
        {
            GL.BindVertexArray(vao);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public GraphicsBuffer<T> CreateBuffer<T>(string attributeName, GraphicsBufferBase.BufferType bufferType) where T : struct
        {
            if (Buffers.ContainsKey(bufferType))
            {
                return Buffers[bufferType] as GraphicsBuffer<T>;
            }
            else
            {
                var vbo = new GraphicsBuffer<T>(this, bufferType.ToString(), attributeName, bufferType);
                Buffers.Add(bufferType, vbo);
                return vbo;
            }
        }

        public void BufferData(Shader shader)
        {
            Bind();
            foreach (var kvp in Buffers)
            {
                kvp.Value.BufferData(shader);
            }
            Unbind();
        }

        public override void CleanUp()
        {
            foreach (var kvp in Buffers)
            {
                kvp.Value.CleanUp();
            }

            Buffers.Clear();

            GL.DeleteVertexArray(vao);
        }
    }
}
