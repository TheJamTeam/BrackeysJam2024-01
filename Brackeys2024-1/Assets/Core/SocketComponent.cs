using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

/* This socket script is intended to be separate from the visuals. Create an empty transform for the socket's attach point, 
 * then give it both this script and a trigger volume to detect the 'keyItem' that the socket is waiting for.
 * This also assumes that only the 'keyItem' will fit into the socket, and won't allow the player to insert other objects. */


public class SocketComponent : MonoBehaviour
{
    public string socketID;
    public string keyItem;
    public string altKeyItem;
    public GameObject socketedItem;
    public bool canAcceptWrongItems;
    Rigidbody itemRB;
    GameObject previousSocketedItem;

    public static event Action<string> OnSocketFilled;
    public static event Action<string> OnSocketCompleted;
    public static event Action<string> OnSocketEmptied;



    private void OnTriggerEnter(Collider other)
    {
        if (canAcceptWrongItems || other.gameObject.name == keyItem || other.gameObject.name == altKeyItem)
        {
            socketedItem = other.gameObject;
            previousSocketedItem = socketedItem;
            itemRB = socketedItem.GetComponent<Rigidbody>();

            // Need to remove the item from the player's hands

            itemRB.useGravity = false;
            itemRB.velocity = Vector3.zero;
            socketedItem.transform.position = transform.position;
            socketedItem.transform.rotation = transform.rotation;

            if (other.gameObject.name == keyItem || other.gameObject.name == altKeyItem)
            {
                OnSocketCompleted?.Invoke(socketID);
            }
            else
            {
                OnSocketFilled?.Invoke(socketedItem.name);
            }
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == socketedItem)
        {
            itemRB.useGravity = true;
            itemRB = null;
            socketedItem = null;

            OnSocketEmptied?.Invoke(socketID);
        }
    }




    private void OnEnable()
    {
        OnSocketFilled += Print;
        OnSocketCompleted += PrintCompleted;
        OnSocketEmptied += PrintEmptied;
    }

    private void OnDisable()
    {
        OnSocketFilled -= Print;
        OnSocketCompleted -= PrintCompleted;
        OnSocketEmptied -= PrintEmptied;
    }


    public void Print(string socket)
    {
        Debug.Log($"Socketed Wrong Item: {socket}");
    }

    public void PrintCompleted(string socket)
    {
        Debug.Log($"Socket Complete: {socket}");
    }

    public void PrintEmptied(string socket)
    {
        Debug.Log($"Unsocketed {socket}");
    }

}
