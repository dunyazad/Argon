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
        
        protected GraphicsBufferArray bufferArray;
        protected GraphicsBuffer<Vector3> vboPosition;
        protected GraphicsBuffer<Vector4> vboColor;

        public GraphicsObject(SceneEntity entity, string name)
            : base(entity, name)
        {
            var material = new Material(SceneEntity, "Default");
            materials.Add(material);

            bufferArray = new GraphicsBufferArray(entity, material.Shader, "Default");
            vboPosition = bufferArray.CreateBuffer<Vector3>("vPosition");
            vboColor = bufferArray.CreateBuffer<Vector4>("vColor");

            Console.WriteLine("GraphicsObject Ctor");
        }

        ~GraphicsObject()
        {
        }

        public override void OnUpdate(double dt)
        {
            if (Dirty)
            {
                materials[0].Shader.Use();

                bufferArray.BufferData();

                materials[0].Shader.Unuse();

                Console.WriteLine("Geometry OnUpdate, dt : " + dt.ToString());

                Dirty = false;
            }
        }

        public override void OnRender()
        {
            materials[0].Shader.Use();

            bufferArray.Bind();

            vboPosition.Enable();
            vboColor.Enable();



            var modelMatrix = SceneEntity.WorldMatrix;
            materials[0].Shader.SetUniformMatrix4("model", false, ref modelMatrix);

            var viewMatrix = SceneEntity.Scene.MainCamera.ViewMatrix;
            materials[0].Shader.SetUniformMatrix4("view", false, ref viewMatrix);

            var projectionMatrix = SceneEntity.Scene.MainCamera.ProjectionMatrix;
            materials[0].Shader.SetUniformMatrix4("projection", false, ref projectionMatrix);

            var mvp = modelMatrix * viewMatrix * projectionMatrix;
            materials[0].Shader.SetUniformMatrix4("mvp", false, ref mvp);
            



            GL.DrawArrays(PrimitiveType.Triangles, 0, vboPosition.Datas.Count);

            vboPosition.Disable();
            vboColor.Disable();


            bufferArray.Unbind();

            materials[0].Shader.Unuse();

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
