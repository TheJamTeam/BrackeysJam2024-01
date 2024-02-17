using System;
using UnityEngine;

namespace Core.Objects.Door
{
    /// <summary>
    /// Stores the close Event, and the Trigger Logic for Closing the Door. Closing Can be invoked without trigger
    /// </summary>
    public class DoorEvents : MonoBehaviour
    {
        public static event Action<int> CloseDoor;

        [SerializeField] private int doorToClose;
        /// <summary>
        /// Method executed on entering the Trigger Collider on the associated GameObject
        /// </summary>
        /// <param name="other">The Gameobject entering this Collider</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                // If CloseDoor isnt null, Invoke
                CloseDoor?.Invoke(doorToClose);
            }
        }
    }
}
