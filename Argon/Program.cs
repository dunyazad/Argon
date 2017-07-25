using System;
using ArtificialNature.Component;

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
                scene.RootEntity.AddComponent(new ANGeometry(scene.RootEntity));

                argon.Run();
                //argon.Run(30, 30);
            }
        }
    }
}
