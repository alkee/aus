// https://bitbucket.org/alkee/aus

using UnityEngine;
using UnityEngine.Events;

namespace aus
{
    [DisallowMultipleComponent]
    public abstract class Singleton<T>
    : MonoBehaviour where T : MonoBehaviour
    {

        private static T _instance = null;
        private static object guard = new object();

        public static T Instance
        {
            get
            {
                lock (guard)
                {
                    if (_instance == null)
                    {
                        var objs = FindObjectsOfType<T>();
                        if (objs.Length > 0)
                            _instance = objs[0];

                        if (objs.Length > 1)
                        {
                            Debug.LogWarning("more than one instance of " + typeof(T).Name);
                            for (var i = 1; i < objs.Length; ++i)
                            {
                                // DisallowMultipleComponent 되어있기 때문에 gameobject 를 지우면 component 한개만 삭제될 것.
                                Debug.LogWarning("    destorying " + objs[i].name);
                                Destroy(objs[i].gameObject);
                            }
                        }

                        if (_instance == null)
                        {
                            var name = typeof(T).ToString();
                            var go = new GameObject(name);
                            _instance = go.AddComponent<T>();
                        }
                        DontDestroyOnLoad(_instance.gameObject);
                    }

                    return _instance;
                }
            }
        }

        public UnityEvent onInit;
        public UnityEvent onQuit;

        private void Awake()
        {
            if (Instance == this)
            {
                Debug.Log($"Singleton {typeof(T)} initialized in {name}");
                onInit.Invoke();
                return;
            }
            Destroy(gameObject);
            Debug.Log($"Singleton {typeof(T)} is duplicated in hierarchy. destroying {name}");
        }

        void OnDestroy()
        {
            var cnt = FindObjectsOfType<T>(true).Length;
            if (cnt == 1) // last one
            {
                onQuit.Invoke();
                _instance = null;
            }
        }
    }
}
