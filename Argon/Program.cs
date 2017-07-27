﻿using System;
using OpenTK;

namespace ArtificialNature
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (LibArgon argon = new LibArgon())
            {
                argon.Title = "Argon App";

                var scene = argon.CreateScene("Argon App");
                scene.OnUpdate += Scene_OnUpdate;

                var entity = scene.CreateSceneEntity("Geometry");
                entity.AddComponent(new GraphicsObject(entity, "Geometry"));
                entity.LocalPosition = new Vector3(1, 0, 0);
                //entity.OnUpdate += Entity_OnUpdate;

                var entity2 = scene.CreateSceneEntity("Triangle");
                entity2.AddComponent(new GeometryTriangle(entity2, "Triangle"));
                entity2.LocalPosition = new Vector3(-1, 0, 0);

                var entity3 = scene.CreateSceneEntity("Rectangle");
                entity3.AddComponent(new GeometryRectangle(entity3, "Rectangle"));
                entity3.LocalPosition = new Vector3(0, 2, 0);

                argon.Run();
                //argon.Run(30, 30);
            }
        }

        //private static void Entity_OnUpdate(SceneObject sceneObject, double dt)
        //{
        //    var entity = sceneObject as SceneEntity;
        //    entity.LocalRotation *= Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians((float)dt * 100));
        //}

        private static void Scene_OnUpdate(SceneObject sceneObject, double dt)
        {
            var scene = sceneObject as Scene;
            if(scene != null)
            {
                scene.FindeSceneEntity("Geometry").LocalRotation *= Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians((float)dt * 100));
                scene.FindeSceneEntity("Triangle").LocalRotation *= Quaternion.FromAxisAngle(-Vector3.UnitY, MathHelper.DegreesToRadians((float)dt * 100));
                scene.FindeSceneEntity("Rectangle").LocalRotation *= Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.DegreesToRadians((float)dt * 100));
            }
        }
    }
}
