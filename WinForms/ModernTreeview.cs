using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Scrolls
{
    class ModernTreeview : TreeView
    {
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

        public static void SetTreeViewTheme(IntPtr treeHandle)
        {
            SetWindowTheme(treeHandle, "explorer", null);
        }

        public ModernTreeview()
        {
            SetTreeViewTheme(this.Handle);
            this.ShowLines = false;
        }
    }
}
