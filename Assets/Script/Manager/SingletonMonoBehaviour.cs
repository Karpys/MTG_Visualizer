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
                _instance = (T)FindObjectOfType(typeof(T));
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
                        _instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogWarning("[Singleton] Something went really wrong with " + typeof(T) +
                                " - there should never be more than 1 singleton!" +
                                " Reopening the scene might fix it.");
                            return _instance;
                        }
                    }
                    return _instance;
                }
            }
        }
    }
}