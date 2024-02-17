using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
