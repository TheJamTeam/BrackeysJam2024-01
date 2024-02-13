using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractComponent : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool CanBePickedUp()
    {
        //TODO Implement
        return true;
    }

    public void ToggleIsHeld(bool toggle)
    {
        //TODO Implement
    }

    public void HoldUpdate(Vector3 holdOrigin)
    {
        transform.position = holdOrigin;
        //Gravitate towards the held origin.
    }
}
