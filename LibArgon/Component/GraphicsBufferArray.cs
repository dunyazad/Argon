﻿using System;
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

        public GraphicsBufferArray(SceneEntity sceneEntity, string name)
            : base(sceneEntity, name)
        {
            GL.GenVertexArrays(1, out vao);
        }

        ~GraphicsBufferArray()
        {
        }

        public override void OnUpdate(double dt)
        {
        }

        public override void OnRender()
        {
        }

        public void Render(Shader shader)
        {
            Bind();

            foreach (var kvp in Buffers)
            {
                int attributeID = shader.AttributeIDs[kvp.Value.AttributeName];
                if (attributeID != -1)
                {
                    GL.EnableVertexAttribArray(attributeID);
                }
            }

            if (Buffers.ContainsKey(GraphicsBufferBase.BufferType.Index))
            {
                //GL.DrawArrays(PrimitiveType.Triangles, 0, vboPosition.Datas.Count);
            }
            else
            {
                GL.DrawArrays(PrimitiveType.Triangles, 0, Buffers[GraphicsBufferBase.BufferType.Vertex].DataCount());
            }

            foreach (var kvp in Buffers)
            {
                int attributeID = shader.AttributeIDs[kvp.Value.AttributeName];
                if (attributeID != -1)
                {
                    GL.DisableVertexAttribArray(attributeID);
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
