using UnityEngine;

namespace CustomScripts.Core.Objects.Door
{
    public class SecretDoorMaterialHandler : MonoBehaviour
    {
        public Material wallMaterial;
        public Material secretDoorMaterial;
        public Renderer rdr;
        Transform player;
        float lookAngle;

        private void Start()
        {
            player = Camera.main.transform;
        }

        private void LateUpdate()
        {
            lookAngle = 1 - Mathf.Clamp01(2 * Vector3.Dot(player.forward, transform.right));
            rdr.material.Lerp(wallMaterial, secretDoorMaterial, lookAngle);
        }

    }
}
