using UnityEngine;

namespace GooyesPlugin
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        [SerializeField] protected bool _dontDestroyOnLoad = false;
        private bool _inited = false;

        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    Create();
                return _instance;
            }
        }

        protected static void Create()
        {
            GameObject go = new GameObject();
            go.name = typeof(T).Name;
            _instance = go.AddComponent(typeof(T)) as T;
        }

        public static void Destroy()
        {
            if (_instance != null && _instance.gameObject != null)
            {
                Destroy(_instance.gameObject);
            }
            _instance = null;
        }

        private void Awake()
        {
            if (_instance == null)
            {
                if (!_inited)
                    Init();
                _instance = this as T;
            }
            else
            {
                if (_instance != this)
                {
                    Destroy(gameObject);
                    Debug.LogError($"Two singletons at a time!", _instance.gameObject);
                }
            }
        }

        protected virtual void Init()
        {
            if (_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(this.gameObject);
            }
            _inited = true;
        }
    }
}
