using UnityEngine;

namespace MTG
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static object _lock = new object();

        public static bool IsCreated(bool search = false)
        {
            if (search)
                _instance = (T)FindFirstObjectByType(typeof(T));
            return _instance != null;
        }

        public static T Instance
        {
            set
            {
                _instance = value;
            }
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T)FindFirstObjectByType(typeof(T));
                    }
                    return _instance;
                }
            }
        }
    }
}