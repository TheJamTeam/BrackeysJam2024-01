using UnityEngine;

namespace CustomScripts.Helpers
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
