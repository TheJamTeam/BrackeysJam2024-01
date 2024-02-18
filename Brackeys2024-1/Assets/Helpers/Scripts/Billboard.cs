using UnityEngine;

namespace CustomScripts.Helpers
{
    public class Billboard : MonoBehaviour
    {
        Camera player;

        void Start()
        {
            player = Camera.main;
        }


        void LateUpdate()
        {
            transform.LookAt(player.transform);
            transform.Rotate(-270, -90, -90);
        }
    }
}
