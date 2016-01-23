using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace LiveViewPlugin.Utils
{
    public static class Permit
    {
        static public void CheckTrust()
        {
            try
            {
                SocketPermission permission =
                    new SocketPermission(PermissionState.Unrestricted);
                s_FullTrust = SecurityManager.IsGranted(permission);
            }
            catch (Exception)
            {
                // ignore
            }
        }

        static private bool s_FullTrust;
        static public bool IsFullTrust
        {
            get
            {
                return s_FullTrust;
            }
        }
    }
}
