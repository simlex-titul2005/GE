using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace GE.WebUI.Controllers.Api
{
    public class LogsController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var dir = System.Web.Hosting.HostingEnvironment.MapPath("~/logs");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var zipname = string.Format("{0}-logs-{1}.zip", "ge" ?? "site", DateTime.Now.ToString("yyyy-MM-dd"));

            var ms = new MemoryStream();
            var zipStream = new ZipOutputStream(ms);
            zipStream.SetLevel(3);

            foreach (string file in Directory.GetFiles(dir))
            {
                FileInfo fi = new FileInfo(file);

                string entryName = ZipEntry.CleanName(fi.Name);
                ZipEntry newEntry = new ZipEntry(entryName);
                newEntry.DateTime = fi.LastWriteTime;
                newEntry.Size = fi.Length;
                zipStream.PutNextEntry(newEntry);

                byte[] buffer = new byte[4096];
                using (FileStream streamReader = System.IO.File.OpenRead(fi.FullName))
                {
                    StreamUtils.Copy(streamReader, zipStream, buffer);
                }
                zipStream.CloseEntry();
            }
            zipStream.IsStreamOwner = false;
            zipStream.Close();

            ms.Position = 0;

            string file_type = "application/zip";

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.GetBuffer())
            };

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = zipname
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(file_type);
            return result;
        }
    }
}
