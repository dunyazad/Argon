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
    public class ANSceneEntity : ANSceneObject
    {
        protected ANSceneEntity parent = null;
        public ANSceneEntity Parent
        {
            get { return parent; }
            set
            {
                if (parent != null)
                {
                    parent.childEntities.Remove(Name);
                }
                parent = value;
                parent.childEntities.Add(Name, this);
            }
        }

        protected Dictionary<string, ANSceneEntity> childEntities = new Dictionary<string, ANSceneEntity>();

        protected List<ANComponent> components = new List<ANComponent>();

        Vector3 localScale = Vector3.One;
        public Vector3 LocalScale { get { return localScale; } set { localScale = value; Dirty = true; } }
        Vector3 worldScale = Vector3.One;
        public Vector3 WorldScale { get { return worldScale; } set { worldScale = value; Dirty = true; } }
        Vector3 localPosition = Vector3.Zero;
        public Vector3 LocalPosition { get { return localPosition; } set { localPosition = value; Dirty = true; } }
        Vector3 worldPosition = Vector3.Zero;
        public Vector3 WorldPosition { get { return worldPosition; } set { worldPosition = value; Dirty = true; } }
        Quaternion localRotation = Quaternion.Identity;
        public Quaternion LocalRotation { get { return localRotation; } set { localRotation = value; Dirty = true; } }
        Quaternion worldRotation = Quaternion.Identity;
        public Quaternion WorldRotation { get { return worldRotation; } set { worldRotation = value; Dirty = true; } }

        public Matrix4 WorldMatrix { get; private set; }

        protected bool dirty = true;
        public bool Dirty { get { return dirty; } set { dirty = value; if (Parent != null) Parent.Dirty = true; } }


        public ANSceneEntity(ANScene scene, string name)
            : base(scene, name)
        {
        }

        public override void Initialize()
        {
            foreach (var component in components)
            {
                component.OnInitialize();
            }

            base.Initialize();

            foreach (var kvp in childEntities)
            {
                kvp.Value.Initialize();
            }
        }

        public override void Update(double dt)
        {
            if (Dirty)
            {
                if (parent == null)
                {
                    worldPosition = localPosition;
                    worldRotation = localRotation;
                    worldScale = localScale;
                }
                else
                {
                    worldPosition = parent.worldPosition + parent.worldRotation * localPosition;
                    worldRotation = parent.worldRotation * localRotation;
                    worldScale = parent.worldScale * localScale;
                }

                WorldMatrix = Matrix4.CreateScale(worldScale) * Matrix4.CreateFromQuaternion(worldRotation) * Matrix4.CreateTranslation(worldPosition);

                dirty = false;
            }

            foreach (var component in components)
            {
                component.OnUpdate(dt);
            }

            base.Update(dt);

            foreach (var kvp in childEntities)
            {
                kvp.Value.Update(dt);
            }
        }

        public override void Render()
        {
            base.Render();

            foreach (var kvp in childEntities)
            {
                kvp.Value.Render();
            }

            foreach (var component in components)
            {
                component.OnRender();
            }
        }

        public override void Terminate()
        {
            foreach (var component in components)
            {
                component.OnTerminate();
            }

            base.Terminate();

            foreach (var kvp in childEntities)
            {
                kvp.Value.Terminate();
            }
        }

        public void AddComponent(ANComponent component)
        {
            if (!components.Contains(component))
            {
                components.Add(component);
            }
        }
    }
}
