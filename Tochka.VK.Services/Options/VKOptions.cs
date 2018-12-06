using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Tochka.VK.Services.Options
{
    public class VKOptions
    {
        public ulong ApplicationId { get; set; }
        public string Login { get; set; }

        private SecureString _password;
        public string Password
        {

            [SecurityCritical]
            get
            {
                using (SecureString securePassword = this._password)
                {
                    IntPtr valuePtr = IntPtr.Zero;
                    try
                    {
                        valuePtr = Marshal.SecureStringToGlobalAllocUnicode(_password);
                        return Marshal.PtrToStringUni(valuePtr);
                    }
                    finally
                    {
                        Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
                    }
                }
            }

            [SecurityCritical]
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;

                try { _password?.Dispose(); } catch { }

                _password = new SecureString();
                foreach (var c in value)
                    _password.AppendChar(c);
            }
        }
    }
}
