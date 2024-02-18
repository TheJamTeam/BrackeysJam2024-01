using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera player;

    void Start()
    {
        player = Camera.main;
    }


    void LateUpdate()
    {
        /*transform.LookAt(player.transform);
        transform.Rotate(-270, -90, -90);*/

        Vector3 lookDirection = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(lookDirection);
        transform.eulerAngles = new Vector3(90, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
