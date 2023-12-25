using UnityEngine;

namespace Script.Manager
{
    public static class Logger
    {
        public static void Log(this object obj, string message)
        {
            Debug.Log(obj.ToString() + " : " + message);
        }
    }
}