using System;
using CustomScripts.Core.Objects;
using UnityEngine;

namespace CustomScripts.Room1
{
    public class PaintCanvas : MonoBehaviour
    {
        public string triggerID;
        public Material paintedMat;
        public Renderer rdr;


        private void OnEnable()
        {
            InteractComponent.OnInteractKeysComplete += ApplyPaint;
            InteractComponent.OnInteractUsed += ApplyPaint;
        }

        private void OnDisable()
        {
            InteractComponent.OnInteractKeysComplete -= ApplyPaint;
            InteractComponent.OnInteractUsed -= ApplyPaint;
        }

        public void ApplyPaint(string interactID)
        {
            // Are the two IDs equal, ignoring case (incase of typo)
            if (String.Equals(interactID, triggerID, StringComparison.OrdinalIgnoreCase))
            {
                Material[] splatterMaterials = new Material[2];
                splatterMaterials.SetValue(paintedMat, 0);
                splatterMaterials.SetValue(paintedMat, 1);
                rdr.materials = splatterMaterials;
                Destroy(this);
            }
        }
    }
}
