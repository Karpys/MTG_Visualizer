using System;
using System.Windows.Forms;

#if UNITY_STANDALONE_WIN
using Ookii.Dialogs;
#endif


namespace Script.Helper
{
    #if UNITY_STANDALONE_OSX
    using SFB;
    #endif
    
    public enum FilterType
    {
        Text,
        Jpg,
    }
    public static class FileHelper
    {
        
        public static string GetFilePath(FilterType filterType)
        {
            #if UNITY_STANDALONE_WIN
                VistaOpenFileDialog openFile = new VistaOpenFileDialog();
                openFile.Filter = "File " + ToFilterText(filterType);
                
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    return openFile.FileName;
                }
                else
                {
                    return String.Empty;
                }
            #elif UNITY_STANDALONE_OSX
                string path = StandaloneFileBrowser.OpenFilePanel("", "", ToFilterText(filterType), false)[0];
                return path;
            #else
                return String.Empty;
            #endif
        }

        private static string ToFilterText(FilterType filter)
        {
            #if UNITY_STANDALONE_OSX
            switch (filter)
            {
                case FilterType.Text:
                    return "txt";
                case FilterType.Jpg:
                    return "jpg";
                default:
                    return "";
            }
            #endif
            
            #if UNITY_STANDALONE_WIN
                switch (filter)
                {
                    case FilterType.Text:
                        return "(*.txt)|*.txt";
                    case FilterType.Jpg:
                        return "(*.jpg)|*.jpg";
                    default:
                        return "";
                }
            #endif
        }
    }
}