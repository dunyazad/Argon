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
    public class GraphicsObject : Component
    {
        protected Color color;
        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                Dirty = true;
            }
        }

        protected int vao;
        public Dictionary<GraphicsBufferBase.BufferType, GraphicsBufferBase> Buffers { get; private set; } = new Dictionary<GraphicsBufferBase.BufferType, GraphicsBufferBase>();

        public List<Material> Materials { get; set; } = new List<Material>();

        public GraphicsObject(string name)
            : base(name)
        {
            vao = GL.GenVertexArray();

            var material = new Material("Default");
            Materials.Add(material);

            Console.WriteLine("GraphicsObject Ctor");
        }

        ~GraphicsObject()
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

            if (Dirty)
            {
                foreach (var material in Materials)
                {
                    material.Shader.Use();

                    Bind();

                    foreach (var kvp in Buffers)
                    {
                        kvp.Value.BufferData(material.Shader);
                    }

                    Unbind();

                    material.Shader.Unuse();
                }

                Console.WriteLine("Geometry OnUpdate, dt : " + dt.ToString());

                Dirty = false;
            }
        }

        public override void OnRender(SceneEntity entity)
        {
            foreach (var material in Materials)
            {
                material.Shader.Use();


                #region Set Uniform To Shader
                var modelMatrix = entity.WorldMatrix;
                material.Shader.SetUniformMatrix4("model", false, ref modelMatrix);

                var viewMatrix = entity.Scene.MainCamera.ViewMatrix;
                material.Shader.SetUniformMatrix4("view", false, ref viewMatrix);

                var projectionMatrix = entity.Scene.MainCamera.ProjectionMatrix;
                material.Shader.SetUniformMatrix4("projection", false, ref projectionMatrix);

                var mvp = modelMatrix * viewMatrix * projectionMatrix;
                material.Shader.SetUniformMatrix4("mvp", false, ref mvp);

                if (material.Textures.Count > 0)
                {
                    for (int i = 0; i < material.Textures.Count; i++)
                    {
                        int index = (int)TextureUnit.Texture0;
                        TextureUnit textureUnit = (TextureUnit)(index + i);
                        GL.ActiveTexture(textureUnit);

                        GL.BindTexture(TextureTarget.Texture2D, material.Textures[i].TextureID);
                        material.Shader.SetUniform1(textureUnit.ToString(), i);
                        material.Shader.SetUniform1("useTexture" + i.ToString(), 1);
                    }
                }
                else
                {
                    material.Shader.SetUniform1("useTexture0", -1);
                }
                #endregion

                Bind();

                foreach (var kvp in Buffers)
                {
                    if (material.Shader.AttributeIDs.ContainsKey(kvp.Value.AttributeName))
                    {
                        int attributeID = material.Shader.AttributeIDs[kvp.Value.AttributeName];
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
                    GL.DrawElements(PrimitiveType.Triangles, Buffers[GraphicsBufferBase.BufferType.Index].DataCount(), DrawElementsType.UnsignedInt, 0);
                }
                //else
                //{
                //    GL.DrawArrays(PrimitiveType.Triangles, 0, Buffers[GraphicsBufferBase.BufferType.Vertex].DataCount());
                //}

                foreach (var kvp in Buffers)
                {
                    if (material.Shader.AttributeIDs.ContainsKey(kvp.Value.AttributeName))
                    {
                        int attributeID = material.Shader.AttributeIDs[kvp.Value.AttributeName];
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

                material.Shader.Unuse();
            }

            Console.WriteLine("Geometry OnRender");
        }

        protected void Bind()
        {
            GL.BindVertexArray(vao);
        }

        protected void Unbind()
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

        public GraphicsBuffer<T> GetBuffer<T>(string attributeName) where T : struct
        {
            foreach (var kvp in Buffers)
            {
                if(kvp.Value.Name == attributeName)
                {
                    return kvp.Value as GraphicsBuffer<T>;
                }
            }
            return null;
        }

        public GraphicsBuffer<T> GetBuffer<T>(GraphicsBufferBase.BufferType bufferType) where T : struct
        {
            if (Buffers.ContainsKey(bufferType))
            {
                return Buffers[bufferType] as GraphicsBuffer<T>;
            }
            return null;
        }

        public override void CleanUp()
        {
            foreach (var material in Materials)
            {
                material.CleanUp();
            }

            Materials.Clear();

            foreach (var kvp in Buffers)
            {
                kvp.Value.CleanUp();
            }

            Buffers.Clear();

            GL.DeleteVertexArray(vao);
        }
    }
}
