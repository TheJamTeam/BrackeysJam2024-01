using UnityEngine;

namespace CustomScripts.Helpers
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static bool Exists => instance != null;

        private static T instance;
        public static T Instance
        {
            get
            {
                return instance;
            }
        }

        protected virtual void Awake()
        {
            Initialize();
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }

        private void Initialize()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
        
            instance = this as T;
        }
    }
}
