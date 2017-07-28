using System;
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

                var entity = scene.CreateSceneEntity("Triangle");
                entity.AddComponent(new GeometryTriangle("Triangle"));
                entity.LocalPosition = new Vector3(1, 0, 0);
                //entity.OnUpdate += Entity_OnUpdate;

                var entity2 = scene.CreateSceneEntity("Triangle2");
                entity2.AddComponent(new GeometryTriangle("Triangle2"));
                entity2.LocalPosition = new Vector3(-1, 0, 0);

                var entity3 = scene.CreateSceneEntity("Rectangle");
                var rectangle = new GeometryRectangle("Rectangle");
                entity3.AddComponent(rectangle);
                entity3.LocalPosition = new Vector3(0, 2, 0);
                rectangle.Materials[0].Textures.Add(new Texture("Satellite.jpg"));

                var entity4 = scene.CreateSceneEntity("Circle");
                entity4.AddComponent(new GeometryCircle("Circle", 0.5f, 36));
                entity4.LocalPosition = new Vector3(0, -2, 0);
                //entity4.PolygonMode = OpenTK.Graphics.OpenGL4.PolygonMode.Line;

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
                scene.FindeSceneEntity("Triangle").LocalRotation *= Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians((float)dt * 100));
                scene.FindeSceneEntity("Triangle2").LocalRotation *= Quaternion.FromAxisAngle(-Vector3.UnitY, MathHelper.DegreesToRadians((float)dt * 100));
                scene.FindeSceneEntity("Rectangle").LocalRotation *= Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.DegreesToRadians((float)dt * 100));
                scene.FindeSceneEntity("Circle").LocalRotation *= Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.DegreesToRadians((float)dt * 100));
            }
        }
    }
}
