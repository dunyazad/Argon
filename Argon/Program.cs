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

                var entity = scene.CreateSceneEntity("Geometry");
                entity.AddComponent(new ANGeometry(entity));
                entity.LocalPosition = new Vector3(1, 0, 0);

                var entity2 = scene.CreateSceneEntity("Geometry2");
                entity2.AddComponent(new ANGeometry(entity2));
                entity2.LocalPosition = new Vector3(-1, 0, 0);

                var entity3 = scene.CreateSceneEntity("Geometry3");
                entity3.AddComponent(new ANGeometry(entity3));
                entity3.LocalPosition = new Vector3(0, 2, 0);

                argon.Run();
                //argon.Run(30, 30);
            }
        }

        private static void Scene_OnUpdate(ANSceneObject sceneObject, double dt)
        {
            var scene = sceneObject as ANScene;
            if(scene != null)
            {
                scene.FindeSceneEntity("Geometry").LocalRotation *= Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians((float)dt * 100));
                scene.FindeSceneEntity("Geometry2").LocalRotation *= Quaternion.FromAxisAngle(-Vector3.UnitY, MathHelper.DegreesToRadians((float)dt * 100));
                scene.FindeSceneEntity("Geometry3").LocalRotation *= Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.DegreesToRadians((float)dt * 100));
            }
        }
    }
}
