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
                ba.OnUpdate(dt);
            }

            if (Dirty)
            {
                foreach (var material in Materials)
                {
                    material.Shader.BufferData(BufferArrays.ToArray());
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

                {
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
                }

                material.Shader.Render(BufferArrays.ToArray());

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
