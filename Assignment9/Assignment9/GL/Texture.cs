/*
 * Name: Luckshihaa Krishnan 
 * Student ID: 186418216
 * Section: GAM 531 NSA 
 */


using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Assignment9
{
    public class Texture
    {
        public int texID;

        // Define wrapping and filtering
        public TextureWrapMode wrapMode = TextureWrapMode.Repeat;
        public TextureMinFilter minFilter = TextureMinFilter.LinearMipmapLinear;
        public TextureMagFilter magFilter = TextureMagFilter.Linear;

        public Texture(string path)
        {
            // If texture file not found, throw exception
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Could not find file {path}");
            }
            texID = GL.GenTexture();

            // Bind texture
            GL.BindTexture(TextureTarget.Texture2D, texID);

            // Initial wrap and filter
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);

            // Load image
            using (Bitmap bmp = new Bitmap(path))
            {
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                var data = bmp.LockBits(
                    new Rectangle(0, 0, bmp.Width, bmp.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                bmp.UnlockBits(data);
            }
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);


        }

        // Used in Mesh class to bind texture
        public void UseTexture(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, texID);
        }
    }
}

