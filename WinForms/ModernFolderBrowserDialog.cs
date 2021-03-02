using Microsoft.WindowsAPICodePack.Dialogs;
using System.ComponentModel;

    class ModernFolderBrowserDialog : DialogControl
    {
        CommonOpenFileDialog dialog = new CommonOpenFileDialog();

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public DialogResult ShowDialog()
        {
            dialog.EnsurePathExists = true;
            dialog.EnsureFileExists = false;
            dialog.AllowNonFileSystemItems = false;
            dialog.IsFolderPicker = true;
            CommonFileDialogResult dr = dialog.ShowDialog();

            switch (dr) {
                case CommonFileDialogResult.Ok:
                    return DialogResult.OK;
                case CommonFileDialogResult.None:
                    return DialogResult.None;
                case CommonFileDialogResult.Cancel:
                    return DialogResult.Cancel;
            }
            return DialogResult.None;
        }

        public string FolderPath
        {
            get { return dialog.FileName; }
        }
    }

    public enum DialogResult
    {
        OK,
        None,
        Cancel
    }
