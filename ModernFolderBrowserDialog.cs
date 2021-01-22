using Microsoft.WindowsAPICodePack.Dialogs;

    class ModernFolderBrowserDialog : DialogControl
    {
        CommonOpenFileDialog dialog = new CommonOpenFileDialog();

        public void ShowDialog()
        {
            dialog.EnsurePathExists = true;
            dialog.EnsureFileExists = false;
            dialog.AllowNonFileSystemItems = false;
            dialog.IsFolderPicker = true;
            dialog.ShowDialog();
            string s = dialog.FileName;
        }

        public string FolderPath
        {
            get { return dialog.FileName; }
        }
    }
