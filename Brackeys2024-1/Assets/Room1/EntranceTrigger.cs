using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceTrigger : MonoBehaviour
{
    bool roomEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovementComponent>() != null && roomEntered == false)
        {
            roomEntered = true;
            FindObjectOfType<CreativityProgression>().RoomEntrance();
            Destroy(gameObject);
        }
    }
}
