// https://bitbucket.org/alkee/aus

using UnityEngine;

namespace aus
{
    public abstract class Singleton<T>
    : MonoBehaviour where T : MonoBehaviour
    {

        private static T _instance = null;
        private static object guard = new object();
        private static bool appIsClosing = false;

        public static T Instance
        {
            get
            {
                if (appIsClosing) return null;

                lock (guard)
                {
                    if (_instance == null)
                    {
                        var objs = FindObjectsOfType<T>();
                        if (objs.Length > 0)
                            _instance = objs[0];

                        if (objs.Length > 1)
                            Debug.LogError("more than one instance of " + typeof(T).Name);

                        if (_instance == null)
                        {
                            var name = typeof(T).ToString();
                            var go = GameObject.Find(name);
                            if (guard == null)
                            {
                                go = new GameObject(name);
                            }
                            _instance = go.AddComponent<T>();
                        }
                        DontDestroyOnLoad(_instance.gameObject);
                    }

                    return _instance;
                }
            }
        }

        protected virtual void OnApplicationQuit()
        {
            appIsClosing = true;
        }
    }
}
