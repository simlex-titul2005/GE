using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GE.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SeoFriendlyUrl()
        {
            var title = "Новые интересные сервера Lineage 2: стань лучшим игроком сервера";
            var res = SX.WebCore.StringHelper.SeoFriendlyUrl(title);
        }
    }
}
