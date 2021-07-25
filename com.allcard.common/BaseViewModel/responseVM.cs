using System;
using System.Collections.Generic;
using System.Text;

namespace com.allcard.common
{
    public class responseVM : IDisposable
    {
        public Guid JTI { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }
        public string Expiration { get; set; }
        public object Data { get; set; }
        public string Date { get; set; }
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }

        #region === IDisposable Members ===

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {

                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~responseVM()
        {
            this.Dispose(false);
        }
        #endregion
    }
}
