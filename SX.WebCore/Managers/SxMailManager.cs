using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace SX.WebCore.Managers
{
    public class SxMailManager : IDisposable
    {
        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public static SxMailManager Create(IdentityFactoryOptions<SxMailManager> options, IOwinContext context)
        {
            return new SxMailManager();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
    }
}
