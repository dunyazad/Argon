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
        protected List<Material> materials = new List<Material>();

        protected List<GraphicsBufferArray> bufferArrays = new List<GraphicsBufferArray>();
        protected GraphicsBuffer<Vector3> vboPosition;
        protected GraphicsBuffer<Vector4> vboColor;

        public GraphicsObject(SceneEntity entity, string name)
            : base(entity, name)
        {
            var material = new Material(SceneEntity, "Default");
            materials.Add(material);

            var bufferArray = new GraphicsBufferArray(entity, "Default");
            bufferArrays.Add(bufferArray);
            vboPosition = bufferArray.CreateBuffer<Vector3>("vPosition", GraphicsBufferBase.BufferType.Vertex);
            vboColor = bufferArray.CreateBuffer<Vector4>("vColor", GraphicsBufferBase.BufferType.Color);

            Console.WriteLine("GraphicsObject Ctor");
        }

        ~GraphicsObject()
        {
        }

        public override void OnUpdate(double dt)
        {
            if (Dirty)
            {
                foreach (var material in materials)
                {
                    material.Shader.BufferData(bufferArrays.ToArray());
                }

                Console.WriteLine("Geometry OnUpdate, dt : " + dt.ToString());

                Dirty = false;
            }
        }

        public override void OnRender()
        {
            foreach (var material in materials)
            {
                material.Shader.Use();

                {
                    var modelMatrix = SceneEntity.WorldMatrix;
                    material.Shader.SetUniformMatrix4("model", false, ref modelMatrix);

                    var viewMatrix = SceneEntity.Scene.MainCamera.ViewMatrix;
                    material.Shader.SetUniformMatrix4("view", false, ref viewMatrix);

                    var projectionMatrix = SceneEntity.Scene.MainCamera.ProjectionMatrix;
                    material.Shader.SetUniformMatrix4("projection", false, ref projectionMatrix);

                    var mvp = modelMatrix * viewMatrix * projectionMatrix;
                    material.Shader.SetUniformMatrix4("mvp", false, ref mvp);
                }


                material.Shader.Render(bufferArrays.ToArray());


                material.Shader.Unuse();
            }

            Console.WriteLine("Geometry OnRender");
        }

        public override void CleanUp()
        {
            foreach (var material in materials)
            {
                material.CleanUp();
            }

            materials.Clear();
        }
    }
}
