using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

using QuickFont;
using QuickFont.Configuration;

namespace ArtificialNature
{
    public class Text : Component
    {
        public static string fontRootPath = "./Resource/Font/";

        //public int TextureID { get; set; }

        private QFontDrawing drawing;
        QFont mainText;
        private QFontRenderOptions mainTextOptions;
        ProcessedText processedText;

        public Text(string fileName)
            : base(fileName)

        {
            //var file = Resources.GetFile(ImageRootPath + Name);
            //Bitmap image = new Bitmap(file);

            //TextureID = GL.GenTexture();

            //GL.BindTexture(TextureTarget.Texture2D, TextureID);

            //BitmapData data = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
            //    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
            //    OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            //image.UnlockBits(data);

            //GL.TextureParameter((int)TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            //GL.TextureParameter((int)TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            //GL.TextureParameter((int)TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            //GL.TextureParameter((int)TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            //GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);




            var builderConfig = new QFontBuilderConfiguration(true)
            {
                ShadowConfig =
                {
                    BlurRadius = 2,
                    BlurPasses = 1,
                    Type = ShadowType.Blurred
                },
                TextGenerationRenderHint = TextGenerationRenderHint.ClearTypeGridFit,
                Characters = CharacterSet.General | CharacterSet.Japanese | CharacterSet.Thai | CharacterSet.Cyrillic
            };
            //reduce blur radius because font is very small
            //best render hint for this font

            drawing = new QFontDrawing();
            mainText = new QFont(fontRootPath + fileName + ".ttf", 14, builderConfig);
            mainTextOptions = new QFontRenderOptions { DropShadowActive = true, Colour = Color.White, WordSpacing = 0.5f };
            processedText = QFontDrawingPrimitive.ProcessText(mainText, mainTextOptions, "Basldfgjwoietghwoifhewohfoiewuew", new SizeF(400 - 40, -1), QFontAlignment.Left);
        }

        ~Text()
        {
        }

        public override void OnUpdate(double dt)
        {
        }

        public override void OnRender(SceneEntity entity)
        {
            drawing.Print(mainText, processedText, new Vector3(0, 0, 0));
            drawing.RefreshBuffers();
            drawing.Draw();
        }

        public override void CleanUp()
        {
        }
    }
}
