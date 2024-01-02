using System;
using System.Windows.Forms;

#if UNITY_STANDALONE_WIN
using Ookii.Dialogs;
#endif

namespace Script.Helper
{
    public static class FileHelper
    {
        public static string GetFilePath(string filter)
        {
            #if UNITY_STANDALONE_WIN
                VistaOpenFileDialog openFile = new VistaOpenFileDialog();
                openFile.Filter = "File " + filter;
                
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    return openFile.FileName;
                }
                else
                {
                    return String.Empty;
                }
            #elif UNITY_STANDALONE_OSX
                return String.Empty;
            #else
                return String.Empty;
            #endif
        }
    }
}