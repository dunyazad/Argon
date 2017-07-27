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

namespace ArtificialNature
{
    public class Scene : SceneObject
    {
        public SceneEntity RootEntity { get; protected set; }

        public SceneEntityCamera MainCamera { get; set; }

        Dictionary<string, SceneEntity> sceneEntities = new Dictionary<string, SceneEntity>();

        internal Scene(string name)
            : base(null, name)
        {
            Scene = this;
            RootEntity = new SceneEntity(this, "RootEntity");
            MainCamera = new SceneEntityCamera(this, "DefaultCamera");
            MainCamera.Parent = RootEntity;
        }

        ~Scene()
        {

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

        public override void Resize(float width, float height)
        {
            base.Resize(width, height);
        }

        public SceneEntity CreateSceneEntity(string name)
        {
            if (sceneEntities.ContainsKey(name))
            {
                return sceneEntities[name];
            }
            else
            {
                var entity = new SceneEntity(this, name);
                entity.Parent = RootEntity;

                sceneEntities.Add(name, entity);
                return entity;
            }
        }

        public SceneEntity FindeSceneEntity(string name)
        {
            if (sceneEntities.ContainsKey(name))
            {
                return sceneEntities[name];
            }
            else
            {
                return null;
            }
        }

        public override void CleanUp()
        {
            RootEntity.CleanUp();
        }
    }
}
