using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceTrigger : MonoBehaviour
{
    bool roomEntered;

    void Awake()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        CreativityProgression progress = FindObjectOfType<CreativityProgression>();
        if (other.GetComponent<PlayerMovementComponent>() != null && roomEntered == false && progress)
        {
            roomEntered = true;
            progress.RoomEntrance();
            Destroy(gameObject);
        }
    }
}
