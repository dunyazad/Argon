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
    class ArFileStreamCache : Cache<FileStream>
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

    class ANShaderCache : Cache<ANShader>
    {
        public override ANShader Get(string name)
        {
            ANShader result = base.Get(name);
            if (result == null)
            {
                result = new ANShader(name);
                Set(name, result);
            }

            return result;
        }
    }

    class ANResources : Singleton<ANResources>
    {
        static ArFileStreamCache s_fileStreamCache = new ArFileStreamCache();
        static ANShaderCache s_shaderCache = new ANShaderCache();

        public static FileStream GetFile(string fileName)
        {
            return s_fileStreamCache.Get(fileName);
        }

        public static ANShader GetShader(string name)
        {
            return s_shaderCache.Get(name);
        }
    }
}
