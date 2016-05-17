using System;
using System.Text;

namespace GE.WebAdmin
{
    public static class ErrorProvider
    {
        private static SX.WebCore.Providers.ErrorProvider _provider;

        public static void Configure(string dir)
        {
            _provider = new SX.WebCore.Providers.ErrorProvider(dir);
        }

        public static void WriteMessage(Exception ex)
        {
            if (_provider == null) return;

            var sb = new StringBuilder();
            sb.AppendLine("Message: "+ ex.Message);
            sb.AppendLine("StackTrace: "+ex.StackTrace);
            _provider.WriteMessage(sb.ToString());
        }
    }
}