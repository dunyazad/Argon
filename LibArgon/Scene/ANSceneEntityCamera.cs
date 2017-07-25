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
    public class ANSceneEntityCamera : ANSceneEntity
    {
        Matrix4 viewMatrix;
        public Matrix4 ViewMatrix { get { return viewMatrix; } set { viewMatrix = value; Dirty = true; } }

        Matrix4 projectionMatrix;
        public Matrix4 ProjectionMatrix { get { return projectionMatrix; } set { projectionMatrix = value; Dirty = true; } }

        protected float fovy;
        public float Fovy { get { return fovy; } set { fovy = value; Dirty = true; } }

        protected float aspectRatio;
        public float AspectRatio { get { return aspectRatio; } set { aspectRatio = value; Dirty = true; } }

        protected float near;
        public float Near { get { return near; } set { near = value; Dirty = true; } }

        protected float far;
        public float Far { get { return far; } set { far = value; Dirty = true; } }


        protected float width;
        public float Width { get { return width; } set { width = value; Dirty = true; } }

        protected float height;
        public float Height { get { return height; } set { height = value; Dirty = true; } }

        Vector3 eyePosition;
        public Vector3 EyePosition { get { return eyePosition; } set { eyePosition = value; Dirty = true; } }

        Vector3 targetPosition;
        public Vector3 TargetPosition { get { return targetPosition; } set { targetPosition = value; Dirty = true; } }

        Vector3 upDirection;
        public Vector3 UpDirection { get { return upDirection; } set { upDirection = value; Dirty = true; } }

        public ANSceneEntityCamera(ANScene scene, string name)
            : base(scene, name)
        {
            EyePosition = new Vector3(0, 3, 5);
            TargetPosition = new Vector3(0, 0, 0);
            UpDirection = new Vector3(0, 1, 0);

            Fovy = 60;
            AspectRatio = 400 / 300;
            Near = 0.1f;
            Far = 1000;

            ViewMatrix = Matrix4.LookAt(EyePosition, TargetPosition, UpDirection);

            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(Fovy * OpenTK.MathHelper.Pi / 180, AspectRatio, Near, Far);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(double dt)
        {
            if (Dirty)
            {
                ViewMatrix = Matrix4.LookAt(EyePosition, TargetPosition, UpDirection);
                ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(Fovy * OpenTK.MathHelper.Pi / 180, AspectRatio, Near, Far);
            }

            base.Update(dt);
        }

        public override void Render()
        {
            base.Render();
        }

        public override void Terminate()
        {
            base.Terminate();
        }

        public override void Resize(float width, float height)
        {
            base.Resize(width, height);

            Width = width;
            Height = height;
            AspectRatio = width / height;

            GL.Viewport(0, 0, (int)width, (int)height);
        }
    }
}
