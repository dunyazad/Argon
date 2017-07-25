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

using ArtificialNature.Core;

namespace ArtificialNature.Scene
{
    public class ANScene : ANSceneObject
    {
        public ANSceneEntity RootEntity { get; protected set; }

        public ANSceneEntityCamera MainCamera { get; set; }

        public ANScene(string name)
            : base(null, name)
        {
            Scene = this;
            RootEntity = new ANSceneEntity(this, "RootEntity");
            MainCamera = new ANSceneEntityCamera(this, "DefaultCamera");
            MainCamera.Parent = RootEntity;
        }

        public override void Initialize()
        {
            base.Initialize();

            RootEntity.Initialize();
        }

        public override void Update(double dt)
        {
            base.Update(dt);

            RootEntity.Update(dt);
        }

        public override void Render()
        {
            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(5f);

            //GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            //GL.Enable(EnableCap.LineSmooth); // This is Optional 
            //GL.Enable(EnableCap.RescaleNormal);

            base.Render();

            RootEntity.Render();

            GL.Flush();
        }

        public override void Terminate()
        {
            base.Terminate();

            RootEntity.Terminate();
        }

        public override void Resize(float width, float height)
        {
            base.Resize(width, height);
        }
    }
}
