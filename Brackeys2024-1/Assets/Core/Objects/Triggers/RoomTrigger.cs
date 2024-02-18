using System;
using Core.Player;
using UnityEngine;

namespace Core.Objects.Triggers
{
    public class RoomTrigger : MonoBehaviour
    {
        public static event Action<int> OnFirstEnter;
        public static event Action<int> OnFirstExit;
        public static event Action<int> OnEnter;
        public static event Action<int> OnExit;

        private int _visitCount;
        [Range(0,4)]
        [SerializeField] private byte roomNumber;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (_visitCount == 0)
            {
                OnFirstEnter?.Invoke(roomNumber);
            }
            else
            {
                OnEnter?.Invoke(roomNumber);
            }
            _visitCount++;
        }

        private void OnTriggerExit(Collider other)
        {
            // If anything that isnt the player left the trigger, return
            if (!other.CompareTag("Player")) return;
            if (_visitCount == 0)
            {
                OnFirstExit?.Invoke(roomNumber);
            }
            else
            {
                OnExit?.Invoke(roomNumber);
            }
        }
    }
}
