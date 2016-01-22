using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SX.WebCore.Abstract
{
    public interface ISxStorage
    {
        /// <summary>
        /// Статьи сайта
        /// </summary>
        SxArticle[] Articles { get; }
    }
}
