using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

/* Create an empty transform for the attach point, then give it this script and a trigger volume to detect the keyItem. */


public class SocketComponent : MonoBehaviour
{
    public string socketID;
    public string keyItem;
    public GameObject socketedItem;

    public static event Action<string> OnSocketFilled;
    public static event Action<string> OnSocketEmptied;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == keyItem)
        {
            socketedItem = other.gameObject;
            socketedItem.GetComponent<Rigidbody>().useGravity = false;

            // Need to remove the item from the player's hands

            socketedItem.transform.position = transform.position;
            socketedItem.transform.rotation = transform.rotation;

            OnSocketFilled?.Invoke(socketID);
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == keyItem)
        {
            socketedItem.GetComponent<Rigidbody>().useGravity = true;
            socketedItem = null;

            OnSocketEmptied?.Invoke(socketID);
        }
    }


/*
 * I have no idea why but this method of subscribing to the action throws an error. Tried adding an anonymous function [OnSocketFilled += () => Print(true)] but that also didn't work :/
 * 
    private void OnEnable()
    {
        OnSocketFilled += Print(true);
        OnSocketEmptied += Print(false);
    }

    private void OnDisable()
    {
        OnSocketFilled -= Print(true);
        OnSocketEmptied -= Print(false);
    }


    public void Print(bool socketed)
    {
        if (socketed)
        {
            Debug.Log($"Socketed: {0}", socketedItem);
        }
        else
        {
            Debug.Log($"Unsocketed {0}", socketedItem);
        }
    }*/
}
