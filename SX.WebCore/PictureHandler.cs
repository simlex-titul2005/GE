using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SX.WebCore
{
    public static class PictureHandler
    {
        private static Image imageFromByteArray(byte[] bytes)
        {
            Image image = null;
            using (var ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }
            return image;
        }

        public static byte[] ScaleImage(byte[] inputByteArray, ImageScaleMode scaleMode = ImageScaleMode.Width, int? destWidth=null, int? destHeight=null)
        {
            var originalImage = imageFromByteArray(inputByteArray);
            
            switch(scaleMode)
            {
                case ImageScaleMode.Width:
                    if (!destWidth.HasValue)
                        throw new ArgumentNullException("Не задано значение ширины изображения");
                    destHeight = (int)(originalImage.Height * (decimal)destWidth / originalImage.Width);
                    break;
                case ImageScaleMode.Height:
                    if (!destHeight.HasValue)
                        throw new ArgumentNullException("Не задано значение высоты изображения");
                    destWidth = (int)(originalImage.Width * (decimal)destHeight / originalImage.Height);
                    break;
                default:
                    throw new NotImplementedException("Такое приведение недопустимо");
            }
            
            var bitmap = new Bitmap(originalImage, (int)destWidth, (int)destHeight);
            byte[] resultByteArray = null;
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, (ImageFormat)originalImage.RawFormat);
                resultByteArray = stream.ToArray();
            }
            return resultByteArray;
        }

        public enum ImageScaleMode : byte
        {
            Unknown=0,
            Width=1,
            Height=2
        }
    }
}
