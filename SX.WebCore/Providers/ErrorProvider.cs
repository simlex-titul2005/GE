using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SX.WebCore.Providers
{
    public class ErrorProvider
    {
        private readonly string _dir;
        public ErrorProvider(string dir)
        {
            _dir = dir;
        }

        public virtual void WriteMessage(string message)
        {
            Task.Run(() =>
            {
                var date = DateTime.Now.ToString("yyyy-MM-dd");
                if (!Directory.Exists(_dir))
                    Directory.CreateDirectory(_dir);

                var filePath = Path.Combine(_dir, date + ".log");

                var sb = new StringBuilder();
                sb.AppendLine("Date: " + DateTime.Now.ToString());
                sb.AppendLine(message);

                File.AppendAllText(filePath, sb.ToString());
            });
        }
    }
}
