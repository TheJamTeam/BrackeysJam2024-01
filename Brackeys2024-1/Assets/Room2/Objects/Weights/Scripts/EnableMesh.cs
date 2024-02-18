using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnableMesh : MonoBehaviour
{
    [SerializeField] private string weightObjName;
    [FormerlySerializedAs("meshRenderer")] [FormerlySerializedAs("_renderer")] [SerializeField] private GameObject weightMeshRenderer;
    [SerializeField] private GameObject highlightMeshRenderer;

    private void OnEnable()
    {
        InteractComponent.OnInteractUsed += OnSocketedWeight;
    }

    private void OnSocketedWeight(string socketedItem)
    {
        if (string.Compare(socketedItem, weightObjName, StringComparison.OrdinalIgnoreCase)==0) // the strings are the same ignoring case
        {
            highlightMeshRenderer.SetActive(false);
            weightMeshRenderer.SetActive(true);
        }
    }

    private void OnDisable()
    {
        InteractComponent.OnInteractUsed -= OnSocketedWeight;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (string.Compare(weightObjName, other.gameObject.name, StringComparison.OrdinalIgnoreCase) == 0)
        {
            highlightMeshRenderer.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (string.Compare(weightObjName, other.gameObject.name, StringComparison.OrdinalIgnoreCase) == 0)
        {
            highlightMeshRenderer.SetActive(false);
        }
    }
}
