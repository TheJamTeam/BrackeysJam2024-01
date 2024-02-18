using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLogic : MonoBehaviour
{
    public GameObject chocolateLock;
    public GameObject secretLock;
    public GameObject glassContainer;
    public Rigidbody thoughtfulGift;
    public GameObject bookSocketed;
    public GameObject canvasSocketed;


    string chocolateSolved = "Keylock";
    string secretSolved = "Secretlock";

    string canvasTrigger = "Creativity";
    bool creativitySolved;
    string bookTrigger = "Comprehension";
    bool comprehensionSolved;


    private void OnEnable()
    {
        InteractComponent.OnInteractKeysComplete += CheckSecretCriteria;
        InteractComponent.OnInteractUsed += CheckSecretCriteria;
    }

    private void OnDisable()
    {
        InteractComponent.OnInteractKeysComplete -= CheckSecretCriteria;
        InteractComponent.OnInteractUsed -= CheckSecretCriteria;
    }


    public void CheckSecretCriteria(string interactID)
    {
        // Are the two IDs equal, ignoring case (incase of typo)
        if (String.Equals(interactID, chocolateSolved, StringComparison.OrdinalIgnoreCase))
        {
            Destroy(chocolateLock);
        }
        else if (String.Equals(interactID, secretSolved, StringComparison.OrdinalIgnoreCase))
        {
            Destroy(secretLock);
        }
        else if (String.Equals(interactID, canvasTrigger, StringComparison.OrdinalIgnoreCase))
        {
            creativitySolved = true;
            canvasSocketed.SetActive(true);
        }
        else if (String.Equals(interactID, bookTrigger, StringComparison.OrdinalIgnoreCase))
        {
            comprehensionSolved = true;
            bookSocketed.SetActive(true);
        }

        if (creativitySolved && comprehensionSolved) { BreakGlass(); }
    }


    public void BreakGlass()
    {
        // Play sound effect
        Destroy(glassContainer);
        thoughtfulGift.isKinematic = false;
    }
}
