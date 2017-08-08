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
        public List<Material> Materials { get; set; } = new List<Material>();

        public List<GraphicsBufferArray> BufferArrays = new List<GraphicsBufferArray>() { new GraphicsBufferArray("Default") };

        public GraphicsObject(string name)
            : base(name)
        {
            var material = new Material("Default");
            Materials.Add(material);

            Console.WriteLine("GraphicsObject Ctor");
        }

        ~GraphicsObject()
        {
        }

        public override void OnUpdate(double dt)
        {
            foreach (var ba in BufferArrays)
            {
                if (!ba.Buffers.ContainsKey(GraphicsBufferBase.BufferType.Index))
                {
                    ba.CreateBuffer<uint>("index", GraphicsBufferBase.BufferType.Index);
                }

                var indices = ba.Buffers[GraphicsBufferBase.BufferType.Index] as GraphicsBuffer<uint>;
                if (indices.DataCount() == 0)
                {
                    for (int i = 0; i < ba.Buffers[GraphicsBufferBase.BufferType.Vertex].DataCount(); i++)
                    {
                        indices.AddData((uint)i);
                    }
                }
            }

            if (Dirty)
            {
                foreach (var material in Materials)
                {
                    material.Shader.Use();

                    foreach (var ba in BufferArrays)
                    {
                        ba.Bind();
                        foreach (var kvp in ba.Buffers)
                        {
                            kvp.Value.BufferData(material.Shader);
                        }
                        ba.Unbind();
                    }

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

                foreach (var ba in BufferArrays)
                {
                    ba.Bind();

                    foreach (var kvp in ba.Buffers)
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
                            if (ba.Buffers.ContainsKey(GraphicsBufferBase.BufferType.Index))
                            {
                                ba.Buffers[GraphicsBufferBase.BufferType.Index].Bind();
                            }
                        }
                    }

                    if (ba.Buffers.ContainsKey(GraphicsBufferBase.BufferType.Index))
                    {
                        //GL.DrawElements<uint>(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt, (Buffers[GraphicsBufferBase.BufferType.Index]as GraphicsBuffer<uint>).Datas.ToArray());
                        GL.DrawElements(PrimitiveType.Triangles, ba.Buffers[GraphicsBufferBase.BufferType.Index].DataCount(), DrawElementsType.UnsignedInt, 0);// (Buffers[GraphicsBufferBase.BufferType.Index] as GraphicsBuffer<uint>).Datas.ToArray());
                    }
                    else
                    {
                        GL.DrawArrays(PrimitiveType.Triangles, 0, ba.Buffers[GraphicsBufferBase.BufferType.Vertex].DataCount());
                    }

                    foreach (var kvp in ba.Buffers)
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
                            if (ba.Buffers.ContainsKey(GraphicsBufferBase.BufferType.Index))
                            {
                                ba.Buffers[GraphicsBufferBase.BufferType.Index].Unbind();
                            }
                        }
                    }

                    ba.Unbind();
                }

                material.Shader.Unuse();
            }

            Console.WriteLine("Geometry OnRender");
        }

        public override void CleanUp()
        {
            foreach (var material in Materials)
            {
                material.CleanUp();
            }

            Materials.Clear();
        }
    }
}
