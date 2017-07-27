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
    public class ANGraphicsObject : ANComponent
    {
        protected List<ANMaterial> materials = new List<ANMaterial>();
        
        protected ANGraphicsBufferArray bufferArray;
        protected ANGraphicsBuffer<Vector3> vboPosition;
        protected ANGraphicsBuffer<Vector4> vboColor;

        public ANGraphicsObject()
            : base()
        {
        }

        public override void OnInitialize()
        {
            {
                var material = new ANMaterial() { Name = "Default" };
                materials.Add(material);

                bufferArray = material.Shader.CreateVAO("Default");
                vboPosition = bufferArray.CreateVBO<Vector3>("vPosition");
                vboColor = bufferArray.CreateVBO<Vector4>("vColor");
            }

            foreach (var material in materials)
            {
                material.OnInitialize();
            }

            //vboPosition.AddData(new Vector3[] { new Vector3(-0.8f, -0.8f, 0f), new Vector3(0.8f, -0.8f, 0f), new Vector3(0f, 0.8f, 0f) });
            //vboColor.AddData(new Vector4[] { new Vector4(1f, 0f, 0f, 0.5f), new Vector4(0f, 0f, 1f, 1f), new Vector4(0f, 1f, 0f, 1f) });

            Console.WriteLine("ANGeometry OnInitialize");
        }
        public override void OnUpdate(double dt)
        {
            if (Dirty)
            {
                materials[0].Shader.Use();

                bufferArray.BufferData();

                materials[0].Shader.Unuse();

                Console.WriteLine("ANGeometry OnUpdate, dt : " + dt.ToString());

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

            Console.WriteLine("ANGeometry OnRender");
        }

        public override void OnTerminate()
        {
            bufferArray.OnTerminate();

            Console.WriteLine("ANGeometry OnTerminate");
        }
    }
}
