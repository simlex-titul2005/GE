using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SX.WebCore
{
    public static class Enums
    {
        public enum ModelType : byte
        {
            Unknown = 0,
            Article = 1,
            News = 2,
            SeoKeyWord = 3,
            SeoInfo = 4
        }

        public enum ImgType : byte
        {
            Unknown = 0,
            Jpeg = 1,
            Png = 2,
            Gif = 3
        }
    }
}
