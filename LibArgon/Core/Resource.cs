using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace ArtificialNature
{
    class FileStreamCache : Cache<FileStream>
    {
        public override FileStream Get(string name)
        {
            FileStream result = base.Get(name);
            if (result == null)
            {
                result = new FileStream(name, FileMode.Open);
                Set(name, result);
            }

            return result;
        }
    }

    class ShaderCache : Cache<Shader>
    {
        public override Shader Get(string name)
        {
            Shader result = base.Get(name);
            if (result == null)
            {
                result = new Shader(name);
                Set(name, result);
            }

            return result;
        }
    }

    class Resources : Singleton<Resources>
    {
        static FileStreamCache s_fileStreamCache = new FileStreamCache();
        static ShaderCache s_shaderCache = new ShaderCache();

        public static FileStream GetFile(string fileName)
        {
            return s_fileStreamCache.Get(fileName);
        }

        public static Shader GetShader(string name)
        {
            return s_shaderCache.Get(name);
        }
    }
}
