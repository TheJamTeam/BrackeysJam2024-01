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
    public List<string> keyItems;
    public GameObject socketedItem;
    public bool canAcceptWrongItems;
    Rigidbody itemRB;
    GameObject previousSocketedItem;

    public static event Action<string> OnSocketFilled;
    public static event Action<string> OnSocketCompleted;
    public static event Action<string> OnSocketEmptied;



    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InteractComponent>() != null)
        {
            string ID = other.GetComponent<InteractComponent>().InteractID;

            if (ID == null || ID == "") { ID = other.gameObject.name; }


            if (canAcceptWrongItems || keyItems.Contains(ID))
            {
                socketedItem = other.gameObject;
                previousSocketedItem = socketedItem;
                itemRB = socketedItem.GetComponent<Rigidbody>();

                other.GetComponent<InteractComponent>().AddToSocket(this);

                itemRB.useGravity = false;
                itemRB.velocity = Vector3.zero;
                socketedItem.transform.position = transform.position;
                socketedItem.transform.rotation = transform.rotation;

                if (keyItems.Contains(ID))
                {
                    OnSocketCompleted?.Invoke(socketID);
                }
                else
                {
                    OnSocketFilled?.Invoke(ID);
                }
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
        Debug.Log($"{socket} Socket Complete!");
    }

    public void PrintEmptied(string socket)
    {
        Debug.Log($"{socket} Unsocketed");
    }

}
