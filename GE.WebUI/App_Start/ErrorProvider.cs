using System;

namespace GE.WebUI
{
    public static class ErrorProvider
    {
        private static SX.WebCore.Providers.SxErrorProvider _provider;

        public static void Configure(string dir)
        {
            _provider = new SX.WebCore.Providers.SxErrorProvider(dir);
        }

        public static void WriteMessage(Exception ex)
        {
            if (_provider == null) return;

            _provider.WriteMessage(ex);
        }
    }
}