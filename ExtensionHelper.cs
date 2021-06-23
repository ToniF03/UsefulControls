using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace uCalc
{
    public static class ExtensionHelper
    {
        /// <summary>
        /// Registers an Extension for your app.
        /// </summary>
        /// <param name="extension">Extension to register.</param>
        /// <param name="icon">Path to the icon.</param>
        /// <param name="AppProgId">Name of the file type.</param>
        /// <param name="progid">Path to the Program.</param>
        /// <returns></returns>
        public static bool RegisterExtension(string extension, string icon, string AppProgId, string progid)
        {
            try
            {
                RegistryKey AppPath = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths");
                AppPath = AppPath.CreateSubKey(Path.GetFileName(progid));
                AppPath.SetValue("", progid);
                AppPath.SetValue("Path", Path.GetDirectoryName(progid));

                RegistryKey ExtensionPath = Registry.ClassesRoot.CreateSubKey(extension.StartsWith(".") ? extension : "." + extension);
                ExtensionPath.SetValue("", AppProgId);
                ExtensionPath = ExtensionPath.CreateSubKey("shell\\open\\command");
                ExtensionPath.SetValue("", "\"" + progid + "\" %1");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public static bool UnregisterExtension(string extension, string icon, string AppProgId, string progid)
        {
            try
            {
                RegistryKey AppPath = Registry.LocalMachine;
                AppPath.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + Path.GetFileName(progid));

                RegistryKey ExtensionPath = Registry.ClassesRoot;
                ExtensionPath.DeleteSubKey(extension.StartsWith(".") ? extension : "." + extension);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
